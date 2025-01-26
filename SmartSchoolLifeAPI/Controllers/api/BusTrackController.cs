using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{

    public class BusTrackController : ApiController
    {
        SmartSchoolsEntities1 db;

        public BusTrackController()
        {
            db = new SmartSchoolsEntities1();
        }

        [HttpGet]
        public void AddBusData(int SchoolID, string BusNumber, string MoveDate, string MoveTime, double Longitude, double Latitude, int CompanyID, int TripNumPerDay, string BusSpeed, string RoadSpeed)
        {
            /*var btn = new BusTrackNavigation();
            btn.SchoolID = SchoolID;
            btn.BusNumber = BusNumber;
            btn.MoveDate = MoveDate;
            btn.MoveTime = MoveTime;
            btn.Longitude = Longitude;
            btn.Latitude = Latitude;
            btn.CompanyID = CompanyID;
            btn.TripNumPerDay = TripNumPerDay;*/
            /*btn.BusStatus = "";
            btn.BusSpeed = "";
            btn.RoadSpeed = "";
            btn.TripStatus = "";*/

            //db.BusTrackNavigations.Add(btn);
            /* Check to prevent the duplicated records */
            ////DataTable LocationInfo = GetDataTble("SELECT CNT = COUNT(*) " +
            ////                                     "FROM TRACK_" + SchoolID + " " +
            ////                                     "WHERE SchoolID = " + SchoolID + " " +
            ////                                     "AND BusNumber = '" + BusNumber + "' " +
            ////                                     "AND MoveDate = '" + MoveDate + "' " +
            ////                                     "AND MoveTime = '" + MoveTime + "' " +
            ////                                     "AND CompanyID = " + CompanyID);
            ////if (LocationInfo != null)
            ////{
            ////    if (LocationInfo.Rows.Count > 0)
            ////    {
            ////        if (int.Parse(LocationInfo.Rows[0]["CNT"].ToString()) > 0)
            ////        {
            ////            return;
            ////        }
            ////    }
            ////}
            /************ Added in 14-11-2023 ************/

            db.Database.ExecuteSqlCommand("insert into TRACK_" + SchoolID +
                "(SchoolID, BusNumber, MoveDate, MoveTime, Longitude, Latitude, CompanyID, TripNumPerDay, BusSpeed, RoadSpeed) " +
                " values " +
                "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                 SchoolID, BusNumber, MoveDate, MoveTime, Longitude, Latitude, CompanyID, TripNumPerDay, BusSpeed, RoadSpeed);
            db.SaveChanges();
        }

        [HttpGet]
        public IEnumerable<BusTrackNavigation> GetTrack(string BusNumber, string MoveDate)
        {
            return db.BusTrackNavigations.Where(x => x.BusNumber == BusNumber && x.MoveDate == MoveDate).ToList();

        }

        [HttpGet]
        public int GetMaxTripNumPerDay(int SchoolID, string BusNumber, string MoveDate)
        {
            var maxid = 1;
            var con = new SqlConnection("server=.;uid=SMARTSCHOOLTRACK;pwd=V1M7yNTzNPJzBZ;database=SmartSchoolsTrack");
            con.Open();
            var cmd = new SqlCommand("select max(TripNumPerDay) from track_" + SchoolID + " where SchoolID=" + SchoolID + " and BusNumber='" + BusNumber + "' and MoveDate='" + MoveDate + "'", con);
            if (cmd.ExecuteScalar() != DBNull.Value)
                maxid = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            con.Close();

            return maxid;
        }

        /*public IEnumerable<TripNumberPerDayMV> GetMaxTripNumPerDay(int SchoolID, string BusNumber, string MoveDate)
        {
            var maxId = 0;
            //try
            //{
                //maxId = (from b in db.BusTrackNavigations where b.SchoolID == SchoolID && b.BusNumber == BusNumber && b.MoveDate == MoveDate select b.TripNumPerDay).Max();
                var obj = db.Database.SqlQuery<TripNumberPerDayMV>("select max(TripNumPerDay) from track_" + SchoolID + " where SchoolID=" + SchoolID + " and BusNumber='" + BusNumber + "' and MoveDate='" + MoveDate + "'").ToList();
                maxId = obj[0].MaxID; 
                
            }
            catch
            {
                maxId = 0;
            }
            return obj;
             
             
        }*/

        private DataTable GetDataTble(string cmdStr)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTRTRACK"].ConnectionString);
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
