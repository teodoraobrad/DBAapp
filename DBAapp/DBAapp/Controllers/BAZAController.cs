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
    public class BAZAController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // Dohvatanje svih baza
        [HttpGet]
        [Route("api/baza/sve/{id}")]
        public IHttpActionResult GetAll(int id)
        {
            List<BAZA> baze = new List<BAZA>();
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM dbo.BAZA where id_sqlservera=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        baze.Add(new BAZA
                        {
                            Id = reader["id"] is DBNull ? 0 : Convert.ToInt32(reader["id"]),
                            IdSqlservera = reader["id_sqlservera"] is DBNull ? 0 : Convert.ToInt32(reader["id_sqlservera"]),
                            Ime = reader["ime"]?.ToString(),
                            RecoveryModel = reader["recovery_model"]?.ToString(),
                            CompatibilityLvl = reader["compatibility_lvl"] is DBNull ? 0 : Convert.ToInt32(reader["compatibility_lvl"]),
                            OwnerName = reader["owner_name"]?.ToString(),
                            DatumKreiranja = reader["datumKreiranja"] is DBNull ? null : (DateTime?)reader["datumKreiranja"],
                            PoslednjiBekap = reader["poslednjiBekap"] is DBNull ? null : (DateTime?)reader["poslednjiBekap"],
                            VelicinaMb = reader["velicina_mb"] is DBNull ? 0 : Convert.ToInt32(reader["velicina_mb"]),
                            Aktivna = reader["aktivna"] is DBNull ? false : Convert.ToBoolean(reader["aktivna"]),
                            Readonly = reader["readonly"] is DBNull ? false : Convert.ToBoolean(reader["readonly"])
                        });
                    }
                }
            }
            return Ok(baze);
        }

        // Update baze po ID
        [HttpPut]
        [Route("api/baza/update/{id}")]
        public IHttpActionResult UpdateBaza(int id, [FromBody] BAZA baza)
        {
            string query = @"
            UPDATE dbo.BAZA
            SET ime = @ime,
                recovery_model = @recovery_model,
                compatibility_lvl = @compatibility_lvl,
                owner_name = @owner_name,
                datumKreiranja = @datumKreiranja,
                poslednjiBekap = @poslednjiBekap,
                velicina_mb = @velicina_mb,
                aktivna = @aktivna,
                readonly = @readonly
            WHERE id = @id";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ime", (object)baza.Ime ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@recovery_model", (object)baza.RecoveryModel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@compatibility_lvl", baza.CompatibilityLvl);
                cmd.Parameters.AddWithValue("@owner_name", (object)baza.OwnerName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@datumKreiranja", (object)baza.DatumKreiranja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@poslednjiBekap", (object)baza.PoslednjiBekap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@velicina_mb", baza.VelicinaMb);
                cmd.Parameters.AddWithValue("@aktivna", baza.Aktivna);
                cmd.Parameters.AddWithValue("@readonly", baza.Readonly);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound();
            }

            return Ok("Baza je uspešno izmenjena.");
        }
        [HttpPost]
        [Route("api/baza/dodaj")]
        public IHttpActionResult CreateBaza([FromBody] BAZA baza)
        {
            string query = @"
        INSERT INTO dbo.BAZA 
            (id_sqlservera, ime, recovery_model, compatibility_lvl, owner_name, 
              poslednjiBekap, velicina_mb, aktivna, readonly)
        VALUES 
            (@id_sqlservera, @ime, @recovery_model, @compatibility_lvl, @owner_name, 
              @poslednjiBekap, @velicina_mb, @aktivna, @readonly)";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id_sqlservera", baza.IdSqlservera);
                cmd.Parameters.AddWithValue("@ime", (object)baza.Ime ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@recovery_model", (object)baza.RecoveryModel ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@compatibility_lvl", baza.CompatibilityLvl);
                cmd.Parameters.AddWithValue("@owner_name", (object)baza.OwnerName ?? DBNull.Value);
               // cmd.Parameters.AddWithValue("@datumKreiranja", (object)baza.DatumKreiranja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@poslednjiBekap", (object)baza.PoslednjiBekap ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@velicina_mb", baza.VelicinaMb);
                cmd.Parameters.AddWithValue("@aktivna", baza.Aktivna);
                cmd.Parameters.AddWithValue("@readonly", baza.Readonly);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Nova baza je uspešno kreirana.");
        }
    }
}