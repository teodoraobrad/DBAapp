using DBAapp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBAapp.Controllers
{
    public class ObavestenjeController : ApiController
    {

        // GET: api/obavestenje/sva
        [HttpGet]
        [Route("api/obavestenje/sva")]
        public IHttpActionResult GetSvaObavestenja()
        {
            List<Obavestenje> obavestenja = new List<Obavestenje>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM OBAVESTENJE", con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        obavestenja.Add(new Obavestenje
                        {
                            Id = (long)reader["id"],
                            KorisnickoIme = reader["korisnickoIme"].ToString(),
                            IdZahtev = reader["id_zahtev"] as long?,
                            IdAkcije = reader["id_akcije"] as long?,
                            Tekst = reader["tekst"]?.ToString(),
                            Datum = (DateTime)reader["datum"],
                            DatumOd = reader["datumOd"] as DateTime?,
                            DatumDo = reader["datumDo"] as DateTime?,
                            Sos = (bool)reader["sos"]
                        });
                    }
                }
            }

            return Ok(obavestenja);
        }
        // POST: api/obavestenje/dodaj
        [HttpPost]
        [Route("api/obavestenje/dodaj")]
        public IHttpActionResult DodajObavestenje([FromBody] Obavestenje o)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO OBAVESTENJE (korisnickoIme, id_zahtev, id_akcije, tekst, datum, datumOd, datumDo, sos)
                VALUES (@korisnickoIme, @id_zahtev, @id_akcije, @tekst, @datum, @datumOd, @datumDo, @sos)", con))
            {
                cmd.Parameters.AddWithValue("@korisnickoIme", o.KorisnickoIme);
                cmd.Parameters.AddWithValue("@id_zahtev", o.IdZahtev != null ? (object)o.IdZahtev : DBNull.Value);
                cmd.Parameters.AddWithValue("@id_akcije", o.IdAkcije != null ? (object)o.IdAkcije : DBNull.Value);
                cmd.Parameters.AddWithValue("@tekst", !string.IsNullOrEmpty(o.Tekst) ? (object)o.Tekst : DBNull.Value);
                cmd.Parameters.AddWithValue("@datum", o.Datum);

                // Ispravka za nullable datume
                cmd.Parameters.Add(new SqlParameter("@datumOd", SqlDbType.Date)
                {
                    Value = o.DatumOd.HasValue ? (object)o.DatumOd.Value : DBNull.Value
                });

                cmd.Parameters.Add(new SqlParameter("@datumDo", SqlDbType.Date)
                {
                    Value = o.DatumDo.HasValue ? (object)o.DatumDo.Value : DBNull.Value
                });

                cmd.Parameters.AddWithValue("@sos", o.Sos);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Uspešno dodato obaveštenje.");
        }


        [HttpPut]
        [Route("api/obavestenje/izmeni/{id:long}")]
        public IHttpActionResult IzmeniObavestenje(long id, [FromBody] Obavestenje o)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE OBAVESTENJE SET
                    korisnickoIme = @korisnickoIme,
                    id_zahtev = @id_zahtev,
                    id_akcije = @id_akcije,
                    tekst = @tekst,
                    datum = @datum,
                    datumOd = @datumOd,
                    datumDo = @datumDo,
                    sos = @sos
                WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
         
                cmd.Parameters.AddWithValue("@korisnickoIme", o.KorisnickoIme);
                cmd.Parameters.AddWithValue("@id_zahtev", o.IdZahtev != null ? (object)o.IdZahtev : DBNull.Value);
                cmd.Parameters.AddWithValue("@id_akcije", o.IdAkcije != null ? (object)o.IdAkcije : DBNull.Value);
                cmd.Parameters.AddWithValue("@tekst", !string.IsNullOrEmpty(o.Tekst) ? (object)o.Tekst : DBNull.Value);
                cmd.Parameters.AddWithValue("@datum", o.Datum);
                cmd.Parameters.Add(new SqlParameter("@datumOd", SqlDbType.Date)
                {
                    Value = o.DatumOd.HasValue ? (object)o.DatumOd.Value : DBNull.Value
                });

                cmd.Parameters.Add(new SqlParameter("@datumDo", SqlDbType.Date)
                {
                    Value = o.DatumDo.HasValue ? (object)o.DatumDo.Value : DBNull.Value
                });

                cmd.Parameters.AddWithValue("@sos", o.Sos);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok("Uspešno izmenjeno obaveštenje.");
        }
        // DELETE: api/obavestenje/obrisi/{id}
        [HttpDelete]
        [Route("api/obavestenje/obrisi/{id:long}")]
        public IHttpActionResult ObrisiObavestenje(long id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand("DELETE FROM OBAVESTENJE WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok("Obaveštenje obrisano.");
        }

        // GET: api/obavestenje/{id}
        [HttpGet]
        [Route("api/obavestenje/{id:long}")]
        public IHttpActionResult GetObavestenje(long id)
        {
            Obavestenje obavestenje = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM OBAVESTENJE WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obavestenje = new Obavestenje
                        {
                            Id = (long)reader["id"],
                            KorisnickoIme = reader["korisnickoIme"].ToString(),
                            IdZahtev = reader["id_zahtev"] != DBNull.Value ? (long?)reader["id_zahtev"] : null,
                            IdAkcije = reader["id_akcije"] != DBNull.Value ? (long?)reader["id_akcije"] : null,
                            Tekst = reader["tekst"]?.ToString(),
                            Datum = (DateTime)reader["datum"],
                            DatumOd = reader["datumOd"] != DBNull.Value ? (DateTime?)reader["datumOd"] : null,
                            DatumDo = reader["datumDo"] != DBNull.Value ? (DateTime?)reader["datumDo"] : null,
                            Sos = (bool)reader["sos"]
                        };
                    }
                }
            }

            if (obavestenje == null)
                return NotFound();

            return Ok(obavestenje);
        }

    }
}
