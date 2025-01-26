using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.ViewModels;
using Newtonsoft.Json;
using SmartSchoolLifeAPI.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class BusController : ApiController
    {
        SmartSchoolsEntities2 db;

        public BusController()
        {
            db = new SmartSchoolsEntities2();
        }

        public IEnumerable<BusStudentMV> GetStudents(int SchoolID, string SchoolYear,string BusNumber, string TripDate, string TripTime)
        {
            //Get last schaduale trip date
            string LastScheduledTripDate = GetLastScheduledTripDate(SchoolID);
            TripDate = LastScheduledTripDate;

            var q = (from s in db.Students
                     join b in db.ScheduledBusTrips
                     on s.StudentID equals b.PassengerID
                     join d in db.DeviceRegistrars
                     on s.GuardianID.ToString() equals d.OwnerID into deviceRegistrars
                     from dr in deviceRegistrars.DefaultIfEmpty()
                     where b.SchoolID == SchoolID && b.BusNumber == BusNumber
                     && b.TripDate == TripDate && b.TripTime == TripTime
                     select new BusStudentMV
                     {
                         PassengerID = b.PassengerID,
                         PassengerName = b.PassengerName,
                         PassengerLongitude = b.PassengerLongitude,
                         PassengerLatitude = b.PassengerLatitude,
                         SchoolLongitude = b.SchoolLongitude,
                         SchoolLatitude = b.SchoolLatitude,
                         AttendantLongitude = b.AttendantLongitude,
                         AttendantLatitude = b.AttendantLatitude,
                         GuardianID = s.GuardianID,
                         AttendantID = b.AttendantID,
                         DeviceRegistrationCode = dr.DeviceRegistrationCode
                     });


            //-- Orginal
            //var q = (from s in db.Students
            //        join b in db.ScheduledBusTrips
            //        on s.StudentID equals b.PassengerID join d in db.DeviceRegistrars 
            //        on s.GuardianID.ToString() equals d.OwnerID 
            //        where b.SchoolID == SchoolID && b.SchoolYear == SchoolYear && b.BusNumber == BusNumber
            //        && b.TripDate == TripDate && b.TripTime == TripTime
            //        select new BusStudentMV
            //        {
            //            PassengerID = b.PassengerID,
            //            PassengerName =b.PassengerName,
            //            PassengerLongitude = b.PassengerLongitude,
            //            PassengerLatitude = b.PassengerLatitude,
            //            SchoolLongitude = b.SchoolLongitude,
            //            SchoolLatitude = b.SchoolLatitude,
            //            AttendantLongitude = b.AttendantLongitude,
            //            AttendantLatitude = b.AttendantLatitude,
            //            GuardianID = s.GuardianID,
            //            AttendantID = b.AttendantID,
            //            DeviceRegistrationCode = d.DeviceRegistrationCode

            //        });
            return q;
        }

        //public IEnumerable<BusStudentMV> GetTimes(int SchoolID, string SchoolYear, string BusNumber, string TripDate)
        //{
        //    return db.ScheduledBusTrips.Where(x => x.SchoolID == SchoolID && x.SchoolYear == SchoolYear
        //    && x.BusNumber == BusNumber && x.TripDate == TripDate)
        //         .Select(x => new BusStudentMV
        //         {
        //             TripTime = x.TripTime,
        //             Direction = x.Direction
        //         }).Distinct().ToList();
        //}

        public IEnumerable<BusStudentMV> GetTimes(int SchoolID, string BusNumber)
        {
            string SchoolYear = "";
            string TripDate = "";

            SqlConnection DBConnection;
            SqlCommand CMD;
            SqlDataReader DR;

            DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (DBConnection.State == ConnectionState.Closed) DBConnection.Open();

            CMD = new SqlCommand("SELECT TOP(1) SchoolYear, TripDate FROM ScheduledBusTrips " +
                                 "WHERE SchoolID = @SchoolID AND BusNumber = @BusNumber " +
                                 "ORDER BY TripPassengerID DESC", DBConnection);
            CMD.Parameters.AddWithValue("@SchoolID", SchoolID);
            CMD.Parameters.AddWithValue("@BusNumber", BusNumber);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                if (DR.HasRows)
                {
                    SchoolYear = DR["SchoolYear"].ToString();
                    TripDate = DR["TripDate"].ToString();
                }
            }

            if (!DR.IsClosed) DR.Close();
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }

            return db.ScheduledBusTrips.Where(x => x.SchoolID == SchoolID && x.SchoolYear == SchoolYear
            && x.BusNumber == BusNumber && x.TripDate == TripDate)
                 .Select(x => new BusStudentMV
                 {
                    TripTime = x.TripTime, Direction = x.Direction
                 }).Distinct().ToList();
        }

        public IEnumerable<BusStudentMV> GetTimesByAttendant(string TripDate, string AttendantID, string SchoolID)
        {
            string LastTripDate = GetLastScheduledTripDate(int.Parse(SchoolID));
            return db.ScheduledBusTrips.Where(x => x.TripDate == LastTripDate && x.AttendantID == AttendantID)
                 .Select(x => new BusStudentMV
                 {
                     TripTime = x.TripTime,
                     Direction = x.Direction,
                     BusNumber = x.BusNumber

                 }).Distinct().ToList();

            //return db.ScheduledBusTrips.Where(x => x.TripDate == TripDate && x.AttendantID == AttendantID)
            //     .Select(x => new BusStudentMV
            //     {
            //         TripTime = x.TripTime,
            //         Direction = x.Direction,
            //         BusNumber = x.BusNumber

            //     }).Distinct().ToList();
        }

        //public IEnumerable<BusStudentMV> GetTimesByAttendant(string TripDate, string AttendantID)
        //{
        //    List <BusStudentMV> busStudentMVs = new List<BusStudentMV>();
        //    string conStr = ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;
        //    using(SqlConnection conn = new SqlConnection(conStr))
        //    {
        //        string query = "SELECT * FROM ScheduledBusTrips " +
        //            "WHERE AttendantID = @AttendantID AND TripDate = @TripDate";
        //        conn.Open();
        //        using(SqlCommand comm = new SqlCommand(query, conn))
        //        {
        //            comm.Parameters.AddWithValue("@AttendantID", AttendantID);
        //            comm.Parameters.AddWithValue("@TripDate", TripDate);
        //            using(SqlDataReader reader = comm.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    busStudentMVs.Add(new BusStudentMV
        //                    {
        //                        TripTime = reader["TripTime"].ToString(),
        //                        Direction = int.Parse(reader["Direction"].ToString()),
        //                        BusNumber = reader["BusNumber"].ToString()
        //                    });
        //                }
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return busStudentMVs.Distinct().ToList();
        //}

        public IEnumerable<BusStudentMV> GetStudentsByAttendant(string TripDate,string TripTime, string AttendantID)
        {
            var q = (from b in db.ScheduledBusTrips join s in db.Students
            on b.PassengerID equals s.StudentID
                    where b.TripDate == TripDate && b.TripTime == TripTime && b.AttendantID == AttendantID
                select new BusStudentMV
                {
                    PassengerID = b.PassengerID,
                    PassengerName =b.PassengerName,
                    PassengerLongitude = b.PassengerLongitude,
                    PassengerLatitude = b.PassengerLatitude,
                    PassengerOnBoard = b.PassengerOnBoard,
                    PassengerOffBoard = b.PassengeroffBoard,
                    GuardianID = s.GuardianID
                    
                });

            return q;
        }
        [HttpGet]
        public void UpdateLoc(double longitude,double latitude,string studentid)
        {
            db.Database.ExecuteSqlCommand("update StudentAdresses set longitude={0},latitude={1} where AddressID = (select AddressID from Student where StudentID={2})", longitude,latitude,studentid);
            db.SaveChanges();
        }

        [HttpGet]
        public void Pickup(string TripDate, int Direction, string studentid,string Chk,string BusNumber)
        {
            if(Chk == "in")
                db.Database.ExecuteSqlCommand("update ScheduledBusTrips set PassengerOnBoard={0},PassengeronBoardDateTime={1} where PassengerID={2} and TripDate={3} and Direction={4}", 1, DateTime.Now, studentid,TripDate,Direction);
            else
                db.Database.ExecuteSqlCommand("update ScheduledBusTrips set PassengerOffBoard={0},PassengeroffBoardDateTime={1} where PassengerID={2} and TripDate={3} and Direction={4}", 0, DateTime.Now, studentid, TripDate, Direction);

            db.Database.ExecuteSqlCommand("exec changeBusStatus {0},{1},{2}", TripDate, Direction, BusNumber);
            db.SaveChanges();
        }

        [HttpGet]
        public IEnumerable<BusStudentMV> GetParentStudents(int GuardianID, string TripDate)
        {
            string _TripDate = TripDate;
            _TripDate = GetLastScheduledTripDateByGuardianID(GuardianID);

            var q = from s in db.Students
                    join b in db.ScheduledBusTrips
                    on s.StudentID equals b.PassengerID
                    where s.GuardianID == GuardianID && b.TripDate == _TripDate
                    select new BusStudentMV
                    {
                        PassengerID = b.PassengerID,
                        PassengerName = b.PassengerName,
                        BusNumber = b.BusNumber,
                        PassengerLongitude = b.PassengerLongitude,
                        PassengerLatitude = b.PassengerLatitude,
                        TripEnded = b.TripEnded,
                        Direction = b.Direction

                    };
            return q;

        }

        [HttpGet]
        public void EndTrip(string BusNumber,string TripDate, int Direction)
        {
            
            db.Database.ExecuteSqlCommand("update ScheduledBusTrips set TripEnded=1,PassengeroffBoard=0,PassengeroffBoardDateTime={0} where BusNumber = {1} and TripDate={2} and  Direction ={3}",  DateTime.Now, BusNumber, TripDate, Direction);
            db.Database.ExecuteSqlCommand("exec changeBusStatus {0},{1},{2}", TripDate, Direction, BusNumber);

            db.SaveChanges();

        }

        [HttpGet]
        public IEnumerable<SchoolBanchesMV> GetSchoolsByCompany(int CompanyID)
        {
            return db.SchoolBranches.Where(x => x.CompanyID == CompanyID).Select(x => new SchoolBanchesMV {
                SchoolID = x.SchoolID,
                SchoolEnglishName = x.SchoolEnglishName,
                SchoolArabicName = x.SchoolArabicName
            }).ToList();
        }

        [HttpGet]
        public IEnumerable<BusInfoMV> GetBusesBySchool(int SchoolID)
        {
            return db.BusInfoes.Where(x => x.SchoolID == SchoolID).Select(x => new BusInfoMV {
                BusNo = x.BusNo,
                BusCurrentStatus = x.BusCurrentStatus
            }).ToList();
        }

        [HttpGet]
        public void AccessTeacherGeofence(string BusNumber)
        {

            db.Database.ExecuteSqlCommand("update businfo set BusCurrentStatus = 2 where busno={0}", BusNumber);
            

            db.SaveChanges();

        }

        [HttpGet]
        public void ChangeBusStatus(string BusNumber,int Status, int SchoolID)
        {

            db.Database.ExecuteSqlCommand("update businfo set BusCurrentStatus = {0} where busno={1} and SchoolID={2}", Status, BusNumber, SchoolID);

            db.SaveChanges();
        }

        [HttpGet]
        public SystemSetting GetSystemSetting()
        {
            return db.SystemSettings.First();
        }

        [HttpPost]
        public dynamic Login(WebUser usr)
        {
            string Password = PasswordEncDec.Encrypt(usr.Password);

            //    var user = db.WebUsers.SingleOrDefault(s => s.UserName == usr.UserName && s.Password == Password);
            //    if (user == null)
            //        return null;
            //    else
            //        return user;

            var guardianUser = GetSingleDataRow(
                "SELECT u.*, g.GuardianArabicName, g.GuardianEnglishName, g.MobileNumber " +
                "FROM WebUsers u " +
                "INNER JOIN Guardians g ON u.UserSystemID = g.GuardianID " +
                "WHERE (u.UserName = '" + usr.UserName + "' AND u.Password = '" + Password + "')"
            );

            if (guardianUser == null)
                return null;
            else
                return guardianUser;
        }

        /* Private Methods */
        #region Private Methods
        private string GetLastScheduledTripDate(int SchoolID)
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

        private string GetLastScheduledTripDateByGuardianID(int GuardianID)
        {
            int SchoolID = 0;
            string LSTripDate = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable StudentInfo =
                GetDataTble("SELECT SD.SchoolID " +
                            "FROM Student S " +
                            "INNER JOIN StudentSchoolDetails SD ON " +
                            "(S.StudentID = SD.StudentID) " +
                            "WHERE S.GuardianID = " + GuardianID);
            if (StudentInfo != null)
            {
                if (StudentInfo.Rows.Count > 0)
                {
                    SchoolID = int.Parse(StudentInfo.Rows[0]["SchoolID"].ToString());

                    LSTripDate = GetLastScheduledTripDate(SchoolID);
                }
            }

            return LSTripDate;
        }

        private DataTable GetDataTble(string cmdStr)
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

        private Dictionary<string, object> GetSingleDataRow(string cmdStr)
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

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return row.Table.Columns.Cast<DataColumn>()
                          .ToDictionary(column => column.ColumnName, column => row[column]);
            }
            else
            {
                return null;
            }
        }
        #endregion

    }
}
