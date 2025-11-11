import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { GenericFormField } from '../models/formsModel';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { GenericFormComponent } from '../components/forms/generic-form/generic-form.component';

interface OpenFormOptions<T> {
  title: string;
  fields: GenericFormField<T>[];
  model?: Partial<T>;
  onSave: (data: T) => Observable<any>; // acción que devuelve observable
  onSuccess?: () => void;               // callback opcional
}

@Injectable({
  providedIn: 'root'
})
export class GenericModalService {
  constructor(private dialog: MatDialog) {}

  /** 🔹 Abre un modal genérico con formulario */
  openForm<T>(opts: OpenFormOptions<T>): void {
    const dialogRef = this.dialog.open(GenericFormComponent<T>, {
      width: '500px',
      data: {
        title: opts.title,
        action: opts.model ? 'Guardar Cambios' : 'Crear',
        fields: opts.fields,
        model: opts.model
      }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      opts.onSave(result)
        .pipe(
          tap(() =>
            Swal.fire({
              icon: 'success',
              title: 'Operación exitosa',
              timer: 1500,
              showConfirmButton: false
            })
          )
        )
        .subscribe({
          next: () => opts.onSuccess?.(),
          error: () =>
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'No se pudo completar la operación'
            })
        });
    });
  }

  /** 🔹 Confirmación genérica antes de eliminar */
  confirmDelete(action: () => Observable<any>, refresh?: () => void): void {
    Swal.fire({
      title: '¿Eliminar registro?',
      text: 'Esta acción no se puede deshacer',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonText: 'Cancelar',
      confirmButtonText: 'Eliminar'
    }).then((res) => {
      if (res.isConfirmed) {
        action().subscribe({
          next: () => {
            Swal.fire('Eliminado', '', 'success');
            refresh?.();
          },
          error: () => Swal.fire('Error', 'No se pudo eliminar', 'error')
        });
      }
    });
  }
}