import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import{SharedService} from './shared.service';

import {HttpClientModule} from '@angular/common/http';
import {FormsModule,ReactiveFormsModule} from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { PassChangeComponent } from './pass-change/pass-change.component';
import { UserComponent } from './user/user.component';
import { LeadComponent } from './lead/lead.component';
import { SqladminComponent } from './sqladmin/sqladmin.component';
import { ContactComponent } from './contact/contact.component';
import { ForgottenComponent } from './forgotten/forgotten.component';
import { HomeComponent } from './home/home.component';
import { UserModule } from './user/user.module';
import { AboutComponent } from './about/about.component';
import { SqladminModule } from './sqladmin/sqladmin.module';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    PassChangeComponent,
    UserComponent,
    LeadComponent,
    SqladminComponent,
    ContactComponent,
    ForgottenComponent,
    HomeComponent,
    AboutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
     HttpClientModule,
     FormsModule,
     ReactiveFormsModule,
     UserModule, 
     SqladminModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
