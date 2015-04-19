using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Npgsql;
using System.Configuration;
using System.Data;

namespace DAL
{
    public static class DBConnector
    {
        #region Users
        public static DataTable GetAllUsers(String sfld = "", String sord = "asc")
        {
            try
            {
                using (var db = new UserContext())
                {
                    var users = from a in db.Users
                                where a.UserName.StartsWith("A")
                                orderby a.UserName
                                select a;

                    return null;
                }
            }
            catch (Exception ex)
            {
                String s = ex.Message;
                int t = 4;
                return null;
            }
            //using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //{
            //    conn.Open();
            //    DataTable result = new DataTable();
            //    String query = "select Id, UserName,Mobile,Country,Active,UpdatedDate,UpdatedFrom from UserAccounts";
            //    if (sfld != "" && sfld != null)
            //        query += " order by " + sfld + " " + sord;

            //    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            //    {
            //        using (NpgsqlDataReader rdr = cmd.ExecuteReader())
            //        {
            //            for (int col = 0; col < rdr.FieldCount; col++)
            //                result.Columns.Add(new DataColumn(rdr.GetName(col)));
            //            result.Load(rdr);
            //            return result;
            //        }
            //    }
            //}
        }
        public static DataTable GetUserById(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                DataTable result = new DataTable();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select Id, UserName,Mobile,Country,Active,UpdatedDate,UpdatedFrom from UserAccounts where Id = @param", conn))
                {
                    cmd.Parameters.AddWithValue("@param", id);
                    using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                    {
                        for (int col = 0; col < rdr.FieldCount; col++)
                            result.Columns.Add(new DataColumn(rdr.GetName(col)));
                        result.Load(rdr);
                        return result;
                    }
                }
            }
        }
        public static void CreateUser(String username, String mobile, String country, Boolean active, String updatedfrom, String updatedby)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select createuser(@param0,@param1,@param2,@param3,@param4, @param5,@param6)", conn))
                {
                    cmd.Parameters.AddWithValue("@param0", username);
                    cmd.Parameters.AddWithValue("@param1", "password");
                    cmd.Parameters.AddWithValue("@param2", mobile);
                    cmd.Parameters.AddWithValue("@param3", country);
                    cmd.Parameters.AddWithValue("@param4", active);
                    cmd.Parameters.AddWithValue("@param5", updatedfrom);
                    cmd.Parameters.AddWithValue("@param6", updatedby);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateUser(int id, String username, String mobile, String country, Boolean active, String updatedfrom, String updatedby)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select updateuser(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)", conn))
                {
                    cmd.Parameters.AddWithValue("@param0", id);
                    cmd.Parameters.AddWithValue("@param1", username);
                    cmd.Parameters.AddWithValue("@param2", "password");
                    cmd.Parameters.AddWithValue("@param3", mobile);
                    cmd.Parameters.AddWithValue("@param4", country);
                    cmd.Parameters.AddWithValue("@param5", active);
                    cmd.Parameters.AddWithValue("@param6", updatedfrom);
                    cmd.Parameters.AddWithValue("@param7", updatedby);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteUser(int id, String updatedfrom, String updatedby)
        {
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select deleteuser(@param0, @param1, @param2)", conn))
                {
                    cmd.Parameters.AddWithValue("@param0", id);
                    cmd.Parameters.AddWithValue("@param1", updatedfrom);
                    cmd.Parameters.AddWithValue("@param2", updatedby);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}
