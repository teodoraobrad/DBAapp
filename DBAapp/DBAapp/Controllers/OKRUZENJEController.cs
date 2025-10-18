using DBAapp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace DBAapp.Controllers
{
    public class OKRUZENJEController : ApiController
    {


        public HttpResponseMessage Get()
        {
            string query = @"
                   select id,naziv,produkcija from dbo.OKRUZENJE";

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


        public string Post(OKRUZENJE okr)
        {
            int bit=0;
            if (okr.produkcija == true) bit = 1; else bit = 0;

            string query = @"
                    insert into dbo.OKRUZENJE(naziv,produkcija) values('" + okr.naziv + @"'," + bit + @")";
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
                return "Failed to Add!!"+query ;
            }
        }

        public string Put(OKRUZENJE okr)
        {
            try
            {
                string query = @"
                    update dbo.OKRUZENJE set naziv='" + okr.naziv + @"' where id="+okr.id+@"";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "updated Successfully!!";
            }
            catch (Exception)
            {
                return "Failed to update!!";
            }
        }

        public string Delete(int id)
        {
            string query = @"
                    delete from dbo.OKRUZENJE where id=" + id + @"
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
                return "Failed to delete!!"+query;
            }
        }

        [Route("api/OKRUZENJE/DohvatiSvaOkruzenja")]
        [HttpGet]
        public HttpResponseMessage DohvatiSvaOkruzenja()
        {
            string query = @"
                   select naziv from dbo.OKRUZENJE";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }





        [Route("api/OKRUZENJE/DohvatiNazivOkruzenja")]
        [HttpGet]
        public HttpResponseMessage DohvatiNazivOkruzenja(int id)
        {
            string query = @"
                   select naziv from dbo.OKRUZENJE where id=" + id ;

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        [Route("api/OKRUZENJE/TipOkruzenja")]
        [HttpGet]
        public IHttpActionResult TipOkruzenja(string naziv)
        {
            string query = @"
                   select produkcija from dbo.OKRUZENJE where naziv= @naziv";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@naziv", naziv);

                con.Open();
                var result = cmd.ExecuteScalar(); // Get a single value

                if (result != null && result != DBNull.Value)
                {
                    bool value;
                    if (bool.TryParse(result.ToString(), out value))
                    {
                        return Ok(value); // returns HTTP 200 with bool value
                    }
                }

                return NotFound(); // returns HTTP 404
            }


        }
        [Route("api/OKRUZENJE/TipOkruzenja/{id}")]
        [HttpGet]
        public IHttpActionResult TipOkruzenja(int id)
        {
            string query = @"
                   select produkcija from dbo.OKRUZENJE where id= @id";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                var result = cmd.ExecuteScalar(); // Get a single value

                if (result != null && result != DBNull.Value)
                {
                    bool value;
                    if (bool.TryParse(result.ToString(), out value))
                    {
                        return Ok(value); // returns HTTP 200 with bool value
                    }
                }

                return NotFound(); // returns HTTP 404
            }


        }

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;


        [HttpGet]
        [Route("api/OKRUZENJE/sva")]
        public IHttpActionResult DohvatiOkruzenja()
        {
            List<OKRUZENJE> okruzenja = new List<OKRUZENJE>();
            string query = "SELECT id, naziv, produkcija FROM dbo.OKRUZENJE";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        okruzenja.Add(new OKRUZENJE
                        {
                            id = (int)reader["id"],
                            naziv = reader["naziv"].ToString(),
                            produkcija = (bool)reader["produkcija"]
                        });
                    }
                }
            }

            // Ako nema podataka, vraća 204 No Content
            if (okruzenja.Count == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(okruzenja);
        }
    }
}
