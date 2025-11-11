import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

@Component({
  selector: 'app-main-layout',
  standalone:false,
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {
@ViewChild('sidenav') sidenav!: MatSidenav;

  toggleSidebar(): void {
    this.sidenav.toggle();
  }
}
