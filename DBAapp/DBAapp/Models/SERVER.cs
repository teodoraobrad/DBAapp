using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBAapp.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string IpAdresa { get; set; }
        public string OS { get; set; }
        public int? BrojJezgara { get; set; }
        public string Lokacija { get; set; }
        public int IdOkruzenja { get; set; }
        public bool Virtuelan { get; set; }
        public bool Aktivan { get; set; }
        public DateTime DatumInstalacije { get; set; }
        public string Klaster { get; set; }
        public long? RAM_GB { get; set; }
        public long? Storage_GB { get; set; }
        public string Status { get; set; }
        public string Napomena { get; set; }
    }

}