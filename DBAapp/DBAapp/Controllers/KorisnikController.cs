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
    public class KorisnikController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @"
                   SELECT  korisnickoIme 
                          , lozinka 
                          , ime 
                          , prezime 
                          , jmbg 
                          , telefon 
                          , email 
                          , tim 
                          , datumRegistracije 
                          , poslednjaPrijava 
                          , aktivan 
                      FROM dbo.KORISNIK ";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public string Post(Korisnik kor)
        {
            
            string query = @"
                    insert into dbo.KORISNIK(korisnickoIme,lozinka,ime,prezime,jmbg,telefon,email,tim) 
                    values('" + kor.korisnickoIme + @"','" + kor.lozinka + @"','" + kor.ime + @"','" + kor.prezime + @"','" + kor.jmbg + @"','" + kor.telefon + @"','" + kor.email + @"','" + kor.tim + @"'"  + @")";
            try
            {
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Added Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to Add!!";
            }
        }

        public string Put(Korisnik kor)
        {
            string query="";
            try
            {
                 query = @"
                    update dbo.KORISNIK set ime='" + kor.ime + @"', prezime='" + kor.prezime + @"', jmbg='"+kor.jmbg + @"', telefon='"+kor.telefon + @"', email='" +kor.email+ @"', tim='"+kor.tim + @"' where korisnickoIme='"+kor.korisnickoIme+@"' ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "updated Successfully!!"+query;
            }
            catch (Exception)
            {
                return "Failed to update!!"+query;
            }
        }
        [Route("api/KORISNIK/PromeniLozinku")]
        [HttpPut]
        public IHttpActionResult PromeniLozinku(Korisnik kor)
        {
            string query = "";
            try
            {
                query = @"
                    update dbo.KORISNIK set  lozinka=@Lozinka where korisnickoIme=@kor ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
               {
                    cmd.Parameters.AddWithValue("@Lozinka", kor.lozinka);
                    cmd.Parameters.AddWithValue("@kor", kor.korisnickoIme);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 1)
                        return Ok("updated successfully!");
                    else
                        return NotFound(); // If no row was deleted
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpDelete]
        [Route("api/KORISNIK/{kor}")]
        public IHttpActionResult Delete(string kor)
        {
            /*string query = @"
                    delete from dbo.KORISNIK where korisnickoIme='" + kor + @"' 
                    ";
            try
            {
                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "deleted Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to delete!!" + query;
            }*/
            string query = "DELETE FROM dbo.KORISNIK WHERE korisnickoIme = @kor";

            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@kor", kor);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return Ok("Deleted successfully!");
                    else
                        return NotFound(); // If no row was deleted
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /*
        [Route("api/KORISNIK/ProveriLozinku")]
        [HttpGet]
        public bool ProveriLozinku(Korisnik kor)
        {
            string query = @"
                   select lozinka from dbo.KORISNIK where korisnickoIme='"+kor.korisnickoIme+@"'";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return DataAnnotations;
        }*/
        [Route("api/KORISNIK/ProveriLozinku")]
        [HttpPost]
        public IHttpActionResult ProveriLozinku(Korisnik kor)
        {
            string query = "SELECT lozinka FROM dbo.KORISNIK WHERE korisnickoIme = @korisnickoIme";

            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@korisnickoIme", kor.korisnickoIme);
                    con.Open();

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string dbLozinka = result.ToString();

                        if (kor.lozinka == dbLozinka)
                            return Ok(true);
                        else
                            return Ok(false);
                    }
                    else
                    {
                        return NotFound(); // User not found
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("api/KORISNIK/proveriKorisnickoIme")]
        [HttpPost]
        public IHttpActionResult ProveriKorisnickoIme(Korisnik kor)
        {
            string query = "SELECT count(*) FROM dbo.KORISNIK WHERE korisnickoIme = @korisnickoIme";

            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@korisnickoIme", kor.korisnickoIme);
                    con.Open();

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int broj = Convert.ToInt32(result); ;
                       
                        if ( broj>0)
                            return Ok(true);
                        else
                            return Ok(false);
                    }
                    else
                    {
                        return NotFound(); // User not found
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/korisnik/dezurniKorisnik")]
        [HttpGet]
        public HttpResponseMessage dohvatiDezurnog()
        {
            string query = @"
                    SELECT  korisnickoIme 
       ,ime 
       , prezime 
       , telefon 
       , email 
       
   FROM dbo.KORISNIK 
   where korisnickoIme=(select dezurni from DEZURSTVO where GETDATE() between datumOd and datumDo) ";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }


        [Route("api/korisnik/resetujLozinku")]
        [HttpPost]
        public IHttpActionResult ResetujLozinku(Korisnik kor)
        {
            if (string.IsNullOrWhiteSpace(kor.korisnickoIme))
                return BadRequest("Korisničko ime je prazno.");

            string query = "INSERT INTO dbo.LOG_RESET_LOZINKE (korisnickoIme) VALUES (@korisnickoIme)";

            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@korisnickoIme", kor.korisnickoIme);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Ok("Uspešno dodato!");
            }
            catch (Exception ex)
            {
                // Opcionalno loguj ex.Message
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/korisnik/dohvatiKorisnika")]
        public HttpResponseMessage dohvatiKorisnika(string korisnickoIme)
        {
            string query = @"
        SELECT TOP 1
            korisnickoIme,
            lozinka,
            ime,
            prezime,
            jmbg,
            telefon,
            email,
            tim,
            datumRegistracije,
            poslednjaPrijava,
            aktivan
        FROM dbo.KORISNIK
        WHERE korisnickoIme = @Ime";  // <-- parametrizovano

            Korisnik korisnik = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Ime", korisnickoIme);  // <-- parametar

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        korisnik = new Korisnik
                        {
                            korisnickoIme = reader["korisnickoIme"].ToString(),
                            tim = reader["tim"].ToString()
                           
                        };
                    }
                }
            }

            if (korisnik == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Korisnik sa imenom '{korisnickoIme}' nije pronađen.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, korisnik);
        }



        [HttpGet]
        [Route("api/korisnik/dohvatiOdgovornog")]
        public HttpResponseMessage dohvatiOdgovornog(string naziv)
        {
            string query = @"
        SELECT 
            korisnickoIme,
            lozinka,
            ime,
            prezime,
            jmbg,
            telefon,
            email,
            tim,
            datumRegistracije,
            poslednjaPrijava,
            aktivan
        FROM dbo.KORISNIK
        WHERE korisnickoIme = (select odgovoranKorisnik
from dbo.TIM where naziv = @Ime)";  // <-- parametrizovano

            Korisnik korisnik = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Ime", naziv);  // <-- parametar

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        korisnik = new Korisnik
                        {
                            korisnickoIme = reader["korisnickoIme"].ToString()
                        };
                    }
                }
            }

            if (korisnik == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Korisnik koji je odgovoran za tim '{naziv}' nije pronađen.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, korisnik);
        }



        [HttpGet]
        [Route("api/korisnik/dohvatiDetaljeKorisnika")]
        public HttpResponseMessage dohvatiDetaljeKorisnika(string korisnickoIme)
        {
            string query = @"
        SELECT TOP 1
            korisnickoIme,
            lozinka,
            ime,
            prezime,
            jmbg,
            telefon,
            email,
            tim,
            datumRegistracije,
            poslednjaPrijava,
            aktivan
        FROM dbo.KORISNIK
        WHERE korisnickoIme = @Ime";  // <-- parametrizovano

            Korisnik korisnik = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Ime", korisnickoIme);  // <-- parametar

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        korisnik = new Korisnik
                        {
                            korisnickoIme = reader["korisnickoIme"].ToString(),
                            lozinka = reader["lozinka"].ToString(),
                            ime = reader["ime"].ToString(),
                            prezime = reader["prezime"].ToString(),

                            // proveravamo nullable polja:
                            jmbg = reader["jmbg"] == DBNull.Value ? null : reader["jmbg"].ToString(),
                            telefon = reader["telefon"] == DBNull.Value ? null : reader["telefon"].ToString(),
                            email = reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                            tim = reader["tim"] == DBNull.Value ? null : reader["tim"].ToString(),

                            // datumRegistracije je NOT NULL, direktno konvertujemo
                            datumRegistracije = (DateTime)reader["datumRegistracije"] as DateTime?,

                            // poslednjaPrijava može biti NULL, pa proveravamo
                            poslednjaPrijava = reader["poslednjaPrijava"] as DateTime?,

                            // aktivan je NOT NULL bit
                            aktivan = (bool)reader["aktivan"]

                        };
                    }
                }
            }

            if (korisnik == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Korisnik sa imenom '{korisnickoIme}' nije pronađen.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, korisnik);
        }

        [Route("api/KORISNIK/sacuvajDetaljeKorisnika")]
        [HttpPut]
        public IHttpActionResult sacuvajDetaljeKorisnika(Korisnik kor)
        {
            string query = "";
            try
            {
                query = @"
                    update dbo.KORISNIK 
                    set  ime=@ime, prezime=@prezime,jmbg=@jmbg,telefon=@telefon,email=@email
                    where korisnickoIme=@kor ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@kor", kor.korisnickoIme);
                    cmd.Parameters.AddWithValue("@ime", kor.ime);
                    cmd.Parameters.AddWithValue("@prezime", kor.prezime);
                    cmd.Parameters.AddWithValue("@jmbg", kor.jmbg);
                    cmd.Parameters.AddWithValue("@telefon", kor.telefon);
                    cmd.Parameters.AddWithValue("@email", kor.email);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 1)
                        return Ok("updated successfully!");
                    else
                        return NotFound(); // If no row was deleted
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
        [HttpGet]
        [Route("api/korisnik/dohvatiSveAktivneAdmine")]
        public IHttpActionResult dohvatiSveAktivneAdmine()
        {
            List<Korisnik> kor = new List<Korisnik>();

            string query = @"
                   SELECT  
            korisnickoIme,
            lozinka,
            ime,
            prezime,
            jmbg,
            telefon,
            email,
            tim,
            datumRegistracije,
            poslednjaPrijava,
            aktivan    
            FROM dbo.KORISNIK where aktivan=1 and tim='sqladmin' ";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kor.Add(new Korisnik
                        {
                            korisnickoIme = reader["korisnickoIme"].ToString(),
                            lozinka = reader["lozinka"].ToString(),
                            ime = reader["ime"].ToString(),
                            prezime = reader["prezime"].ToString(),

                            // proveravamo nullable polja:
                            jmbg = reader["jmbg"] == DBNull.Value ? null : reader["jmbg"].ToString(),
                            telefon = reader["telefon"] == DBNull.Value ? null : reader["telefon"].ToString(),
                            email = reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                            tim = reader["tim"] == DBNull.Value ? null : reader["tim"].ToString(),

                            // datumRegistracije je NOT NULL, direktno konvertujemo
                            datumRegistracije = (DateTime)reader["datumRegistracije"] as DateTime?,

                            // poslednjaPrijava može biti NULL, pa proveravamo
                            poslednjaPrijava = reader["poslednjaPrijava"] as DateTime?,

                            // aktivan je NOT NULL bit
                            aktivan = (bool)reader["aktivan"]

                        });
                    }
                }
            }

            return Ok(kor);
        }

        [HttpGet]
        [Route("api/korisnik/dohvatiSveKorisnike")]
        public IHttpActionResult dohvatiSveKorisnike()
        {
            List<Korisnik> kor = new List<Korisnik>();

            string query = @"
                   SELECT  
            korisnickoIme,
            lozinka,
            ime,
            prezime,
            jmbg,
            telefon,
            email,
            tim,
            datumRegistracije,
            poslednjaPrijava,
            aktivan    
            FROM dbo.KORISNIK ";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kor.Add(new Korisnik
                        {
                            korisnickoIme = reader["korisnickoIme"].ToString(),
                            lozinka = reader["lozinka"].ToString(),
                            ime = reader["ime"].ToString(),
                            prezime = reader["prezime"].ToString(),

                            // proveravamo nullable polja:
                            jmbg = reader["jmbg"] == DBNull.Value ? null : reader["jmbg"].ToString(),
                            telefon = reader["telefon"] == DBNull.Value ? null : reader["telefon"].ToString(),
                            email = reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                            tim = reader["tim"] == DBNull.Value ? null : reader["tim"].ToString(),

                            // datumRegistracije je NOT NULL, direktno konvertujemo
                            datumRegistracije = (DateTime)reader["datumRegistracije"] as DateTime?,

                            // poslednjaPrijava može biti NULL, pa proveravamo
                            poslednjaPrijava = reader["poslednjaPrijava"] as DateTime?,

                            // aktivan je NOT NULL bit
                            aktivan = (bool)reader["aktivan"]

                        });
                    }
                }
            }

            return Ok(kor);
        }



        [Route("api/KORISNIK/aktiviraj")]
        [HttpPut]
        public IHttpActionResult aktiviraj(Korisnik kor)
        {
            string query = "";
            try
            {
                query = @"
                    update dbo.KORISNIK 
                    set aktivan=1
                    where korisnickoIme=@kor ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@kor", kor.korisnickoIme);
                    
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 1)
                        return Ok("updated successfully!");
                    else
                        return NotFound(); // If no row was deleted
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }



    }




}
