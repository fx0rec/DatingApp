import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

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
  constructor(private http: HttpClient) 
  {

  }
  //On initialization we make a request to our API server through Http, 
  //which is injected into our class through the constructor
  ngOnInit(): void {
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
}
