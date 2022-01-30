import { Component } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'HomeFinanceAngular';

  constructor(public router: Router) { }

  onLogout(){
    localStorage.removeItem('token');
    this.router.navigateByUrl('/home');
  }

  isLogIn(){
    return localStorage.getItem('token')!=null;
  }
}
