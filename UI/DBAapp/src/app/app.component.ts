import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from './shared.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DBAapp';
  isLoggedIn = false;
  username: string | null = null;
  sq:string;
 isSQLadmin:boolean=false;
  constructor(private router: Router, private sharedService: SharedService) {}

  ngOnInit(): void {
    // proveri localStorage i sessionStorage
    const localUser = localStorage.getItem('username');
    const sessionUser = sessionStorage.getItem('username');
this.sharedService.user$.subscribe(user => {
      this.username = user;
    });
    if (localUser || sessionUser) {
      this.isLoggedIn = true;
      this.username = localUser || sessionUser;
    }
    this.sq= sessionStorage.getItem('sqla');
    if(this.sq=="true") this.isSQLadmin=true;
  }

  logout(): void {
    this.sharedService.logout();
    this.isLoggedIn = false;
    this.isSQLadmin=false;
    this.username = null;
    this.router.navigate(['/login']);
    sessionStorage.removeItem('sqla');
  }
}
