import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'; 
import { HttpClientModule } from '@angular/common/http';
import { OndutyComponent } from './onduty/onduty.component';
import { IntersectionComponent } from './intersection/intersection.component';
import { ServersComponent } from './servers/servers.component';
import { AccountsComponent } from './accounts/accounts.component';
import { DatabasesComponent } from './databases/databases.component';
import { RequestsComponent } from './requests/requests.component';
import { ActionsComponent } from './actions/actions.component';
import { ReportsComponent } from './reports/reports.component';
import { LoginsComponent } from './logins/logins.component';
import { SqlserversComponent } from './sqlservers/sqlservers.component';


@NgModule({
  declarations: [
    OndutyComponent,
    IntersectionComponent,
    ServersComponent,
    AccountsComponent,
    DatabasesComponent,
    RequestsComponent,
    ActionsComponent,
    ReportsComponent,
    LoginsComponent,
    SqlserversComponent
  ],
  imports: [
    CommonModule, RouterModule, FormsModule,       
        HttpClientModule     
  ]
})
export class SqladminModule { }
