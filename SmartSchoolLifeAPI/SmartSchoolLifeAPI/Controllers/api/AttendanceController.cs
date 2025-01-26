using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartSchoolLifeAPI.ViewModels;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class AttendanceController : ApiController
    {
        SmartSchoolsEntities2 db;

        public AttendanceController()
        {
            db = new SmartSchoolsEntities2();

        }

        [HttpGet]
        public IEnumerable<TeacherClasses> GetTeacherClasses(int SchoolID,int SchoolYear,string TeacherID,int WeekDay)
        {
            var SchoolIDParameter = new SqlParameter("@SchoolID", SchoolID);
            var SchoolYearParameter = new SqlParameter("@SchoolYear", SchoolYear);
            var TeacherIDParameter = new SqlParameter("@TeacherID", TeacherID);
            var WeekDayParameter = new SqlParameter("@WeekDay", WeekDay);

            var q = db.Database.SqlQuery<TeacherClasses>("GetTeacherClasses @SchoolID, @SchoolYear, @TeacherID,@WeekDay", SchoolIDParameter,SchoolYearParameter,TeacherIDParameter,WeekDayParameter).ToList();

            return q;
        }

        [HttpGet]
        public IEnumerable<SectionStudents> GetSectionStudents(int CompanyID, int SchoolID, int SchoolClassID, int SectionID)
        {
            var CompanyIDParameter = new SqlParameter("@CompanyID", CompanyID);
            var SchoolIDParameter = new SqlParameter("@SchoolID", SchoolID);
            var SchoolClassIDParameter = new SqlParameter("@SchoolClassID", SchoolClassID);
            var SectionIDParameter = new SqlParameter("@SectionID", SectionID);

            var q = db.Database.SqlQuery<SectionStudents>("GetSectionStudents @CompanyID, @SchoolID, @SchoolClassID, @SectionID", CompanyIDParameter, SchoolIDParameter, SchoolClassIDParameter, SectionIDParameter).ToList();

            return q;
        }

        [HttpGet]
        public IHttpActionResult GetAttendanceSettings(int code)
        {
            if(code==1)
                return Ok(db.AttendanceTypes.ToList());
            else
                return Ok(db.AbsenceReasons.ToList());
        }

        [HttpPost]
        public void SaveAttendance(AttendanceVM attvm)
        {
            var attendance = db.Attendances.SingleOrDefault(x => x.StudentID == attvm.StudentID && x.AttendanceDate == attvm.AttendanceDate);
            if(attendance == null)
            {
                attendance = new Attendance();
                attendance.SchoolID = attvm.SchoolID;
                attendance.SchoolYear = attvm.SchoolYear;
                attendance.StudentID = attvm.StudentID;
                attendance.AttendanceDate = attvm.AttendanceDate;
                attendance.AttendanceType = attvm.AttendanceType;
                attendance.AbsenceReason = attvm.AbsenseReason;
                attendance.Description = attvm.Description;
                switch(attvm.ClassOrder)
                {
                    case 1:
                        attendance.FirstSession = -1;
                        attendance.FirstSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 2:
                        attendance.SecondSession = -1;
                        attendance.SecondSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 3:
                        attendance.ThirdSession = -1;
                        attendance.ThirdSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 4:
                        attendance.FourthSession = -1;
                        attendance.FourthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 5:
                        attendance.FifthSession = -1;
                        attendance.FifthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 6:
                        attendance.SixthSession = -1;
                        attendance.SixthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 7:
                        attendance.SeventhSession = -1;
                        attendance.SeventhSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 8:
                        attendance.EighthSession = -1;
                        attendance.EighthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                }
                db.Attendances.Add(attendance);
                db.SaveChanges();
            }
            else
            {
                switch (attvm.ClassOrder)
                {
                    case 1:
                        attendance.FirstSession = -1;
                        attendance.FirstSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 2:
                        attendance.SecondSession = -1;
                        attendance.SecondSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 3:
                        attendance.ThirdSession = -1;
                        attendance.ThirdSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 4:
                        attendance.FourthSession = -1;
                        attendance.FourthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 5:
                        attendance.FifthSession = -1;
                        attendance.FifthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 6:
                        attendance.SixthSession = -1;
                        attendance.SixthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 7:
                        attendance.SeventhSession = -1;
                        attendance.SeventhSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                    case 8:
                        attendance.EighthSession = -1;
                        attendance.EighthSessionTimeStamp = attvm.ClassOrderTime;
                        break;
                }
                db.SaveChanges();
            } 
        }

        [HttpGet]
        public DeviceRegistrar GetParentTokenByStudentID(string StudentID)
        {
            var std = db.Students.SingleOrDefault(x => x.StudentID == StudentID);
            return db.DeviceRegistrars.SingleOrDefault(y => y.OwnerID == std.GuardianID.ToString());
        }
    }
}
