import { Component, OnInit} from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  getUsers(){
    this.http.get<any>("https://localhost:40443/api/users").subscribe({
      next: response => {this.users = response},
      error: error => {console.error(error)},
      complete: () => {console.log("Request has completed")}
    });
  }

  cancelRegisterHandler(){
    this.registerToggle();
  }
}
