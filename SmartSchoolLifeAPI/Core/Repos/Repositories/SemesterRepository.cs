using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Settings.Semester;
using SmartSchoolLifeAPI.Core.Models.Shared;
using SmartSchoolLifeAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class SemesterRepository : ISemestersRepository
    {
        public IEnumerable<SemesterModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public SemesterModel Add(SemesterModel entity)
        {
            throw new NotImplementedException();
        }

        public SemesterModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(SemesterModel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SemesterVM> GetSchoolSemesters(int schoolId,
            string schoolYear)
        {
            List<dynamic> semesters = new List<dynamic>();
            string query = "SELECT ID, SemesterArabicName, SemesterEnglishName FROM Semesters " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", schoolId);
                    comm.Parameters.AddWithValue("@SchoolYear", !string.IsNullOrEmpty(schoolYear) ? schoolYear :
                        new SystemSettingsRepository().GetSystemSettings().CurrentAcademicYear);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        semesters = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return semesters.Any() ? semesters.MapListTo<SemesterVM>() : new List<SemesterVM>();
        }
    }
}