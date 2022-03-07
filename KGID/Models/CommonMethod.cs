using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace KGID.Models
{
    public class CommonMethod
    {
        public void ErrorHandler(string ErrorMsg, int LineNo)
        {
            try
            {
                var relativePath = "~/uploads/LogFiles/" + DateTime.Now.ToShortDateString() + "-LogFile.txt";
                string absolutePath = Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
                string Text = string.Empty;
                if (!System.IO.File.Exists(absolutePath))
                {
                    using (FileStream fs = System.IO.File.Create(absolutePath))
                    {
                        Text = "Errors: " + Environment.NewLine + Environment.NewLine;
                        System.IO.File.AppendAllText(absolutePath, Text);
                    }
                }
                Text = Environment.NewLine + "Error : " + Environment.NewLine + "Error Message : " + ErrorMsg + Environment.NewLine + "Line No : " + LineNo + Environment.NewLine + "Date and Time : " + DateTime.Now + Environment.NewLine + "Page Title : dashboard" + "----------------------------------------------------------------------------------------------";
                System.IO.File.AppendAllText(absolutePath, Text);
            }
            catch
            {

            }
        }
        public DataSet ExecuteDataset(string connectionString, string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(ds);
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
                //return ds;
            }
        }

        public int ExecuteNonQuery(string connetionString, string sql)
        {
            SqlConnection cnn;
            SqlCommand cmd;
            cnn = new SqlConnection(connetionString);
            Int32 count = 0;

            try
            {
                cnn.Open();
                cmd = new SqlCommand(sql, cnn);
                count = Convert.ToInt32(cmd.ExecuteNonQuery());
                cmd.Dispose();
                cnn.Close();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}