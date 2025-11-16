import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GenericFormField } from '../../../shared/models/formsModel';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { TypeCitationListDto, TypeCitationCreateDto, TypeCitationEditDto } from '../../../shared/models/typeCitation';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';

@Component({
  selector: 'app-type-citation',
  standalone:false,
    templateUrl: './type-citation.component.html',
  styleUrl: './type-citation.component.css'
})
export class TypeCitationComponent implements OnInit {
  private readonly typeCitationService: GenericService<
    TypeCitationListDto,
    TypeCitationCreateDto,
    TypeCitationEditDto
  >;

  dataSource: TypeCitationListDto[] = [];
  dataSourceFiltered: TypeCitationListDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.typeCitationService = new GenericService<
      TypeCitationListDto,
      TypeCitationCreateDto,
      TypeCitationEditDto
    >(this.http, EntityName.TypeCitations); // 👈 asegúrate que exista en tu enum EntityName
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Carga los tipos de cita */
  loadData(): void {
    this.typeCitationService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar tipos de cita:', err),
    });
  }

  /** 🔹 Define las columnas de la tabla */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'name', label: 'Nombre' },
      { key: 'description', label: 'Descripción' },
      { key: 'icon', label: 'Ícono' },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
getFormFields<T extends TypeCitationCreateDto | TypeCitationEditDto>(): GenericFormField<T>[] {
  return [
    { key: 'name', label: 'Nombre', type: 'text', required: true },
    { key: 'description', label: 'Descripción', type: 'text', required: true },
    { key: 'icon', label: 'Ícono', type: 'text', required: false },
  ];
}


  /** 🔹 Crear nuevo tipo de cita */
  crear(): void {
    this.modalService.openForm<TypeCitationCreateDto>({
      title: 'Nuevo Tipo de Cita',
      fields: this.getFormFields(),
      onSave: (data) => this.typeCitationService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar tipo de cita existente */
  editar(row: TypeCitationEditDto): void {
    this.modalService.openForm<TypeCitationEditDto>({
      title: 'Editar Tipo de Cita',
      fields: this.getFormFields(),
      model: row,
      onSave: (data) => this.typeCitationService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar tipo de cita */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.typeCitationService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Acciones de la tabla */
  onAction(event: { action: string; element: TypeCitationListDto }): void {
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