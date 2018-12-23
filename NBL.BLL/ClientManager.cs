﻿using System;
using System.Collections.Generic;
using System.Linq;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class ClientManager
    {
        readonly ClientGateway _clientGateway = new ClientGateway();
        readonly OrderManager _orderManager=new OrderManager();
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
            var lastClientNo = _clientGateway.GetMaxSerialNoOfClientByAccountPrefix(acountPrefix);
            var accountCode = Generator.GenerateAccountCode(acountPrefix, lastClientNo);
            client.SubSubAccountCode = acountPrefix;
            client.SubSubSubAccountCode = accountCode;
            int result = _clientGateway.Save(client);
            return result > 0 ? "Saved Successfully!" : "Failed to Insert into database";
        }

        public bool ApproveClient(Client aClient, ViewUser anUser)
        {
           
            int rowAffected=_clientGateway.ApproveClient(aClient,anUser);
            return rowAffected != 0;
        }

        public List<Client> GetPendingClients()
        {
            return _clientGateway.GetPendingClients();
        }

        private bool IsEmailAddressUnique(string email)
        {
            var result = _clientGateway.GetClientByEmailAddress(email);
            return result.Email == null;
        }

        public Client GetClientByEmailAddress(string email)
        {
            return _clientGateway.GetClientByEmailAddress(email);
        }

        private bool CheckEmail(string email)
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }

       // public IEnumerable<Client> GetAll => _clientGateway.GetAll;

        public IEnumerable<Client> GetAll
        {
            get
            {
                var clients = _clientGateway.GetAll;
                foreach (Client client in clients)
                {
                    client.Orders = _orderManager.GetOrdersByClientId(client.ClientId).ToList();
                }
                return clients;
            }
        }
        public IEnumerable<Client> GetClientByBranchId(int branchId)
        {
            var clients = _clientGateway.GetClientByBranchId(branchId);
            foreach (Client client in clients)
            {
                client.Orders = _orderManager.GetOrdersByClientId(client.ClientId).ToList();
            }
            return clients;
        }
        public Client GetClientById(int clientId)
        {
           var client=_clientGateway.GetClientById(clientId);
            client.Outstanding =GetClientOustandingBalanceBySubSubSubAccountCode(client.SubSubSubAccountCode);
            return client;

        }
        public ViewClient GetClientDeailsById(int clientId)
        {
            var client = _clientGateway.GetClientDeailsById(clientId);
             client.Orders = _orderManager.GetOrdersByClientId(clientId).ToList();
            return client;

        }
        public string Update(int id, Client client)
        {
            int result = _clientGateway.Update(id, client);
            return result > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public IEnumerable<ViewClient> GetAllClientDetails()
        {
            return _clientGateway.GetAllClientDetails();
        }

        public IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId)
        {
            return _clientGateway.GetAllClientDetailsByBranchId(branchId);
        }

        public decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode)
        {
            return _clientGateway.GetClientOustandingBalanceBySubSubSubAccountCode(subsubsubAccountCode);
        }

        public bool UploadClientDocument(ClientAttachment clientAttachment)
        {
            int rowAffected = _clientGateway.UploadClientDocument(clientAttachment);
            return rowAffected > 0;
        }

        public IEnumerable<ClientAttachment> GetClientAttachments()
        {
            return _clientGateway.GetClientAttachments();
        }

        public IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId)
        {
            return _clientGateway.GetClientAttachmentsByClientId(clientId);
        }

        public IEnumerable<ViewClientSummaryModel> GetClientSummary()
        {
            return _clientGateway.GetClientSummary();
        }
    }
}