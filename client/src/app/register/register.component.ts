import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input() userFromHomeComponent: any; //Parent to child (Input)
  @Output() cancelRegister = new EventEmitter(); //Child to parent (Output)
  model: any = {};
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  minDate: Date = new Date();

  constructor(private accountService: AccountService, private toastr: ToastrService, 
    private fb: FormBuilder, private router: Router) { }


  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);  //Cant be younger than 18 years
    this.minDate.setFullYear(this.minDate.getFullYear() -100); //Cant be older than 100 years
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, 
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo:string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }



  register(){
    // this.accountService.register(this.model).subscribe({
    //   next: () => {
    //     this.toastr.success('Registered!')
    //     this.cancel();
    //   },
    //   error: error => {this.toastr.error(error.error);
    //   console.log(error);
    //   }

    // })

    console.log(this.registerForm?.value);
  }

  cancel(){
    this.cancelRegister.emit(false); //emits the value of false
  }

}
