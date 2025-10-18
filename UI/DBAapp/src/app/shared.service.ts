import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Korisnik } from './models/korisnik';
import { catchError, map } from 'rxjs/operators'; // Dodaj catchError import
import { Obavestenje } from './models/obavestenje';
import { Zahtev } from './models/zahtev';
import { Tip_zahteva } from './models/tip_zahteva';
import { Dezurstvo } from './models/dezurstvo';
import { Server } from './models/server';
import { OKRUZENJE } from './models/okruzenje';
import { RESTART } from './models/restart';
import { SqlServer } from './models/sqlserver';
import { Baza } from './models/baza';
import { Backup } from './models/backup';
import { Query } from './models/query';
import { LogResetLozinke } from './models/logresetlozinke';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
 

  readonly APIUrl = 'http://localhost:54524/api';
  readonly photoUrl = 'http://localhost:54524/Photos';
  private userSubject = new BehaviorSubject<string | null>(null);
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) {
    const user = localStorage.getItem('username') || sessionStorage.getItem('username');
    this.userSubject.next(user);
  }
  /*
   login(username: string, password: string) {
      const body = { "korisnickoIme":username, "lozinka":password };
      return this.http.post(`${this.APIUrl}/korisnik/ProveriLozinku`, body);
    }*/
  loginA(username: string, rememberMe: boolean) {
    if (rememberMe) {
      localStorage.setItem('username', username);
    } else {
      sessionStorage.setItem('username', username);
    }
    this.userSubject.next(username); // obavesti sve koji subskribuju
  }

  logout() {
    /*localStorage.clear();
    sessionStorage.clear();*/
    localStorage.removeItem('username');
    sessionStorage.removeItem('username');
    this.userSubject.next(null);
  }


  login(username: string, password: string): Observable<any> {
    const body = { "korisnickoIme": username, "lozinka": password };
    return this.http.post(`${this.APIUrl}/korisnik/ProveriLozinku`, body);
  }
  registrujKorisnika(korisnik: any) {
    return this.http.post(this.APIUrl + '/korisnik/', korisnik);
  }

  getTimovi() {
    return this.http.get<any[]>(this.APIUrl + '/tim');
  }

  proveriKorisnickoIme(username: string) {
    const body = { "korisnickoIme": username };
    return this.http.post(`${this.APIUrl}/korisnik/proveriKorisnickoIme`, body);
  }
  passChange(username: string, newpassword: string) {
    const body = { "korisnickoIme": username, "lozinka": newpassword };
    return this.http.put(`${this.APIUrl}/korisnik/PromeniLozinku`, body);
  }

  dohvatiDezurnog(): Observable<Korisnik[]> {
    return this.http.get<Korisnik[]>(this.APIUrl + '/korisnik/dezurniKorisnik');
  }

  resetujLozinku(korisnickoIme: string) {
    const body = { "korisnickoIme": korisnickoIme };
    return this.http.post(this.APIUrl + '/korisnik/resetujLozinku', body);
  }
  dohvatiOdgovornog(imeTima: string): Observable<any> {
    return this.http.get<Korisnik>(this.APIUrl + '/korisnik/dohvatiOdgovornog', {
      params: { "naziv": imeTima }
    });
  }
  dohvatiKorisnika(korisnickoIme: string): Observable<Korisnik> {
    return this.http.get<Korisnik>(this.APIUrl + '/korisnik/dohvatiKorisnika', {
      params: { "korisnickoIme": "" + korisnickoIme + "" }
    });
  }
  dohvatiDetaljeKorisnika(korisnickoIme: string): Observable<Korisnik> {
    return this.http.get<Korisnik>(this.APIUrl + '/korisnik/dohvatiDetaljeKorisnika', {
      params: { "korisnickoIme": "" + korisnickoIme + "" }
    });
  }

  sacuvajDetaljeKorisnika(korisnickoIme: string, ime: string, prezime: string,
    jmbg: string,
    telefon: string,
    email: string) {
    const body = {
      "korisnickoIme": korisnickoIme, "ime": ime, "prezime": prezime,
      "jmbg": jmbg, "telefon": telefon, "email": email
    };
    return this.http.put(`${this.APIUrl}/korisnik/sacuvajDetaljeKorisnika`, body);
  }

  dohvatiObavestenja(): Observable<Obavestenje[]> {
    return this.http.get<Obavestenje[]>(this.APIUrl + '/obavestenje/sva');
  }

  getSviZahtevi(): Observable<Zahtev[]> {
    return this.http.get<Zahtev[]>(this.APIUrl + '/ZAHTEV/svi');
  }

  obrisiZahtev(id: number) {
    return this.http.delete(this.APIUrl + '/ZAHTEV/' + id + '');
  }
  izmeniZahtev(Id: number, izabraniZahtev: Zahtev) {
    const body = {
      "Naslov": izabraniZahtev.Naslov, "Tekst": izabraniZahtev.Tekst, "Prioritet": izabraniZahtev.Prioritet,
      "tip": izabraniZahtev.Tip
    };
    return this.http.put(`${this.APIUrl}/ZAHTEV/ProneniOsnovno/${Id}`, body);
  }
  dodajZahtev(noviZahtev: Zahtev) {
    const body = { "PodneoKorisnik": noviZahtev.PodneoKorisnik, "Naslov": noviZahtev.Naslov, "Tekst": noviZahtev.Tekst, "Prioritet": noviZahtev.Prioritet, "Tip": noviZahtev.Tip, };
    return this.http.post(this.APIUrl + '/ZAHTEV', body);
  }

  getTipovi(): Observable<Tip_zahteva[]> {
    return this.http.get<Tip_zahteva[]>(this.APIUrl + '/TIP_ZAHTEVA');
  }

  getSqlAdmini(): Observable<Korisnik[]> {
    return this.http.get<Korisnik[]>(this.APIUrl + '/KORISNIK/dohvatiSveAktivneAdmine');
  }

  getDezurstva(): Observable<Dezurstvo[]> {
    return this.http.get<Dezurstvo[]>(this.APIUrl + '/DEZURSTVO/sva');
  }

  dodajDezurstvo(novi: Dezurstvo) {
    const body = { "datumOd": novi.datumOd, "datumDo": novi.datumDo, "dezurni": novi.dezurni, "postavioKorisnik": novi.postavioKorisnik, "datumUnosa": novi.datumUnosa };
    return this.http.post(this.APIUrl + '/DEZURSTVO', body);
  }

  obrisiDezurstvo(novi: Dezurstvo) {
    const body = { "datumOd": novi.datumOd, "datumDo": novi.datumDo, "dezurni": novi.dezurni, "postavioKorisnik": novi.postavioKorisnik, "datumUnosa": novi.datumUnosa };

    return this.http.delete(this.APIUrl + '/DEZURSTVO/', { body });
  }


  dohvatiServere(): Observable<Server[]> {
    return this.http.get<Server[]>(this.APIUrl + '/SERVER/dohvatiSve');
  }

  dohvatiSQLServere(): Observable<SqlServer[]> {
    return this.http.get<SqlServer[]>(this.APIUrl + '/SQLSERVER/svi');
  }
  dohvatiOkruzenja(): Observable<OKRUZENJE[]> {
    return this.http.get<OKRUZENJE[]>(this.APIUrl + '/OKRUZENJE/sva');
  }
  produkcija(id:number): Observable<boolean> {
    return this.http.get<boolean>(this.APIUrl + '/OKRUZENJE/TipOkruzenja/'+id+'');
  }
  izmeniServer(id:number,server:Server){
  const body = {
      
        "Id": id,
        "Hostname": server.Hostname,
        "IpAdresa": server.IpAdresa,
        "OS": server.OS,
        "BrojJezgara": server.BrojJezgara,
        "Lokacija": server.Lokacija,
        "IdOkruzenja": server.IdOkruzenja,
        "Virtuelan": server.Virtuelan,
        "Aktivan": server.Aktivan,
        "DatumInstalacije": server.DatumInstalacije,
        "Klaster": server.Klaster,
        "RAM_GB": server.RAM_GB,
        "Storage_GB": server.Storage_GB,
        "Status": server.Status,
        "Napomena": server.Napomena
    
    };
    return this.http.put(`${this.APIUrl}/SERVER/update/${id}`, body);
  }

  izmeniSQLServer(id:number,server:SqlServer){
  const body = {
      
        "Id": id,
        "IdServera": server.IdServera,
        "Naziv": server.Naziv,
        "Verzija": server.Verzija,
        "Edicija": server.Edicija,
        "Verzija1": server.Verzija1,
        "Kolacija": server.Kolacija,
        "Port": server.Port,
        "Klaster": server.Klaster,
        "Aktivan": server.Aktivan,
        "DatumInstalacije": server.DatumInstalacije,
        "Nalog": server.Nalog,
        "Status": server.Status
    
    };
    return this.http.put(`${this.APIUrl}/SQLSERVER/update/${id}`, body);
  }
     dodajServer(server: Server) {
    const body = {
      
        
        "Hostname": server.Hostname,
        "IpAdresa": server.IpAdresa,
        "OS": server.OS,
        "BrojJezgara": server.BrojJezgara,
        "Lokacija": server.Lokacija,
        "IdOkruzenja": server.IdOkruzenja,
        "Virtuelan": server.Virtuelan,
        "Aktivan": server.Aktivan,
        "DatumInstalacije": server.DatumInstalacije,
        "Klaster": server.Klaster,
        "RAM_GB": server.RAM_GB,
        "Storage_GB": server.Storage_GB,
        "Status": server.Status,
        "Napomena": server.Napomena
    
    };return this.http.post(this.APIUrl + '/SERVER/dodaj', body);
  }
  dodajSQLServer(server: SqlServer) {
    const body = {
        "IdServera": server.IdServera,
        "Naziv": server.Naziv,
        "Verzija": server.Verzija,
        "Edicija": server.Edicija,
        "Verzija1": server.Verzija1,
        "Kolacija": server.Kolacija,
        "Port": server.Port,
        "Klaster": server.Klaster,
        "Aktivan": server.Aktivan,
        "DatumInstalacije": server.DatumInstalacije,
        "Nalog": server.Nalog,
        "Status": server.Status
    
    };return this.http.post(this.APIUrl + '/SQLSERVER/dodaj', body);
  }


archiveServer(id: number): Observable<any> {
  return this.http.put(this.APIUrl + '/SERVER/arhiviraj/' + id, {});
}

restart(server:Server,korisnik:string) {
    const body = { "Hostname": server.Hostname, "Korisnik": korisnik };
    return this.http.post(this.APIUrl + '/RESTART/dodaj', body);
  }

dohvatiRestarte(srv:string): Observable<RESTART[]> {
    return this.http.get<RESTART[]>(this.APIUrl + '/RESTART/svi/'+srv);
  }

  getBazeByServer(id:number): Observable<Baza[]> {
    return this.http.get<Baza[]>(this.APIUrl + '/baza/sve/'+id);
  }
  updateBaza(id:number,baza:Baza){
  const body = {
      
         "Id": id,
        "IdSqlservera": baza.IdSqlservera,
        "Ime": baza.Ime,
        "RecoveryModel": baza.RecoveryModel,
        "CompatibilityLvl": baza.CompatibilityLvl,
        "OwnerName": baza.OwnerName,
        "DatumKreiranja": baza.DatumKreiranja,
        "PoslednjiBekap": baza.PoslednjiBekap,
        "VelicinaMb": baza.VelicinaMb,
        "Aktivna": baza.Aktivna,
        "Readonly": baza.Readonly
    
    };
    return this.http.put(`${this.APIUrl}/baza/update/${id}`, body);
  }


  getBackupsByBaza(id:number): Observable<Backup[]> {
    return this.http.get<Backup[]>(this.APIUrl + '/backup/'+id);
  }
   dohvatiUpite(): Observable<Query[]> {
    return this.http.get<Query[]>(this.APIUrl + '/query');
  }


  dodajBazu(baza: Baza) {
    const body = {
        "Id": baza.Id,
        "IdSqlservera": baza.IdSqlservera,
        "Ime": baza.Ime,
        "RecoveryModel": baza.RecoveryModel,
        "CompatibilityLvl": baza.CompatibilityLvl,
        "OwnerName": baza.OwnerName,
        "DatumKreiranja": baza.DatumKreiranja,
        "PoslednjiBekap": baza.PoslednjiBekap,
        "VelicinaMb": baza.VelicinaMb,
        "Aktivna": baza.Aktivna,
        "Readonly": baza.Readonly
    
    };return this.http.post(this.APIUrl + '/baza/dodaj', body);
  }
  
  dodajUpit(u: Query) {
    const body = {
        "Id": u.Id,
        "Naslov":u.Naslov,
        "Upit":u.Upit
    
    };return this.http.post(this.APIUrl + '/query', body);
  }

 dohvatiLogPromene(): Observable<LogResetLozinke[]> {
    return this.http.get<LogResetLozinke[]>(this.APIUrl + '/logresetlozinke');
  }
promeniLozSilovito(korisnickoIme:string,datumZahteva:Date){
const body = {
      korisnickoIme: korisnickoIme,
      datumZahteva: datumZahteva,
      izvrseno: true
    };

  return this.http.put(`${this.APIUrl}/logresetlozinke/izvrseno`, body);

}

obrisiLog(l:LogResetLozinke){

    const body = { "korisnickoIme": l.KorisnickoIme, "datumZahteva": l.DatumZahteva};

    return this.http.delete(this.APIUrl + '/logresetlozinke', { body });
}
dohvatiKorisnike(): Observable<Korisnik[]> {
    return this.http.get<Korisnik[]>(this.APIUrl + '/korisnik/dohvatiSveKorisnike');
  }

aktivirajKor(ime:string){
const body = {
      korisnickoIme: ime
    };

  return this.http.put(`${this.APIUrl}/korisnik/aktiviraj`, body);
}

}
