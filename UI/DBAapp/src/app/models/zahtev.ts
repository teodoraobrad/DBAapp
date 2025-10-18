export class Zahtev {
  Id?: number;
  PodneoKorisnik: string;
  Naslov: string;
  DatumPodnosenja?: Date;
  Tekst?: string;
  UradioAdmin?: string;
  DatumPreuzimanja?: Date;
  Status?: string;
  Prioritet: number;
  Tip: number;
  PotrebnaSaglasnost: boolean;
  SaglasanKorisnik?: string;
  DatumSaglasnosti?: Date;
  DatumZavrsetka?: Date;
  Zavrsen?: boolean;
}