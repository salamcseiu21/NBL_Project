﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class ClientGateway:DbGateway,IClientGateway
    {
       // readonly OrderManager _orderManager = new OrderManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        public int Save(Client client)
        {
            try
            {
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_AddNewClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Name", client.ClientName);
                CommandObj.Parameters.AddWithValue("@CommercialName", client.CommercialName);
                CommandObj.Parameters.AddWithValue("@Address", client.Address);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", client.SubSubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@SubSubAccountCode",client.SubSubAccountCode);
                CommandObj.Parameters.AddWithValue("@PostOfficeId", client.PostOfficeId);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", client.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@Phone", client.Phone);
                CommandObj.Parameters.AddWithValue("@AltPhone", client.AlternatePhone);
                CommandObj.Parameters.AddWithValue("@Gender", client.Gender);
                CommandObj.Parameters.AddWithValue("@ClientImage", client.ClientImage);
                CommandObj.Parameters.AddWithValue("@ClientSignature", client.ClientSignature);
                CommandObj.Parameters.AddWithValue("@Email", client.Email);
                //CommandObj.Parameters.AddWithValue("@Fax", client.Fax);
                //CommandObj.Parameters.AddWithValue("@Website", client.Website);
                CommandObj.Parameters.AddWithValue("@CreditLimit", client.CreditLimit);
                CommandObj.Parameters.AddWithValue("@UserId", client.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", client.Branch.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", client.CompanyId);
                CommandObj.Parameters.AddWithValue("@NationalIdNo", client.NationalIdNo);
                CommandObj.Parameters.AddWithValue("@TinNo", client.TinNo);
                CommandObj.Parameters.AddWithValue("@TerritoryId", client.TerritoryId);
                CommandObj.Parameters.AddWithValue("@RegionId", client.RegionId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Save", sqlException);
            }
            catch(Exception e)
            {
                throw new Exception("Clould not Save Client",e);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public List<Client> GetPendingClients()
        {
            try
            {

                List<Client> clients = new List<Client>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAllPendingClients";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        SerialNo = Convert.ToInt32(reader["SlNo"])

                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect pending Clients due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to collect pending Clients", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        public IEnumerable<ClientAttachment> GetClientAttachmentsByClientId(int clientId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetClientAttachmentsByClientId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                List<ClientAttachment> attachments = new List<ClientAttachment>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    attachments.Add(new ClientAttachment
                    {
                        AttachmentName = reader["AttachmentName"].ToString(),
                        ClientId = clientId,
                        FileExtension = reader["FileExtension"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedByUserId = Convert.ToInt32(reader["UploadedByUserId"]),
                        Id = Convert.ToInt32(reader["AttachmentId"])
                    });
                }
                reader.Close();
                return attachments;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Clients attachment by Client Id due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to collect Client attachment by Client Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
    
        public IEnumerable<ClientAttachment> GetClientAttachments()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetClientAttachments";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ClientAttachment> attachments=new List<ClientAttachment>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    attachments.Add(new ClientAttachment
                    {
                        AttachmentName = reader["AttachmentName"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        FileExtension = reader["FileExtension"].ToString(),
                        FilePath = reader["FilePath"].ToString(),
                        UploadedByUserId = Convert.ToInt32(reader["UploadedByUserId"]),
                        Id = Convert.ToInt32(reader["AttachmentId"])
                    });
                }
                reader.Close();
                return attachments;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Clients attachment due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to collect Client attachment", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public Client GetClientByEmailAddress(string email)
        {
            try
            {
                Client client = new Client();
                CommandObj.CommandText = "spGetClientByEmailAddress";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Email",email);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                if(reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone = reader["AltPhone"].ToString();
                    client.Email = reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = reader["Fax"].ToString();
                    client.Website = reader["Website"].ToString();
                    client.PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]);
                    client.ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.ClientImage = reader["ClientImage"].ToString();
                    client.ClientSignature = reader["ClientSignature"].ToString();
                    client.NationalIdNo = reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.TerritoryId = Convert.ToInt32(reader["TerritoryId"]);
                }
                reader.Close();
                return client;
            }
            catch(SqlException sqlException)
            {
                throw new Exception("Unable to Get Client By Email", sqlException);
            }
            catch (Exception e)
            {

                throw new Exception("Unable to Get Client By Email",e);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        IEnumerable<Client> IClientGateway.GetAll()
        {
            try
            {

                List<Client> clients = new List<Client>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetAllClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new Client
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        ClientImage = reader["ClientImage"].ToString(),
                        ClientSignature = reader["ClientSignature"].ToString(),
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        Active = reader["Active"].ToString(),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        RegionId = Convert.ToInt32(reader["RegionId"]),

                        ClientType = _commonGateway.GetAllClientType.ToList()
                            .Find(n => n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"]))
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Clients due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to collect Clients", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

       
        public int Update(int id, Client client)
        {
            try
            {
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_UpdateClientInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", id);
                CommandObj.Parameters.AddWithValue("@Name", client.ClientName);
                CommandObj.Parameters.AddWithValue("@CommercialName", client.CommercialName);
                CommandObj.Parameters.AddWithValue("@Address", client.Address);
                CommandObj.Parameters.AddWithValue("@PostOfficeId", client.PostOfficeId);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", client.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@Phone", client.Phone);
                CommandObj.Parameters.AddWithValue("@AltPhone", client.AlternatePhone);
                CommandObj.Parameters.AddWithValue("@Gender", client.Gender);
                CommandObj.Parameters.AddWithValue("@ClientImage", client.ClientImage);
                CommandObj.Parameters.AddWithValue("@ClientSignature", client.ClientSignature);
                CommandObj.Parameters.AddWithValue("@Email", client.Email);
                CommandObj.Parameters.AddWithValue("@CreditLimit", client.CreditLimit);
                //CommandObj.Parameters.AddWithValue("@Fax", client.Fax);
                //CommandObj.Parameters.AddWithValue("@Website", client.Website);
                CommandObj.Parameters.AddWithValue("@UserId", client.UserId);
                CommandObj.Parameters.AddWithValue("@NationalIdNo", client.NationalIdNo);
                CommandObj.Parameters.AddWithValue("@TinNo", client.TinNo);
                CommandObj.Parameters.AddWithValue("@TerritoryId", client.TerritoryId);
                CommandObj.Parameters.AddWithValue("@RegionId", client.RegionId);
                CommandObj.Parameters.AddWithValue("@Active", client.Active);
                CommandObj.Parameters.AddWithValue("@BranchId", client.BranchId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to update  client info due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to update  client info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Client GetClientById(int clientId)
        {

            try
            {
                Client client = new Client();
                CommandObj.CommandText = "UDSP_GetClientById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId",clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.CommercialName = reader["CommercialName"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone = reader["AltPhone"].ToString();
                    client.Email = reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = reader["Fax"].ToString();
                    client.Website = reader["Website"].ToString();
                    client.PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]);
                    client.ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]);
                    client.UpazillaId = Convert.ToInt32(reader["UpazillaId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.DistrictId = Convert.ToInt32(reader["DistrictId"]);
                    client.DivisionId = Convert.ToInt32(reader["DivisionId"]);
                    client.ClientImage = reader["ClientImage"].ToString();
                    client.ClientSignature = reader["ClientSignature"].ToString();
                    client.NationalIdNo = reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.TerritoryId = Convert.ToInt32(reader["TerritoryId"]);
                    client.RegionId = Convert.ToInt32(reader["RegionId"]);
                    client.SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString();
                    client.BranchId = Convert.ToInt32(reader["BranchId"]);
                    client.ClientType = new ClientType
                    {
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"])
                    };

                }
                reader.Close();
                return client;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Client Information by client Id due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {

                throw new Exception("Unable to collect Client Information by client Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }

        }
        public ViewClient GetClientDeailsById(int clientId) 
        {

            try
            {
                ViewClient client = new ViewClient();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "UDSP_GetClientDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    client.ClientId = Convert.ToInt32(reader["ClientId"]);
                    client.ClientName = reader["Name"].ToString();
                    client.CommercialName = reader["CommercialName"].ToString();
                    client.Address = reader["Address"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.AlternatePhone = reader["AltPhone"].ToString();
                    client.Email = reader["Email"].ToString();
                    client.Gender = reader["Gender"].ToString();
                    client.Fax = reader["Fax"].ToString();
                    client.Website = reader["Website"].ToString();
                    client.PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]);
                    client.UserId = Convert.ToInt32(reader["UserId"]);
                    client.Division = new Division
                    {
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        DivisionName = reader["DivisionName"].ToString()
                    };
                    client.District = new District
                    {
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionId = Convert.ToInt32(reader["DivisionId"])
                    };
                    client.Upazilla = new Upazilla
                    {
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"]),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        DistrictId = Convert.ToInt32(reader["DistrictId"])
                    };
                    client.PostOffice = new PostOffice
                    {
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        PostOfficeName = reader["PostOfficeName"].ToString(),
                        Code = reader["PostCode"].ToString(),
                    };
                    client.ClientType = new ClientType
                    {
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        ClientTypeName = reader["ClientTypeName"].ToString()
                    };
                    client.ClientTypeName = reader["ClientTypeName"].ToString();
                    client.Discount = Convert.ToDecimal(reader["Discount"]);
                    client.SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString();
                    client.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                    client.ClientImage = reader["ClientImage"].ToString();
                    client.ClientSignature = reader["ClientSignature"].ToString();
                    client.NationalIdNo = reader["NationalIdNo"].ToString();
                    client.Active = reader["Active"].ToString();
                    client.BranchName = reader["ClientBranch"].ToString();
                    client.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                    client.MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]);
                    client.ClientType = _commonGateway.GetAllClientType.ToList()
                        .Find(n => n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"]));
                  
                    client.Outstanding = Convert.ToDecimal(reader["Outstanding"]);
                   

                }
                reader.Close();
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                client.ClientAttachments = GetClientAttachmentsByClientId(clientId).ToList();
                return client;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Client Information due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {

                throw new Exception("Unable to collect Client Information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }

        }

       

        public IEnumerable<ViewClient> GetAllClientDetails()
        {
            try
            {
                List<ViewClient> clients = new List<ViewClient>();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "spGetAllClientDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Fax = reader["Fax"].ToString(),
                        Website = reader["Website"].ToString(),
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        PostCode = reader["PostCode"].ToString(),
                        PostOfficeName = reader["PostOfficeName"].ToString(),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionName = reader["DivisionName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                        ClientImage = reader["ClientImage"].ToString(),
                        ClientSignature = reader["ClientSignature"].ToString(),
                        NationalIdNo = reader["NationalIdNo"].ToString(),
                        Active = reader["Active"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"])
                    });

                }
                reader.Close();
                return clients;

            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Client Information due to Sql Exception", sqlException);
            }
            catch (Exception e)
            {

                throw new Exception("Unable to collect Client Information", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxSerialNoOfClientByAccountPrefix(string acountPrefix)
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "spGetMaxSerialNoOfClientByAccountPrefix";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@Prefix", acountPrefix);
               
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxSlno = Convert.ToInt32(reader["MaxClientNo"]);
                }
                reader.Close();
                return maxSlno;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not get  Client max serial", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewClient> GetAllClientDetailsByBranchId(int branchId)
        {
            try
            {
                List<ViewClient> clients = new List<ViewClient>();
                CommandObj.Parameters.Clear();
                CommandObj.CommandText = "spGetAllClientDetailsByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {


                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Fax = reader["Fax"].ToString(),
                        Website = reader["Website"].ToString(),
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        UpazillaId = Convert.ToInt32(reader["UpazillaId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        DistrictId = Convert.ToInt32(reader["DistrictId"]),
                        DivisionId = Convert.ToInt32(reader["DivisionId"]),
                        ClientTypeName = reader["ClientTypeName"].ToString(),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        PostCode = reader["PostCode"].ToString(),
                        PostOfficeName = reader["PostOfficeName"].ToString(),
                        UpazillaName = reader["UpazillaName"].ToString(),
                        DistrictName = reader["DistrictName"].ToString(),
                        DivisionName = reader["DivisionName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                        ClientImage = reader["ClientImage"].ToString(),
                        ClientSignature = reader["ClientSignature"].ToString(),
                        NationalIdNo = reader["NationalIdNo"].ToString(),
                        Active = reader["Active"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),

                    });

                }
                reader.Close();
                return clients;

            }
            catch (Exception e)
            {

                throw new Exception("Unable to collect Client Information", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Client> GetClientByBranchId(int branchId)
        {
            try
            {

                List<Client> clients = new List<Client>();
                ConnectionObj.Open();
                CommandObj.CommandText = "UDSP_GetClientByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {

                    clients.Add(new Client
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        ClientImage = reader["ClientImage"].ToString(),
                        ClientSignature = reader["ClientSignature"].ToString(),
                        PostOfficeId = Convert.ToInt32(reader["PostOfficeId"]),
                        Phone = reader["Phone"].ToString(),
                        AlternatePhone = reader["AltPhone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                        Active = reader["Active"].ToString(),
                        CreditLimit = Convert.ToDecimal(reader["CreditLimit"]),
                        MaxCreditDay = Convert.ToInt32(reader["MaxCreditDay"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        RegionId = Convert.ToInt32(reader["RegionId"]),
                      
                        ClientType = _commonGateway.GetAllClientType.ToList().Find(n =>
                            n.ClientTypeId == Convert.ToInt32(reader["ClientTypeId"]))
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Unable to collect Clients by Branch Id due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to collect Clients by Branch Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();

            }
        }

        

        public int ApproveClient(Client aClient, ViewUser anUser)
        {
            try
            {
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                CommandObj.CommandText = "spApproveNewClient";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", aClient.ClientId);
                CommandObj.Parameters.AddWithValue("@ApprovedByUserId", anUser.UserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not approve new client due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve new client", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public decimal GetClientOustandingBalanceBySubSubSubAccountCode(string subsubsubAccountCode)
        {
            try
            {
                CommandObj.Dispose();
                
                decimal outstangingBalance=0;
                CommandObj.CommandText = "UDSP_ClientOustandingBalanceBySubSubSubAccountCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@SubSubSubCode", subsubsubAccountCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    outstangingBalance = Convert.ToDecimal(reader["Outstanding"]);
                }
                reader.Close();
                return outstangingBalance;
            }
            
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect outstanding balance of client by Account code due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect outstanding balance of client by Account code", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UploadClientDocument(ClientAttachment clientAttachment)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UploadClientDocument";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", clientAttachment.ClientId);
                CommandObj.Parameters.AddWithValue("@AttachmentName", clientAttachment.AttachmentName);
                CommandObj.Parameters.AddWithValue("@UploadedByUserId", clientAttachment.UploadedByUserId);
                CommandObj.Parameters.AddWithValue("@FileExtension", clientAttachment.FileExtension);
                CommandObj.Parameters.AddWithValue("@FilePath", clientAttachment.FilePath);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not upload client document due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not upload client document", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewClientSummaryModel> GetClientSummary()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetClientSummary";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                List<ViewClientSummaryModel> summary=new List<ViewClientSummaryModel>();
                while (reader.Read())
                {
                    summary.Add(new ViewClientSummaryModel
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["Name"].ToString(),
                        CommercialName = reader["CommercialName"].ToString(),
                        Debit = Convert.ToDecimal(reader["DebitAmount"]),
                        Credit = Convert.ToDecimal(reader["CreditAmount"]),
                        Outstanding = Convert.ToDecimal(reader["OutStanding"]),
                        TotalOrder = Convert.ToInt32(reader["TotalOrder"])
                    });
                }

                return summary;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not upload client summary due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not upload client summary", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
    }
}