using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class SQL_SERVER
    {
        public int Id { get; set; }
        public int IdServera { get; set; }
        public string Naziv { get; set; }
        public string Verzija { get; set; }
        public string Edicija { get; set; }
        public string Verzija1 { get; set; }
        public string Kolacija { get; set; }
        public int? Port { get; set; }
        public string Klaster { get; set; }
        public bool Aktivan { get; set; }
        public DateTime? DatumInstalacije { get; set; }
        public string Nalog { get; set; }
        public string Status { get; set; }
    }
}