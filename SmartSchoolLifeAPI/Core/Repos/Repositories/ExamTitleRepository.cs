using SmartSchoolLifeAPI.Core.Models.ExamTitles;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class ExamTitleRepository : IExamTitleRepository
    {
        public IEnumerable<ExamTitle> GetAll()
        {
            List<dynamic> examTitles = new List<dynamic>();
            string query = "SELECT ID as Id, ExamTitleArabicName, ExamTitleEnglishName FROM ExamTitles";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        examTitles = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return examTitles.Any() ? examTitles.MapListTo<ExamTitle>() : new List<ExamTitle>();
        }

        public ExamTitle GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ExamTitle Add(ExamTitle entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ExamTitle entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}