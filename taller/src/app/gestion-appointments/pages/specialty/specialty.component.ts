import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GenericFormField } from '../../../shared/models/formsModel';
import { SpecialtyListDto, SpecialtyCreatedDto, SpecialtyEditDto } from '../../../shared/models/specialtymodels';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';

@Component({
  selector: 'app-specialty',
  standalone:false,
  templateUrl: './specialty.component.html',
  styleUrl: './specialty.component.css'
})
export class SpecialtyComponent implements OnInit {
  private readonly specialtyService: GenericService<
    SpecialtyListDto,
    SpecialtyCreatedDto,
    SpecialtyEditDto
  >;

  dataSource: SpecialtyListDto[] = [];
  dataSourceFiltered: SpecialtyListDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.specialtyService = new GenericService<
      SpecialtyListDto,
      SpecialtyCreatedDto,
      SpecialtyEditDto
    >(this.http, EntityName.Specialties); // 👈 asegúrate de tener este valor en tu enum EntityName
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Cargar especialidades */
  loadData(): void {
    this.specialtyService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar especialidades:', err),
    });
  }

  /** 🔹 Columnas de la tabla */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'name', label: 'Nombre' },
      { key: 'description', label: 'Descripción' },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields(): GenericFormField<SpecialtyCreatedDto>[] {
    return [
      { key: 'name', label: 'Nombre', type: 'text', required: true },
      { key: 'description', label: 'Descripción', type: 'text', required: true },
    ];
  }

  /** 🔹 Crear nueva especialidad */
  crear(): void {
    this.modalService.openForm<SpecialtyCreatedDto>({
      title: 'Nueva Especialidad',
      fields: this.getFormFields(),
      onSave: (data) => this.specialtyService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar especialidad existente */
  editar(row: SpecialtyEditDto): void {
    this.modalService.openForm<SpecialtyEditDto>({
      title: 'Editar Especialidad',
      fields: this.getFormFields(),
      model: row,
      onSave: (data) => this.specialtyService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar especialidad */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.specialtyService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Manejo de acciones de la tabla */
  onAction(event: { action: string; element: SpecialtyListDto }): void {
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