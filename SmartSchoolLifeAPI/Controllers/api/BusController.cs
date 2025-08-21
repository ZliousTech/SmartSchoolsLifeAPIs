using Common.Base;
using SmartSchoolLifeAPI.Core.Models;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Helpers;
using SmartSchoolLifeAPI.Core.Models.Shared;
using SmartSchoolLifeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class BusController : ApiController
    {
        SmartSchoolsEntities2 db;

        public BusController()
        {
            db = new SmartSchoolsEntities2();
        }

        // Bus application.
        public IEnumerable<dynamic> GetStudents(int SchoolID, string SchoolYear, string BusNumber, string TripTime)
        {
            //Get last schaduale trip date
            string lastScheduledTripDate = SysBase.GetLastScheduledTripDate(SchoolID);
            dynamic q = null;

            string query = "SELECT b.PassengerID, b.PassengerName, b.PassengerLongitude, b.PassengerLatitude, b.SchoolLongitude, " +
                "b.SchoolLatitude, b.AttendantLongitude, b.AttendantLatitude, s.GuardianID, b.AttendantID, dr.DeviceRegistrationCode, b.PickupOrder " +
                "FROM Student s " +
                "INNER JOIN ScheduledBusTrips b ON s.StudentID = b.PassengerID " +
                "LEFT JOIN DeviceRegistrar dr ON CAST(s.GuardianID AS NVARCHAR(MAX)) = dr.OwnerID " +
                "WHERE b.SchoolID = @SchoolID AND b.BusNumber = @BusNumber AND b.TripDate = @TripDate AND b.TripTime = @TripTime AND b.IsAbsenceByParent = @IsAbsenceByParent " +
                "ORDER BY b.PickupOrder ASC";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@BusNumber", BusNumber);
                    comm.Parameters.AddWithValue("@TripDate", lastScheduledTripDate);
                    comm.Parameters.AddWithValue("@TripTime", TripTime);
                    comm.Parameters.AddWithValue("@IsAbsenceByParent", 0);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        q = reader.MapAll();
                    }
                }
            }

            //var q = (from s in _db.Students
            //         join b in _db.ScheduledBusTrips
            //         on s.StudentID equals b.PassengerID
            //         join d in _db.DeviceRegistrars
            //         on s.GuardianID.ToString() equals d.OwnerID into deviceRegistrars
            //         from dr in deviceRegistrars.DefaultIfEmpty()
            //         where b.SchoolID == SchoolID && b.BusNumber == BusNumber
            //         && b.TripDate == TripDate && b.TripTime == TripTime
            //         select new BusStudentMV
            //         {
            //             PassengerID = b.PassengerID,
            //             PassengerName = b.PassengerName,
            //             PassengerLongitude = b.PassengerLongitude,
            //             PassengerLatitude = b.PassengerLatitude,
            //             SchoolLongitude = b.SchoolLongitude,
            //             SchoolLatitude = b.SchoolLatitude,
            //             AttendantLongitude = b.AttendantLongitude,
            //             AttendantLatitude = b.AttendantLatitude,
            //             GuardianID = s.GuardianID,
            //             AttendantID = b.AttendantID,
            //             DeviceRegistrationCode = dr.DeviceRegistrationCode
            //         });


            //-- Orginal
            //var q = (from s in _db.Students
            //        join b in _db.ScheduledBusTrips
            //        on s.StudentID equals b.PassengerID join d in _db.DeviceRegistrars 
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

        public IEnumerable<BusStudentMV> GetStudentsForTeacher(int SchoolID, string TeacherID, string TripTime)
        {
            //Get last schaduale trip date
            string LastScheduledTripDate = SysBase.GetLastScheduledTripDate(SchoolID);
            string TripDate = LastScheduledTripDate;

            var q = from s in db.Students
                    join b in db.ScheduledBusTrips
                    on s.StudentID equals b.PassengerID
                    join d in db.DeviceRegistrars
                    on s.GuardianID.ToString() equals d.OwnerID into deviceRegistrars
                    from dr in deviceRegistrars.DefaultIfEmpty()
                    where b.SchoolID == SchoolID && b.AttendantID == TeacherID
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
                    };

            return q;
        }

        //public IEnumerable<BusStudentMV> GetTimes(int SchoolID, string SchoolYear, string BusNumber, string TripDate)
        //{
        //    return _db.ScheduledBusTrips.Where(x => x.SchoolID == SchoolID && x.SchoolYear == SchoolYear
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
                     TripTime = x.TripTime,
                     Direction = x.Direction
                 }).Distinct().ToList();
        }

        public IEnumerable<BusStudentVM> GetTimesByAttendant(string attendantId, string schoolId)
        {
            string lastTripDate = SysBase.GetLastScheduledTripDate(int.Parse(schoolId));
            return db.ScheduledBusTrips.Where(x => x.TripDate == lastTripDate && x.AttendantID == attendantId)
                 .Select(x => new BusStudentVM
                 {
                     TripTime = x.TripTime,
                     Direction = x.Direction,
                     BusNumber = x.BusNumber,
                     SchoolYear = x.SchoolYear
                 }).Distinct().ToList();

            //return _db.ScheduledBusTrips.Where(x => x.TripDate == TripDate && x.AttendantID == AttendantID)
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

        // Teacher application.

        [HttpGet]
        public IEnumerable<dynamic> GetStudentsByAttendant(string TripTime, string AttendantID, bool AbsenceStudents = false)
        {
            string lastScheduledTripDate = GetLastScheduledTripDate(TripTime, AttendantID);

            dynamic q = null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT b.PassengerID, b.PassengerName, b.PassengerLongitude, b.PassengerLatitude, b.PassengerOnBoard, " +
                "b.PassengeroffBoard, s.GuardianID, b.IsAbsenceByParent " +
                "FROM ScheduledBusTrips b " +
                "INNER JOIN Student s ON b.PassengerID = s.StudentID " +
                "WHERE b.TripTime = @TripTime AND TripDate = @TripDate AND b.AttendantID = @AttendantID ");
            query.Append(AbsenceStudents ? "" : "AND b.IsAbsenceByParent = @IsAbsenceByParent ");
            query.Append("ORDER BY s.StudentID ASC;");
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                {
                    comm.Parameters.AddWithValue("@TripDate", lastScheduledTripDate);
                    comm.Parameters.AddWithValue("@TripTime", TripTime);
                    comm.Parameters.AddWithValue("@AttendantID", AttendantID);
                    if (!AbsenceStudents)
                        comm.Parameters.AddWithValue("@IsAbsenceByParent", 0);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        q = reader.MapAll();
                    }
                }
            }

            //var q = (from b in _db.ScheduledBusTrips
            //         join s in _db.Students
            //on b.PassengerID equals s.StudentID
            //         where b.TripTime == TripTime && b.AttendantID == AttendantID
            //         select new BusStudentMV
            //         {
            //             PassengerID = b.PassengerID,
            //             PassengerName = b.PassengerName,
            //             PassengerLongitude = b.PassengerLongitude,
            //             PassengerLatitude = b.PassengerLatitude,
            //             PassengerOnBoard = b.PassengerOnBoard,
            //             PassengerOffBoard = b.PassengeroffBoard,
            //             GuardianID = s.GuardianID,
            //             Photo = s.Photo
            //         });

            return q;
        }

        [HttpGet]
        public void UpdateLoc(double longitude, double latitude, string studentid)
        {
            //_db.Database.ExecuteSqlCommand("update StudentAdresses set longitude={0},latitude={1} where AddressID = (select AddressID from Student where StudentID={2})", longitude, latitude, studentid);
            //_db.SaveChanges();
            string query = "";

            string conStr = ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                query = $"UPDATE StudentAdresses SET " +
                        $"longitude = @longitude, " +
                        $"latitude = @latitude " +
                        $"WHERE AddressID = ( " +
                        $"                    SELECT AddressID " +
                        $"                    FROM Student " +
                        $"                    WHERE StudentID = @StudentID " +
                        $"                  )";

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@longitude", longitude);
                    comm.Parameters.AddWithValue("@latitude", latitude);
                    comm.Parameters.AddWithValue("@StudentID", studentid);
                    comm.ExecuteNonQuery();
                }

                query = $"UPDATE ScheduledBusTrips SET " +
                        $"PassengerLatitude = @PassengerLatitude, " +
                        $"PassengerLongitude = @PassengerLongitude " +
                        $"WHERE PassengerID = '{studentid}'";

                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@PassengerLatitude", latitude);
                    comm.Parameters.AddWithValue("@PassengerLongitude", longitude);
                    comm.Parameters.AddWithValue("@StudentID", studentid);
                    comm.ExecuteNonQuery();
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        [HttpGet]
        public IHttpActionResult Pickup(int direction, string studentId, string chk, string busNumber, int schoolId)
        {
            string lastScheduledTripDate = SysBase.GetLastScheduledTripDate(schoolId);

            var isStudentOnLastScheduledTrip = db.ScheduledBusTrips.Where(x => x.TripDate == lastScheduledTripDate && x.Direction == direction &&
                x.PassengerID == studentId && x.BusNumber == busNumber && x.SchoolID == schoolId)
                .Distinct().ToList().Any();

            if (!isStudentOnLastScheduledTrip)
                return Content(HttpStatusCode.NotFound, Messages.CustomMessage("This student is not on last trip"));

            if (chk == "in")
                db.Database.ExecuteSqlCommand("update ScheduledBusTrips set PassengerOnBoard={0}, PassengerOffBoard={1},PassengeronBoardDateTime={2} where PassengerID={3} and TripDate={4} and Direction={5}", 1, 0, DateTime.Now, studentId, lastScheduledTripDate, direction);
            else
                db.Database.ExecuteSqlCommand("update ScheduledBusTrips set PassengerOffBoard={0}, PassengerOnBoard={1}, PassengeroffBoardDateTime={2} where PassengerID={3} and TripDate={4} and Direction={5}", 1, 0, DateTime.Now, studentId, lastScheduledTripDate, direction);

            db.Database.ExecuteSqlCommand("exec changeBusStatus {0},{1},{2},{3}", lastScheduledTripDate, direction, busNumber, schoolId);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IEnumerable<BusStudentMV> GetParentStudents(int GuardianID)
        {
            string lastScheduledTripDate = GetLastScheduledTripDateByGuardianID(GuardianID);

            var q = from s in db.Students
                    join b in db.ScheduledBusTrips
                    on s.StudentID equals b.PassengerID
                    where s.GuardianID == GuardianID && b.TripDate == lastScheduledTripDate
                    select new BusStudentMV
                    {
                        PassengerID = b.PassengerID,
                        PassengerName = b.PassengerName,
                        BusNumber = b.BusNumber,
                        PassengerLongitude = b.PassengerLongitude,
                        PassengerLatitude = b.PassengerLatitude,
                        TripEnded = b.TripEnded,
                        Direction = b.Direction,
                        PassengerOffBoard = b.PassengeroffBoard,
                        PassengerOnBoard = b.PassengerOnBoard
                    };
            return q;

        }

        [HttpGet]
        public IHttpActionResult EndTrip(string busNumber, int direction, int schoolId)
        {
            string lastScheduledTripDate = SysBase.GetLastScheduledTripDate(schoolId);

            var isSchedulTripExist = db.ScheduledBusTrips.Where(x => x.TripDate == lastScheduledTripDate && x.Direction == direction &&
                x.BusNumber == busNumber && x.SchoolID == schoolId)
                .Distinct().ToList().Any();

            if (!isSchedulTripExist)
                return Content(HttpStatusCode.NotFound, Messages.CustomMessage("No schedule trips found for the provided paremeters"));

            db.Database.ExecuteSqlCommand("update ScheduledBusTrips set TripEnded=1,PassengeroffBoard=1,PassengerOnBoard=0,PassengeroffBoardDateTime={0} where BusNumber = {1} and TripDate={2} and  Direction ={3}", DateTime.Now, busNumber, lastScheduledTripDate, direction);
            db.Database.ExecuteSqlCommand("exec changeBusStatus {0},{1},{2},{3}", lastScheduledTripDate, direction, busNumber, schoolId);

            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IEnumerable<SchoolBanchesMV> GetSchoolsByCompany(int CompanyID)
        {
            return db.SchoolBranches.Where(x => x.CompanyID == CompanyID).Select(x => new SchoolBanchesMV
            {
                SchoolID = x.SchoolID,
                SchoolEnglishName = x.SchoolEnglishName,
                SchoolArabicName = x.SchoolArabicName
            }).ToList();
        }

        [HttpGet]
        public IEnumerable<BusInfoMV> GetBusesBySchool(int SchoolID)
        {
            return db.BusInfoes.Where(x => x.SchoolID == SchoolID).Select(x => new BusInfoMV
            {
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
        public void ChangeBusStatus(string BusNumber, int Status, int SchoolID)
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

            //    var user = _db.WebUsers.SingleOrDefault(s => s.UserName == usr.UserName && s.Password == Password);
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

        private string GetLastScheduledTripDateByGuardianID(int GuardianID)
        {
            int SchoolID = 0;
            string LSTripDate = DateTime.Now.ToString("dd/MM/yyyy");

            DataTable StudentInfo =
                SysBase.GetDataTble("SELECT SD.SchoolID " +
                            "FROM Student S " +
                            "INNER JOIN StudentSchoolDetails SD ON " +
                            "(S.StudentID = SD.StudentID) " +
                            "WHERE S.GuardianID = " + GuardianID);
            if (StudentInfo != null)
            {
                if (StudentInfo.Rows.Count > 0)
                {
                    SchoolID = int.Parse(StudentInfo.Rows[0]["SchoolID"].ToString());

                    LSTripDate = SysBase.GetLastScheduledTripDate(SchoolID);
                }
            }

            return LSTripDate;
        }

        private string GetLastScheduledTripDate(string TripTime, string AttendantID)
        {
            DataTable LastScheduledTripDateDB =
                SysBase.GetDataTble($"SELECT TOP 1 TripDate " +
                            $"FROM ScheduledBusTrips " +
                            $"WHERE TripTime = '{TripTime}' AND AttendantID = '{AttendantID}' " +
                            $"ORDER BY TripPassengerID DESC");
            if (LastScheduledTripDateDB != null)
            {
                if (LastScheduledTripDateDB.Rows.Count > 0)
                {
                    return LastScheduledTripDateDB.Rows[0]["TripDate"].ToString();
                }
            }

            return DateTime.Now.ToString("dd/MM/yyyy");
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
