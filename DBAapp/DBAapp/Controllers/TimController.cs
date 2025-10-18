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
    public class TimController : ApiController
    {
        
        public HttpResponseMessage Get()
        {
            string query = @"
                   select naziv,admin,odgovoranKorisnik from dbo.TIM";

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
/*
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
*/
        // POST api/<controller>
        public string Post(TIM tim)
        {
          
            string query = @"
                    insert into dbo.TIM(naziv) values('" + tim.naziv + @"')";
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
                return "Failed to Add!!" + query;
            }

        }

        // PUT api/<controller>/5
        public string Put(TIM tim)
        {
            string query=""; 
            try
            {
                int bit = (tim.admin) ? 1 : 0;
                query = @"
                    update dbo.TIM set admin=" + bit + @", odgovoranKorisnik='"+tim.odgovoranKorisnik+@"' where naziv='" + tim.naziv + @"'";

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
                return "Failed to update!!"+ query;
            }
        }

        // DELETE api/<controller>/5
        public string Delete(string id)
        {
            string query = @"
                    delete from dbo.TIM where naziv='" + id + @"'
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
            }
        }
    }
}