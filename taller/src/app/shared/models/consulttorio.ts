// 🩺 consulting-room-list.dto.ts
export interface ConsultingRoomListDto {
  /** Identificador base (heredado de BaseModel) */
  id: number;
  /** Nombre de la sala */
  name: string;
  /** Número de la sala */
  roomNumber: number;
  /** Piso donde se encuentra */
  floor: number;
  /** Imagen opcional asociada */
  image?: string;
}
// 🏗 consulting-room-create.dto.ts
export interface ConsultingRoomCreateDto {
  /** Nombre de la sala (requerido, 2-100 caracteres) */
  name: string;
  /** Número de la sala (1-9999) */
  roomNumber: number;
  /** Piso donde se encuentra (0-100) */
  floor: number;
  /** Imagen opcional */
  image?: string;
}

// ✏ consulting-room-edit.dto.ts
export interface ConsultingRoomEditDto {
  /** Identificador base (heredado de BaseDto) */
  id: number;
  /** Nombre de la sala (requerido, 2-100 caracteres) */
  name: string;
  /** Número de la sala (1-9999) */
  roomNumber: number;
  /** Piso donde se encuentra (0-100) */
  floor: number;
  /** Imagen opcional */
  image?: string;
}
