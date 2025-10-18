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
   
    public class TIP_ZAHTEVAController : ApiController
    { private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        [HttpGet]
        [Route("api/TIP_ZAHTEVA")]
        public IHttpActionResult GetTipoviZahteva()
        {
            List<TIP_ZAHTEVA> tipovi = new List<TIP_ZAHTEVA>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM TIP_ZAHTEVA", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipovi.Add(new TIP_ZAHTEVA
                        {
                            id = (int)reader["id"],

                            ime = reader["ime"].ToString(),

                        });
                    }
                }
            }

            return Ok(tipovi);
        }


        [HttpGet]
        [Route("api/TIP_ZAHTEVA/{id}")]
        public IHttpActionResult GetTipZahteva(int id)
        {
            TIP_ZAHTEVA ZAHTEV = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT ime FROM TIP_ZAHTEVA WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ZAHTEV = new TIP_ZAHTEVA
                        {
                            id = id,
                            ime = reader["ime"].ToString(),

                        };
                    }
                }
            }

            if (ZAHTEV == null)
                return NotFound();

            return Ok(ZAHTEV);
        }
    }
}