import { Injectable } from '@angular/core';
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  public setToken(token: string): void {
    sessionStorage.setItem('token', token);
  }

  public getToken(): string {
    return sessionStorage.getItem('token') ?? '';
  }

  private decodeJWT(): void {
      const helper = new JwtHelperService();
      const decode = helper.decodeToken(this.getToken());
  }

  public isTokenExpired(): boolean {
    if(this.getToken() == '')
      return false;
    const helper = new JwtHelperService();
    return helper.isTokenExpired(this.getToken());
  }
}
