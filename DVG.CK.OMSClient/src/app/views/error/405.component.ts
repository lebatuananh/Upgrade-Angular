import { Component } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';

@Component({
  templateUrl: '405.component.html'
})
export class P405Component {
  constructor(private modalService: BsModalService) {
    this.modalService.hide(1);
  }

}
