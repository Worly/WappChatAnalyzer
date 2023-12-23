import { Component, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DialogComponent } from 'src/app/components/dialog/dialog.component';
import { Workspace } from 'src/app/dtos/workspace';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-single-workspace',
  templateUrl: './single-workspace.component.html',
  styleUrls: ['./single-workspace.component.css']
})
export class SingleWorkspaceComponent implements OnInit {

  @ViewChild("deleteDialog") deleteDialog: DialogComponent;

  @Input() public workspace: Workspace;
  @Input() public showControls: boolean = true;

  public deleteName: string;
  public isDeleting: boolean;
  
  constructor(private workspaceService: WorkspaceService, private router: Router) { 
    
  }

  ngOnInit(): void {
  }

  selectWorkspace() {
    this.workspaceService.selectWorkspace(this.workspace).subscribe(o => this.router.navigate(["events"]));
  }

  openDeleteDialog() {
    this.deleteName = "";
    this.deleteDialog.open();
  }

  deleteWorkspace() {
    this.isDeleting = true;
    this.workspaceService.deleteWorkspace(this.workspace).subscribe(() => {
      this.isDeleting = false;
      this.deleteDialog.close();
    });
  }
}
