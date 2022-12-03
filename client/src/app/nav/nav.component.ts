import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { RouterModule, Routes } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService) { //Injecting the services (AccountService + Router)
    
  }

  ngOnInit(): void {

  }



  login(){
    //Observables need to be unsubscribed from, UNLESS they're an HTTP request, and HTTP requests complete.
    //So they essentially automatically unsub.
    this.accountService.login(this.model).subscribe({
      next: () => { 
        this.toastr.success('Welcome!');
        this.router.navigateByUrl('/members')
      }
    }); 
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }

}
