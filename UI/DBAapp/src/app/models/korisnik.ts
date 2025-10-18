export class Korisnik
{
    korisnickoIme:string;
    lozinka:string;
    ime:string;
    prezime:string;
    jmbg:string;
    telefon:string;
    email:string;
    tim:string;

    datumRegistracije?:Date;
    poslednjaPrijava?:Date;
    aktivan:boolean;
}