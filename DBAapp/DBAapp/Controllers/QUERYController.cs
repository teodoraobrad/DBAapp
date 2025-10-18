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
    public class QUERYController : ApiController
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // GET api/query
        [HttpGet]
        [Route("api/query")]
        public IEnumerable<QUERY> GetAll()
        {
            List<QUERY> list = new List<QUERY>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Naslov, Upit FROM QUERY", con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new QUERY
                    {
                        Id = (int)dr["Id"],
                        Naslov = dr["Naslov"].ToString(),
                        Upit = dr["Upit"].ToString()
                    });
                }
            }

            return list;
        }

        // GET api/query/5
        [HttpGet]
        [Route("api/query/{id}")]
        public IHttpActionResult GetById(int id)
        {
            QUERY query = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Naslov, Upit FROM QUERY WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    query = new QUERY
                    {
                        Id = (int)dr["Id"],
                        Naslov = dr["Naslov"].ToString(),
                        Upit = dr["Upit"].ToString()
                    };
                }
            }

            if (query == null) return NotFound();

            return Ok(query);
        }

        // POST api/query
        [HttpPost]
        [Route("api/query")]
        public IHttpActionResult Create([FromBody] QUERY model)
        {
            if (model == null) return BadRequest("Invalid data.");

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
            INSERT INTO QUERY (Naslov, Upit)
            VALUES (@Naslov, @Upit)", con))
            {
                cmd.Parameters.AddWithValue("@Naslov", model.Naslov);
                cmd.Parameters.AddWithValue("@Upit", model.Upit);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("Query uspešno kreiran.");
        }

        // PUT api/query/5
        [HttpPut]
        [Route("api/query/{id}")]
        public IHttpActionResult Update(int id, [FromBody] QUERY model)
        {
            if (model == null) return BadRequest("Invalid data.");

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
            UPDATE QUERY
            SET Naslov = @Naslov, Upit = @Upit
            WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Naslov", model.Naslov);
                cmd.Parameters.AddWithValue("@Upit", model.Upit);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound();
            }

            return Ok("Query uspešno izmenjen.");
        }

        // DELETE api/query/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM QUERY WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) return NotFound();
            }

            return Ok("Query obrisan.");
        }

    }
}