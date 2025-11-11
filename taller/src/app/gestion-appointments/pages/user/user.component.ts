import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { GenericFormField } from '../../../shared/models/formsModel';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { UserSelectDto, UserCreate, UserEditDto } from '../../../shared/models/usermodel';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';

@Component({

  selector: 'app-user',
  standalone:false,
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  private readonly userService: GenericService<UserSelectDto, UserCreate, UserEditDto>;
  private readonly personService: GenericService<any, any, any>;

  dataSource: UserSelectDto[] = [];
  dataSourceFiltered: UserSelectDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.userService = new GenericService<UserSelectDto, UserCreate, UserEditDto>(
      this.http,
      EntityName.Users
    );

    this.personService = new GenericService<any, any, any>(
      this.http,
      EntityName.Persons
    );
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Carga usuarios */
  loadData(): void {
    this.userService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar usuarios:', err),
    });
  }

  /** 🔹 Define columnas */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'name', label: 'Nombre' },
      { key: 'email', label: 'Correo electrónico' },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields(): GenericFormField<UserCreate>[] {
    return [
      { key: 'name', label: 'Nombre', type: 'text', required: true },
      { key: 'email', label: 'Correo electrónico', type: 'text', required: true },
      { key: 'password', label: 'Contraseña', type: 'text', placeholder: '********' },
      {
        key: 'personId',
        label: 'Persona asociada',
        type: 'select',
        required: false,
        serviceFn: async () => await firstValueFrom(this.personService.getAll()),
        mapLabel: 'firstName',
        mapValue: 'id',
      },
    ];
  }

  /** 🔹 Crear nuevo usuario */
  crear(): void {
    this.modalService.openForm<UserCreate>({
      title: 'Nuevo Usuario',
      fields: this.getFormFields(),
      onSave: (data) => this.userService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar usuario existente */
  editar(row: UserEditDto): void {
    this.modalService.openForm<UserEditDto>({
      title: 'Editar Usuario',
      fields: this.getFormFields(),
      model: row,
      onSave: (data) => this.userService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar usuario */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.userService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Manejo de acciones */
  onAction(event: { action: string; element: UserSelectDto }): void {
    switch (event.action) {
      case 'edit':
        this.editar(event.element as unknown as UserEditDto);
        break;
      case 'delete':
        this.eliminar(event.element.id);
        break;
      default:
        console.log('Acción desconocida:', event);
        break;
    }
  }

  /** 🔹 Columnas visibles */
  get columns(): string[] {
    return this.columnDefs.map((c) => c.key);
  }
}