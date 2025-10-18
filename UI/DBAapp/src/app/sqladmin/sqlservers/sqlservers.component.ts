import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { forkJoin } from 'rxjs';
import { SqlServer } from 'src/app/models/sqlserver';
import { Server } from 'src/app/models/server';



@Component({
  selector: 'app-sqlservers',
  templateUrl: './sqlservers.component.html',
  styleUrls: ['./sqlservers.component.css']
})
export class SqlserversComponent implements OnInit {
servers: SqlServer[] = null;

  filteredServers: SqlServer[] = [];
  selectedServer: SqlServer | null = null;
  showProd: string = '';
  showModal: boolean = false;
  searchTerm: string = '';
  showConfirm: boolean = false;
  srcsrv: Server[] = [];
  serverToRestart: SqlServer;
  
  constructor(public sharedservice: SharedService) { }

  ngOnInit(): void {
  
        this.sharedservice.dohvatiServere().subscribe({
          next: (data) => {
            this.srcsrv = data;
          },
          error: (err) => {
            console.error('Greška prilikom učitavanja servera', err);
          }
        });
        this.sharedservice.dohvatiSQLServere().subscribe({
          next: (data) => {
            this.servers = data;
            this.filteredServers = [...this.servers];//.filter(s => s.aktivan == true);
            this.filterServers();
          },
          error: (err) => {
            console.error('Greška prilikom učitavanja sql servera', err);
          }
        });
      }
    
      findSrv(id: number): string {
        const okr = this.srcsrv.find(o => o.Id === id);
        return okr ? okr.Hostname : '';
      }
      filterServers() {
        if (!this.searchTerm && this.showProd == '') {
          this.filteredServers =[...this.servers];//= this.servers.filter(s => s.Status !== "Arhiviran");
    
          return;
        }
   
        if (this.showProd == 'a') {
    
          this.filteredServers = this.servers
            .filter(s => s.Aktivan == true)
            .filter(s => !this.searchTerm || s.Naziv.toLowerCase().includes(this.searchTerm.toLowerCase()));
          return;
        }
       if (this.showProd == 'ne') {
    
          this.filteredServers = this.servers
            .filter(s => s.Aktivan == false)
            .filter(s => !this.searchTerm || s.Naziv.toLowerCase().includes(this.searchTerm.toLowerCase()));
          return;
        }
        
    
      }
      clearSearch() {
        this.searchTerm = '';
        this.filterServers();
      }
      openServerDetails(server: SqlServer) {
        this.selectedServer = { ...server };
        this.showModal = true;
      }
      closeModal() {
        this.showModal = false;
        this.selectedServer = null;
        this.sharedservice.dohvatiSQLServere().subscribe({
          next: (data) => {
            this.servers = data;
            this.filteredServers = [...this.servers];
            this.filterServers();
          },
          error: (err) => {
            console.error('Greška prilikom učitavanja sql servera', err);
          }
        });
      }
    
      saveServer() {
    
        if (!this.selectedServer) return;
        const index = this.servers.findIndex(s => s.Id === this.selectedServer!.Id);
        if (index > -1) {
          this.servers[index] = { ...this.selectedServer };
          this.sharedservice.izmeniSQLServer(this.selectedServer.Id, this.selectedServer).subscribe(() => {
            this.sharedservice.dohvatiSQLServere().subscribe({
              next: (data) => {
                this.servers = data;
                this.filteredServers = [...this.servers];
                this.filterServers();
                this.closeModal();
    
              },
              error: (err) => {
                console.error('Greška prilikom UPDATE SQLSRV', err);
              }
            });
          });
        } else {
          this.sharedservice.dodajSQLServer(this.selectedServer).subscribe(() => {
    
            this.sharedservice.dohvatiSQLServere().subscribe({
              next: (data) => {
                this.servers = data;
                this.filteredServers = [...this.servers];
                this.filterServers();
                this.closeModal();
    
              },
              error: (err) => {
                console.error('Greška prilikom UPDATE SQLSRV', err);
              }
            });
          });
        }
    
    
      }
    
    /*  restartServer(server: SqlServer) {
    
        this.serverToRestart = server;
        this.showConfirm = true;
      }*/
    
      openAddServerModal() {
        this.selectedServer = {
          Id: this.servers.length + 1,
          Naziv: '',
          Verzija: '',
          Edicija: '',
          IdServera: 0,
          Verzija1: '',
          Kolacija: '',
          Aktivan: true,
          DatumInstalacije: new Date(),
          Klaster: '',
          Port: 1433,
          Status: 'OK',
          Nalog: ''
          
        };
        this.showModal = true;
      }
    
      cancelRestart() {
        this.showConfirm = false;
        this.serverToRestart = null;
      }
    
    /*  confirmRestart() {
        if (this.serverToRestart) {
          alert(`Server ${this.serverToRestart.Hostname} se restartuje!`);
          
          this.sharedservice.restart(this.serverToRestart,sessionStorage.getItem("username")).subscribe(() => {
          this.showConfirm = false;
        this.serverToRestart = null;
        });
    
        }
      }*/
    /*
     restartModal:boolean=false;
      restarts:RESTART[]=[];
    
      restartIst(server: SqlServer){
        this.serverToRestart=server;
        this.sharedservice.dohvatiRestarte(server.Hostname).subscribe({
          next: (data) => {
            this.restarts = data;
            this.restartModal=true;
          },
          error: (err) => {
            console.error('Greška prilikom učitavanja sqladm', err);
          }
        });
      }
      closeRestartModal(){
        this.serverToRestart=null;
        this.restarts=[];
        this.restartModal=false;
      }
    */
    /*
      archiveServer(selectedServer: any) {
        this.sharedservice.archiveServer(selectedServer.Id).subscribe({
          next: (res) => {
            alert(res); // prikazuje poruku iz backenda: "Server je uspešno arhiviran."
            // možeš i osvežiti listu servera:
            this.closeModal();
          },
          error: (err) => {
            if (err.status === 400) {
              alert(err.error); // prikazuje backend poruku "Server je još uvek aktivan..."
            } else if (err.status === 404) {
              alert("Server nije pronađen.");
            } else {
              alert("Došlo je do greške prilikom arhiviranja.");
            }
          }
        });
      }*/

}
