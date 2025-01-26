using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolLifeAPI.Models.Shared
{
    public interface IRepository <T> where T : class
    {
        IEnumerable<T> GetAll ();
        T GetById (int id);
        T Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
