using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Labo03
{
    public static class registration
    {
        [FunctionName("registration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string con = Environment.GetEnvironmentVariable("connectionstring");
                string json = await new StreamReader(req.Body).ReadToEndAsync();
                registrate newReg = JsonConvert.DeserializeObject<registrate>(json);
                newReg.RegistrationID = Guid.NewGuid().ToString();

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = con;
                    await conn.OpenAsync();

                    using (SqlCommand com = new SqlCommand())
                    {
                        com.Connection = conn;
                        com.CommandText = "insert into TblRegistration values(@regID, @lastname, @firstname" +
                            ",@email, @ziocode, @age, @isfirsttime) ";
                        com.Parameters.AddWithValue("@regID", newReg.RegistrationID);
                        com.Parameters.AddWithValue("@lastname", newReg.LastName);
                        com.Parameters.AddWithValue("@firstname", newReg.FirstName);
                        com.Parameters.AddWithValue("@email", newReg.Email);
                        com.Parameters.AddWithValue("@ziocode", newReg.ZipCode);
                        com.Parameters.AddWithValue("@age", newReg.Age);
                        com.Parameters.AddWithValue("@isfirsttime", newReg.IsFirstTimer);

                        await com.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult(newReg);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Addregistration");
                return new StatusCodeResult(500);
            }

  
        }//end static async
    }//end class
}//end namespace
