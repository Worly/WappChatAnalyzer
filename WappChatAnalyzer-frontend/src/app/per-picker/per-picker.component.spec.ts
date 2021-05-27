import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerPickerComponent } from './per-picker.component';

describe('PerPickerComponent', () => {
  let component: PerPickerComponent;
  let fixture: ComponentFixture<PerPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PerPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
