import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from '../shared/components/layout/main-layout/main-layout.component';
import { ConsultoringComponent } from './pages/consultoring/consultoring.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import { PersonComponent } from './pages/person/person.component';
import { SpecialtyComponent } from './pages/specialty/specialty.component';
import { TypeCitationComponent } from './pages/type-citation/type-citation.component';
import { SheduleComponent } from './pages/shedule/shedule.component';
import { UserComponent } from './pages/user/user.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

const routes: Routes = [

   {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', component: DashboardComponent },
      { path: 'consultorio', component: ConsultoringComponent },
      { path: 'doctors', component: DoctorsComponent },
      { path: 'person', component: PersonComponent },
      { path: 'especialidades', component: SpecialtyComponent },
      { path: 'tCitas', component: TypeCitationComponent },
      { path: 'horarios', component: SheduleComponent },
      { path: 'users', component: UserComponent },
      { path: 'users', component: UserComponent },
      { path: 'Dashboard', component: DashboardComponent },






      


    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionAppointmentsRoutingModule { }
