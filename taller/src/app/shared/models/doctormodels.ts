// 🩺 doctor.dto.ts

/** 🧾 DTO para crear doctor */
export interface DoctorCreateDto {
  /** Id de la especialidad (requerido) */
  specialtyId: number;
  /** Id de la persona asociada */
  personId: number;
  /** Estado activo/inactivo */
  active: boolean;
  /** Imagen del doctor */
  image: string;
  /** Correo del doctor */
  emailDoctor: string;
}

/** 🧍 DTO para listar doctores */
export interface DoctorListDto {
  /** Identificador único */
  id: number;
  /** Nombre de la especialidad */
  specialtyName: string;
  /** Estado activo/inactivo */
  active: boolean;
  /** Imagen del doctor */
  image: string;
  /** Nombre completo */
  fullName?: string;
  /** Correo del doctor */
  emailDoctor: string;
  /** Id de la persona asociada */
  personId: number;
}

/** ✏ DTO para editar doctor */
export interface DoctorEditDto {
  /** Identificador único */
  id: number;
  /** Id de la especialidad (requerido) */
  specialtyId: number;
  /** Id de la persona asociada */
  personId: number;
  /** Estado activo/inactivo */
  active: boolean;
  /** Imagen del doctor */
  image: string;
  /** Correo del doctor */
  emailDoctor: string;
}
