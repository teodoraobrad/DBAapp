using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class BAZA
    {
        public int Id { get; set; }
        public int IdSqlservera { get; set; }
        public string Ime { get; set; }
        public string RecoveryModel { get; set; }
        public int CompatibilityLvl { get; set; }
        public string OwnerName { get; set; }
        public DateTime? DatumKreiranja { get; set; }
        public DateTime? PoslednjiBekap { get; set; }
        public int VelicinaMb { get; set; }
        public bool Aktivna { get; set; }
        public bool Readonly { get; set; }
    }
}