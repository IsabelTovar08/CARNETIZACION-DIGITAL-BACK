﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Entity.DataInit.Operational;
using Entity.DataInit.Parameter;
using Entity.Models;
using Entity.Models.Auth;
using Entity.Models.ModelSecurity;
using Entity.Models.Notifications;
using Entity.Models.Operational;
using Entity.Models.Operational.BulkLoading;
using Entity.Models.Organizational;
using Entity.Models.Organizational.Assignment;
using Entity.Models.Organizational.Location;
using Entity.Models.Organizational.Structure;
using Entity.Models.Parameter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Entity.Context
{
    /// <summary>
    /// Representa el contexto de la base de datos de la aplicación, proporcionando configuraciones y métodos
    /// para la gestión de entidades y consultas personalizadas con Dapper.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Configuración de la aplicación.
        /// </summary>
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor del contexto de la base de datos.
        /// </summary>
        /// <param name="options">Opciones de configuración para el contexto de base de datos.</param>
        /// <param name="configuration">Instancia de IConfiguration para acceder a la configuración de la aplicación.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }

        //Security
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Models.Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<RolFormPermission> RoleFormPermissions { get; set; }
        public DbSet<Person> People { get; set; }

        //Auth 
        public DbSet<RefreshToken> RefreshToken { get; set; }

        //Organizational 
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<OrganizationalUnit> OrganizationalUnits { get; set; }
        public DbSet<OrganizationalUnitBranch> OrganizationalUnitBranches { get; set; }
        public DbSet<AreaCategory> AreaCategories { get; set; }
        public DbSet<InternalDivision> InternalDivisions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ContactOrganization> ContactOrganizations { get; set; }

        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<CardConfiguration> CardsConfigurations { get; set; }
        //public DbSet<Card> Cards { get; set; }
        public DbSet<IssuedCard> IssuedCards { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationReceived> NotificationReceiveds { get; set; }
        public DbSet<MenuStructure> MenuStructure { get; set; }

        //Events
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventTargetAudience> EventTargetAudiences { get; set; }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AccessPoint> AccessPoints { get; set; }
        public DbSet<EventAccessPoint> EventAccessPoints { get; set; }

        //Others
        public DbSet<Status> Statuses { get; set; }
        public DbSet<CustomType> CustomTypes { get; set; }
        public DbSet<TypeCategory> TypeCategories { get; set; }

        public DbSet<ImportBatch> ImportBatches { get; set; }
        public DbSet<ImportBatchRow> ImportBatchRows { get; set; }

        public DbSet<CardTemplate> CardTemplates { get; set; }

        /// <summary>
        /// Configura los modelos de la base de datos aplicando configuraciones desde ensamblados.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de base de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversión para TimeOnly → TimeSpan
            modelBuilder.Entity<Schedule>()
                .Property(s => s.StartTime)
                .HasConversion(
                    v => v,
                    v => v);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.EndTime)
                .HasConversion(
                    v => v,
                    v => v);

            modelBuilder.Entity<EventAccessPoint>(eb =>
            {
                eb.ToTable("EventAccessPoints", "Operational");

                eb.HasKey(eap => new { eap.EventId, eap.AccessPointId }); // Clave compuesta

                eb.HasOne(eap => eap.Event)
                  .WithMany(e => e.EventAccessPoints)
                  .HasForeignKey(eap => eap.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(eap => eap.AccessPoint)
                  .WithMany(ap => ap.EventAccessPoints)
                  .HasForeignKey(eap => eap.AccessPointId)
                  .OnDelete(DeleteBehavior.Restrict);
            });


            // ================= ÍNDICES OPTIMIZACIÓN =================
            modelBuilder.Entity<Attendance>(b =>
            {
                b.HasIndex(x => x.PersonId);
                b.HasIndex(x => x.TimeOfEntry);
                b.HasIndex(x => x.TimeOfExit);
                b.HasIndex(x => x.IsDeleted);
                // Compuesto: acelerar consultas de "abierto" (TimeOfExit == null) por persona.
                b.HasIndex(x => new { x.PersonId, x.TimeOfExit });

                // Si usas SQL Server y quieres índice filtrado, puedes agregarlo por migración SQL manual:
                // b.HasIndex(x => x.PersonId).HasFilter("[TimeOfExit] IS NULL AND [IsDeleted] = 0");
            });
            // Configuración de EventTargetAudience
            modelBuilder.Entity<EventTargetAudience>(eb =>
            {
                eb.ToTable("EventTargetAudiences", "Operational");

                eb.HasOne(e => e.Profile)
                    .WithMany()
                    .HasForeignKey(e => e.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.HasOne(e => e.OrganizationalUnit)
                    .WithMany()
                    .HasForeignKey(e => e.OrganizationalUnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.HasOne(e => e.InternalDivision)
                    .WithMany()
                    .HasForeignKey(e => e.InternalDivisionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // Conversión global UTC para DateTime
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                             .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
                {
                    property.SetValueConverter(
                        new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }

            // Otras configuraciones del ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Otras configuraciones adicionales pueden ir aquí
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Precisión estándar para decimales
            configurationBuilder
                .Properties<decimal>()
                .HavePrecision(18, 2);

            configurationBuilder
        .Properties<DateTime>()
        .HaveConversion<DateTimeToUtcConverter>()   // 👈 usamos un conversor genérico
        .HaveColumnType("timestamp with time zone");


            // Manejo de DateOnly (si se usa)
            configurationBuilder
                .Properties<DateOnly>()
                .HaveColumnType("date");

            // Manejo de TimeOnly (si se usa)
            configurationBuilder
                .Properties<TimeOnly>()
                .HaveColumnType("time");

            // Enums se almacenan como int (compatible en todos los motores)
            configurationBuilder
                .Properties<Enum>()
                .HaveConversion<int>()
                .HaveColumnType("int");
        }
        /// <summary>
        /// Conversor global para normalizar todos los DateTime como UTC.
        /// </summary>
        public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
        {
            public DateTimeToUtcConverter()
                : base(
                    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc), // Guardar
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))                                 // Leer
            { }
        }
        /// <summary>
        /// Guarda los cambios en la base de datos, asegurando la auditoría antes de persistir los datos.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guarda los cambios en la base de datos de manera asíncrona, asegurando la auditoría antes de la persistencia.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indica si se deben aceptar todos los cambios en caso de éxito.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación.</param>
        /// <returns>Número de filas afectadas de forma asíncrona.</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            //EnsureAudit();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve una colección de resultados de tipo genérico.
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve un solo resultado o el valor predeterminado si no hay resultados.
        /// </summary>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            public CommandDefinition Definition { get; }

            public void Dispose()
            {
            }
        }
    }
}