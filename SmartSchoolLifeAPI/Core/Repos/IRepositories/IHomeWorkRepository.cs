using FireBase.Service;
using SmartSchoolLifeAPI.Core.Models.HomeWork;
using SmartSchoolLifeAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Core.Repos
{
    internal interface IHomeWorkRepository : IRepository<HomeWorkModel>
    {
        HomeWorkVM GetViewById(int id);
        dynamic GetAttachmentByHomeworkId(int homeworkId);
        IEnumerable<dynamic> GetStudentHomeWork(int sectionId, int pageNumber, int pageSize);
        IEnumerable<dynamic> GetTeacherHomeWorks(string teacherId, string schoolClassId, string sectionId, string subjectId, int pageNumber, int pageSize);
        Task<HomeWorkModel> AddAsync(HomeWorkModel model, DeviceType deviceType);
    }
}
