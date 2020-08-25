import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Inject, Injectable } from '@angular/core';
import { AppConfigs } from '../app.config';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { NotifierTypeConst } from '../utils/enums/notifier.enum';
import { environment } from '../../environments/environment';
import { JwtHelper } from '../utils/helpers/jwl.helper';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private notifier: NotifierService,
    private router: Router
  ) { }

  public ValidChecksumClient(token: string) {
    const checksumClient = window.localStorage.getItem(AppConfigs.ChecksumClient);
    if (token && token.length > 0) {
      const decode = JwtHelper.decodeToken(token);
      if (decode) {
        const unique_name = decode.unique_name.toLowerCase();
        if (unique_name && unique_name.length > 0 && checksumClient && unique_name === checksumClient.toLowerCase()) {
          return true;
        }
      }
    }
    return false;
  }

  public DoPost(route, model = null, params = null): Observable<any> {
    const authorizationKey = AppConfigs.Authorization;
    const token = window.localStorage.getItem(authorizationKey);
    if (token == null || token.length <= 0) {
      this.doRedirectLogin();
      return new Observable<any>();
    }
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: token
      })
    };
    if (params != null) {
      route = route + this.buildQueryStrings(params);
    }
    const httpRequest = model != null
      ? this.http.post<any>(route, JSON.stringify(model), httpOptions)
      : this.http.post<any>(route, null, httpOptions);
    return httpRequest.pipe(
      map(res => {
        if (res != null && res.RefreshToken && res.RefreshToken === true) {
          // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
          if (this.ValidChecksumClient(res.Token)) {
            window.localStorage.setItem(AppConfigs.Authorization, res.Token);
            res.Token = '';
          } else {
            this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
            setTimeout(() => {
              window.localStorage.removeItem(AppConfigs.Authorization);
            }, 2000);
          }
        }
        window.localStorage.removeItem(AppConfigs.RetryCount401);
        return res;
      }),
      catchError((error) => this.handleError(error))
    );
  }

  public DoPostFile(route, fileToUpload: File): Observable<any> {
    const authorizationKey = AppConfigs.Authorization;
    const token = window.localStorage.getItem(authorizationKey);
    const formData: FormData = new FormData();
    formData.append(fileToUpload.name, fileToUpload);
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token
      })
    };
    return this.http
      .post<any>(route, formData, httpOptions)
      .pipe(
        map(res => {
          if (res != null && res.RefreshToken && res.RefreshToken === true) {
            // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
            if (this.ValidChecksumClient(res.Token)) {
              window.localStorage.setItem(AppConfigs.Authorization, res.Token);
              res.Token = '';
            } else {
              this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
              setTimeout(() => {
                window.localStorage.removeItem(AppConfigs.Authorization);
              }, 2000);
            }
          }
          window.localStorage.removeItem(AppConfigs.RetryCount401);
          return res;
        }),
        catchError((error) => this.handleError(error))
      );
  }

  public DoPostFileMulti(route, fileToUpload: FileList): Observable<any> {
    const authorizationKey = AppConfigs.Authorization;
    const token = window.localStorage.getItem(authorizationKey);
    const formData: FormData = new FormData();
    for (let i = 0; i < fileToUpload.length; i++) {
      formData.append(fileToUpload[i].name, fileToUpload[i]);
    }
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token
      })
    };
    return this.http
      .post<any>(route, formData, httpOptions)
      .pipe(
        map(res => {
          if (res != null && res.RefreshToken && res.RefreshToken === true) {
            // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
            if (this.ValidChecksumClient(res.Token)) {
              window.localStorage.setItem(AppConfigs.Authorization, res.Token);
              res.Token = '';
            } else {
              this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
              setTimeout(() => {
                window.localStorage.removeItem(AppConfigs.Authorization);
              }, 2000);
            }
          }
          window.localStorage.removeItem(AppConfigs.RetryCount401);
          return res;
        }),
        catchError((error) => this.handleError(error))
      );
  }

  public DoGet(route, params = null): Observable<any> {
    const authorizationKey = AppConfigs.Authorization;
    const token = window.localStorage.getItem(authorizationKey);
    if (token == null || token.length <= 0) {
      this.doRedirectLogin();
      return new Observable<any>();
    }
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: token
      })
    };
    if (params != null) {
      route = route + this.buildQueryStrings(params);
    }
    return this.http.get<any>(route, httpOptions).pipe(
      map(res => {
        if (res != null && res.RefreshToken && res.RefreshToken === true) {
          // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
          if (this.ValidChecksumClient(res.Token)) {
            window.localStorage.setItem(AppConfigs.Authorization, res.Token);
            res.Token = '';
          } else {
            this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
            setTimeout(() => {
              window.localStorage.removeItem(AppConfigs.Authorization);
            }, 2000);
          }
        }
        window.localStorage.removeItem(AppConfigs.RetryCount401);
        return res;
      }),
      catchError((error) => this.handleError(error))
    );
  }

  public handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      const err =
        `Backend returned code ${error.status}, ` + `body was: ${error.message}`;
      console.error(err);
    }
    if (error.status === 401) { this.doRedirectLogin(); }
    if (error.status === 405) { this.doRedirect405(); }
    // return an observable with a user-facing error message
    return throwError('Something bad happened; please try again later.');
  }

  public doRedirectLogin(isLogout = false) {
    window.localStorage.removeItem(AppConfigs.Authorization);
    const isNotSSO = window.localStorage.getItem(AppConfigs.IsNotSSO);
    if (environment.loginSSO === true && !isLogout && (isNotSSO === null || isNotSSO === '0')) {
      window.localStorage.setItem(AppConfigs.ReturnUrl, this.router.url);
      window.location.href = environment.loginUrl + '?returnUrl=' + environment.APP_CLIENT_URL;
      return;
    }

    this.router.navigate(['/login'], {
      queryParams: { returnUrl: this.router.url }
    });
  }

  public doRedirect405() {
    this.router.navigate(['/405']).then(e => {
      if (e) {
        console.log('Navigation is successful!');
      } else {
        console.log('Navigation has failed!');
      }
    });
  }

  private buildQueryParams(source: Object): HttpParams {
    let target: HttpParams = new HttpParams();
    Object.keys(source).forEach((key: string) => {
      const value: string | number | boolean | Date = source[key];
      if ((typeof value !== 'undefined') && (value !== null)) {
        target = target.append(key, value.toString());
      }
    });
    return target;
  }

  private buildQueryStrings(source: Object) {
    let target = '';
    let isFirst = true;
    Object.keys(source).forEach((key: string) => {
      const value: string | number | boolean | Date = source[key];
      if ((typeof value !== 'undefined') && (value !== null)) {
        target += (isFirst ? '?' : '&') + key + '=' + value.toString();
      }
      isFirst = false;
    });
    return target;
  }
}
