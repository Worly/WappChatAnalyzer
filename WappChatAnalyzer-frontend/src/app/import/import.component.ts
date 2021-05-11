import { Component, OnInit } from '@angular/core';
import { ImportHistory } from '../dtos/importHistory';
import { MessagesService } from '../services/messages.service';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.css']
})
export class ImportComponent implements OnInit {

  imports: ImportHistory[] = [];

  constructor(private messagesService: MessagesService) { }

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.messagesService.getImportHistory().subscribe(response => {
      this.imports = response;
    });
  }

  onImportedNew() {
    this.load();
  }

}
