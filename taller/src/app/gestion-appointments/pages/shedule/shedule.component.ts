import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ConsultingRoomListDto } from '../../../shared/models/consulttorio';
import { DoctorListDto } from '../../../shared/models/doctormodels';
import { GenericFormField } from '../../../shared/models/formsModel';
import { SheduleListDto, SheduleCreateDto, SheduleEditDto } from '../../../shared/models/shedulemodel';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { TypeCitationListDto } from '../../../shared/models/typeCitation';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';

@Component({
    
  selector: 'app-shedule',
  standalone:false,
  templateUrl: './shedule.component.html',
  styleUrl: './shedule.component.css'
})
export class SheduleComponent implements OnInit {
  private readonly sheduleService: GenericService<
    SheduleListDto,
    SheduleCreateDto,
    SheduleEditDto
  >;

  private readonly typeCitationService: GenericService<
    TypeCitationListDto,
    any,
    any
  >;

  private readonly doctorService: GenericService<DoctorListDto, any, any>;

  private readonly consultingRoomService: GenericService<
    ConsultingRoomListDto,
    any,
    any
  >;

  dataSource: SheduleListDto[] = [];
  dataSourceFiltered: SheduleListDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.sheduleService = new GenericService<
      SheduleListDto,
      SheduleCreateDto,
      SheduleEditDto
    >(this.http, EntityName.Shedules);

    this.typeCitationService = new GenericService<
      TypeCitationListDto,
      any,
      any
    >(this.http, EntityName.TypeCitations);

    this.doctorService = new GenericService<DoctorListDto, any, any>(
      this.http,
      EntityName.Doctors
    );

    this.consultingRoomService = new GenericService<
      ConsultingRoomListDto,
      any,
      any
    >(this.http, EntityName.Consultorios);
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Cargar horarios */
  loadData(): void {
    this.sheduleService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar horarios:', err),
    });
  }

  /** 🔹 Columnas de la tabla */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'typeCitationName', label: 'Tipo de Cita' },
      { key: 'nameDoctor', label: 'Doctor' },
      { key: 'consultingRoomName', label: 'Consultorio' },
      { key: 'numberCitation', label: 'N° de Citas' },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields<T extends SheduleCreateDto | SheduleEditDto>(): GenericFormField<T>[] {
    return [
      {
        key: 'typeCitationId',
        label: 'Tipo de Cita',
        type: 'select',
        required: true,
        serviceFn: async () => await firstValueFrom(this.typeCitationService.getAll()),
        mapLabel: 'name',
        mapValue: 'id',
      },
      {
        key: 'doctorId',
        label: 'Doctor',
        type: 'select',
        required: true,
        serviceFn: async () => await firstValueFrom(this.doctorService.getAll()),
        mapLabel: 'fullName',
        mapValue: 'id',
      },
      {
        key: 'consultingRoomId',
        label: 'Consultorio',
        type: 'select',
        required: true,
        serviceFn: async () => await firstValueFrom(this.consultingRoomService.getAll()),
        mapLabel: 'name',
        mapValue: 'id',
      },
      {
        key: 'numberCitation',
        label: 'Número de Citas',
        type: 'number',
        required: true,
      },
    ];
  }

  /** 🔹 Crear horario */
  crear(): void {
    this.modalService.openForm<SheduleCreateDto>({
      title: 'Nuevo Horario',
      fields: this.getFormFields<SheduleCreateDto>(),
      onSave: (data) => this.sheduleService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar horario */
  editar(row: SheduleEditDto): void {
    this.modalService.openForm<SheduleEditDto>({
      title: 'Editar Horario',
      fields: this.getFormFields<SheduleEditDto>(),
      model: row,
      onSave: (data) => this.sheduleService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar horario */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.sheduleService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Manejo de acciones */
  onAction(event: { action: string; element: SheduleListDto }): void {
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

  get columns(): string[] {
    return this.columnDefs.map((c) => c.key);
  }
}