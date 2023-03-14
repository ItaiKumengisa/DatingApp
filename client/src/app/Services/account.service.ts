import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { BehaviorSubject, map } from 'rxjs';

import { environment } from 'src/environments/environment';
import { User } from 'src/app/models/user.model'

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseAccountUrl: string = environment.baseUrl + "/account";

  private currentUserSource = new BehaviorSubject<User | null>(null);

  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }
 
  login(model: any){
    return this.http.post<User>(this.baseAccountUrl + "/login", model).pipe( map( (response: User) => {
      var user = response;
      if(user){
        localStorage.setItem('user', JSON.stringify(user));
        this.setCurrentUser(user);
      }
      return response;
    }))
  }

  register(model: any){
    return this.http.post<User>(this.baseAccountUrl + "/register", model).pipe(
      map( (response: User) => {
        var user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);
        }

        return response;
      })
    )
  }

  logout(){
    localStorage.removeItem('user')
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }
}
