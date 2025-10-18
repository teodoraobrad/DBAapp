import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SeeRequestComponent } from './see-request/see-request.component';
import { AboutMeComponent } from './about-me/about-me.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { FormsModule } from '@angular/forms'; 
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    SeeRequestComponent,
    AboutMeComponent,
    NotificationsComponent
  ],
  imports: [
    CommonModule, RouterModule, FormsModule,       
    HttpClientModule     
  
  ]
})
export class UserModule { }
