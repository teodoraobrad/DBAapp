import { Component, OnInit } from '@angular/core';
import { Server } from '../../models/server'
import { SharedService } from 'src/app/shared.service';
import { forkJoin } from 'rxjs';
import { OKRUZENJE } from 'src/app/models/okruzenje';
import { RESTART } from 'src/app/models/restart';

@Component({
  selector: 'app-servers',
  templateUrl: './servers.component.html',
  styleUrls: ['./servers.component.css']
})

export class ServersComponent implements OnInit {

  servers: Server[] = null;

  filteredServers: Server[] = [];
  selectedServer: Server | null = null;
  showProd: string = '';
  showModal: boolean = false;
  searchTerm: string = '';
  showConfirm: boolean = false;
  serverToRestart: Server | null = null;
  okruzenja: OKRUZENJE[] = [];

  serverFields = [
    { key: 'Id', label: 'Id' },
    { key: 'Hostname', label: 'Hostname' },
    { key: 'IpAdresa', label: 'IP adresa' },
    { key: 'OS', label: 'OS' },
    { key: 'BrojJezgara', label: 'Broj jezgara' },
    { key: 'IdOkruzenja', label: 'Okruzenje' },
    { key: 'Lokacija', label: 'Lokacija' },
    { key: 'Virtuelan', label: 'Virtuelan' },
    { key: 'Aktivan', label: 'Aktivan' },
    { key: 'DatumInstalacije', label: 'Datum instalacije' },
    { key: 'Klaster', label: 'Klaster' },
    { key: 'RAM_GB', label: 'RAM (GB)' },
    { key: 'Storage_GB', label: 'Storage (GB)' },
    { key: 'Status', label: 'Status' },
    { key: 'Napomena', label: 'Napomena' }
  ];

  constructor(public sharedservice: SharedService) {

  }

  ngOnInit(): void {

    this.sharedservice.dohvatiOkruzenja().subscribe({
      next: (data) => {
        this.okruzenja = data;
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja sqladm', err);
      }
    });
    this.sharedservice.dohvatiServere().subscribe({
      next: (data) => {
        this.servers = data;
        this.filteredServers = this.servers.filter(s => s.Status !== "Arhiviran");

        this.filterServers();
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja sqladm', err);
      }
    });


  }

  findOkr(id: number): string {
    const okr = this.okruzenja.find(o => o.id === id);
    return okr ? okr.naziv : '';
  }
  filterServers() {
    if (!this.searchTerm && this.showProd == '') {
      this.filteredServers = this.servers.filter(s => s.Status !== "Arhiviran");

      return;
    }

    if (this.showProd == 'arh') {

      this.filteredServers = this.servers
        .filter(s => s.Status == "Arhiviran")
        .filter(s => !this.searchTerm || s.Hostname.toLowerCase().includes(this.searchTerm.toLowerCase()));
      return;
    }
    const observables = this.servers.map(s =>
      this.sharedservice.produkcija(s.IdOkruzenja)
    );

    if (this.showProd === 'prod') {
      // forkJoin sačeka sve Observable-ove i vrati niz boolean rezultata
      forkJoin(observables).subscribe(results => {
        this.filteredServers = this.servers
          .filter((s, index) => results[index] === true) 
          .filter(s => !this.searchTerm || s.Hostname.toLowerCase().includes(this.searchTerm.toLowerCase()));
      });
    } else if (this.showProd = 'neprod') {
      // forkJoin sačeka sve Observable-ove i vrati niz boolean rezultata
      forkJoin(observables).subscribe(results => {
        this.filteredServers = this.servers
          .filter((s, index) => results[index] === false) 
          .filter(s => !this.searchTerm || s.Hostname.toLowerCase().includes(this.searchTerm.toLowerCase()));
      });
    }

  }
  clearSearch() {
    this.searchTerm = '';
    this.filterServers();
  }
  openServerDetails(server: Server) {
    this.selectedServer = { ...server };
    this.showModal = true;
  }
  closeModal() {
    this.showModal = false;
    this.selectedServer = null;
    this.sharedservice.dohvatiServere().subscribe({
      next: (data) => {
        this.servers = data;
        this.filteredServers = [...this.servers];
        this.filterServers();
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja sqladm', err);
      }
    });
  }

  saveServer() {

    if (!this.selectedServer) return;
    const index = this.servers.findIndex(s => s.Id === this.selectedServer!.Id);
    if (index > -1) {
      this.servers[index] = { ...this.selectedServer };
      this.sharedservice.izmeniServer(this.selectedServer.Id, this.selectedServer).subscribe(() => {
        this.sharedservice.dohvatiServere().subscribe({
          next: (data) => {
            this.servers = data;
            this.filteredServers = [...this.servers];
            this.filterServers();
            this.closeModal();

          },
          error: (err) => {
            console.error('Greška prilikom učitavanja sqladm', err);
          }
        });
      });
    } else {
      this.sharedservice.dodajServer(this.selectedServer).subscribe(() => {

        this.filterServers();
        this.closeModal();
      });
    }


  }

  restartServer(server: Server) {

    this.serverToRestart = server;
    this.showConfirm = true;
  }

  openAddServerModal() {
    this.selectedServer = {
      Id: this.servers.length + 1,
      Hostname: '',
      IpAdresa: '',
      OS: '',
      BrojJezgara: 1,
      Lokacija: '',
      IdOkruzenja: this.showProd === 'prod' ? 1 : 2,
      Virtuelan: false,
      Aktivan: true,
      DatumInstalacije: new Date().toISOString().substring(0, 10),
      Klaster: '',
      RAM_GB: 8,
      Storage_GB: 100,
      Status: 'OK',
      Napomena: ''
    };
    this.showModal = true;
  }

  cancelRestart() {
    this.showConfirm = false;
    this.serverToRestart = null;
  }

  confirmRestart() {
    if (this.serverToRestart) {
      alert(`Server ${this.serverToRestart.Hostname} se restartuje!`);
      
      this.sharedservice.restart(this.serverToRestart,sessionStorage.getItem("username")).subscribe(() => {
      this.showConfirm = false;
    this.serverToRestart = null;
    });

    }
  }

 restartModal:boolean=false;
  restarts:RESTART[]=[];

  restartIst(server: Server){
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


  archiveServer(selectedServer: any) {
    this.sharedservice.archiveServer(selectedServer.Id).subscribe({
      next: (res) => {
        alert(res); 
        this.closeModal();
      },
      error: (err) => {
        if (err.status === 400) {
          alert(err.error); 
        } else if (err.status === 404) {
          alert("Server nije pronađen.");
        } else {
          alert("Došlo je do greške prilikom arhiviranja.");
        }
      }
    });
  }

}
