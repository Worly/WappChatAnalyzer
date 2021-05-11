import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadChatExportComponent } from './upload-chat-export.component';

describe('UploadChatExportComponent', () => {
  let component: UploadChatExportComponent;
  let fixture: ComponentFixture<UploadChatExportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadChatExportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadChatExportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
