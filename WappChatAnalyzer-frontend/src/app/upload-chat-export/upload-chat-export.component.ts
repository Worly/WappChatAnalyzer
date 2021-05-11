import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MessagesService } from '../services/messages.service';

@Component({
  selector: 'app-upload-chat-export',
  templateUrl: './upload-chat-export.component.html',
  styleUrls: ['./upload-chat-export.component.css']
})
export class UploadChatExportComponent implements OnInit {

  @Output()
  onUploaded: EventEmitter<void> = new EventEmitter();

  fileToUpload: File = null;

  isLoading: boolean = false;

  message: string = "";

  constructor(private messagesService: MessagesService) { }

  ngOnInit(): void {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  upload() {
    if (this.fileToUpload != null) {
      this.message = "";
      this.isLoading = true;
      this.messagesService.uploadChatExport(this.fileToUpload).subscribe((response: Number) => {
        this.isLoading = false;
        this.fileToUpload = null;
        this.message = "Imported " + response.toFixed(0) + " messages";
        this.onUploaded.next();
      },
      (error) => {
        this.isLoading = false;
        this.message = error;
      });
    }
  }
}
