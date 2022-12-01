import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
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
  //This is for other components to ascertain whether or not a user is logged in.
  //This is a special kind of observable, called a BehaviorSubject
  //A BehaviorSubject which allows us to give an observable an initial value, that can be used elsewhere in the app.                                
  private currentUserSource = new BehaviorSubject<User | null>(null);
  //$ is a convention to signify that this is an observable
  currentUser$ = this.currentUserSource.asObservable();
  
  //Injecting http client
  constructor(private http: HttpClient) {}

  login(model: any){
              //We specify what type of thing it is we're getting returned <User>
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user))
          this.currentUserSource.next(user);
        }
      }) 
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  //convenience method, to be called from the component to set the user.
  setCurrentUser(user:User){
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null)
  }
}
