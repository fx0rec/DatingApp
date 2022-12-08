import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { MembersService } from './_services/members.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  //properties
  title: string = 'DatingApp';

  //constructor
  constructor(private accountService: AccountService) // Injecting AccountService
  {

  }

  //On initialization we make a request to our API server through Http, 
  //which is injected into our class through the constructor
  ngOnInit(): void {
   this.setCurrentUser(); //sets current User on init, if something is in local storage
  }

  

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

}
