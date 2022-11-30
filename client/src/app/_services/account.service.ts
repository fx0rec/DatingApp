import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
//  Angular Services can be injected into our component or other services.
//  It's represented by the @Injectable decorator.
@Injectable({  
  //metadata 'provided in root' (the root module, aka our app.module)
  //we can have multiple app modules, and if so we just change the providedIn to that specific one.
  //this automatically provides it in the app.module.ts as suggested by "providers: []"
  providedIn: 'root'
})
//AccountServices class is gonna be responsible for making the http requests from our client -> server
//Why use a service when you can do this inside the component?
//Using a service it gives us an opportunity to centralize our http-requests

/* Services are singletones, meaning they're instantiated whenever our application starts, 
   and terminated when it ends. (Runs full length)

   As we move from component to component, the comp is destroyed along with any State
   that is stored inside that component. Whereas a service is always available for the lifetime
   of the application. Making it a good place to store any kind of State that we want our app to remember.
*/
export class AccountService { 
  baseUrl = 'https://localhost:5001/api/';

  //Injecting http client
  constructor(private http: HttpClient) {}

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model);
  }
}
