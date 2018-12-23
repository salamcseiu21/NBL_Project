
using System;
using System.Collections.Generic;
using NBL.DAL.Contracts;

namespace NBL.DAL
{
    public class Gateway<T>:IGateway<T> where T:class
    {
        public bool Add(T model)
        {
            throw new NotImplementedException();
        }

        public bool Update(T model)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T model)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
