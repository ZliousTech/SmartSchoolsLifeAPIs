using SmartSchoolLifeAPI.Core.Models.ExamTypes;
using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class ExamTypeRepository : IExamTypeRepository
    {
        public IEnumerable<ExamType> GetAll()
        {
            List<dynamic> examTypes = new List<dynamic>();
            string query = "SELECT ExamTypeID as Id, TypeArabicName, TypeEnglishName FROM ExamTypes";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        examTypes = reader.MapAll();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return examTypes.Any() ? examTypes.MapListTo<ExamType>() : new List<ExamType>();
        }

        public ExamType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ExamType Add(ExamType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ExamType entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}