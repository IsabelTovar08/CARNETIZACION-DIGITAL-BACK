export interface GenericFormField<T = any> {
  key: keyof T;                               // Propiedad del modelo
  label: string;                              // Etiqueta visible
  type: 'text' | 'number' | 'select' | 'file';// Tipo de campo
  required?: boolean;
  placeholder?: string;
  options?: { value: any; label: string }[];  // Opciones para selects
  serviceFn?: () => Promise<any[]>;           // Carga remota de selects
  mapLabel?: string;                          // Propiedad visible del select
  mapValue?: string;                          // Propiedad usada como value
  disabled?: boolean;
}
