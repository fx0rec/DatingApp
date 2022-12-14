import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';
  

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this; 
    //  We're gonna pass one our controls, set it's value accessor to the TextInputComponent
    //  which implements the ControlValueAccessor. 
    //  Which allows us to writeValue(), registerOnChange(), registerOnTouched()
   }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl; //Casting this... into a FormControl to get around the TS error
  }

}
