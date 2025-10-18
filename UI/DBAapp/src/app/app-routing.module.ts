import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PassChangeComponent } from './pass-change/pass-change.component';
import { ContactComponent } from './contact/contact.component';
import { ForgottenComponent } from './forgotten/forgotten.component';
import { UserComponent } from './user/user.component';
import { SqladminComponent } from './sqladmin/sqladmin.component';
import { LeadComponent } from './lead/lead.component';
import { SeeRequestComponent } from './user/see-request/see-request.component';
import { AboutMeComponent } from './user/about-me/about-me.component';
import { NotificationsComponent } from './user/notifications/notifications.component';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';

import { IntersectionComponent } from './sqladmin/intersection/intersection.component';
import { OndutyComponent } from './sqladmin/onduty/onduty.component';
import { ServersComponent } from './sqladmin/servers/servers.component';
import { AccountsComponent } from './sqladmin/accounts/accounts.component';
import { ActionsComponent } from './sqladmin/actions/actions.component';
import { DatabasesComponent } from './sqladmin/databases/databases.component';
import { LoginsComponent } from './sqladmin/logins/logins.component';
import { ReportsComponent } from './sqladmin/reports/reports.component';
import { RequestsComponent } from './sqladmin/requests/requests.component';
import { SqlserversComponent } from './sqladmin/sqlservers/sqlservers.component';


const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'register', component: RegisterComponent
  },
  {
    path: 'pass-change', component: PassChangeComponent
  },
  {
    path: 'contact', component: ContactComponent
  },
  {
    path: 'forgotten', component: ForgottenComponent
  },
  {
    path: 'about', component: AboutComponent
  },
  {
    path: 'user', component: UserComponent, children: [
      {
        path: '', component: NotificationsComponent
      },
      {
        path: 'see-request', component: SeeRequestComponent
      },
      {
        path: 'about-me', component: AboutMeComponent
      },
      {
        path: 'notifications', component: NotificationsComponent
      },
      {
        path: 'forgotten', component: ForgottenComponent
      }

    ]
  },
  {
    path: 'sqladmin', component: SqladminComponent, children: [
      {
        path: 'intersection', component: IntersectionComponent
      },{
        path: 'onduty', component: OndutyComponent
      },{
        path: 'servers', component: ServersComponent
      },{
        path: 'accounts', component: AccountsComponent
      },{
        path: 'actions', component: ActionsComponent
      },{
        path: 'databases', component: DatabasesComponent
      },{
        path: 'logins', component: LoginsComponent
      },{
        path: 'reports', component: ReportsComponent
      },{
        path: 'requests', component: RequestsComponent
      },{
        path: 'sqlservers', component: SqlserversComponent
      }
    ]
  },
  {
    path: 'lead', component: LeadComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
