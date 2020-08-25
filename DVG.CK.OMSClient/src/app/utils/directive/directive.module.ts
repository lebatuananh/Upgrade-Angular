import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoWhitespaceDirective } from '../../utils/directive/no-whitespace.directive';

@NgModule({
  declarations: [NoWhitespaceDirective],
  exports: [NoWhitespaceDirective],
  imports: [
    CommonModule
  ]
})
export class DirectiveModule { }
