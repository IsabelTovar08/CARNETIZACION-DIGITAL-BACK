import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GenericFormField } from '../../../shared/models/formsModel';
import { PersonSelectDto, PersonCreated, PersonDto } from '../../../shared/models/personmodel';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';

@Component({
  selector: 'app-person',
  standalone:false,
  templateUrl: './person.component.html',
  styleUrl: './person.component.css'
})
export class PersonComponent implements OnInit {
  private readonly personService: GenericService<
    PersonSelectDto,
    PersonCreated,
    PersonDto
  >;

  dataSource: PersonSelectDto[] = [];
  dataSourceFiltered: PersonSelectDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.personService = new GenericService<
      PersonSelectDto,
      PersonCreated,
      PersonDto
    >(this.http, EntityName.Persons);
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Cargar datos */
  loadData(): void {
    this.personService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar personas:', err),
    });
  }

  /** 🔹 Columnas de la tabla */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'firstName', label: 'Nombre' },
      { key: 'lastName', label: 'Apellido' },
      { key: 'identification', label: 'Identificación' },
      { key: 'phoneNumber', label: 'Teléfono' },
      { key: 'address', label: 'Dirección' },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields(): GenericFormField<PersonCreated>[] {
    return [
      { key: 'firstName', label: 'Nombre', type: 'text', required: true },
      { key: 'lastName', label: 'Apellido', type: 'text', required: true },
      { key: 'identification', label: 'Identificación', type: 'text', required: true },
      { key: 'phoneNumber', label: 'Teléfono', type: 'text', required: true },
      { key: 'address', label: 'Dirección', type: 'text', required: true },
    ];
  }

  /** 🔹 Crear nueva persona */
  crear(): void {
    this.modalService.openForm<PersonCreated>({
      title: 'Nueva Persona',
      fields: this.getFormFields(),
      onSave: (data) => this.personService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar persona existente */
  editar(row: PersonDto): void {
    this.modalService.openForm<PersonDto>({
      title: 'Editar Persona',
      fields: this.getFormFields(),
      model: row,
      onSave: (data) => this.personService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar persona */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.personService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Acciones desde la tabla */
  onAction(event: { action: string; element: PersonSelectDto }): void {
    switch (event.action) {
      case 'edit':
        this.editar(event.element);
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