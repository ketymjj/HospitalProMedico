import { Directive, HostListener, Optional, Self } from '@angular/core';
import { NgControl } from '@angular/forms';

/**
 * Formats free typing into dd/mm/yyyy while the user types, and — once all
 * 8 digits are present — becomes the authority for the bound FormControl's
 * Date value. This is necessary because Angular Material's NativeDateAdapter
 * parses typed strings using the JS engine's locale-naive Date parsing
 * (which reads slash-separated dates as mm/dd/yyyy), so it cannot be trusted
 * to correctly interpret a dd/mm/yyyy string on its own. The authoritative
 * setValue is deferred to a microtask so it always wins over whatever
 * MatDatepickerInput's own (possibly wrong) parse already assigned for the
 * same keystroke.
 */
@Directive({
  selector: '[appDateMask]',
  standalone: true
})
export class DateMaskDirective {
  constructor(@Optional() @Self() private ngControl: NgControl | null) {}

  @HostListener('input', ['$event'])
  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const digits = input.value.replace(/\D/g, '').slice(0, 8);

    let formatted = digits;
    if (digits.length > 4) {
      formatted = `${digits.slice(0, 2)}/${digits.slice(2, 4)}/${digits.slice(4)}`;
    } else if (digits.length > 2) {
      formatted = `${digits.slice(0, 2)}/${digits.slice(2)}`;
    }

    if (input.value !== formatted) {
      input.value = formatted;
    }

    if (digits.length === 8) {
      const day = Number(digits.slice(0, 2));
      const month = Number(digits.slice(2, 4));
      const year = Number(digits.slice(4, 8));
      const date = new Date(year, month - 1, day);
      const isValidDate = date.getFullYear() === year && date.getMonth() === month - 1 && date.getDate() === day;

      queueMicrotask(() => {
        if (isValidDate) {
          this.ngControl?.control?.setValue(date);
        } else {
          this.ngControl?.control?.setErrors({ ...this.ngControl.control.errors, invalidDate: true });
        }
      });
    }
  }
}
