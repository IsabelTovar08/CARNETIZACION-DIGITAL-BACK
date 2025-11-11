import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GenericFormField } from '../../../models/formsModel';

@Component({
  selector: 'app-generic-form',
  standalone:false,
  templateUrl: './generic-form.component.html',
  styleUrl: './generic-form.component.css'
})
export class GenericFormComponent<T> implements OnInit {
  form!: FormGroup;
  fields: GenericFormField<T>[] = [];
  title = '';
  action = 'Guardar';

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<GenericFormComponent<T>>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      title: string;
      action: string;
      fields: GenericFormField<T>[];
      model?: Partial<T>;
    }
  ) {}

  async ngOnInit(): Promise<void> {
    this.title = this.data.title;
    this.action = this.data.action;
    this.fields = this.data.fields;

    const group: Record<string, any> = {};

    for (const field of this.fields) {
      if (field.type === 'select' && field.serviceFn) {
        try {
          const items = await field.serviceFn();
          field.options = items.map((x: any) => ({
            label: field.mapLabel ? x[field.mapLabel] : x.name,
            value: field.mapValue ? x[field.mapValue] : x.id,
          }));
        } catch {
          field.options = [];
        }
      }

      group[this.asKey(field.key)] = [
        { value: this.data.model?.[field.key] ?? '', disabled: field.disabled },
        field.required ? Validators.required : [],
      ];
    }

    this.form = this.fb.group(group);
  }

  /** 🔹 Convierte key genérica a string */
  asKey(key: keyof T): string {
    return String(key);
  }

  /** 🔹 Manejo de archivos sin errores de EventTarget */
  onFileChange(event: Event, key: keyof T): void {
    const input = event.target as HTMLInputElement | null;
    const file = input?.files?.[0] ?? null;
    this.form.get(this.asKey(key))?.setValue(file);
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.dialogRef.close(this.form.getRawValue());
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}