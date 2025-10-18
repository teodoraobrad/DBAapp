import { Component, OnInit } from '@angular/core';
import { Obavestenje } from '../../models/obavestenje';
import { SharedService } from 'src/app/shared.service';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {

   obavestenja: Obavestenje[] =[];
   sosObavestenja: Obavestenje[] = [];
constructor(private sharedService:SharedService){

}
   ngOnInit(): void {
     this.sharedService.dohvatiObavestenja().subscribe({
    next: (data) => { 
      this.obavestenja =data.sort((a, b) => {
       
        const dateA = a.DatumOd ? new Date(a.DatumOd) : new Date(0);
        const dateB = b.DatumOd ? new Date(b.DatumOd) : new Date(0);
        return dateB.getTime() - dateA.getTime();
      });

      this.sosObavestenja = this.obavestenja.filter(o => o.Sos&&o.DatumDo>'2025-10-08');
    },
    error: (err) => {
      console.error('Greška pri dohvatanju obaveštenja:', err);
    }
  });
    
  }

  // Metoda koja vraća true ako obaveštenje nije aktuelno (datumOd i datumDo su manji od danas)
  nijeAktuelno(o: Obavestenje): boolean {
    if (!o.DatumOd || !o.DatumDo) return false;
    const danas = new Date();
    return new Date(o.DatumDo) < danas;
  }

  
}
