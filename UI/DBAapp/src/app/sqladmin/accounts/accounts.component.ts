import { Component, OnInit } from '@angular/core';
import { Korisnik } from 'src/app/models/korisnik';
import { LogResetLozinke } from 'src/app/models/logresetlozinke';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {
  logovi: LogResetLozinke[] = [];
  kor: Korisnik[] = [];
  aktivniKorisnici: Korisnik[] = [];
  neaktivniKorisnici: Korisnik[] = [];
  constructor(private shared: SharedService) { }

  ngOnInit(): void {
    this.ucitajNeizvrseneZahteve();
    this.shared.dohvatiKorisnike().subscribe({
      next: (data) => {
        this.kor = data;//.filter(log => !log.izvrseno);
        this.aktivniKorisnici = data.filter(u => u.aktivan);
        this.neaktivniKorisnici = data.filter(u => !u.aktivan);
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja kor:', err);
      }
    });
  }


  ucitajNeizvrseneZahteve(): void {
    this.shared.dohvatiLogPromene()
      .subscribe({
        next: (data) => {
          this.logovi = data;//.filter(log => !log.izvrseno);
        },
        error: (err) => {
          console.error('Greška prilikom učitavanja logova:', err);
        }
      });
  }

  potvrdiIzvrsenje(log: LogResetLozinke): void {


    this.shared.promeniLozSilovito(log.KorisnickoIme, log.DatumZahteva)
      .subscribe({
        next: () => {
          this.shared.passChange(log.KorisnickoIme, "1111").subscribe({
            next: () => {
              this.ucitajNeizvrseneZahteve();
            },
            error: (err) => {
              console.error('Greška prilikom promene:', err);
            }
          });
        },
        error: (err) => {
          console.error('Greška prilikom potvrde:', err);
        }
      });
  }

  obrisiZahtev(log: LogResetLozinke): void {


    this.shared.obrisiLog(log).subscribe({
      next: () => {
        this.ucitajNeizvrseneZahteve();
      },
      error: (err) => {
        console.error('Greška prilikom brisanja:', err);
      }
    });
  }
  aktivirajKorisnika(user: Korisnik): void {

    this.shared.aktivirajKor(user.korisnickoIme).
      subscribe({
        next: () => {
          this.shared.dohvatiKorisnike().subscribe({
            next: (data) => {
              this.kor = data;//.filter(log => !log.izvrseno);
              this.aktivniKorisnici = data.filter(u => u.aktivan);
              this.neaktivniKorisnici = data.filter(u => !u.aktivan);
            },
            error: (err) => {
              console.error('Greška prilikom učitavanja kor:', err);
            }
          });
        },
      error: (err) => console.error('Greška kod aktivacije:', err)
      });
  }

}
