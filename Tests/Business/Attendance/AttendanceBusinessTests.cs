using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Implementations.Operational;
using Business.Interfaces.Auth;
using Business.Interfaces.Notifications;
using Business.Interfaces.Operational;
using Business.Interfaces.Security;
using Data.Interfases.Operational;
using Data.Interfases.Security;
using Entity.DTOs.ModelSecurity.Response;
using Entity.DTOs.Operational.Request;
using Entity.Models.ModelSecurity;
using Entity.Models.Operational;
using Entity.Models.Organizational;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Business.Attendances
{
    public class AttendanceBusinessTests
    {
        // ================== Mocks reutilizables ==================

        private readonly Mock<IAttendanceData> _attendanceData = new();
        private readonly Mock<IEventData> _eventData = new();
        private readonly Mock<IPersonData> _personData = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IUserBusiness> _userBusiness = new();
        private readonly Mock<IEventAccessPointBusiness> _eventAccessPointBusiness = new();
        private readonly Mock<INotificationBusiness> _notificationBusiness = new();
        private readonly Mock<IAttendanceNotifier> _attendanceNotifier = new();

        private readonly ILogger<Attendance> _logger = new NullLogger<Attendance>();
        private readonly IMapper _mapper;

        public AttendanceBusinessTests()
        {
            // AutoMapper real (solo para pruebas)
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps("Entity");
                cfg.AddMaps("Business");
            });

            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Método auxiliar para crear instancia con todos los mocks.
        /// </summary>
        private AttendanceBusiness CreateInstance()
        {
            return new AttendanceBusiness(
                _attendanceData.Object,
                _eventData.Object,
                _personData.Object,
                _logger,
                _mapper,
                _currentUser.Object,
                _userBusiness.Object,
                _eventAccessPointBusiness.Object,
                _notificationBusiness.Object,
                _attendanceNotifier.Object
            );
        }

        // =====================================================================
        //                                TESTS
        // =====================================================================


        // ---------------------------------------------------------------
        // 1️ RegisterAttendanceByQrAsync — QR vacío
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterAttendanceByQrAsync_QrEmpty_ReturnsError()
        {
            var business = CreateInstance();

            var result = await business.RegisterAttendanceByQrAsync("", 1);

            Assert.False(result.Success);
            Assert.Equal("QR vacío o inválido.", result.Message);
        }


        // ---------------------------------------------------------------
        // 2️ QR con formato inválido
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterAttendanceByQrAsync_InvalidFormat_ReturnsError()
        {
            var business = CreateInstance();

            var result = await business.RegisterAttendanceByQrAsync("EVT|1", 1);

            Assert.False(result.Success);
            Assert.Equal("Formato de QR inválido.", result.Message);
        }


        // ---------------------------------------------------------------
        // 3️ Evento no encontrado
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterAttendanceByQrAsync_EventNotFound_ReturnsError()
        {
            _eventData.Setup(x => x.GetQueryable())
                      .Returns(new List<Event>().AsQueryable());

            var business = CreateInstance();

            var result = await business.RegisterAttendanceByQrAsync("EVT|1|X|P1", 5);

            Assert.False(result.Success);
            Assert.Equal("Evento no encontrado.", result.Message);
        }


        // ---------------------------------------------------------------
        // 4️ Persona no encontrada
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterAttendanceByQrAsync_PersonNotFound_ReturnsError()
        {
            var eventAccessPoints = new List<EventAccessPoint>();
            var evt = new Event
            {
                Id = 1,
                Name = "Event",
                IsDeleted = false,
                EventAccessPoints = eventAccessPoints
            };

            _eventData.Setup(x => x.GetQueryable())
                .Returns(new List<Event> { evt }.AsQueryable());

            _personData.Setup(x => x.GetQueryable())
                .Returns(new List<Person>().AsQueryable());  // person not found

            var business = CreateInstance();

            var result = await business.RegisterAttendanceByQrAsync("EVT|1|Event|Main", 999);

            Assert.False(result.Success);
            Assert.Equal("Persona no encontrada.", result.Message);
        }



        // ---------------------------------------------------------------
        // 5️ Registro exitoso
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterAttendanceByQrAsync_SaveSuccessful_ReturnsSuccess()
        {
            var accessPoint = new AccessPoint
            {
                Id = 10,
                Name = "Gate"
            };

            var evento = new Event
            {
                Id = 1,
                Name = "My Event",
                IsDeleted = false,
                EventAccessPoints = new List<EventAccessPoint>
                {
                    new EventAccessPoint { AccessPoint = accessPoint }
                }
            };

            _eventData.Setup(x => x.GetQueryable())
                      .Returns(new List<Event> { evento }.AsQueryable());

            _personData.Setup(x => x.GetQueryable())
                       .Returns(new List<Person> { new Person { Id = 20 } }.AsQueryable());

            _attendanceData.Setup(x => x.SaveAsync(It.IsAny<Attendance>()))
                           .ReturnsAsync((Attendance a) =>
                           {
                               a.Id = 99;
                               return a;
                           });

            _attendanceData.Setup(x => x.GetAllAsync())
     .ReturnsAsync(new List<Attendance>
     {
        new Attendance
        {
            Id = 99,
            PersonId = 20,
            TimeOfEntry = DateTime.UtcNow,

            EventAccessPointEntryId = 10,
            EventAccessPointEntry = new EventAccessPoint
            {
                Id = 10,
                EventId = 1,
                AccessPointId = 10,

                AccessPoint = new AccessPoint
                {
                    Id = 10,
                    Name = "Gate"
                },

                Event = new Event
                {
                    Id = 1,
                    Name = "My Event",
                    EventAccessPoints = new List<EventAccessPoint>()
                }
            }
        }
     });




            var business = CreateInstance();

            var result = await business.RegisterAttendanceByQrAsync("EVT|1|My Event|Gate", 20);

            Assert.True(result.Success);
            Assert.Contains("Asistencia registrada", result.Message);
        }


        // ---------------------------------------------------------------
        // 6️ RegisterEntryAsync — ya existe una asistencia abierta
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterEntryAsync_OpenAttendanceExists_ReturnsError()
        {
            _currentUser.Setup(x => x.UserId).Returns(1);

            _userBusiness.Setup(x => x.GetById(1))
                         .ReturnsAsync(new UserDTO { PersonId = 8 });

            _eventAccessPointBusiness.Setup(x => x.GetByQrKey("QR123"))
                                     .ReturnsAsync(new EventAccessPoint { Id = 55 });

            _attendanceData.Setup(x => x.GetOpenAttendanceAsync(8, 55))
                           .ReturnsAsync(new Attendance());

            var business = CreateInstance();

            var dto = new AttendanceDtoRequestSpecific { QrCodeKey = "QR123" };

            var result = await business.RegisterEntryAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Ya existe una entrada registrada. ¿Desea realizar la salida?", result.Message);
        }


        // ---------------------------------------------------------------
        // 7️ RegisterExitAsync — sin asistencia abierta
        // ---------------------------------------------------------------
        [Fact]
        public async Task RegisterExitAsync_NoOpenAttendance_ReturnsError()
        {
            _currentUser.Setup(x => x.UserId).Returns(1);

            _userBusiness.Setup(x => x.GetById(1))
                         .ReturnsAsync(new UserDTO { PersonId = 8 });

            _eventAccessPointBusiness.Setup(x => x.GetByQrKey("EXITQR"))
                                     .ReturnsAsync(new EventAccessPoint { Id = 77 });

            _attendanceData.Setup(x => x.GetOpenAttendanceAsync(8, 77))
                           .ReturnsAsync((Attendance)null);

            var business = CreateInstance();

            var dto = new AttendanceDtoRequestSpecific { QrCodeKey = "EXITQR" };

            var result = await business.RegisterExitAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("No se encontró una asistencia abierta para registrar salida.", result.Message);
        }
    }
}
