import { Injectable, Input } from '@angular/core';
import { Observable, throwError, Subject } from 'rxjs';
import { ResponseData } from '../model/ResponseData';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { NotifierService } from 'angular-notifier';
import { SystemUserLoginModel } from '../model/SystemUserLoginModel';
import { map, catchError } from 'rxjs/operators';
import { AppConfigs } from '../app.config';
import { NotifierTypeConst } from '../utils/enums/notifier.enum';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from './http.service';
import { JwtHelper } from '../utils/helpers/jwl.helper';

@Injectable({
    providedIn: 'root'
})

export class AuthenService {
    @Input() loading = false;
    constructor(private http: HttpClient,
        private httpService: HttpService,
        private notifier: NotifierService,
        private activatedRoute: ActivatedRoute,
        private router: Router) {
    }

    public static get IsAuthenticated(): boolean {
        const token = window.localStorage.getItem(AppConfigs.Authorization);
        if (token && token.length > 0) {
            return true;
        }
        return false;
    }

    public static getRoles() {
        const token = window.localStorage.getItem(AppConfigs.Authorization);
        if (token && token.length > 0) {
            const decode = JwtHelper.decodeToken(token);
            if (decode && decode.role && decode.role.length > 0) {
                return decode.role;
            }
        }
        return '';
    }

    public static getGroups() {
        const token = window.localStorage.getItem(AppConfigs.Authorization);
        if (token && token.length > 0) {
            const decode = JwtHelper.decodeToken(token);
            if (decode && decode.groups_role && decode.groups_role.length > 0) {
                return decode.groups_role;
            }
        }
        return '';
    }

    public static getUsername() {
        const token = window.localStorage.getItem(AppConfigs.Authorization);
        if (token && token.length > 0) {
            const decode = JwtHelper.decodeToken(token);
            if (decode && decode.unique_name && decode.unique_name.length > 0) {
                return decode.unique_name;
            }
        }
        return '';
    }

    public static getDisplayName() {
        const token = window.localStorage.getItem(AppConfigs.Authorization);
        if (token && token.length > 0) {
            const decode = JwtHelper.decodeToken(token);
            if (decode) {
                if (decode.family_name && decode.family_name.length > 0) {
                    return decode.family_name;
                } else if (decode.unique_name && decode.unique_name.length > 0) {
                    return decode.unique_name;
                }
            }
        }
        return '';
    }

    public static getUserId() {
      const token = window.localStorage.getItem(AppConfigs.Authorization);
      if (token && token.length > 0) {
          const decode = JwtHelper.decodeToken(token);
          if (decode && decode.certserialnumber && decode.certserialnumber.length > 0) {
              return decode.certserialnumber;
          }
      }
      return '';
    }
    public tryReload() {
        this.loading = !this.loading;
        const authorizationKey = AppConfigs.Authorization;
        const token = window.localStorage.getItem(authorizationKey);
        const count = window.localStorage.getItem(AppConfigs.RetryCount401);
        if (token != null && token.length > 0 && count == null) {
            window.localStorage.setItem(AppConfigs.RetryCount401, '1');
            let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];
            if (returnUrl === undefined) { returnUrl = '/'; }
            this.router.navigateByUrl(returnUrl);
            this.loading = !this.loading;
        } else {
            window.localStorage.removeItem(AppConfigs.RetryCount401);
            window.localStorage.removeItem(authorizationKey);
            this.loading = false;
        }
    }

    public doLogin(systemUserLoginModel: SystemUserLoginModel): Observable<ResponseData> {
        this.loading = !this.loading;
        window.localStorage.setItem(AppConfigs.IsNotSSO, '1');
        // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
        const checksumClient = systemUserLoginModel.UserName;
        if (checksumClient != null && checksumClient.length > 0) {
            window.localStorage.setItem(AppConfigs.ChecksumClient, checksumClient);
        }
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        const route = environment.APP_API_URL + '/Account/DoLogin';
        return this.http.post<ResponseData>(route, JSON.stringify(systemUserLoginModel), httpOptions)
            .pipe(
                map(res => {
                    // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
                    if (res != null && res.Success === true && res.Token.length > 0) {
                        if (this.httpService.ValidChecksumClient(res.Token)) {
                            window.localStorage.setItem(AppConfigs.Authorization, res.Token);
                            this.notifier.notify(NotifierTypeConst.Success, 'Login success!');
                            setTimeout(() => {
                                let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];
                                if (returnUrl === undefined) {
                                    returnUrl = '/';
                                }
                                this.router.navigateByUrl(returnUrl);
                                this.loading = !this.loading;
                            }, 2000);
                        } else {
                            this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
                        }
                    } else {
                        if (res != null && res.Message.length > 0) {
                            this.notifier.notify(NotifierTypeConst.Error, res.Message);
                        }
                    }
                    this.loading = !this.loading;
                    return res;
                }),
                catchError(() => this.httpService.handleError)
            );
    }

    public doLoginSSO(data: string): Observable<ResponseData> {
        this.loading = !this.loading;
        window.localStorage.setItem(AppConfigs.IsNotSSO, '0');
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        const params = { Token: data };
        const route = environment.APP_API_URL + '/Account/LogOnSSO';
        return this.http.post<ResponseData>(route, JSON.stringify(params), httpOptions)
            .pipe(
                map(res => {
                    if (res != null && res.Success === true && res.Token.length > 0) {
                        // xử lý lưu thêm checksumClient = username để phòng server lỗi trả ra token của user khác
                        if (res.Data != null && res.Data.toString().length > 0) {
                            window.localStorage.setItem(AppConfigs.ChecksumClient, res.Data.toString());
                        }
                        if (this.httpService.ValidChecksumClient(res.Token)) {
                            window.localStorage.setItem(AppConfigs.Authorization, res.Token);
                            this.notifier.notify(NotifierTypeConst.Success, 'Login success!');
                            setTimeout(() => {
                                let returnUrl = window.localStorage.getItem(AppConfigs.ReturnUrl);
                                if (returnUrl === undefined) { returnUrl = '/'; }
                                this.router.navigateByUrl(returnUrl);
                                this.loading = !this.loading;
                            }, 2000);
                        } else {
                            this.notifier.notify(NotifierTypeConst.Error, 'Checksum client invalid. Please login again!');
                        }
                    } else {
                        if (res != null && res.Message.length > 0) {
                            this.notifier.notify(NotifierTypeConst.Error, res.Message);
                        }
                        this.loading = !this.loading;
                    }
                    return res;
                }),
                catchError(() => this.httpService.handleError)
            );
    }

    public doLogout(): Observable<ResponseData> {
        const route = environment.APP_API_URL + '/Account/DoLogout';
        return this.httpService.DoPost(route).pipe(
            map(res => {
                if (res != null && res.Success === true) {
                    window.localStorage.removeItem(AppConfigs.Authorization);
                    window.localStorage.removeItem(AppConfigs.ChecksumClient);
                    this.httpService.doRedirectLogin(true);
                }
                return res;
            }),
            catchError(() => this.httpService.handleError)
        );
    }

    public doGoToSSO() {
        window.localStorage.removeItem(AppConfigs.IsNotSSO);
        window.location.href = environment.loginUrl + '?returnUrl=' + environment.APP_CLIENT_URL;
        return;
      }
}
