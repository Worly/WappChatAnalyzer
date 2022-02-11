import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { throwIfEmpty } from 'rxjs/operators';
import { DialogComponent } from 'src/app/components/dialog/dialog.component';
import { Workspace } from 'src/app/dtos/workspace';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-workspace-edit',
  templateUrl: './workspace-edit.component.html',
  styleUrls: ['./workspace-edit.component.css']
})
export class WorkspaceEditComponent implements OnInit {

  @ViewChild(DialogComponent) dialog: DialogComponent;

  @Input()
  workspace: Workspace;

  isNew: boolean = false;

  name: string;

  public validationErrors: {
    [key: string]: string
  } = {};

  public isLoading: boolean = false;

  constructor(private workspaceService: WorkspaceService, private router: Router) { }

  ngOnInit(): void {
  }

  startEdit(): void {
    this.isNew = false;
    this.reset();
    this.dialog.open();
  }

  startAddNew(): void {
    this.isNew = true;
    this.reset();
    this.dialog.open();
  }

  reset(): void {
    if (this.isNew)
      this.name = "";
    else
      this.name = this.workspace.name;

    this.validationErrors = {};
  }

  validate(): boolean {
    this.validationErrors = {};

    if (this.name == null || this.name == "")
      this.validationErrors["name"] = "Please enter a name";

    return Object.entries(this.validationErrors).length == 0;
  }

  save(): void {
    if (!this.validate())
      return;

    this.isLoading = true;

    let request;
    if (this.isNew)
      request = this.workspaceService.addNew(this.name);
    else
      request = this.workspaceService.edit(this.workspace.id, this.name);

    request.subscribe({
      next: w => {

        if (this.isNew) {
          this.workspaceService.selectWorkspace(w).subscribe({
            next: () => {
              this.isLoading = false;
              this.dialog.close();

              this.router.navigate(["home"])
            },
            error: () => {
              this.isLoading = false;
              this.dialog.close();
            }
          });
        }
        else {
          this.isLoading = false;
          this.dialog.close();
        }
      },
      error: e => {
        this.isLoading = false;
        this.validationErrors = e.error.errors;
      }
    });
  }
}
