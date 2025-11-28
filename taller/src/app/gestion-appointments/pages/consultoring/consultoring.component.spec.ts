import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultoringComponent } from './consultoring.component';

describe('ConsultoringComponent', () => {
  let component: ConsultoringComponent;
  let fixture: ComponentFixture<ConsultoringComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultoringComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsultoringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
