import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environments';

/** DTO de entrada para login */
export interface LoginRequestDto {
  email: string;
  password: string;
}

/** DTO de respuesta del backend */
export interface LoginResponseDto {
  token: string;
  refreshToken?: string;
  expiresIn?: number;
  userName?: string;
  roles?: string[];
}

/**
 * Servicio de autenticación
 * Realiza las peticiones de login/logout al backend
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly baseUrl = `${environment.apiUrl}/v1/auth`;

  constructor(private readonly http: HttpClient) {}

  /**
   * 🔹 Llama al endpoint de login
   * @param credentials email y password del usuario
   * @returns Observable con el token o datos del usuario autenticado
   */
  login(credentials: LoginRequestDto): Observable<LoginResponseDto> {
    const url = `${this.baseUrl}/login`;

    return this.http.post<LoginResponseDto>(url, credentials).pipe(
      catchError(err => {
        console.error('Error en login:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * 🔹 Ejemplo de logout (opcional)
   */
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
  }
}
