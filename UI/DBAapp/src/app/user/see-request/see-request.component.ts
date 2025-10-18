import { Component, OnInit } from '@angular/core';
import { Zahtev } from 'src/app/models/zahtev';
import { Tip_zahteva } from 'src/app/models/tip_zahteva';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-see-request',
  templateUrl: './see-request.component.html',
  styleUrls: ['./see-request.component.css']
})
export class SeeRequestComponent implements OnInit {

  sviZahtevi: Zahtev[] = [];
  mojiZahtevi: Zahtev[] = [];
  trenutniKorisnik: string = null;

  // Modal logika
  showDetailModal: boolean = false;
  showCreateModal: boolean = false;
  izabraniZahtev: Zahtev | null = null;

  noviZahtev: Zahtev ;
  tipoviZahteva: Tip_zahteva[] = [];

  constructor(private zahtevService: SharedService) {}

  ngOnInit(): void {
    
    this.ucitajTipove();
    this.ucitajZahteve();

this.zahtevService.user$.subscribe(user => {
      this.trenutniKorisnik = user;
    });
  

    this.noviZahtev = {
    PodneoKorisnik: this.trenutniKorisnik,
    Naslov: '',
    Tekst: '',
    Prioritet: 0,
    Tip: 0,
    PotrebnaSaglasnost: false
  };
  }
ucitajTipove(): void {
    this.zahtevService.getTipovi().subscribe(data => {
      this.tipoviZahteva = data;
    
    });
  }
  ucitajZahteve(): void {
    this.zahtevService.getSviZahtevi().subscribe(data => {
      this.sviZahtevi = data;
      this.mojiZahtevi = data.filter(z => z.PodneoKorisnik === this.trenutniKorisnik);
    
    });
  }

  otvoriDetalje(zahtev: Zahtev): void {
    this.izabraniZahtev = { ...zahtev };
    this.showDetailModal = true;
  }

  zatvoriDetalje(): void {
    this.showDetailModal = false;
    this.izabraniZahtev = null;
  }

  otvoriKreiranje(): void {
    this.showCreateModal = true;
  }

  zatvoriKreiranje(): void {
    this.showCreateModal = false;
    this.noviZahtev = {
      PodneoKorisnik: this.trenutniKorisnik,
      Naslov: '',
      Tekst: '',
      Prioritet: 0,
      Tip: 0,
      PotrebnaSaglasnost: false
    };
  }

  kreirajZahtev(): void {
    this.zahtevService.dodajZahtev(this.noviZahtev).subscribe(() => {
      this.zatvoriKreiranje();
      this.ucitajZahteve();
    });
  }

  izmeniZahtev(): void {
    
    if (this.izabraniZahtev && this.izabraniZahtev.Id) {
      this.zahtevService.izmeniZahtev(this.izabraniZahtev.Id, this.izabraniZahtev).subscribe(() => {
        this.zatvoriDetalje();
        this.ucitajZahteve();
      });
    }
  }

  obrisiZahtev(id: number | undefined): void {
    if (!id) return;
    this.zahtevService.obrisiZahtev(id).subscribe(() => {
      this.ucitajZahteve();
    });
  }

  mozeSeMenjati(zahtev: Zahtev): boolean {
    return zahtev.PodneoKorisnik === this.trenutniKorisnik && !zahtev.DatumPreuzimanja;
  }

  mozeSeBrisati(zahtev: Zahtev): boolean {
    return this.mozeSeMenjati(zahtev);
  }
}
