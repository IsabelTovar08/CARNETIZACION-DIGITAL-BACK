// 👤 person.dto.ts

/** 🧾 DTO para creación de persona */
export interface PersonCreated {
  /** Nombre */
  firstName: string;
  /** Apellido */
  lastName: string;
  /** Identificación */
  identification: string;
  /** Teléfono */
  phoneNumber: string;
  /** Dirección */
  address: string;
}

/** 🧍 DTO para selección de persona (por ejemplo en listas o combos) */
export interface PersonSelectDto {
  /** Identificador único */
  id: number;
  /** Nombre */
  firstName: string;
  /** Apellido */
  lastName: string;
  /** Identificación */
  identification: string;
  /** Teléfono */
  phoneNumber: string;
  /** Dirección */
  address: string;
}

/** ✏ DTO para edición de persona */
export interface PersonDto {
  /** Identificador único */
  id: number;
  /** Nombre */
  firstName: string;
  /** Apellido */
  lastName: string;
  /** Identificación */
  identification: string;
  /** Teléfono */
  phoneNumber: string;
  /** Dirección */
  address: string;
}
