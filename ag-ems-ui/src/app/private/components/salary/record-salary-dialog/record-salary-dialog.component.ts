import {Component, inject, OnInit} from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {MatDatepicker, MatDatepickerModule} from "@angular/material/datepicker";
import {MatDialogClose, MatDialogContent, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ApiBaseService} from "../../../../common/services/api-base.service";
import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment'
import {MaterialModule} from "../../../../common/modules/material/material.module";
import {NgIf} from "@angular/common";
import {Dropdown, Employee} from "../../../../common/models/employee";
const moment = _rollupMoment || _moment;
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};


@Component({
  selector: 'app-record-salary-dialog',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDatepickerModule,
    MatDialogContent,
    ReactiveFormsModule,
    MatDialogClose,
    MaterialModule,
    NgIf
  ],
  templateUrl: './record-salary-dialog.component.html',
  styleUrl: './record-salary-dialog.component.scss'
})
export class RecordSalaryDialogComponent implements OnInit{
  salaryForm!: FormGroup;
  date = new FormControl(moment());
  empDropdown : Dropdown[] = []
  private apiBaseService = inject(ApiBaseService);

  constructor(private dialogRef: MatDialogRef<RecordSalaryDialogComponent>) {

  }


  ngOnInit(): void {
    this.apiBaseService.getApi(['dropdown', 'employee']).subscribe((res: any) => {
      this.empDropdown = res;
    });
    this.formGroupInit();
  }

  private formGroupInit() {
    this.salaryForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [Validators.required]),
      paidDate: new FormControl(null, [Validators.required])
    })
  }

  setMonthAndYear(normalizedMonthAndYear: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date?.value ?? moment();
    ctrlValue.month(normalizedMonthAndYear.month());
    ctrlValue.year(normalizedMonthAndYear.year());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  onSubmit() {
    if (this.salaryForm.valid) {
        console.log(this.salaryForm.value);
        this.apiBaseService.postApi(['salary'], this.salaryForm.value).subscribe((res: any) => {
          console.log(res);
          this.dialogRef.close({message: 'create'})
        });
    } else{
      Object.values(this.salaryForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({onlySelf: true})
        }
      })
    }

  }
}
