// 👤 user.dto.ts

/** 🧾 DTO para creación de usuario */
export interface UserCreate {
  /** Nombre del usuario */
  name: string;
  /** Correo electrónico */
  email: string;
  /** Contraseña opcional */
  password?: string;
  /** Id de la persona asociada (opcional) */
  personId?: number;
}

/** 🧍 DTO para selección de usuario (por ejemplo en combos o listas) */
export interface UserSelectDto {
  /** Identificador único */
  id: number;
  /** Nombre del usuario */
  name: string;
  /** Correo electrónico */
  email: string;
}

/** ✏ DTO para edición de usuario */
export interface UserEditDto {
  /** Identificador único */
  id: number;
  /** Nombre del usuario */
  name: string;
  /** Correo electrónico */
  email: string;
  /** Contraseña opcional */
  password?: string;
  /** Id de la persona asociada (opcional) */
  personId?: number;
}
