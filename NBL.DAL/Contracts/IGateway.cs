using System.Collections.Generic;

namespace NBL.DAL.Contracts
{
    public interface IGateway<T> where T:class 
    {
        bool Add(T model);
        bool Update(T model);
        bool Remove(T model); 
        T GetById(int id);
        ICollection<T> GetAll();
    }
}
