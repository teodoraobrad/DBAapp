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
    public class LogResetLozinkeController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;
        [HttpGet]
        [Route("api/logresetlozinke")]
        public IHttpActionResult GetLog()
        {
            List<LogResetLozinke> log = new List<LogResetLozinke>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT korisnickoIme, datumZahteva, izvrseno, emailPoslat 
        FROM LOG_RESET_LOZINKE 
        WHERE izvrseno=0", con))
            {
                

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    log.Add( new LogResetLozinke
                    {
                        KorisnickoIme = dr["korisnickoIme"].ToString(),
                        DatumZahteva = (DateTime)dr["datumZahteva"],
                        Izvrseno = (bool)dr["izvrseno"],
                        EmailPoslat = (bool)dr["emailPoslat"]
                    });
                }
            }

            if (log == null) return NotFound();

            return Ok(log);
        }
        [HttpDelete]
        [Route("api/logresetlozinke")]
        public IHttpActionResult DeleteLog([FromBody] LogResetLozinke model)
        {
            int rowsAffected = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        DELETE FROM LOG_RESET_LOZINKE 
        WHERE korisnickoIme = @korisnickoIme AND datumZahteva = @datumZahteva", con))
            {
                cmd.Parameters.AddWithValue("@korisnickoIme", model.KorisnickoIme);
                cmd.Parameters.AddWithValue("@datumZahteva", model.DatumZahteva);

                con.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }

            if (rowsAffected == 0) return NotFound();

            return Ok("Zapis je uspešno obrisan.");
        }
        // PUT api/logresetlozinke/izvrseno
        [HttpPut]
        [Route("api/logresetlozinke/izvrseno")]
        public IHttpActionResult UpdateIzvrseno([FromBody] LogResetLozinke model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.KorisnickoIme))
                return BadRequest("Neispravni podaci.");

            int rowsAffected = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        UPDATE LOG_RESET_LOZINKE 
        SET izvrseno = 1 
        WHERE korisnickoIme = @korisnickoIme AND datumZahteva = @datumZahteva", con))
            {
                cmd.Parameters.AddWithValue("@korisnickoIme", model.KorisnickoIme);
                cmd.Parameters.AddWithValue("@datumZahteva", model.DatumZahteva);

                con.Open();
                rowsAffected = cmd.ExecuteNonQuery();
            }

            if (rowsAffected == 0) return NotFound();

            return Ok("Polje 'izvrseno' je uspešno ažurirano.");
        }


    }
}