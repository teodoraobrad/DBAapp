import { Component, OnInit } from '@angular/core';
import { SqlServer } from 'src/app/models/sqlserver';
import { Baza } from 'src/app/models/baza';
import { Backup } from 'src/app/models/backup';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html',
  styleUrls: ['./databases.component.css']
})
export class DatabasesComponent implements OnInit {

  servers: SqlServer[] = [];
  selectedServerId: number | null = null;
  baze: Baza[] = [];

  backups: Backup[] = [];
  backupModal: boolean = false;

  editModal: boolean = false;
  selectedBaza: Baza | null = null;

  constructor(private sharedService: SharedService) { }

  ngOnInit(): void {
    this.loadServers();
  }

  loadServers() {
    this.sharedService.dohvatiSQLServere().subscribe({
      next: (data) => this.servers = data,
      error: (err) => console.error("Greška prilikom učitavanja servera", err)
    });
  }

  add() {
    this.selectedBaza = {
      Id: 255,
      IdSqlservera: this.selectedServerId,
      Ime: '',
      RecoveryModel: '',
      CompatibilityLvl: 150,
      OwnerName: '',
      DatumKreiranja: new Date(),
      PoslednjiBekap: new Date(),
      VelicinaMb: 0,
      Aktivna: false,
      Readonly: false
    };
    this.editModal = true;
  }
  onServerChange() {
    if (!this.selectedServerId) return;
    this.sharedService.getBazeByServer(this.selectedServerId).subscribe({
      next: (data) => this.baze = data,
      error: (err) => console.error("Greška prilikom učitavanja baza", err)
    });
  }
  get poslednjiBekap(): string {
    return this.selectedBaza.PoslednjiBekap
      ? this.selectedBaza.PoslednjiBekap.toISOString().substring(0, 10)
      : '';
  }
  get DatumKreiranja(): string {
    return this.selectedBaza.DatumKreiranja
      ? this.selectedBaza.DatumKreiranja.toISOString().substring(0, 10)
      : '';
  }



  openBackups(baza: Baza) {
    this.sharedService.getBackupsByBaza(baza.Id).subscribe({
      next: (data) => {
        this.backups = data;
        this.backupModal = true;
      },
      error: (err) => console.error("Greška prilikom učitavanja backupa", err)
    });
  }

  openEdit(baza: Baza) {
    this.selectedBaza = { ...baza }; // kopija da ne menja odmah

    this.editModal = true;
  }

  saveEdit() {
    if (!this.selectedBaza) return;
    const index = this.servers.findIndex(s => s.Id === this.selectedBaza!.Id);
    if (index > -1) {
      this.baze[index] = { ...this.selectedBaza };
      this.sharedService.updateBaza(this.selectedBaza.Id, this.selectedBaza).subscribe({
        next: () => {
          this.editModal = false;
          this.onServerChange(); // refresh
        },
        error: (err) => console.error("Greška prilikom izmene baze", err)
      });
    } else {
      this.sharedService.dodajBazu(this.selectedBaza).subscribe(() => {
        this.editModal = false;
        this.onServerChange(); // refresh
        error: (err) => {
          console.error('Greška prilikom UPDATE ', err);
        }

      });
    }
  }

  closeModals() {
    this.backupModal = false;
    this.editModal = false;
    this.selectedBaza = null;
  }
}
