
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class ClientManager:IClientManager
    {
        readonly IClientGateway _iClientGateway;
        readonly IOrderManager _iOrderManager;

        public ClientManager(IClientGateway iClientGateway,IOrderManager iOrderManager)
        {
            _iClientGateway = iClientGateway;
            _iOrderManager = iOrderManager;
        }
        
        public string Save(Client client)
        {

            bool isEmailValid = CheckEmail(client.Email);
            if (!isEmailValid) return "Invalid Email Address";
            bool isUnique = IsEmailAddressUnique(client.Email);
            if (!isUnique) return "Email Address must be Unique";
            string acountPrefix = "33050";
            switch (client.ClientTypeId)
            {
                case 1:
                    acountPrefix += "4";
                    break;
                case 2:
                    acountPrefix += "5";
                    break;
                case 3: acountPrefix += "3";
                    break;
            }
            var lastClientNo = _iClientGateway.GetMaxSerialNoOfClientByAccountPrefix(acountPrefix);
            var accountCode = Generator.GenerateAccountCode(acountPrefix, lastClientNo);
            client.SubSubAccountCode = acountPrefix;
            client.SubSubSubAccountCode = accountCode;
            int result = _iClientGateway.Save(client);
            return result > 0 ? "Saved Successfully!" : "Failed to Insert into database";
        }

        public bool ApproveClient(Client aClient, ViewUser anUser)
        {
           
            int rowAffected= _iClientGateway.ApproveClient(aClient,anUser);
            return rowAffected != 0;
        }

        public List<Client> GetPendingClients()
        {
            return _iClientGateway.GetPendingClients();
        }

        private bool IsEmailAddressUnique(string email)
        {
            var result = _iClientGateway.GetClientByEmailAddress(email);
            return result.Email == null;
        }

        public Client GetClientByEmailAddress(string email)
        {
            return _iClientGateway.GetClientByEmailAddress(email);
        }

        private bool CheckEmail(string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }

       // public IEnumerable<Client> GetAll => _clientGateway.GetAll;

        public IEnumerable<Client> GetAll()
        {
            var clients = _iClientGateway.GetAll();
            foreach (Client client in clients)
            {
                client.Orders = _iOrderManager.GetOrdersByClientId(client.ClientId).ToList();
            }
            return clients;
        }
        public IEnumerable<Client> GetClientByBranchId(int branchId)
        {
            var clients = _iClientGateway.GetClientByBranchId(branchId);
            foreach (Client client in clients)
            {
                client.Orders = _iOrderManager.GetOrdersByClientId(client.ClientId).ToList();
            }
            return clients;
        }
        public Client GetClientById(int clientId)
        {
           var client= _iClientGateway.GetClientById(clientId);
            client.Outstanding =GetClientOustandingBalanceBySubSubSubAccountCode(client.SubSubSubAccountCode);
            return client;

        }
        public ViewClient GetClientDeailsById(int clientId)
        {
            var client = _iClientGateway.GetClientDeailsById(clientId);
             client.Orders = _iOrderManager.GetOrdersByClientId(clientId).ToList();
            return client;

        }
        public string Update(int id, Client client)
        {
            int result = _iClientGateway.Update(id, client);
            return result > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public IEnumerable<ViewClient> GetAllClientDetails()
        {
            return _iClientGateway.GetAllClientDetails();
        }

        public IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId)
        {
            return _iClientGateway.GetAllClientDetailsByBranchId(branchId);
        }

        public decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode)
        {
            return _iClientGateway.GetClientOustandingBalanceBySubSubSubAccountCode(subsubsubAccountCode);
        }

        public bool UploadClientDocument(ClientAttachment clientAttachment)
        {
            int rowAffected = _iClientGateway.UploadClientDocument(clientAttachment);
            return rowAffected > 0;
        }

        public IEnumerable<ClientAttachment> GetClientAttachments()
        {
            return _iClientGateway.GetClientAttachments();
        }

        public IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId)
        {
            return _iClientGateway.GetClientAttachmentsByClientId(clientId);
        }

        public IEnumerable<ViewClientSummaryModel> GetClientSummary()
        {
            return _iClientGateway.GetClientSummary();
        }
    }
}