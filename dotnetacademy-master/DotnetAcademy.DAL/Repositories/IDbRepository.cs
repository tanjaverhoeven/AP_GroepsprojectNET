using System.Collections.Generic;

namespace DotnetAcademy.DAL.Repositories
{
    public interface IDbRepository<T> {
        void Create(T t);
        void Update(T t);
        void Delete(T t);
        T FindById(int? id);
        List<T> GetAll();
    }
}
