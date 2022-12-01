import { Component, OnInit } from '@angular/core';
import { UrlSerializer } from '@angular/router';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn = false;

  constructor(private accountService: AccountService) { //Injecting the service
    
  }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next: user => this.loggedIn = !!user, // Double exclamation !! means we're turning the user into a boolean. if User = true, none = false
      error: error => console.log(error)
    })
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => { 
        console.log(response);
        this.loggedIn = true;
      },
      error: error => console.log(error)

    }); 
  }

  logout(){
    this.accountService.logout();
    this.loggedIn = false;
  }

}
