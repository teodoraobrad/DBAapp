import { Component, OnInit } from '@angular/core';
import { SharedService } from '../shared.service';
import { catchError } from 'rxjs/operators'; // Dodaj catchError import
import { Korisnik } from '../models/korisnik';
@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  ime: string = '';
  telefon: string = '';
  email: string = '';
  emailfirme: string = 'sqladmin@etf.rs';
  prezime: string = '';
  adresafirme: string = 'Bulevar Kralja Aleksandra';
  loadError: string = '';

  constructor(private sharedService: SharedService) { }

  ngOnInit(): void {
    
    this.dohvatiDezurnog();
  }


  dohvatiDezurnog():void{
    this.loadError = null;
     this.sharedService.dohvatiDezurnog().subscribe(
      {next:(data) => {
        // Kada dobijemo podatke, popunjavamo članove
        this.ime = data[0].ime;
        this.prezime = data[0].prezime;
        this.telefon = data[0].telefon;
        this.email = data[0].email;
      },
      error:(err) => {
         this.loadError = err.message || 'Nepoznata greška';
      }
  });
  }

}
