using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class Korisnik
    {
        public string korisnickoIme { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string  jmbg { get; set; }
        public string  telefon { get; set; }
	    public string  email { get; set; }
        public string  tim { get; set; }

        public DateTime? datumRegistracije  { get; set; }

        public DateTime?  poslednjaPrijava  { get; set; }

        public bool aktivan { get; set; }
    }
}