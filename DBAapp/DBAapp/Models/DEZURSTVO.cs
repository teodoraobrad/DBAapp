using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class DEZURSTVO
    {
        public DateTime datumOd { get; set; }
        public DateTime datumDo { get; set; }
        public string dezurni { get; set; }
        public string postavioKorisnik { get; set; }
        public DateTime? datumUnosa { get; set; }
    }
}