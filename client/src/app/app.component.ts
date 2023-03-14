import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { AccountService } from './Services/account.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    

  users: any; //no type safety here

  constructor(private accountService: AccountService){}

  //Happens after the constructor, don't fetch data in constructor
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');

    if(userString == null) return

    const user : User = JSON.parse(userString);

    this.accountService.setCurrentUser(user);
  }

 
}
