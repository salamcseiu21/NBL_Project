﻿
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IClientManager:IManager<Client>
   {
       
       bool ApproveClient(Client aClient, ViewUser anUser);
       List<Client> GetPendingClients();
       Client GetClientByEmailAddress(string email);
       IEnumerable<Client> GetClientByBranchId(int branchId);
       ViewClient GetClientDeailsById(int clientId);
       IEnumerable<ViewClient> GetAllClientDetails();
       IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId);
       decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode);
       bool UploadClientDocument(ClientAttachment clientAttachment);
       IEnumerable<ClientAttachment> GetClientAttachments();
       IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId);
       IEnumerable<ViewClientSummaryModel> GetClientSummary();

   }
}
