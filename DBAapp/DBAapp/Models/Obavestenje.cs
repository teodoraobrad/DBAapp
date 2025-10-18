using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    
        public class Obavestenje
        {
            public long Id { get; set; }                    // id BIGINT
            public string KorisnickoIme { get; set; }       // varchar(20)
            public long? IdZahtev { get; set; }             // bigint NULL
            public long? IdAkcije { get; set; }             // bigint NULL
            public string Tekst { get; set; }               // varchar(1000) NULL
            public DateTime Datum { get; set; }             // datetime NOT NULL
            public DateTime? DatumOd { get; set; }          // date NULL
            public DateTime? DatumDo { get; set; }          // date NULL
            public bool Sos { get; set; }                   // bit NOT NULL
        }
    
}