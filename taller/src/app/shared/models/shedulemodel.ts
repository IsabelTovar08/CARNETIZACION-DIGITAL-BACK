// 🗓️ shedule.dto.ts

/** 🧾 DTO para creación de horario */
export interface SheduleCreateDto {
  /** Id del tipo de cita (requerido) */
  typeCitationId: number;
  /** Id del doctor asignado (requerido) */
  doctorId: number;
  /** Id del consultorio asignado (requerido) */
  consultingRoomId: number;
  /** Número de citas (1–500) */
  numberCitation: number;
}

/** 🧍 DTO para listado de horarios */
// 🗓️ shedule.dto.ts

/** 🧍 DTO para listado de horarios */
export interface SheduleListDto {
  /** Identificador único */
  id: number;
  /** Nombre del tipo de cita */
  typeCitationName: string;
  /** Id del tipo de cita */
  typeCitationId: number;
  /** Nombre del doctor */
  nameDoctor: string;
  /** Id del doctor */
  doctorId: number;
  /** Nombre del consultorio */
  consultingRoomName: string;
  /** Id del consultorio */
  consultingRoomId: number;
  /** Número de citas configuradas */
  numberCitation: number;
  /** Número de sala */
  roomNumber: number;
}

/** ✏ DTO para edición de horario */
export interface SheduleEditDto {
  /** Identificador único */
  id: number;
  /** Id del tipo de cita (requerido) */
  typeCitationId: number;
  /** Id del doctor asignado (requerido) */
  doctorId: number;
  /** Id del consultorio asignado (requerido) */
  consultingRoomId: number;
  /** Número de citas (1–500) */
  numberCitation: number;
}
