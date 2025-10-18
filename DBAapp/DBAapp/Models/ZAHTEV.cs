using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class ZAHTEV
    {
        public long Id { get; set; }
        public string PodneoKorisnik { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumPodnosenja { get; set; }
        public string Tekst { get; set; }
        public string UradioAdmin { get; set; }
        public DateTime? DatumPreuzimanja { get; set; }
        public string Status { get; set; }
        public int Prioritet { get; set; }
        public int Tip { get; set; }
        public bool PotrebnaSaglasnost { get; set; }
        public string SaglasanKorisnik { get; set; }
        public DateTime? DatumSaglasnosti { get; set; }
        public DateTime? DatumZavrsetka { get; set; }
        public bool? Zavrsen { get; set; }
    }
}