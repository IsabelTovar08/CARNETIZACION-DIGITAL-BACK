import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone:false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

   quickLinks = [
    { icon: 'meeting_room', label: 'Consultorios', route: '/appointments/consultorio' },
    { icon: 'local_hospital', label: 'Doctores', route: '/appointments/doctors' },
    { icon: 'group', label: 'Pacientes', route: '/appointments/person' },
    { icon: 'medical_services', label: 'Especialidades', route: '/appointments/especialidades' },
    { icon: 'event_note', label: 'Tipos de Citas', route: '/appointments/tCitas' },
    { icon: 'schedule', label: 'Horarios', route: '/appointments/horarios' },
    { icon: 'manage_accounts', label: 'Usuarios', route: '/appointments/users' },
  ];
}
