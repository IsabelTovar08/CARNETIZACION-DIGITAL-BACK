// 💊 specialty.dto.ts

/** 🧾 DTO para creación de especialidad */
export interface SpecialtyCreatedDto {
  /** Nombre de la especialidad */
  name: string;
  /** Descripción detallada de la especialidad */
  description: string;
}

/** 🧍 DTO para listar especialidades */
export interface SpecialtyListDto {
  /** Identificador único */
  id: number;
  /** Nombre de la especialidad */
  name: string;
  /** Descripción detallada */
  description: string;
}

/** ✏ DTO para edición de especialidad */
export interface SpecialtyEditDto {
  /** Identificador único */
  id: number;
  /** Nombre de la especialidad */
  name: string;
  /** Descripción detallada */
  description: string;
}
