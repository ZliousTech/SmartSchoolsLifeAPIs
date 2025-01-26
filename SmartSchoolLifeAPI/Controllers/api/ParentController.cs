using SmartSchoolLifeAPI.Core.DTOs;
using SmartSchoolLifeAPI.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public IEnumerable<ParentStudentDTO> GetParentStudents(int GuardianID)
        {
            //return from s in db.Students
            //       where s.GuardianID == GuardianID
            //       select new BusStudentMV {
            //           PassengerID = s.StudentID,
            //           PassengerName = s.StudentArabicName };

            var studentsList = from s in db.Students
                               join d in db.StudentSchoolDetails
                               on s.StudentID equals d.StudentID
                               where s.GuardianID == GuardianID
                               select new ParentStudentDTO
                               {
                                   PassengerID = s.StudentID,
                                   PassengerName = s.StudentArabicName,
                                   Photo = s.Photo,
                                   SchoolID = d.SchoolID.ToString(),
                                   SchoolClassID = d.ClassID.ToString(),
                                   SectionID = d.SectionID.ToString(),
                                   StudentBusTrips = from b in db.ScheduledBusTrips
                                                     where b.PassengerID == s.StudentID && b.SchoolID == d.SchoolID
                                                     select new StudentBusTrip
                                                     {
                                                         BusNumber = b.BusNumber,
                                                         TripDate = b.TripDate,
                                                         TripTime = b.TripTime,
                                                         Direction = b.Direction.ToString()
                                                     }
                               };

            return studentsList.ToList();
        }
    }
}
