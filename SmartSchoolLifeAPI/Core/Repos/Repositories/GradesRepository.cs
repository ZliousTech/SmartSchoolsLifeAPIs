using SmartSchoolLifeAPI.Core.Models.Extensions;
using SmartSchoolLifeAPI.Core.Models.Grades;
using SmartSchoolLifeAPI.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SmartSchoolLifeAPI.Core.Repos
{
    public class GradesRepository : IGradesRepository
    {
        public IEnumerable<GradesModel> GetAll()
        {
            return new List<GradesModel>();
        }

        public GradesModel GetById(int id)
        {
            return new GradesModel();
        }

        public GradesModel Add(GradesModel entity)
        {
            return new GradesModel();
        }

        public void Update(GradesModel entity)
        {

        }

        public void Delete(int id)
        {

        }

        private IEnumerable<dynamic> PrepareStudentGrades(string studentId)
        {
            List<dynamic> gradesList = new List<dynamic>();
            string query = "SELECT g.ID, t.SemesterArabicName, t.SemesterEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "et.TypeArabicName, et.TypeEnglishName, el.ExamTitleArabicName, el.ExamTitleEnglishName, " +
                "g.GradeValue, e.TotalGrades, e.ExamDate, s.StaffArabicName, s.StaffEnglishName, " +
                "CASE " +
                "WHEN CAST(e.ExamDate AS DATE) < CAST(CURRENT_TIMESTAMP AS DATE) THEN 'Finished' " +
                "WHEN CAST(e.ExamDate AS DATE) = CAST(CURRENT_TIMESTAMP AS DATE) THEN 'In Progress' " +
                "ELSE 'Not Start Yet' END AS [ExamStatus] " +
                "FROM Grades g " +
                "INNER JOIN Exams e ON g.ExamID = e.ID " +
                "INNER JOIN Semesters t ON e.SemesterID = t.ID " +
                "INNER JOIN Subjects j ON e.SubjectID = j.SubjectID " +
                "INNER JOIN ExamTypes et ON e.ExamTypeID = et.ExamTypeID " +
                "INNER JOIN ExamTitles el ON e.ExamTitleID = el.ID " +
                "LEFT JOIN Staff s ON e.TeacherID = s.StaffID " +
                "WHERE g.StudentID = @StudentID AND " +
                "e.SubjectID IN " +
                "(SELECT DISTINCT j.SubjectID FROM Subjects j " +
                "INNER JOIN Exams e ON j.SubjectID = e.SubjectID " +
                "WHERE j.SchoolClassID = " +
                "(SELECT SchoolClassID FROM Sections WHERE SectionID = " +
                "(SELECT SectionID FROM StudentSchoolDetails WHERE StudentID = @StudentID))) AND " +
                "e.IsCounted = 1 AND g.SchoolYear = @SchoolYear";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentId);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        gradesList = reader.MapAll("dd-MM-yyyy");
                    }
                }
                conn.Close();
            }

            return gradesList;
        }

        public IEnumerable<GradesViewModel> GetStudentGrades(string studentId)
        {
            List<dynamic> gradesList = (List<dynamic>)PrepareStudentGrades(studentId);

            List<GradesViewModel> viewModelList = new List<GradesViewModel>();

            foreach (var item in gradesList)
            {
                // Check if the item already exists in viewModelList
                var existingItem = viewModelList.Find(g => g.SemesterEnglishName == item.SemesterEnglishName && g.SubjectEnglishName == item.SubjectEnglishName);

                if (existingItem == null)
                {
                    // If not, add a new GradesViewModel
                    viewModelList.Add(new GradesViewModel()
                    {
                        SemesterArabicName = item.SemesterArabicName,
                        SemesterEnglishName = item.SemesterEnglishName,
                        SubjectArabicName = item.SubjectArabicName,
                        SubjectEnglishName = item.SubjectEnglishName
                    });

                    // Get the newly added item
                    existingItem = viewModelList.Last();
                }

                SetExamProperties(existingItem, item);
            }

            return viewModelList;
        }

        private void SetExamProperties(GradesViewModel existingItem, dynamic gradeDetails)
        {
            string examTitle = gradeDetails.ExamTitleEnglishName.Split(' ')[0];

            existingItem.GetType().GetProperty($"ExamTitleArabicName{examTitle}").SetValue(existingItem, gradeDetails.ExamTitleArabicName);
            existingItem.GetType().GetProperty($"ExamTitleEnglishName{examTitle}").SetValue(existingItem, gradeDetails.ExamTitleEnglishName);
            existingItem.GetType().GetProperty($"TypeArabicName{examTitle}").SetValue(existingItem, gradeDetails.TypeArabicName);
            existingItem.GetType().GetProperty($"TypeEnglishName{examTitle}").SetValue(existingItem, gradeDetails.TypeEnglishName);
            existingItem.GetType().GetProperty($"GradeValue{examTitle}").SetValue(existingItem, gradeDetails.GradeValue);
            existingItem.GetType().GetProperty($"TotalGrades{examTitle}").SetValue(existingItem, gradeDetails.TotalGrades);
            existingItem.GetType().GetProperty($"ExamDate{examTitle}").SetValue(existingItem, gradeDetails.ExamDate);
            existingItem.GetType().GetProperty($"StaffArabicName{examTitle}").SetValue(existingItem, gradeDetails.StaffArabicName);
            existingItem.GetType().GetProperty($"StaffEnglishName{examTitle}").SetValue(existingItem, gradeDetails.StaffEnglishName);
            existingItem.GetType().GetProperty($"ExamStatus{examTitle}").SetValue(existingItem, gradeDetails.ExamStatus);
        }

        public IEnumerable<dynamic> GetTeacherGrades(int semesterID, int sectionID, int subjectID,
            int examTypeID, int examTitleID)
        {
            List<dynamic> gradesList = new List<dynamic>();
            string query = "SELECT DISTINCT g.ID AS GradeID, g.ExamID, g.StudentID, " +
                "s.StudentArabicName, StudentEnglishName, g.GradeValue, e.TotalGrades FROM Grades g " +
                "INNER JOIN Exams e ON g.ExamID = e.ID " +
                "INNER JOIN StudentSchoolDetails d ON d.SectionID = e.SectionID " +
                "INNER JOIN Student s ON s.StudentID = d.StudentID " +
                "WHERE SemesterID = @SemesterID AND e.SectionID = @SectionID AND SubjectID = @SubjectID " +
                "AND ExamTypeID = @ExamTypeID AND ExamTitleID = @ExamTitleID " +
                "AND (g.StudentID = s.StudentID)";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SemesterID", semesterID);
                    comm.Parameters.AddWithValue("@SectionID", sectionID);
                    comm.Parameters.AddWithValue("@SubjectID", subjectID);
                    comm.Parameters.AddWithValue("@ExamTypeID", examTypeID);
                    comm.Parameters.AddWithValue("@ExamTitleID", examTitleID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        gradesList = reader.MapAll();
                    }
                }
                conn.Close();
            }

            return gradesList;
        }

        public void AddStudentsGrades(Dictionary<int, double> studentsGrades)
        {
            StringBuilder query = new StringBuilder();
            foreach (var kvp in studentsGrades)
            {
                query.Append("UPDATE Grades SET GradeValue = ").Append(kvp.Value).Append(" WHERE ID = ")
                    .Append(kvp.Key).Append(";");
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}