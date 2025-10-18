import { Component, OnInit } from '@angular/core';
import { Query } from 'src/app/models/query';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {

  constructor(private sharedservice:SharedService) { }

  ngOnInit(): void {
    this.sharedservice.dohvatiUpite().subscribe({
      next: (data) => this.queries = data,
      error: (err) => console.error("Greška prilikom učitavanja servera", err)
    });

  }
 queries: Query[] = [];

  selectedQuery: Query | null = null;
  showModal = false;
selectedId:number=null;
  newNaslov = '';
  newUpit = '';

  onSelectQuery() {
    if(this.selectedId==null)return;
    const found = this.queries.find(q => q.Id === this.selectedId);
    this.selectedQuery = found || null;
  }

  // open/close modal
  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.newNaslov = '';
    this.newUpit = '';
  }
onChange(){
  this.sharedservice.dohvatiUpite().subscribe({
      next: (data) => this.queries = data,
      error: (err) => console.error("Greška prilikom učitavanja servera", err)
    });
}  // save new query
  saveQuery() {
    if (!this.newNaslov.trim() || !this.newUpit.trim()) return;

    const newQuery: Query = {
      Id: this.queries.length > 0 ? Math.max(...this.queries.map(q => q.Id)) + 1 : 1,
      Naslov: this.newNaslov,
      Upit: this.newUpit
    };


    this.queries.push(newQuery);
    this.sharedservice.dodajUpit(newQuery).subscribe(() => {
        this.showModal = false;
        this.onChange(); // refresh
        error: (err) => {
          console.error('Greška prilikom UPDATE ', err);
        }

      });
    this.closeModal();
  }

  // Copy displayed text to clipboard
  copyText(): void {
    const textToCopy = this.selectedQuery.Upit;

    navigator.clipboard.writeText(textToCopy)
      .then(() => {
        alert('Tekst je kopiran u clipboard!');
      })
      .catch(err => {
        console.error('Greška pri kopiranju:', err);
        alert('Neuspešno kopiranje.');
      });
  }

}
