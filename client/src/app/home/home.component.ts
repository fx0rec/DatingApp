import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any; //To work with the getUsers()

  constructor(private http: HttpClient) { } //Injecting HttpClient to work with getUsers()

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode; //sets registerMode to the opposite of whatever it is currently
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

 cancelRegisterMode(event: boolean){
  this.registerMode = event;
 }
}
