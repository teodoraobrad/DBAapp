import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../shared.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  

  constructor(private sharedService: SharedService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    
  }

  goTo(path: string) {
  this.router.navigate([path]);
}

}
