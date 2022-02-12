import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-selected-workspace',
  templateUrl: './selected-workspace.component.html',
  styleUrls: ['./selected-workspace.component.scss']
})
export class SelectedWorkspaceComponent implements OnInit {

  constructor(public workspaceService: WorkspaceService, private router: Router) { }

  ngOnInit(): void {
  }

  navigateToWorkspaces() {
    this.router.navigate(["workspaces"]);
  }

}
