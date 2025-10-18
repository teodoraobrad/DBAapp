using DBAapp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DBAapp.Controllers
{
    public class ZAHTEVController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // GET: api/ZAHTEV/svi
        [HttpGet]
        [Route("api/ZAHTEV/svi")]
        public IHttpActionResult GetSviZAHTEVi()
        {
            List<ZAHTEV> ZAHTEVi = new List<ZAHTEV>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM ZAHTEV", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ZAHTEVi.Add(new ZAHTEV
                        {
                            Id = (long)reader["id"],
                            PodneoKorisnik = reader["podneoKorisnik"].ToString(),
                            Naslov = reader["naslov"].ToString(),
                            DatumPodnosenja = (DateTime)reader["datumPodnosenja"],
                            Tekst = reader["tekst"]?.ToString(),
                            UradioAdmin = reader["uradioAdmin"]?.ToString(),
                            DatumPreuzimanja = reader["datumPreuzimanja"] as DateTime?,
                            Status = reader["status"].ToString(),
                            Prioritet = (int)reader["prioritet"],
                            Tip = (int)reader["tip"],
                            PotrebnaSaglasnost = (bool)reader["potrebnaSaglasnost"],
                            SaglasanKorisnik = reader["saglasanKorisnik"]?.ToString(),
                            DatumSaglasnosti = reader["datumSaglasnosti"] as DateTime?,
                            DatumZavrsetka = reader["datumZavrsetka"] as DateTime?,
                            Zavrsen = reader["zavrsen"] as bool?
                        });
                    }
                }
            }

            return Ok(ZAHTEVi);
        }

        // GET: api/ZAHTEV/{id}
        [HttpGet]
        [Route("api/ZAHTEV/{id}")]
        public IHttpActionResult GetZAHTEV(long id)
        {
            ZAHTEV ZAHTEV = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM ZAHTEV WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ZAHTEV = new ZAHTEV
                        {
                            Id = (long)reader["id"],
                            PodneoKorisnik = reader["podneoKorisnik"].ToString(),
                            Naslov = reader["naslov"].ToString(),
                            DatumPodnosenja = (DateTime)reader["datumPodnosenja"],
                            Tekst = reader["tekst"]?.ToString(),
                            UradioAdmin = reader["uradioAdmin"]?.ToString(),
                            DatumPreuzimanja = reader["datumPreuzimanja"] as DateTime?,
                            Status = reader["status"].ToString(),
                            Prioritet = (int)reader["prioritet"],
                            Tip = (int)reader["tip"],
                            PotrebnaSaglasnost = (bool)reader["potrebnaSaglasnost"],
                            SaglasanKorisnik = reader["saglasanKorisnik"]?.ToString(),
                            DatumSaglasnosti = reader["datumSaglasnosti"] as DateTime?,
                            DatumZavrsetka = reader["datumZavrsetka"] as DateTime?,
                            Zavrsen = reader["zavrsen"] as bool?
                        };
                    }
                }
            }

            if (ZAHTEV == null)
                return NotFound();

            return Ok(ZAHTEV);
        }

        // POST: api/ZAHTEV
        [HttpPost]
        [Route("api/ZAHTEV")]
        public IHttpActionResult PostZAHTEV(ZAHTEV ZAHTEV)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO ZAHTEV 
                    (podneoKorisnik, naslov, datumPodnosenja, tekst, status, prioritet, tip, zavrsen)
                VALUES 
                    (@podneoKorisnik, @naslov, GETDATE(), @tekst,  @status, @prioritet, @tip, @zavrsen)", con))
            {
                cmd.Parameters.AddWithValue("@podneoKorisnik", ZAHTEV.PodneoKorisnik);
                cmd.Parameters.AddWithValue("@naslov", ZAHTEV.Naslov);
                cmd.Parameters.AddWithValue("@tekst", (object)ZAHTEV.Tekst ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", ZAHTEV.Status ?? "Kreiran");
                cmd.Parameters.AddWithValue("@prioritet", ZAHTEV.Prioritet);
                cmd.Parameters.AddWithValue("@tip", ZAHTEV.Tip);
                cmd.Parameters.AddWithValue("@zavrsen",  false);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("ZAHTEV uspešno dodat.");
        }

        // PUT: api/ZAHTEV/{id}
        [HttpPut]
        [Route("api/ZAHTEV/ProneniOsnovno/{id}")]
        public IHttpActionResult PutZAHTEV(long id, ZAHTEV ZAHTEV)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE ZAHTEV SET
                   
                    naslov = @naslov,
                    tekst = @tekst,
                    status=@status,
                    prioritet = @prioritet,
                    tip = @tip
                   
                WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@naslov", ZAHTEV.Naslov);
                cmd.Parameters.AddWithValue("@tekst", (object)ZAHTEV.Tekst ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", "Izmenjen");
                cmd.Parameters.AddWithValue("@prioritet", ZAHTEV.Prioritet);
                cmd.Parameters.AddWithValue("@tip", ZAHTEV.Tip);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound();
            }

            return Ok("ZAHTEV uspešno izmenjen.");
        }

        // DELETE: api/ZAHTEV/{id}
        [HttpDelete]
        [Route("api/ZAHTEV/{id}")]
        public IHttpActionResult DeleteZAHTEV(long id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM ZAHTEV WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound();
            }

            return Ok("ZAHTEV uspešno obrisan.");
        }


        
    }
}