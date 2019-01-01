
using System.Collections.Generic;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IDivisionGateway
    {
        IEnumerable<Division> GetAll();
    }
}
