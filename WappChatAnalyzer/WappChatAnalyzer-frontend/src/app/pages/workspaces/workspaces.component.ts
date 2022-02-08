import { Component } from '@angular/core';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-workspaces',
  templateUrl: './workspaces.component.html',
  styleUrls: ['./workspaces.component.css']
})
export class WorkspacesComponent {
  constructor(public workspaceService: WorkspaceService) { } 
}
