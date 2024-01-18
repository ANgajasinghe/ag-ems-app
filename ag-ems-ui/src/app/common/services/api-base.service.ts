import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {catchError, map, Observable, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiBaseService {

  constructor(private httpClient:HttpClient,
              ) { }
  baseUrl = '/api/';
  getApi<T>(path: string[],param?: HttpParams): Observable<T>{
    const apiPath = `${this.baseUrl}${path.join('/')}`;
    return this.httpClient.get<T>(apiPath, {params: param})
      .pipe(
        map((response: any) => {
          this.showSuccess(response.message, true);
          return response
        }),
        catchError((err: any) => {
          this.showErrors(err.error, true);
          return throwError(err)
        }))
  }

  postApi<T>(path: string[], body: any): Observable<T> {
    const apiPath = `${this.baseUrl}${path.join('/')}`;
    return this.httpClient.post<T>(apiPath, body)
      .pipe(
        map((response: any) => {
          this.showSuccess(response.message, true);
          return response
        }),
        catchError((err: any) => {
          this.showErrors(err.error, true);
          return throwError(err)
        })
      )
  }

  putApi<T>(path: string[], body: any): Observable<T> {
    const apiPath = `${this.baseUrl}${path.join('/')}`;
    return this.httpClient.put<T>(apiPath, body)
      .pipe(
        map((response: any) => {
          this.showSuccess(response.message, true);
          return response
        }),
        catchError((err: any) => {
          this.showErrors(err.error, true);
          return throwError(err)
        }))
  }

  deleteApi<T>(path: string[]): Observable<T> {
    const apiPath = `${this.baseUrl}${path.join('/')}`;
    return this.httpClient.delete<T>(apiPath)
      .pipe(
        map((response: any) => {
          this.showSuccess(response.message, true);
          return response
        }),
        catchError((err: any) => {
          this.showErrors(err.error, true);
          return throwError(err)
        }))
  }

  private showErrors(error: any, isShowError: boolean) {
    if (isShowError) {
      if (error && error.message) {
        // this.toastr.error(error.message, 'Error');
      } else {
        // this.toastr.error('Something went wrong', 'Error');
      }
    }
  }

  showSuccess(message: string, isShowSuccess: boolean) {
    if (isShowSuccess) {
      // this.toastr.success(message, 'Success');
    }
  }
}
