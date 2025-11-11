import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  standalone:false,
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  // 🔹 Menú principal: puedes personalizar fácilmente
menuItems = [
  { icon: 'dashboard', label: 'Dashboard', route: '/appointments/Dashboard' },
  { icon: 'meeting_room', label: 'Consultorios', route: '/appointments/consultorio' },
  { icon: 'group', label: 'Personas', route: '/appointments/person' },
  { icon: 'medical_services', label: 'Especialidades', route: '/appointments/especialidades' },
  { icon: 'local_hospital', label: 'Doctores', route: '/appointments/doctors' },
  { icon: 'event_note', label: 'Tipos de Citas', route: '/appointments/tCitas' },
  { icon: 'schedule', label: 'Horarios', route: '/appointments/horarios' },
  { icon: 'manage_accounts', label: 'Usuarios', route: '/appointments/users' },
];


  // 🔹 Puedes manejar el estado del sidenav si lo integras con layout responsive
  isOpen = true;

  toggle() {
    this.isOpen = !this.isOpen;
  }
}
