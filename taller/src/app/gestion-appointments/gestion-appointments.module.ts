import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionAppointmentsRoutingModule } from './gestion-appointments-routing.module';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { ConsultoringComponent } from './pages/consultoring/consultoring.component';
import { PersonComponent } from './pages/person/person.component';
import { SpecialtyComponent } from './pages/specialty/specialty.component';
import { DoctorsComponent } from './pages/doctors/doctors.component';
import { TypeCitationComponent } from './pages/type-citation/type-citation.component';
import { SheduleComponent } from './pages/shedule/shedule.component';
import { UserComponent } from './pages/user/user.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';


@NgModule({
  declarations: [ConsultoringComponent,
    PersonComponent,SpecialtyComponent
    ,DoctorsComponent
    ,TypeCitationComponent,
    SheduleComponent,UserComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    GestionAppointmentsRoutingModule,MaterialModule,SharedModule
  ]
})
export class GestionAppointmentsModule { }
