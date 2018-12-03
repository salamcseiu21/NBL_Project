using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace NblClassLibrary.Models
{


    public class Client
    {
        public int ClientId { get; set; }
        [Required(ErrorMessage = "Client Name is required")]
        [Display(Name ="Name")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Client Address is required")]
        [Display(Name = "NID")]
        public string NationalIdNo { get; set; }
        [Display(Name = "TIN")]
        public string TinNo { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Client Phone is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Client Alternate Phone is required")]
        [Display(Name ="Alternate Phone")]
        public string AlternatePhone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        [Display(Name = "Father's Name")]
        public string FathersName { get; set; }
        [Display(Name = "Mother's Name")]
        public string MothersName { get; set; }
        public DateTime DoB { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Required]
        public string Gender { get; set; }
        [Display(Name = "Image")]
        public string ClientImage { get; set; }
        [Display(Name = "Signature")]
        public string ClientSignature { get; set; }

        [Display(Name = "Document")]
        public string ClientDocument { get; set; } 
        [Required]
        [Display(Name = "Sub Sub Sub Account Code")]
        public string SubSubSubAccountCode { get; set; }
        [Display(Name = "Sub Sub Account Code")]
        public string SubSubAccountCode { get; set; }
        [Required]
        [Display(Name = "Client Type")]
        public int ClientTypeId { get; set; } 
        [Required]
        [Display(Name = "Region")]
        public int RegionId { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        [Required]
        [Display(Name = "Upazilla")]
        public int UpazillaId { get; set; }
        [Required]
        [Display(Name = "Post Office")]
        public int PostOfficeId { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string Active { get; set; }
        [Display(Name = "Credit Limit")]
        [Required]
        public decimal CreditLimit { get; set; }
        public decimal RemainingCredit { get; set; } 
        public int MaxCreditDay { get; set; }
        public decimal Outstanding { set; get; }
        [Display(Name ="Territory")]
        public int TerritoryId { get; set; }
        public bool EmailInUse { get; set; }
        public int SerialNo { get; set; }

        public List<Order> Orders { get; set; }
        public List<ClientAttachment> ClientAttachments { set; get; }
        public ClientType ClientType { get; set; }
        public Territory Territory { set; get; }
        public Branch Branch { get; set; }
        public Company Company { get; set; }
        public PostOffice PostOffice { get; set; }
        public District District { get; set; }
        public Division Division { get; set; }
        public Upazilla Upazilla { get; set; }

        public MailingAddress MailingAddress { get; set; }
        public Client()
        {
            ClientType = new ClientType();
            Orders = new List<Order>();
            MailingAddress = new MailingAddress();
            PostOffice = new PostOffice();
            District = new District();
            Upazilla = new Upazilla();
            Division = new Division();
            Branch=new Branch();
            ClientAttachments=new List<ClientAttachment>();
        }
        public int GetTotalOrder()
        {
            return Orders.Count;
        }

        public string GetFullInformaiton()
        {
            return ClientName + "<br/>Account Code :" + SubSubSubAccountCode + "<br/>Address :" + Address + "<br/>Phone:" + Phone + "<br/>E-mail:" + Email;
        }

        public string GetBasicInformation()
        {
            return "<strong>" + ClientName + "</strong><br/>Account Code :" + SubSubSubAccountCode + "<br/>Client Type:" +
                   ClientType.ClientTypeName;
        }
        public string GetContactInformation()
        {
            return "Address :" + Address + "<br/>Phone:" + Phone + "<br/>E-mail:" + Email;
        }
        public string GetMailingAddress()
        {
            string address = "<strong>Phone :" + Phone + "<br/>Alternate Phone :" + AlternatePhone + "<br/>E-mail:" + Email + "<br/>Website:" + Website + "<br/>District: " + District.DistrictName + "<br/>Upazilla:" + Upazilla.UpazillaName + "<br/>Post Office:" + PostOffice.PostOfficeName + "<br/>Post Code:" + PostOffice.Code + "</strong>";
            return address;

        }
    }
}