import { Component, OnInit } from '@angular/core';
import { AccountService } from '../Services/account.service';
import { Observable, of } from 'rxjs';
import { User } from 'src/app/models/user.model';
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn = false;

  constructor(public accountService : AccountService) { }

  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log("response");
        this.loggedIn = true;
      },
      error: (error) => {
        console.log(error)
      } 
    })
  }

  logout(){
    this.loggedIn = false;
    this.accountService.logout()
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next: user => {this.loggedIn = !!user; console.log("Logged In: ",this.loggedIn); console.log("user: ", user)},
      error: error => console.log(error)
    })
  }
}
