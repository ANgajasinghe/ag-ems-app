import { Component } from '@angular/core';
import {MaterialModule} from "../common/modules/material/material.module";
import {RouterLink, RouterOutlet} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {
  EmployeeCreateDialogComponent
} from "./components/employee/employee-create-dialog/employee-create-dialog.component";

@Component({
  selector: 'app-private',
  standalone: true,
  imports: [MaterialModule, RouterOutlet, RouterLink],
  templateUrl: './private.component.html',
  styleUrl: './private.component.scss'
})
export class PrivateComponent {

  constructor(public dialog: MatDialog) {
  }

  onProfileClick() {
    const dialogRef = this.dialog.open(EmployeeCreateDialogComponent, {
      width: '400px',
      data: {
        userRole: 'user'
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }


}
