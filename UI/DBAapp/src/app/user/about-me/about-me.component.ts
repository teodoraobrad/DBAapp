import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Korisnik } from 'src/app/models/korisnik';
import { SharedService } from 'src/app/shared.service';
import { FormsModule } from '@angular/forms'; // ✅ Add this
@Component({
  selector: 'app-about-me',
  templateUrl: './about-me.component.html',
  styleUrls: ['./about-me.component.css']
})
export class AboutMeComponent implements OnInit {

  constructor(private sharedService:SharedService, private fv:FormsModule) { }
   korisnik: Korisnik = new Korisnik();
  isSaving = false;
  saveSuccess = false;
  username:string=null;

  ngOnInit(): void {
    this.ucitajKorisnika();
  }
  async ucitajKorisnika() {
    const localUser = localStorage.getItem('username');
    const sessionUser = sessionStorage.getItem('username');
    this.username=localUser==null?sessionUser:localUser;
    this.korisnik = await firstValueFrom(this.sharedService.dohvatiDetaljeKorisnika(this.username)); 
  }

  sacuvaj(): void {
    this.isSaving = true;
    this.sharedService.sacuvajDetaljeKorisnika(this.korisnik.korisnickoIme,this.korisnik.ime,this.korisnik.prezime,this.korisnik.jmbg,this.korisnik.telefon,this.korisnik.email).subscribe(
        {
          next:(rez:string)=>{
            
            this.saveSuccess=true;
      }, error:(err)=>{
            this.saveSuccess=false;
          }
        }
      );
      this.isSaving=false;
  }

}
