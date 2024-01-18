import {Component, OnInit} from '@angular/core';
import {MaterialModule} from "../../common/modules/material/material.module";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {ApiBaseService} from "../../common/services/api-base.service";
import {TokenService} from "../../common/services/token.service";
import {TokenResponse} from "../../common/models/employee";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{

  public loginForm: FormGroup = new FormGroup({})
  hide = true;

  constructor(private router: Router, private apiBaseService: ApiBaseService, private storageService: TokenService) {
  }
  ngOnInit(): void {
      this.loginForm = new FormGroup({
        email: new FormControl(null, [Validators.required, Validators.email]),
        password: new FormControl(null, [Validators.required])
      })
  }

  getErrorMessage() {
    if (this.loginForm.controls['email'].hasError('required')) {
      return 'You must enter a value';
    }
    return this.loginForm.controls['email'].hasError('email') ? 'Not a valid email' : '';
  }


  onLoginClicked() {
    console.log(this.loginForm.value)
    if (this.loginForm.valid) {
      this.apiBaseService.postApi<TokenResponse>(['auth', 'login'], this.loginForm.value)
        .subscribe({
          next: data => {
            console.log(data);
            this.storageService.setToken(data.accessToken);
            this.router.navigate(['/private'])
          },
          error: error => {
            console.log(error);
          }
        });

    }


  }
}
