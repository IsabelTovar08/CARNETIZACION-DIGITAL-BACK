import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from './components/table/generic-table/generic-table.component';
import { MaterialModule } from './material.module';
import { SidebarComponent } from './components/layout/sidebar/sidebar.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToolbarComponent } from './components/layout/toolbar/toolbar.component';
import { MainLayoutComponent } from './components/layout/main-layout/main-layout.component';
import { ToolbarSectionComponent } from './components/toolbar-section/toolbar-section.component';
import { GenericFormComponent } from './components/forms/generic-form/generic-form.component';

const sharedComponents =[

  GenericTableComponent,SidebarComponent,ToolbarComponent,MainLayoutComponent,ToolbarSectionComponent,GenericFormComponent
];

@NgModule({
  declarations: [...sharedComponents],
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    ReactiveFormsModule,

  ],
  exports: [...sharedComponents] // ← agrega esto también
})
export class SharedModule { }
