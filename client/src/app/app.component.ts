import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';

  users: any; //no type safety here

  constructor(private http: HttpClient){}

  //Happens after the constructor, don't fetch data in constructor
  ngOnInit(): void {
    this.http.get<any>("https://localhost:40443/api/users").subscribe({
      next: response => {this.users = response},
      error: error => {console.error(error)},
      complete: () => {console.log("Request has completed")}
    });
  }

}
