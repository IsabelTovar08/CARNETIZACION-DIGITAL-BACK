// 📅 type-citation.dto.ts

/** 🧾 DTO para creación de tipo de cita */
export interface TypeCitationCreateDto {
  /** Nombre del tipo de cita (3–50 caracteres) */
  name: string;
  /** Descripción del tipo de cita (5–50 caracteres) */
  description: string;
  /** Icono representativo (máx. 50 caracteres) */
  icon: string;
}

/** 🧍 DTO para listado de tipos de cita */
export interface TypeCitationListDto {
  /** Identificador único */
  id: number;
  /** Nombre del tipo de cita */
  name: string;
  /** Descripción del tipo de cita */
  description: string;
  /** Icono representativo */
  icon: string;
}

/** ✏ DTO para edición de tipo de cita */
export interface TypeCitationEditDto {
  /** Identificador único */
  id: number;
  /** Nombre del tipo de cita (3–50 caracteres) */
  name: string;
  /** Descripción opcional (5–50 caracteres si se proporciona) */
  description?: string;
  /** Icono opcional (máx. 50 caracteres) */
  icon?: string;
}
