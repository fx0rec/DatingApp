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

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', Validators.required)
    })
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
