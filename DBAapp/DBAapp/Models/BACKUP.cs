using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class BACKUP
    {
        public int Id { get; set; }
        public int IdBaze { get; set; }
        public string Tip { get; set; }
        public DateTime? Datum { get; set; }
        public string Putanja { get; set; }
        public int VelicinaMB { get; set; }
    }
}