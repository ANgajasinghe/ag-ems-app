import {Component, OnInit} from '@angular/core';
import {Employee} from "../../../common/models/employee";
import {MaterialModule} from "../../../common/modules/material/material.module";
import {MatDialog} from "@angular/material/dialog";
import {EmployeeCreateDialogComponent} from "./employee-create-dialog/employee-create-dialog.component";
import {ApiBaseService} from "../../../common/services/api-base.service";
import {DatePipe, NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";


@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [MaterialModule, NgForOf, DatePipe, FormsModule],
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.scss'
})
export class EmployeeComponent implements OnInit {
  displayedColumns: string[] = ['fullName', 'joinDate', 'email', 'telephone', 'address', 'salary', 'action'];
  dataSource = [] as Employee[];

  constructor(public dialog: MatDialog, private apiBaseService: ApiBaseService) {
  }


  openDialog() {
    const dialogRef = this.dialog.open(EmployeeCreateDialogComponent, {
      width: '400px',
      data: {
        userRole: 'admin'
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.message === 'create') {
        this.loadEmployee();
      }
    });
  }

  ngOnInit(): void {
    this.loadEmployee();
  }

  onDeactivate(email: string) {
    this.apiBaseService.postApi<Employee[]>(['employee', 'lock'], {
        email: email
      }
    ).subscribe((res: any) => {
      this.loadEmployee();

    });
  }


  private loadEmployee() {
    this.apiBaseService.getApi<Employee[]>(['employee']).subscribe((res: any) => {
      this.dataSource = res;
    });
  }
}

