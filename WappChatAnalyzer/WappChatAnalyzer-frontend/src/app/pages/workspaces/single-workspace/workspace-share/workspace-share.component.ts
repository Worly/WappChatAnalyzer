import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonComponent } from 'src/app/components/button/button.component';
import { DialogComponent } from 'src/app/components/dialog/dialog.component';
import { Workspace } from 'src/app/dtos/workspace';
import { WorkspaceShare } from 'src/app/dtos/workspaceShare';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-workspace-share',
  templateUrl: './workspace-share.component.html',
  styleUrls: ['./workspace-share.component.css']
})
export class WorkspaceShareComponent implements OnInit {

  @ViewChild(DialogComponent) dialog: DialogComponent;

  @Input() workspace: Workspace;

  isLoading: boolean = false;
  isLoadingShare: boolean = false;
  shares: WorkspaceShare[] = [];

  usersEmail: string = "";

  public validationErrors: {
    [key: string]: string
  } = {};

  constructor(
    private workspaceService: WorkspaceService,
  ) { }

  ngOnInit(): void {
  }

  startShare(): void {
    this.dialog.open();
    this.load();
  }

  load() {
    this.isLoading = true;
    this.workspaceService.getWorkspaceShares(this.workspace.id).subscribe(s => {
      this.shares = s;
      this.isLoading = false;
    });
  }

  validate(): boolean {
    this.validationErrors = {};

    if (this.usersEmail == null || this.usersEmail == "")
      this.validationErrors["usersEmail"] = "Please enter an email";
    else if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.usersEmail))
      this.validationErrors["usersEmail"] = "Email address is not valid";

    return Object.entries(this.validationErrors).length == 0;
  }

  share() {
    if (!this.validate())
      return;

    this.isLoadingShare = true;
    this.workspaceService.shareWorkspace(this.workspace.id, this.usersEmail).subscribe(s => {
      this.shares = s;
      this.usersEmail = "";
      this.isLoadingShare = false;
    });
  }

  unshare(button: ButtonComponent, share: WorkspaceShare) {
    button.isLoading = true;
    button.disabled = true;
    this.workspaceService.unshareWorkspace(this.workspace.id, share.sharedUserEmail).subscribe(s => {
      this.shares = s;
    });
  }
}
