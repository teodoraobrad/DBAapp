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
    public class BACKUPController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBAapp"].ConnectionString;
        [HttpGet]
        [Route("api/backup/svi")]
        public IHttpActionResult GetAll()
        {
            List<BACKUP> bekapi = new List<BACKUP>();
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM [DBAapp].[dbo].[BACKUP] ", con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bekapi.Add(new BACKUP
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            IdBaze = reader["id_baze"] != DBNull.Value ? Convert.ToInt32(reader["id_baze"]) : 0,
                            Tip = reader["tip"] != DBNull.Value ? reader["tip"].ToString() : null,
                            Datum = reader["datum"] != DBNull.Value ? Convert.ToDateTime(reader["datum"]) : (DateTime?)null,
                            Putanja = reader["putanja"] != DBNull.Value ? reader["putanja"].ToString() : null,
                            VelicinaMB = reader["velicinaMB"] != DBNull.Value ? Convert.ToInt32(reader["velicinaMB"]) : 0,

                        });
                    }
                }
            }
            return Ok(bekapi);
        }
        [HttpGet]
        [Route("api/backup/{id}")]
        public IHttpActionResult GetDatabaseBck(int id)
        {
            List<BACKUP> bekapi = new List<BACKUP>();
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM [DBAapp].[dbo].[BACKUP] where id_baze=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bekapi.Add(new BACKUP
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            IdBaze = reader["id_baze"] != DBNull.Value ? Convert.ToInt32(reader["id_baze"]) : 0,
                            Tip = reader["tip"] != DBNull.Value ? reader["tip"].ToString() : null,
                            Datum = reader["datum"] != DBNull.Value ? Convert.ToDateTime(reader["datum"]) : (DateTime?)null,
                            Putanja = reader["putanja"] != DBNull.Value ? reader["putanja"].ToString() : null,
                            VelicinaMB = reader["velicinaMB"] != DBNull.Value ? Convert.ToInt32(reader["velicinaMB"]) : 0,
                        });
                    }
                }
            }
            return Ok(bekapi);
        }
       

        [HttpPut]
        [Route("api/backup/{id}")]
        public IHttpActionResult UpdateBackup(int id, [FromBody] BACKUP backup)
        {
            if (backup == null)
                return BadRequest("Podaci o backupu nisu prosleđeni.");

            string query = @"
        UPDATE [DBAapp].[dbo].[BACKUP]
        SET id_baze   = @id_baze,
            tip       = @tip,
            datum     = @datum,
            putanja   = @putanja,
            velicinaMB= @velicinaMB
        WHERE id = @id";

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id_baze", backup.IdBaze);
                cmd.Parameters.AddWithValue("@tip", (object)backup.Tip ?? DBNull.Value);

              
               
                cmd.Parameters.AddWithValue("@datum", (object)backup.Datum ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@putanja", (object)backup.Putanja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@velicinaMB", backup.VelicinaMB);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                    return NotFound();
            }

            return Ok("Backup je uspešno izmenjen.");
        }
        }
}