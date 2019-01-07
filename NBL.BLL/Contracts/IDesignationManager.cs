
using NBL.Models;

namespace NBL.BLL.Contracts
{
   public interface IDesignationManager:IManager<Designation>
   {
        Designation GetDesignationByCode(string code);

    }
}
