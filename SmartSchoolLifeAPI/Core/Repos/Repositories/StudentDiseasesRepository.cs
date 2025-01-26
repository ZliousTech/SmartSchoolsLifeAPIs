using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Shared;
using SmartSchoolLifeAPI.Core.Models.Student.StudentDiseases;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class StudentDiseasesRepository : IStudentDiseasesRepository
    {
        public IEnumerable<StudentDiseasesModel> GetAll()
        {
            return new List<StudentDiseasesModel>();
        }

        public StudentDiseasesModel GetById(int id)
        {
            return new StudentDiseasesModel();
        }

        public StudentDiseasesModel Add(StudentDiseasesModel entity)
        {
            return new StudentDiseasesModel();
        }

        public void Update(StudentDiseasesModel entity)
        {
            string query = "UPDATE StudentDiseases SET " +
                "Mumps =@Mumps, Chickenpox = @Chickenpox, rubella = @rubella, " +
                "Rheumaticfever = @Rheumaticfever, Diabetes = @Diabetes, Heartdiseases = @Heartdiseases, " +
                "Pissingoff = @Pissingoff, Jointbonediseases = @Jointbonediseases, sprayer = @sprayer, " +
                "Hearingimpairment = @Hearingimpairment, Visualimpairment = @Visualimpairment, " +
                "Speechimpairment = @Speechimpairment, Bladderdiseases = @Bladderdiseases, " +
                "Epilepsy = @Epilepsy, Hepatitis = @Hepatitis, Shakika = @Shakika, Fainting = @Fainting, " +
                "Kidneydisease = @Kidneydisease, Surgery = @Surgery, " +
                "Urinarysystemdiseases = @Urinarysystemdiseases " +
                "WHERE InternalStudentID = @InternalStudentID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@Mumps", entity.Mumps);
                    comm.Parameters.AddWithValue("@Chickenpox", entity.Chickenpox);
                    comm.Parameters.AddWithValue("@rubella", entity.rubella);
                    comm.Parameters.AddWithValue("@Rheumaticfever", entity.Rheumaticfever);
                    comm.Parameters.AddWithValue("@Diabetes", entity.Diabetes);
                    comm.Parameters.AddWithValue("@Heartdiseases", entity.Heartdiseases);
                    comm.Parameters.AddWithValue("@Pissingoff", entity.Pissingoff);
                    comm.Parameters.AddWithValue("@Jointbonediseases", entity.Jointbonediseases);
                    comm.Parameters.AddWithValue("@sprayer", entity.sprayer);
                    comm.Parameters.AddWithValue("@Hearingimpairment", entity.Hearingimpairment);
                    comm.Parameters.AddWithValue("@Visualimpairment", entity.Visualimpairment);
                    comm.Parameters.AddWithValue("@Speechimpairment", entity.Speechimpairment);
                    comm.Parameters.AddWithValue("@Bladderdiseases", entity.Bladderdiseases);
                    comm.Parameters.AddWithValue("@Epilepsy", entity.Epilepsy);
                    comm.Parameters.AddWithValue("@Hepatitis", entity.Hepatitis);
                    comm.Parameters.AddWithValue("@Shakika", entity.Shakika);
                    comm.Parameters.AddWithValue("@Fainting", entity.Fainting);
                    comm.Parameters.AddWithValue("@Kidneydisease", entity.Kidneydisease);
                    comm.Parameters.AddWithValue("@Surgery", entity.Surgery);
                    comm.Parameters.AddWithValue("@Urinarysystemdiseases", entity.Urinarysystemdiseases);
                    comm.Parameters.AddWithValue("@InternalStudentID", entity.InternalStudentID);
                    comm.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }

        public void Delete(int id)
        {

        }

        public dynamic GetStudentDiseases(string internalStudentID)
        {

            dynamic studentDiseases = new System.Dynamic.ExpandoObject();
            string query = "SELECT * FROM StudentDiseases " +
                "WHERE InternalStudentID = @InternalstudentID";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@InternalstudentID", internalStudentID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        studentDiseases = reader.MapSingle();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return studentDiseases;
        }
    }
}