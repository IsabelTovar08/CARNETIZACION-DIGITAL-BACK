import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DoctorListDto, DoctorCreateDto, DoctorEditDto } from '../../../shared/models/doctormodels';
import { GenericFormField } from '../../../shared/models/formsModel';
import { PersonSelectDto } from '../../../shared/models/personmodel';
import { SpecialtyListDto } from '../../../shared/models/specialtymodels';
import { ColumnDefinition } from '../../../shared/models/tablemodule';
import { EntityName } from '../../../shared/services/enums/EntityName';
import { GenericModalService } from '../../../shared/services/generic-modal.service';
import { GenericService } from '../../../shared/services/generic.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-doctors',
  standalone:false,
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css'
})
export class DoctorsComponent   implements OnInit{
  private readonly doctorService: GenericService<
    DoctorListDto,
    DoctorCreateDto,
    DoctorEditDto
  >;

  private readonly specialtyService: GenericService<SpecialtyListDto, any, any>;
  private readonly personService: GenericService<PersonSelectDto, any, any>;

  dataSource: DoctorListDto[] = [];
  dataSourceFiltered: DoctorListDto[] = [];
  columnDefs: ColumnDefinition[] = [];

  constructor(
    private readonly http: HttpClient,
    private readonly modalService: GenericModalService
  ) {
    this.doctorService = new GenericService<
      DoctorListDto,
      DoctorCreateDto,
      DoctorEditDto
    >(this.http, EntityName.Doctors);

    this.specialtyService = new GenericService<
      SpecialtyListDto,
      any,
      any
    >(this.http, EntityName.Specialties);

    this.personService = new GenericService<
      PersonSelectDto,
      any,
      any
    >(this.http, EntityName.Persons);
  }

  ngOnInit(): void {
    this.loadData();
    this.initColumns();
  }

  /** 🔹 Cargar doctores */
  loadData(): void {
    this.doctorService.getAll().subscribe({
      next: (data) => {
        this.dataSource = data;
        this.dataSourceFiltered = [...data];
      },
      error: (err) => console.error('Error al cargar doctores:', err),
    });
  }

  /** 🔹 Columnas */
  initColumns(): void {
    this.columnDefs = [
      { key: 'index', label: '#' },
      { key: 'fullName', label: 'Nombre del Doctor' },
      { key: 'specialtyName', label: 'Especialidad' },
      { key: 'emailDoctor', label: 'Correo' },
      {
        key: 'active',
        label: 'Estado',
        type: 'chip',
        colorFn: (e) => (e.active ? 'primary' : 'warn'),
        format: (e) => (e.active ? 'Activo' : 'Inactivo'),
      },
      { key: 'actions', label: 'Acciones', type: 'actions' },
    ];
  }

  /** 🔹 Campos del formulario */
  getFormFields(): GenericFormField<DoctorCreateDto>[] {
    return [
      {
        key: 'specialtyId',
        label: 'Especialidad',
        type: 'select',
        required: true,
        serviceFn: async () =>
          await firstValueFrom(this.specialtyService.getAll()),
        mapLabel: 'name',
        mapValue: 'id',
      },
      {
        key: 'personId',
        label: 'Persona',
        type: 'select',
        required: true,
        serviceFn: async () =>
          await firstValueFrom(this.personService.getAll()),
        mapLabel: 'firstName',
        mapValue: 'id',
      },
      {
        key: 'emailDoctor',
        label: 'Correo Electrónico',
        type: 'text',
        required: true,
        placeholder: 'doctor@correo.com',
      },
      {
        key: 'image',
        label: 'Imagen (URL)',
        type: 'text',
        placeholder: 'https://...',
      },
      {
        key: 'active',
        label: 'Estado',
        type: 'select',
        required: true,
        options: [
          { label: 'Activo', value: true },
          { label: 'Inactivo', value: false },
        ],
      },
    ];
  }

  /** 🔹 Crear doctor */
  crear(): void {
    this.modalService.openForm<DoctorCreateDto>({
      title: 'Nuevo Doctor',
      fields: this.getFormFields(),
      onSave: (data) => this.doctorService.create(data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Editar doctor */
  editar(row: DoctorListDto): void {
    // Convertimos DoctorListDto a DoctorEditDto
    const editModel: DoctorEditDto = {
      id: row.id,
      specialtyId: 0, // 👈 puedes cambiar a valor real si el backend lo retorna
      personId: row.personId,
      active: row.active,
      image: row.image,
      emailDoctor: row.emailDoctor,
    };

    this.modalService.openForm<DoctorEditDto>({
      title: 'Editar Doctor',
      fields: this.getFormFields(),
      model: editModel,
      onSave: (data) => this.doctorService.update(row.id, data),
      onSuccess: () => this.loadData(),
    });
  }

  /** 🔹 Eliminar doctor */
  eliminar(id: number): void {
    this.modalService.confirmDelete(
      () => this.doctorService.delete(id),
      () => this.loadData()
    );
  }

  /** 🔹 Acciones de la tabla */
  onAction(event: { action: string; element: DoctorListDto }): void {
    switch (event.action) {
      case 'edit':
        this.editar(event.element);
        break;
      case 'delete':
        this.eliminar(event.element.id);
        break;
    }
  }

  /** 🔹 Columnas visibles */
  get columns(): string[] {
    return this.columnDefs.map((c) => c.key);
  }
}
