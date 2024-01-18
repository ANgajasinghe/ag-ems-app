import {Component, inject, OnInit} from '@angular/core';
import {MaterialModule} from "../../../common/modules/material/material.module";
import {NgIf} from "@angular/common";
import {Salary} from "../../../common/models/salary";
import {MatDialog} from "@angular/material/dialog";
import {RecordSalaryDialogComponent} from "./record-salary-dialog/record-salary-dialog.component";
import {ApiBaseService} from "../../../common/services/api-base.service";
import {Employee} from "../../../common/models/employee";




@Component({
  selector: 'app-salary',
  standalone: true,
  imports: [
    MaterialModule,
    NgIf
  ],
  templateUrl: './salary.component.html',
  styleUrl: './salary.component.scss'
})
export class SalaryComponent implements OnInit{
  displayedColumns: string[] = ['fullName', 'year', 'month', 'amount', 'action' ];
  dataSource = [] as Salary[];
  isEmployee = false;

  private dialog = inject(MatDialog);
  private apiBaseService = inject(ApiBaseService);

  openDialog() {
    const dialogRef = this.dialog.open(RecordSalaryDialogComponent, {
      width: '400px',
      data: {
        userRole: 'admin'
      },
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.message === 'create'){
        this.apiBaseService.getApi<Salary[]>(['salary']).subscribe((res: any) => {
          this.dataSource = res;
        });
      }
    });
  }

  ngOnInit(): void {
    this.apiBaseService.getApi<Salary[]>(['salary']).subscribe((res: any) => {
      this.dataSource = res;
    });
  }
}
