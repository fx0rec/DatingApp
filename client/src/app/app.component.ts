import { HttpClient } from '@angular/common/http';
import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  //properties
  title: string = 'DatingApp';
  users: any;
  //constructor
  constructor(private http: HttpClient, private accountService: AccountService) // Injecting HttpClient & AccountService
  {

  }

  //On initialization we make a request to our API server through Http, 
  //which is injected into our class through the constructor
  ngOnInit(): void {
   this.getUsers(); //gets Users on init
   this.setCurrentUser(); //sets current User on init, if something is in local storage
  }

  getUsers(){
     //@return — An Observable of the response, with the response body as an ArrayBuffer.
    //An observable is a stream of data, that we wish to observe in some way as it's returned from the API. 
    //Observables are lazy by nature and won't happen unless subscribed to them using the .subscribe() method.
    //We specify what happens next:, followed by a call-back function describing what we wan't to do with the returned data.
    //We''re gonna get a response back from Get(), and the lambda => specifies what do to with it. 
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response, 
      error: error => {console.log(error)},
      complete: () => console.log('Request has completed!')
    });
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

}
