import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ConsultingRoomListDto, ConsultingRoomCreateDto, ConsultingRoomEditDto } from '../../../shared/models/consulttorio';
import { GenericService } from '../../../shared/services/generic.service';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { MatDialog } from '@angular/material/dialog';
import { GenericFormComponent } from '../../../shared/components/forms/generic-form/generic-form.component';
import { GenericFormField } from '../../../shared/models/formsModel';
import { GenericModalService } from '../../../shared/services/generic-modal.service';

@Component({
  selector: 'app-consultoring',
  standalone:false, 
  templateUrl: './consultoring.component.html',
  styleUrl: './consultoring.component.css'
})
export class ConsultoringComponent implements OnInit {
  private readonly consultor: GenericService<
    ConsultingRoomListDto,
    ConsultingRoomCreateDto,
    ConsultingRoomEditDto
  >;

  dataSource: ConsultingRoomListDto[] = [];
  dataSourceFiltered: ConsultingRoomListDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.consultor = new GenericService<
      ConsultingRoomListDto,
      ConsultingRoomCreateDto,
      ConsultingRoomEditDto
    >(this.http, EntityName.Consultorios);
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Carga los datos desde el backend */
  loadData(): void {
    this.consultor.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar consultorios:', err),
    });
  }

  /** 🔹 Define columnas para la tabla */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'name', label: 'Nombre' },
      { key: 'roomNumber', label: 'Número de sala' },
      { key: 'floor', label: 'Piso' },
      {
        key: 'image',
        label: 'Imagen',
        type: 'icon',
        icon: 'image',
        tooltip: 'Ver imagen',
      },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields(): GenericFormField<ConsultingRoomCreateDto>[] {
    return [
      { key: 'name', label: 'Nombre', type: 'text', required: true },
      { key: 'roomNumber', label: 'Número de sala', type: 'number', required: true },
      { key: 'floor', label: 'Piso', type: 'number', required: true },
      {
        key: 'image',
        label: 'URL de imagen',
        type: 'text',
        placeholder: 'https://...',
      },
    ];
  }

  /** 🔹 Crear nuevo registro */
  crear(): void {
    this.modalService.openForm<ConsultingRoomCreateDto>({
      title: 'Nuevo Consultorio',
      fields: this.getFormFields(),
      onSave: (data) => this.consultor.create(data),
      onSuccess: () => this.refresh(),
    });
  }

  /** 🔹 Editar registro existente */
  editar(row: ConsultingRoomEditDto): void {
    this.modalService.openForm<ConsultingRoomEditDto>({
      title: 'Editar Consultorio',
      fields: this.getFormFields(),
      model: row,
      onSave: (data) => this.consultor.update(row.id, data),
      onSuccess: () => this.refresh(),
    });
  }

  /** 🔹 Refresca datos después de crear/editar/eliminar */
  refresh(): void {
    this.loadData();
  }

  /** 🔹 Maneja acciones de la tabla */
  onAction(event: { action: string; element: ConsultingRoomListDto }): void {
    switch (event.action) {
      case 'edit':
        this.editar(event.element);
        break;
      case 'delete':
        this.modalService.confirmDelete(
          () => this.consultor.delete(event.element.id),
          () => this.refresh()
        );
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