using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class LogResetLozinke
    {
        public string KorisnickoIme { get; set; }
        public DateTime DatumZahteva { get; set; }
        public bool Izvrseno { get; set; }
        public bool EmailPoslat { get; set; }
    }
}