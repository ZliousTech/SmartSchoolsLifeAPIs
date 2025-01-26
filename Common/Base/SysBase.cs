using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Common.Base
{
    public class SysBase
    {
        public static string GetLastScheduledTripDate(int SchoolID)
        {
            string LSTripDate = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable ScheduledTripInfo =
                GetDataTble("SELECT TOP 1 * " +
                            "FROM ScheduledBusTrips " +
                            "WHERE SchoolID = '" + SchoolID + "' " +
                            "ORDER BY TripPassengerID DESC");


            if (ScheduledTripInfo != null)
            {
                if (ScheduledTripInfo.Rows.Count > 0)
                {
                    LSTripDate = ScheduledTripInfo.Rows[0]["TripDate"].ToString();
                }
            }

            return LSTripDate;
        }

        public static DataTable GetDataTble(string cmdStr)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (Conn.State == ConnectionState.Closed) Conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            da.Fill(ds);
            dt = ds.Tables[0];
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                cmd.Cancel();
                da.Dispose();
                ds.Dispose();
            }
            return dt;
        }
    }
}
