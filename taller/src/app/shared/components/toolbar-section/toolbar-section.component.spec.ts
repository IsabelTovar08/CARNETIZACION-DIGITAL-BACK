import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToolbarSectionComponent } from './toolbar-section.component';

describe('ToolbarSectionComponent', () => {
  let component: ToolbarSectionComponent;
  let fixture: ComponentFixture<ToolbarSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToolbarSectionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToolbarSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
