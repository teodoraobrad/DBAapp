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
{[RoutePrefix("api/RESTART")]
    public class RESTARTController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        [HttpPost]
        [Route("dodaj")]
        public IHttpActionResult InsertRestart([FromBody] RESTART restart)
        {
            string query = @"
            INSERT INTO dbo.RESTART (hostname, komanda,  korisnik)
            VALUES (@hostname, @komanda, @korisnik)";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@hostname", (object)restart.Hostname ?? DBNull.Value);
                string kom = "Restart-Computer -ComputerName \"" + restart.Hostname + "\" -Force -ErrorAction Stop";
                cmd.Parameters.AddWithValue("@komanda", kom);
                cmd.Parameters.AddWithValue("@korisnik", (object)restart.Korisnik ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Restart uspešno dodat.");
        }

        
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAll()
        {
            List<RESTART> restarts = new List<RESTART>();
            string query = "SELECT hostname, komanda, uneto, korisnik FROM dbo.RESTART";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        restarts.Add(new RESTART
                        {
                            Hostname = reader["hostname"] == DBNull.Value ? null : reader["hostname"].ToString(),
                            Komanda = reader["komanda"] == DBNull.Value ? null : reader["komanda"].ToString(),
                            Uneto = (DateTime)reader["uneto"],
                            Korisnik = reader["korisnik"] == DBNull.Value ? null : reader["korisnik"].ToString()
                        });
                    }
                }
            }

            return Ok(restarts);
        }

        
        [HttpGet]
        [Route("najskoriji/{hostname}")]
        public IHttpActionResult GetByHostname(string hostname)
        {
            RESTART restart = null;
            string query = "SELECT top(1) hostname, komanda, uneto, korisnik FROM dbo.RESTART WHERE hostname = @hostname order by uneto desc";
 
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@hostname", hostname);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        restart = new RESTART
                        {
                            Hostname = reader["hostname"] == DBNull.Value ? null : reader["hostname"].ToString(),
                            Komanda = reader["komanda"] == DBNull.Value ? null : reader["komanda"].ToString(),
                            Uneto = (DateTime)reader["uneto"],
                            Korisnik = reader["korisnik"] == DBNull.Value ? null : reader["korisnik"].ToString()
                        };
                    }
                }
            }

            if (restart == null)
                return NotFound();

            return Ok(restart);
        }
        [HttpGet]
        [Route("svi/{hostname}")]
        public IHttpActionResult GetAllByHostname(string hostname)
        {
            List<RESTART> restarts = new List<RESTART>();
            string query = "SELECT  hostname, komanda, uneto, korisnik FROM dbo.RESTART WHERE hostname = @hostname order by uneto desc";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@hostname", hostname);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        restarts.Add(new RESTART
                        {
                            Hostname = reader["hostname"] == DBNull.Value ? null : reader["hostname"].ToString(),
                            Komanda = reader["komanda"] == DBNull.Value ? null : reader["komanda"].ToString(),
                            Uneto = (DateTime)reader["uneto"],
                            Korisnik = reader["korisnik"] == DBNull.Value ? null : reader["korisnik"].ToString()
                        });
                    }
                }
            }

          

            return Ok(restarts);
        }



    }
}