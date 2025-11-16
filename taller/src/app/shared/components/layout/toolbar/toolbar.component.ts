import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auht.service';
import Swal from 'sweetalert2'; // 👈 Importa SweetAlert2

@Component({
  selector: 'app-toolbar',
  standalone:false,
  templateUrl: './toolbar.component.html',
  styleUrl: './toolbar.component.css'
})
export class ToolbarComponent {
// 🔹 Evento para que el padre (sidebar layout) pueda abrir/cerrar el menú
  @Output() menuToggle = new EventEmitter<void>();

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  onToggleMenu(): void {
    this.menuToggle.emit();
  }

  /** 🔹 Cierra sesión con confirmación */
  onLogout(): void {
    Swal.fire({
      title: '¿Cerrar sesión?',
      text: 'Tu sesión actual se cerrará y deberás iniciar nuevamente.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, cerrar sesión',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6'
    }).then(result => {
      if (result.isConfirmed) {
        // ✅ Ejecutar cierre de sesión
        this.authService.logout();
        localStorage.clear();

        Swal.fire({
          title: 'Sesión cerrada',
          text: 'Has salido correctamente.',
          icon: 'success',
          confirmButtonText: 'Aceptar',
          confirmButtonColor: '#3085d6'
        }).then(() => {
          // ✅ Redirigir al login
          this.router.navigate(['/auth/login']);
        });
      }
    });
  }
}
