using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IClientManager
   {
       string Save(Client client);
       bool ApproveClient(Client aClient, ViewUser anUser);
       List<Client> GetPendingClients();
       Client GetClientByEmailAddress(string email);
       IEnumerable<Client> GetAll();
       IEnumerable<Client> GetClientByBranchId(int branchId);
       Client GetClientById(int clientId);
       ViewClient GetClientDeailsById(int clientId);
       string Update(int id, Client client);
       IEnumerable<ViewClient> GetAllClientDetails();
       IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId);
       decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode);
       bool UploadClientDocument(ClientAttachment clientAttachment);
       IEnumerable<ClientAttachment> GetClientAttachments();
       IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId);
       IEnumerable<ViewClientSummaryModel> GetClientSummary();

   }
}
