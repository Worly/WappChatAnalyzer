import { Component, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Workspace } from 'src/app/dtos/workspace';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-single-workspace',
  templateUrl: './single-workspace.component.html',
  styleUrls: ['./single-workspace.component.css']
})
export class SingleWorkspaceComponent implements OnInit {

  @Input()
  public workspace: Workspace;

  constructor(private workspaceService: WorkspaceService, private router: Router) { 

  }

  ngOnInit(): void {
  }

  selectWorkspace() {
    this.workspaceService.selectWorkspace(this.workspace).subscribe(o => this.router.navigate(["home"]));
  }
}
