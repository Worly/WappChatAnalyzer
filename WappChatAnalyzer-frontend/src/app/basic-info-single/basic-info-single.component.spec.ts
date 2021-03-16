import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicInfoSingleComponent } from './basic-info-single.component';

describe('BasicInfoSingleComponent', () => {
  let component: BasicInfoSingleComponent;
  let fixture: ComponentFixture<BasicInfoSingleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BasicInfoSingleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BasicInfoSingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
