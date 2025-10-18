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
using System.Web.Http.Results;

namespace DBAapp.Controllers
{
    public class DezurstvoController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // GET: api/DEZURSTVO/sva
        [HttpGet]
        [Route("api/DEZURSTVO/sva")]
        public IHttpActionResult GetSvaDezurstva()
        {
            List<DEZURSTVO> result = new List<DEZURSTVO>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM DEZURSTVO", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DEZURSTVO
                        {
                            datumOd = (DateTime)reader["datumOd"],
                            datumDo = (DateTime)reader["datumDo"],
                            dezurni = reader["dezurni"].ToString(),
                            postavioKorisnik = reader["postavioKorisnik"]?.ToString(),
                            datumUnosa = reader["datumUnosa"] as DateTime?
                        });
                    }
                }
            }

            return Ok(result);
        }

        // GET: api/DEZURSTVO/teodora
        [HttpGet]
        [Route("api/DEZURSTVO/{id}")]
        public IHttpActionResult GetDezurni(string id)
        {
            
            List<DEZURSTVO> result = new List<DEZURSTVO>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM DEZURSTVO WHERE dezurni = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DEZURSTVO
                        {
                            datumOd = (DateTime)reader["datumOd"],
                            datumDo = (DateTime)reader["datumDo"],
                            dezurni = reader["dezurni"].ToString(),
                            postavioKorisnik = reader["postavioKorisnik"]?.ToString(),
                            datumUnosa = reader["datumUnosa"] as DateTime?
                        });
                    }
                }
            }

            

            return Ok(result);
        }

        // POST: api/DEZURSTVO
        [HttpPost]
        [Route("api/DEZURSTVO")]
        public IHttpActionResult CreateDezurstvo([FromBody] DEZURSTVO model)
        {


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // 1. Provera preklapanja
                using (SqlCommand checkCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM DEZURSTVO
            WHERE dezurni = @dezurni
              AND NOT (datumDo < @datumOd OR datumOd > @datumDo)", con))
                {
                    checkCmd.Parameters.AddWithValue("@dezurni", model.dezurni);
                    checkCmd.Parameters.AddWithValue("@datumOd", model.datumOd);
                    checkCmd.Parameters.AddWithValue("@datumDo", model.datumDo);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return BadRequest("Period se preklapa sa već postojećim dežurstvom za ovog korisnika.");
                    }
                }

                // 2. Insert ako nema preklapanja
                using (SqlCommand cmd = new SqlCommand(@"
            INSERT INTO DEZURSTVO (datumOd, datumDo, dezurni, postavioKorisnik)
            VALUES (@datumOd, @datumDo, @dezurni, @postavioKorisnik)", con))
                {
                    cmd.Parameters.AddWithValue("@datumOd", model.datumOd);
                    cmd.Parameters.AddWithValue("@datumDo", model.datumDo);
                    cmd.Parameters.AddWithValue("@dezurni", model.dezurni);
                    cmd.Parameters.AddWithValue("@postavioKorisnik", (object)model.postavioKorisnik ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            return Ok("Dežurstvo uspešno dodato.");
        }


        /*   [HttpPut]
           [Route("api/DEZURSTVO/promeni")]
           public IHttpActionResult PromeniDezurnog([FromBody] dezurstvoupdaterequest request)
           {
               var model1 = request.Novi;
               var model2 = request.Stari;
               using (SqlConnection con = new SqlConnection(connectionString))
               using (SqlCommand cmd = new SqlCommand(@"
                   UPDATE DEZURSTVO SET
                       dezurni = @dezurni,
                       datumOd = @datumOd ,
                       datumDo = @datumDo ,
                       postavioKorisnik = @postavioKorisnik,
                       datumUnosa = getdate()
                   WHERE 
                       dezurni = @dezurni2 and datumOd = @datumOd2 and
                       datumDo = @datumDo2 ", con))
               {
                   cmd.Parameters.AddWithValue("@datumOd", model1.datumOd);
                   cmd.Parameters.AddWithValue("@datumDo", model1.datumDo);
                   cmd.Parameters.AddWithValue("@dezurni", model1.dezurni);
                   cmd.Parameters.AddWithValue("@postavioKorisnik", (object)model1.postavioKorisnik ?? DBNull.Value);
                   cmd.Parameters.AddWithValue("@datumOd2", model2.datumOd);
                   cmd.Parameters.AddWithValue("@datumDo2", model2.datumDo);
                   cmd.Parameters.AddWithValue("@dezurni2", model2.dezurni);

                   con.Open();
                   int affected = cmd.ExecuteNonQuery();

                   if (affected == 0)
                       return NotFound();
               }

               return Ok("Dežurstvo uspešno izmenjeno.");
           }{
     "novi": {
           "datumOd": "2025-09-26T00:00:00",
           "datumDo": "2025-09-30T00:00:00",
           "dezurni": "teodora",
           "postavioKorisnik": "teodora"
       },
      "stari": {
            "datumOd": "2025-09-26T00:00:00",
           "datumDo": "2025-09-29T00:00:00",
           "dezurni": "teodora"

       }}

         */

        // DELETE: api/DEZURSTVO/
        [HttpDelete]
        [Route("api/DEZURSTVO/")]
        public IHttpActionResult DeleteDezurstvo([FromBody] DEZURSTVO model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM DEZURSTVO WHERE datumOd = @datumOd and  datumDo = @datumDo and dezurni = @dezurni ", con))
            {
                cmd.Parameters.AddWithValue("@datumOd", model.datumOd);
                cmd.Parameters.AddWithValue("@datumDo", model.datumDo);
                cmd.Parameters.AddWithValue("@dezurni", model.dezurni);

                con.Open();
                int affected = cmd.ExecuteNonQuery();

                if (affected == 0)
                    return NotFound();
            }

            return Ok("Dežurstvo uspešno obrisano.");
        }




    }
}