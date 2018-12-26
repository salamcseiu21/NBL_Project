using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IClientGateway
    {
        int Save(Client client);
        int ApproveClient(Client aClient, ViewUser anUser);
        List<Client> GetPendingClients();
        Client GetClientByEmailAddress(string email);
        IEnumerable<Client> GetAll();
        IEnumerable<Client> GetClientByBranchId(int branchId);
        Client GetClientById(int clientId);
        ViewClient GetClientDeailsById(int clientId);
        int Update(int id, Client client);
        IEnumerable<ViewClient> GetAllClientDetails();
        IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId);
        decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode);
        int UploadClientDocument(ClientAttachment clientAttachment);
        IEnumerable<ClientAttachment> GetClientAttachments();
        IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId);
        IEnumerable<ViewClientSummaryModel> GetClientSummary();
        int GetMaxSerialNoOfClientByAccountPrefix(string acountPrefix);


    }
}
