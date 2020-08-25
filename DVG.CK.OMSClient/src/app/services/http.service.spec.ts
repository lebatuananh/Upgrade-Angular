import { TestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { HttpService } from './http.service';
describe('HttpService', () => {
  let service: HttpService;
  beforeEach(() => {
    const httpClientStub = {
      post: () => ({ pipe: () => ({}) }),
      get: () => ({ pipe: () => ({}) })
    };
    const httpErrorResponseStub = {
      error: { message: {} },
      status: {},
      message: {}
    };
    const routerStub = { navigate: () => ({ then: () => ({}) }), url: {} };
    const notifierServiceStub = {};
    TestBed.configureTestingModule({
      providers: [
        HttpService,
        { provide: HttpClient, useValue: httpClientStub },
        { provide: HttpErrorResponse, useValue: httpErrorResponseStub },
        { provide: Router, useValue: routerStub },
        { provide: NotifierService, useValue: notifierServiceStub }
      ]
    });
    service = TestBed.get(HttpService);
  });
  it('can load instance', () => {
    expect(service).toBeTruthy();
  });
  describe('handleError', () => {
    it('makes expected calls', () => {
      spyOn(component, 'doRedirectLogin');
      spyOn(component, 'doRedirect405');
      service.handleError(httpErrorResponseStub);
      expect(service.doRedirectLogin).toHaveBeenCalled();
      expect(service.doRedirect405).toHaveBeenCalled();
    });
  });
  describe('doRedirectLogin', () => {
    it('makes expected calls', () => {
      const routerStub: Router = TestBed.get(Router);
      spyOn(routerStub, 'navigate');
      service.doRedirectLogin();
      expect(routerStub.navigate).toHaveBeenCalled();
    });
  });
  describe('doRedirect405', () => {
    it('makes expected calls', () => {
      const routerStub: Router = TestBed.get(Router);
      spyOn(routerStub, 'navigate');
      service.doRedirect405();
      expect(routerStub.navigate).toHaveBeenCalled();
    });
  });
});
