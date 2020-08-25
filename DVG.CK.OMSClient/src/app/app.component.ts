import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: '<router-outlet></router-outlet>',
  styleUrls: ['app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit() {

    // xử lý khi đăng nhập sso
    const url = new URL(window.location.href);
    const ssoCallbackObj = url.searchParams.get('data');
    if (ssoCallbackObj != null) {
      this.router.navigate(['/logonsso'], {
        queryParams: { data: ssoCallbackObj }
      });
    }

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }
}
