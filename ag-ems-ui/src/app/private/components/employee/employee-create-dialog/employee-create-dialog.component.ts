import {Component, inject, Inject, OnInit} from '@angular/core';
import {MaterialModule} from "../../../../common/modules/material/material.module";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgIf} from "@angular/common";
import {ApiBaseService} from "../../../../common/services/api-base.service";
import {Employee} from "../../../../common/models/employee";

@Component({
  selector: 'app-employee-create-dialog',
  standalone: true,
  imports: [MaterialModule, ReactiveFormsModule, NgIf],
  templateUrl: './employee-create-dialog.component.html',
  styleUrl: './employee-create-dialog.component.scss'
})
export class EmployeeCreateDialogComponent implements OnInit {
  employeeForm!: FormGroup

  constructor(@Inject(MAT_DIALOG_DATA) public data: { userRole: string },
              private apiBaseService: ApiBaseService,
              public dialogRef: MatDialogRef<EmployeeCreateDialogComponent>) {
  }


  ngOnInit(): void {
    this.initForm()
  }

  private initForm(): void {
    this.employeeForm = new FormGroup({
      fullName: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, Validators.email]),
      telephone: new FormControl(null, [Validators.required]),
      address: new FormControl(null, [Validators.required]),
      salary: new FormControl(null, [Validators.required]),
      joinDate: new FormControl(null, [Validators.required]),
    })
  }


  onSubmit() {
    if (this.employeeForm.valid) {
      const emp = this.employeeForm.value as Employee
      console.log(emp);
      this.apiBaseService.postApi(['employee'], emp).subscribe((res: any) => {
        console.log(res);
        this.dialogRef.close({message: 'create'});
      });
    } else{
      Object.values(this.employeeForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({onlySelf: true})
        }
      })
    }

  }
}
