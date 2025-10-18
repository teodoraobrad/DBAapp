import { Component, OnInit } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { Dezurstvo } from 'src/app/models/dezurstvo';
import { Korisnik } from 'src/app/models/korisnik';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-onduty',
  templateUrl: './onduty.component.html',
  styleUrls: ['./onduty.component.css']
})
export class OndutyComponent implements OnInit {

  duties: Dezurstvo[] = [];
  upcomingDuties: Dezurstvo[] = [];
  pastDuties: Dezurstvo[] = [];
  users: Korisnik[];
  currentUser: string = sessionStorage.getItem("username");
  showModal = false;
  newDezurstvo: any = { datumOd: null, datumDo: null, dezurni: "" };
  korisnik: Korisnik = null;

  constructor(public sharedservice: SharedService) { }

  ngOnInit(): void {

    this.sharedservice.getSqlAdmini().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja sqladm', err);
      }
    });
    this.sharedservice.getDezurstva().subscribe({
      next: (data) => {
        this.duties = data;
         this.splitDuties();
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja dezurstava', err);
      }
    });

  
    this.sharedservice.dohvatiDetaljeKorisnika(this.currentUser).subscribe({
      next: (res) => {
        this.korisnik = res;
      },
      error: (err) => {
        console.error('Greška prilikom dohvaćanja korisnika:', err);
      }
    });
  }
  splitDuties() {
    
  const today = new Date();

  this.upcomingDuties = this.duties
    .filter(d => new Date(d.datumOd) >= today)
    .sort((a, b) => +new Date(a.datumOd) - +new Date(b.datumOd));

  this.pastDuties = this.duties
    .filter(d => new Date(d.datumOd) < today)
    .sort((a, b) => +new Date(b.datumOd) - +new Date(a.datumOd));


  }

  openModal() { this.showModal = true; }
  closeModal() { this.showModal = false; }

  addDezurstvo(): void {
    if (new Date(this.newDezurstvo.datumOd) >= new Date(this.newDezurstvo.datumDo)) {
      alert("Datum od mora biti manji od datuma do!");
      return;
    }
    if (this.overlapsWithExisting(this.newDezurstvo.datumOd, this.newDezurstvo.datumDo)) {
    alert("Period se preklapa sa već postojećim dežurstvom!");
    return;
  }

    const dezurstvo: Dezurstvo = {
      datumOd: new Date(this.newDezurstvo.datumOd),
      datumDo: new Date(this.newDezurstvo.datumDo),
      dezurni: this.newDezurstvo.dezurni,
      postavioKorisnik: this.currentUser,
      datumUnosa: new Date()
    };

    this.duties.push(dezurstvo);
alert(this.newDezurstvo.dezurni)
    this.sharedservice.dodajDezurstvo(dezurstvo).subscribe(() => {
      this.splitDuties();
      this.closeModal();
      this.newDezurstvo = { datumOd: null, datumDo: null, dezurni: "" };
    });

    this.sharedservice.getDezurstva().subscribe({
      next: (data) => {
        this.duties = data;
         this.splitDuties();
      },
      error: (err) => {
        console.error('Greška prilikom učitavanja dezurstava', err);
      }
    });

  }

  isAdmin() {
    if (this.korisnik.tim = "sqladmin")
      return true;
    else return false;
  }

  deleteDezurstvo(d: Dezurstvo) {
    this.sharedservice.obrisiDezurstvo(d).subscribe(() => {


      this.sharedservice.getDezurstva().subscribe({
        next: (data) => {
          this.duties = data;
           this.splitDuties();
        },
        error: (err) => {
          console.error('Greška prilikom učitavanja dezurstava', err);
        }
      });

     
    });
  }

  private overlapsWithExisting(newOd: Date, newDo: Date): boolean {
  const newStart = new Date(newOd).getTime();
  const newEnd = new Date(newDo).getTime();

  return this.duties.some(d => {
    const od = new Date(d.datumOd).getTime();
    const doo = new Date(d.datumDo).getTime();

    // dva intervala se preklapaju ako:
    // početak novog <= kraj starog && kraj novog >= početak starog
    return newStart <= doo && newEnd >= od;
  });
}

}



