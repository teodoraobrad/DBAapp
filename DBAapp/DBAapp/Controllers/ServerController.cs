using DBAapp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBAapp.Controllers
{
    [RoutePrefix("api/server")]
    public class ServerController : ApiController
    {
       
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // 1️⃣ Dohvati sve servere
        [HttpGet]
        [Route("dohvatiSve")]
        public IHttpActionResult DohvatiSve()
        {
            List<Server> servers = new List<Server>();
            string query = @"
            SELECT 
                id,
                hostname,
                ip_adresa,
                os,
                broj_jezgara,
                lokacija,
                id_okruzenja,
                virtuelan,
                aktivan,
                datumInstalacije,
                klaster,
                RAM_GB,
                storage_GB,
                status,
                napomena
            FROM dbo.SERVER";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        servers.Add(new Server
                        {
                            Id = (int)reader["id"],
                            Hostname = reader["hostname"].ToString(),
                            IpAdresa = reader["ip_adresa"].ToString(),
                            OS = reader["os"] == DBNull.Value ? null : reader["os"].ToString(),
                            BrojJezgara = reader["broj_jezgara"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["broj_jezgara"]),
                            Lokacija = reader["lokacija"] == DBNull.Value ? null : reader["lokacija"].ToString(),
                            IdOkruzenja = (int)reader["id_okruzenja"],
                            Virtuelan = (bool)reader["virtuelan"],
                            Aktivan = (bool)reader["aktivan"],
                            DatumInstalacije = (DateTime)reader["datumInstalacije"],
                            Klaster = reader["klaster"] == DBNull.Value ? null : reader["klaster"].ToString(),
                            RAM_GB = reader["RAM_GB"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["RAM_GB"]),
                            Storage_GB = reader["storage_GB"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["storage_GB"]),
                            Status = reader["status"] == DBNull.Value ? null : reader["status"].ToString(),
                            Napomena = reader["napomena"] == DBNull.Value ? null : reader["napomena"].ToString()
                        });
                    }
                }
            }

            return Ok(servers);
        }

        // 2️⃣ Dodaj server
        [HttpPost]
        [Route("dodaj")]
        public IHttpActionResult DodajServer([FromBody] Server server)
        {
            string query = @"
            INSERT INTO dbo.SERVER
                (hostname, ip_adresa, os, broj_jezgara, lokacija, id_okruzenja, virtuelan, aktivan, datumInstalacije, klaster, RAM_GB, storage_GB, status, napomena)
            VALUES
                (@hostname, @ip_adresa, @os, @broj_jezgara, @lokacija, @id_okruzenja, @virtuelan, @aktivan, @datumInstalacije, @klaster, @RAM_GB, @storage_GB, @status, @napomena)";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@hostname", server.Hostname);
                cmd.Parameters.AddWithValue("@ip_adresa", server.IpAdresa);
                cmd.Parameters.AddWithValue("@os", (object)server.OS ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@broj_jezgara", (object)server.BrojJezgara ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lokacija", (object)server.Lokacija ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id_okruzenja", server.IdOkruzenja);
                cmd.Parameters.AddWithValue("@virtuelan", server.Virtuelan);
                cmd.Parameters.AddWithValue("@aktivan", server.Aktivan);
                cmd.Parameters.AddWithValue("@datumInstalacije", server.DatumInstalacije);
                cmd.Parameters.AddWithValue("@klaster", (object)server.Klaster ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RAM_GB", (object)server.RAM_GB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@storage_GB", (object)server.Storage_GB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object)server.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@napomena", (object)server.Napomena ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Server uspešno dodat.");
        }

        // 3️⃣ Update server
        [HttpPut]
        [Route("update/{id}")]
        public IHttpActionResult UpdateServer(int id, [FromBody] Server server)
        {
            string query = @"
            UPDATE dbo.SERVER SET
                hostname = @hostname,
                ip_adresa = @ip_adresa,
                os = @os,
                broj_jezgara = @broj_jezgara,
                lokacija = @lokacija,
                id_okruzenja = @id_okruzenja,
                virtuelan = @virtuelan,
                aktivan = @aktivan,
                datumInstalacije = @datumInstalacije,
                klaster = @klaster,
                RAM_GB = @RAM_GB,
                storage_GB = @storage_GB,
                status = @status,
                napomena = @napomena
            WHERE id = @id";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@hostname", server.Hostname);
                cmd.Parameters.AddWithValue("@ip_adresa", server.IpAdresa);
                cmd.Parameters.AddWithValue("@os", (object)server.OS ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@broj_jezgara", (object)server.BrojJezgara ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lokacija", (object)server.Lokacija ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id_okruzenja", server.IdOkruzenja);
                cmd.Parameters.AddWithValue("@virtuelan", server.Virtuelan);
                cmd.Parameters.AddWithValue("@aktivan", server.Aktivan);
                cmd.Parameters.AddWithValue("@datumInstalacije", server.DatumInstalacije);
                cmd.Parameters.AddWithValue("@klaster", (object)server.Klaster ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RAM_GB", (object)server.RAM_GB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@storage_GB", (object)server.Storage_GB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object)server.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@napomena", (object)server.Napomena ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Server uspešno ažuriran.");
        }


        [HttpPut]
        [Route("arhiviraj/{id}")]
        public IHttpActionResult ArhivirajServer(int id)
        {
            // prvo proveravamo da li je server neaktivan
            string checkQuery = "SELECT aktivan FROM dbo.SERVER WHERE id = @id";
            bool? aktivan = null;

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(checkQuery, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    aktivan = (bool)result;
                }
                else
                {
                    return NotFound(); // server ne postoji
                }
            }

            if (aktivan == true)
            {
                return BadRequest("Server je još uvek aktivan i ne može biti arhiviran.");
            }

            // update statusa na "Arhiviran"
            string updateQuery = "UPDATE dbo.SERVER SET status = 'Arhiviran' WHERE id = @id";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(updateQuery, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Server je uspešno arhiviran.");
        }





    }
}