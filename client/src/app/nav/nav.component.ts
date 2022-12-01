import { Component, OnInit } from '@angular/core';
import { UrlSerializer } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService) { //Injecting the service
    
  }

  ngOnInit(): void {

  }



  login(){
    //Observables need to be unsubscribed from, UNLESS they're an HTTP request, and HTTP requests complete.
    //So they essentially automatically unsub.
    this.accountService.login(this.model).subscribe({
      next: response => { 
        console.log(response);
      },
      error: error => console.log(error)

    }); 
  }

  logout(){
    this.accountService.logout();
    
  }

}
