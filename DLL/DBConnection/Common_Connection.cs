using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using Common;

namespace DLL.DBConnection
{
    public class Common_Connection
    {
        #region connection
        public SqlConnection getConn()
        {
            //var pass = decriptConnection(ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString);
            var pass = ConfigurationManager.ConnectionStrings["DbconnectionKGID"].ConnectionString;
            //var encrypt = Encrypt("kgid@123");
            SqlConnection con = new SqlConnection(pass);
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "getConn():" + ex.Message);
            }
            return con;
        }
        #endregion
        #region ExecuteCommand
        public string ExecuteCmd(SqlParameter[] sqlParm, string sp)
        {
            string strReturnValue = "";
            SqlConnection con = new SqlConnection();
            con = getConn();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;
                foreach (SqlParameter LoopVar_param in sqlParm)
                {
                    cmd.Parameters.Add(LoopVar_param);
                }
                cmd.Parameters.Add("@Output", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                strReturnValue = (cmd.Parameters["@Output"].Value == DBNull.Value) ? string.Empty : cmd.Parameters["@Output"].Value.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "ExecuteCmd-SP->" + sp + "-Errors->" + ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return strReturnValue;
        }
        #endregion
        #region ExecuteCommandForSMS
        public string ExecuteCmdForSMS(SqlParameter[] sqlParm, string sp)
        {
            string strReturnValue = "";
            SqlConnection con = new SqlConnection();
            con = getConn();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;
                foreach (SqlParameter LoopVar_param in sqlParm)
                {
                    cmd.Parameters.Add(LoopVar_param);
                }
                cmd.Parameters.Add("@Output", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                strReturnValue = (cmd.Parameters["@Output"].Value == DBNull.Value) ? string.Empty : cmd.Parameters["@Output"].Value.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "ExecuteCmdForSMS-SP->" + sp + "-Errors->" + ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return strReturnValue;
        }
        #endregion
        #region Execute Dataset
        public DataSet ExeccuteDataset(SqlParameter[] sqlParm, string sp)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection();
            try
            {
                con = getConn();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;
                foreach (SqlParameter LoopVar_param in sqlParm)
                {
                    cmd.Parameters.Add(LoopVar_param);
                }

                //cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "ExecuteCmd-SP->" + sp + "-Errors->" + ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ds;
        }
        #endregion
        #region Execute Datattable
        public DataTable ExeccuteDatatablet(SqlParameter[] sqlParm, string sp)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection();
                con = getConn();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp;
                foreach (SqlParameter LoopVar_param in sqlParm)
                {
                    cmd.Parameters.Add(LoopVar_param);
                }
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                Logger.LogMessage(TracingLevel.INFO, "ExeccuteDatatablet-SP->" + sp + "-Errors->" + ex.Message);
            }
            return dt;
        }
        #endregion



        public static string decriptConnection(string connectionString)
        {

            string sqlConn = connectionString;
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(sqlConn);
            string dataSource = connBuilder.DataSource;
            if (connBuilder.UserID != "" && connBuilder.Password != "")
            {
                string userID = connBuilder.UserID;
                string password = Decrypt(connBuilder.Password);
                string db = connBuilder.InitialCatalog;
                string connectionstring = "Data Source=" + dataSource + ";Initial Catalog=" + db + ";User ID=" + userID + ";Password=" + password + "";
                return connectionstring;

            }
            else
            {
                return sqlConn;
            }


        }
        private static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
