using SmartSchoolLifeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class ParentController : ApiController
    {
        SmartSchoolsEntities2 db;

        public ParentController()
        {
            db = new SmartSchoolsEntities2();
        }

        [HttpPost]
        public void AttendByParent(AttendanceFullDayVM attvm)
        {
            var attendance = new Attendance
            {
                SchoolID = attvm.SchoolID,
                SchoolYear = attvm.SchoolYear,
                StudentID = attvm.StudentID,
                AttendanceDate = attvm.AttendanceDate,
                AttendanceType = attvm.AttendanceType,
                AbsenceReason = attvm.AbsenseReason,
                Description = attvm.Description,
                FirstSession = attvm.FirstSession,
                SecondSession = attvm.SecondSession,
                ThirdSession = attvm.ThirdSession,
                FourthSession = attvm.FourthSession,
                FifthSession = attvm.FifthSession,
                SixthSession = attvm.SixthSession,
                SeventhSession = attvm.SeventhSession,
                EighthSession = attvm.EighthSession
            };
            db.Attendances.Add(attendance);
            db.SaveChanges();
        }

        [HttpGet]
        public IEnumerable<BusStudentMV> GetParentStudents(int GuardianID)
        {
            //return from s in db.Students
            //       where s.GuardianID == GuardianID
            //       select new BusStudentMV {
            //           PassengerID = s.StudentID,
            //           PassengerName = s.StudentArabicName };

            return from s in db.Students
                    join d in db.StudentSchoolDetails
                    on s.StudentID equals d.StudentID
                    where s.GuardianID == GuardianID
                    select new BusStudentMV
                    {
                        PassengerID = s.StudentID,
                        PassengerName = s.StudentArabicName,
                        Photo = s.Photo,
                        SchoolID = d.SchoolID.ToString(),
                        SchoolClassID = d.ClassID.ToString(),
                        SectionID = d.SectionID.ToString(),
                    };
        }
     }
}
