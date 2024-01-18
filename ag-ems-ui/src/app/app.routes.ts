import {Routes} from '@angular/router';
import {PrivateComponent} from "./private/private.component";
import {LoginComponent} from "./public/login/login.component";
import {EmployeeComponent} from "./private/components/employee/employee.component";
import {SalaryComponent} from "./private/components/salary/salary.component";
import {authGuard} from "./common/guard/auth.guard";

export const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {
    path: 'private', component: PrivateComponent, canActivate:[authGuard] ,children: [
      {path: 'employee', component: EmployeeComponent},
      {path: 'salary', component: SalaryComponent},
    ]
  },
];
