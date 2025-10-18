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
    public class SQL_SERVERController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;

        // 1️⃣ Dohvati sve SQL servere
        [HttpGet]
        [Route("api/sqlserver/svi")]
        public IHttpActionResult GetAll()
        {
            List<SQL_SERVER> servers = new List<SQL_SERVER>();
            string query = @"SELECT [id], [id_servera], [naziv], [verzija], [edicija], [verzija1], [kolacija], 
                                [port], [klaster], [aktivan], [datumInstalacije], [nalog], [status]
                         FROM [DBAapp].[dbo].[SQL_SERVER]";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        servers.Add(new SQL_SERVER
                        {
                            Id = (int)reader["id"],
                            IdServera = (int)reader["id_servera"],
                            Naziv = reader["naziv"].ToString(),
                            Verzija = reader["verzija"]?.ToString(),
                            Edicija = reader["edicija"]?.ToString(),
                            Verzija1 = reader["verzija1"]?.ToString(),
                            Kolacija = reader["kolacija"]?.ToString(),
                            Port = reader["port"] == DBNull.Value ? null : (int?)reader["port"],
                            Klaster = reader["klaster"]?.ToString(),
                            Aktivan = (bool)reader["aktivan"],
                            DatumInstalacije = reader["datumInstalacije"] == DBNull.Value ? null : (DateTime?)reader["datumInstalacije"],
                            Nalog = reader["nalog"]?.ToString(),
                            Status = reader["status"]?.ToString()
                        });
                    }
                }
            }
            return Ok(servers);
        }

        
        [HttpPost]
        [Route("api/sqlserver/dodaj")]
        public IHttpActionResult AddServer([FromBody] SQL_SERVER server)
        {
            string query = @"INSERT INTO [DBAapp].[dbo].[SQL_SERVER]
                         (id_servera, naziv, verzija, edicija, verzija1, kolacija, port, klaster, aktivan, nalog, status)
                         VALUES
                         (@id_servera, @naziv, @verzija, @edicija, @verzija1, @kolacija, @port, @klaster, @aktivan,  @nalog, @status)";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id_servera", server.IdServera);
                cmd.Parameters.AddWithValue("@naziv", server.Naziv ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@verzija", server.Verzija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@edicija", server.Edicija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@verzija1", server.Verzija1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kolacija", server.Kolacija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@port", server.Port ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@klaster", server.Klaster ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@aktivan", server.Aktivan);
                cmd.Parameters.AddWithValue("@nalog", server.Nalog ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", server.Status ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("SQL server uspešno dodat.");
        }

        
        [HttpPut]
        [Route("api/sqlserver/update/{id}")]
        public IHttpActionResult UpdateServer(int id, [FromBody] SQL_SERVER server)
        {
            string query = @"UPDATE [DBAapp].[dbo].[SQL_SERVER] SET
                            id_servera = @id_servera,
                            naziv = @naziv,
                            verzija = @verzija,
                            edicija = @edicija,
                            verzija1 = @verzija1,
                            kolacija = @kolacija,
                            port = @port,
                            klaster = @klaster,
                            aktivan = @aktivan,
                            nalog = @nalog,
                            status = @status
                         WHERE id = @id";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@id_servera", server.IdServera);
                cmd.Parameters.AddWithValue("@naziv", server.Naziv ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@verzija", server.Verzija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@edicija", server.Edicija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@verzija1", server.Verzija1 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kolacija", server.Kolacija ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@port", server.Port ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@klaster", server.Klaster ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@aktivan", server.Aktivan);
                cmd.Parameters.AddWithValue("@nalog", server.Nalog ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", server.Status ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Ok("SQL server uspešno ažuriran.");
        }
    }
}