import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.toastr.success('Registered!')
        this.cancel();
      },
      error: error => {this.toastr.error(error.error);
      console.log(error);
      }

    })
  }

  cancel(){
    this.cancelRegister.emit(false); //emits the value of false
  }

}
