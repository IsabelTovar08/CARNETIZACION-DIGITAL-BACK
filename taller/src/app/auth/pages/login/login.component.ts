import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../shared/services/auht.service';
import Swal from 'sweetalert2'; // ✅ Importa SweetAlert2
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone:false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

 loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router // ✅ Router para redirigir
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);

        // ✅ Muestra alerta de éxito
        Swal.fire({
          title: '¡Login exitoso!',
          text: 'Bienvenido nuevamente 👋',
          icon: 'success',
          confirmButtonText: 'Continuar',
          confirmButtonColor: '#3085d6'
        }).then(() => {
          // ✅ Redirige al módulo de citas
          this.router.navigate(['/appointments']);
        });
      },
      error: (err) => {
        console.error('Error en login:', err);

        // ⚠️ Muestra alerta de error
        Swal.fire({
          title: 'Error de autenticación',
          text: err.error?.message || 'Credenciales inválidas',
          icon: 'error',
          confirmButtonText: 'Reintentar'
        });
      }
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}
