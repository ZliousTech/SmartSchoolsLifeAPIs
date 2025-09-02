using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Http;
using System.Data;
using Common.Base;

namespace SmartSchoolLifeAPI.Controllers.api
{
    public class SmartSchoolWebLiteController : ApiController
    {
        public SmartSchoolWebLiteController()
        {
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolDomainNameIPAddress?SchoolID=1078
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolCurriculums?SchoolID=1078&Lang=en
            //http://localhost:29628/api/SmartSchoolWebLite/GetCurriculumsSchoolClasses?SchoolID=1078&CurriculumID=103&Lang=en
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolClassesSubjects?SchoolID=1078&SchoolClassID=437&Lang=en
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolSubjectsBooks?SchoolID=1078&SubjectID=2147&Lang=en
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolBooksChapters?BookID=8&Lang=en
            //http://localhost:29628/api/SmartSchoolWebLite/GetSchoolChaptersLessons?ChapterID=10&Lang=en
        }

        public IEnumerable<dynamic> GetSchoolDomainNameStaticIPAddress(string SchoolID)
        {
            List<SSLDomainNameIPAddress> _SSLDomainNameIPAddress = new List<SSLDomainNameIPAddress>();

            DataTable DomainNameIPAddressDB =
                SysBase.GetDataTble($"SELECT DomainName, StaticIPAddress " +
                                    $"FROM SystemSettingsperSchool " +
                                    $"WHERE SchoolID = '{SchoolID}'");
            if (DomainNameIPAddressDB != null)
            {
                foreach (DataRow row in DomainNameIPAddressDB.Rows)
                {
                    _SSLDomainNameIPAddress.Add(new SSLDomainNameIPAddress
                    {
                        DomainName = row["DomainName"].ToString(),
                        StaticIPAddress = row["StaticIPAddress"].ToString()
                    });
                }
            }

            return _SSLDomainNameIPAddress;
        }

        public IEnumerable<dynamic> GetSchoolCurriculums(string SchoolID, string lang)
        {
            List<SSLCurriculums> _SSLCurriculums = new List<SSLCurriculums>();
            string CurriculumName = lang.Contains("en") ? "CurriculumEnglishName" : "CurriculumArabicName";

            DataTable CurrriculumsDB =
                SysBase.GetDataTble($"SELECT CurriculumID, CurriculumName = {CurriculumName} " +
                                    $"FROM Curriculums " +
                                    $"WHERE SchoolID = '{SchoolID}' AND (CurriculumArabicName IS NOT NULL OR CurriculumEnglishName IS NOT NULL)");
            if (CurrriculumsDB != null)
            {
                foreach (DataRow row in CurrriculumsDB.Rows)
                {
                    _SSLCurriculums.Add(new SSLCurriculums
                    {
                        CuruuiculumID = row["CurriculumID"].ToString(),
                        CurriculumName = row["CurriculumName"].ToString()
                    });
                }
            }

            return _SSLCurriculums;
        }

        public IEnumerable<dynamic> GetCurriculumsSchoolClasses(string SchoolID, string CurriculumID, string lang)
        {
            List<SSLSchoolClasses> _SSLSchoolClasses = new List<SSLSchoolClasses>();
            string SchoolClassName = lang.Contains("en") ? "SchoolClassEnglishName" : "SchoolClassArabicName";

            DataTable SchoolClassessDB =
                SysBase.GetDataTble($"SELECT SchoolClassID, SchoolClassName = {SchoolClassName} " +
                                    $"FROM SchoolClasses " +
                                    $"WHERE SchoolID = '{SchoolID}' AND CurriculumID = '{CurriculumID}'");
            if (SchoolClassessDB != null)
            {
                foreach (DataRow row in SchoolClassessDB.Rows)
                {
                    _SSLSchoolClasses.Add(new SSLSchoolClasses
                    {
                        SchoolClassID = row["SchoolClassID"].ToString(),
                        SchoolClassName = row["SchoolClassName"].ToString()
                    });
                }
            }

            return _SSLSchoolClasses;
        }

        public IEnumerable<dynamic> GetSchoolClassesSubjects(string SchoolID, string SchoolClassID, string lang)
        {
            List<SSLSchoolSubjects> _SSLSchoolSubjects = new List<SSLSchoolSubjects>();
            string SubjectName = lang.Contains("en") ? "SubjectEnglishName" : "SubjectArabicName";

            DataTable SchoolSubjectsDB =
                SysBase.GetDataTble($"SELECT SubjectID, SubjectName = {SubjectName} " +
                                    $"FROM Subjects " +
                                    $"WHERE SchoolID = '{SchoolID}' AND SchoolClassID = '{SchoolClassID}'");
            if (SchoolSubjectsDB != null)
            {
                foreach (DataRow row in SchoolSubjectsDB.Rows)
                {
                    _SSLSchoolSubjects.Add(new SSLSchoolSubjects
                    {
                        SubjectID = row["SubjectID"].ToString(),
                        SubjectName = row["SubjectName"].ToString()
                    });
                }
            }

            return _SSLSchoolSubjects;
        }

        public IEnumerable<dynamic> GetSchoolSubjectsBooks(string SchoolID, string SubjectID, string lang)
        {
            List<SSLBooks> _SSLBooks = new List<SSLBooks>();
            string BookName = lang.Contains("en") ? "BookEnglishName" : "BookArabicName";

            DataTable SubjectsBooksDB =
                SysBase.GetDataTble($"SELECT BookID, BookName = {BookName} " +
                                    $"FROM SubjectBooks " +
                                    $"WHERE SchoolID = '{SchoolID}' AND SubjectId = '{SubjectID}'");
            if (SubjectsBooksDB != null)
            {
                foreach (DataRow row in SubjectsBooksDB.Rows)
                {
                    _SSLBooks.Add(new SSLBooks
                    {
                        BookID = row["BookID"].ToString(),
                        BookName = row["BookName"].ToString()
                    });
                }
            }

            return _SSLBooks;
        }

        public IEnumerable<dynamic> GetSchoolBooksChapters(string BookID, string lang)
        {
            List<SSLChapters> _SSLChapters = new List<SSLChapters>();
            string ChapterName = lang.Contains("en") ? "ChapterEnglishName" : "ChapterArabicName";

            DataTable BooksChaptersDB =
                SysBase.GetDataTble($"SELECT ChapterId, ChapterName = {ChapterName} " +
                                    $"FROM BookChapters " +
                                    $"WHERE BookId = '{BookID}'");
            if (BooksChaptersDB != null)
            {
                foreach (DataRow row in BooksChaptersDB.Rows)
                {
                    _SSLChapters.Add(new SSLChapters
                    {
                        ChapterID = row["ChapterId"].ToString(),
                        ChapterName = row["ChapterName"].ToString()
                    });
                }
            }

            return _SSLChapters;
        }

        public IEnumerable<dynamic> GetSchoolChaptersLessons(string ChapterID, string lang)
        {
            List<SSLLessons> _SSLLessons = new List<SSLLessons>();
            string LessonName = lang.Contains("en") ? "LessonEnglishName" : "LessonArabicName";

            DataTable ChaptersLessonsDB =
                SysBase.GetDataTble($"SELECT LessonId, LessonName = {LessonName} " +
                                    $"FROM ChapterLessons " +
                                    $"WHERE ChapterId = '{ChapterID}'");
            if (ChaptersLessonsDB != null)
            {
                foreach (DataRow row in ChaptersLessonsDB.Rows)
                {
                    _SSLLessons.Add(new SSLLessons
                    {
                        LessonID = row["LessonId"].ToString(),
                        LessonName = row["LessonName"].ToString()
                    });
                }
            }

            return _SSLLessons;
        }
    }

    public class SSLDomainNameIPAddress
    {
        public string DomainName { get; set; }
        public string StaticIPAddress { get; set; }
    }

    public class SSLCurriculums
    {
        public string CuruuiculumID { get; set; }
        public string CurriculumName { get; set; }
    }

    public class SSLSchoolClasses
    {
        public string SchoolClassID { get; set; }
        public string SchoolClassName { get; set; }
    }

    public class SSLSchoolSubjects
    {
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
    }

    public class SSLBooks
    {
        public string BookID { get; set; }
        public string BookName { get; set; }
    }

    public class SSLChapters
    {
        public string ChapterID { get; set; }
        public string ChapterName { get; set; }
    }

    public class SSLLessons
    {
        public string LessonID { get; set; }
        public string LessonName { get; set; }
    }
}