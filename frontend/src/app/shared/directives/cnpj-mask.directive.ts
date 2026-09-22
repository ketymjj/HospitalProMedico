import { Directive, HostListener, Optional, Self } from '@angular/core';
import { NgControl } from '@angular/forms';
import { formatCnpj } from '../utils/mask.util';

@Directive({
  selector: '[appCnpjMask]',
  standalone: true
})
export class CnpjMaskDirective {
  constructor(@Optional() @Self() private ngControl: NgControl | null) {}

  @HostListener('input', ['$event'])
  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const formatted = formatCnpj(input.value);
    this.ngControl?.control?.setValue(formatted);
  }
}
