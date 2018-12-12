using System;
using AutoMapper;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.Models.AutoMapper
{
    class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<ViewClient, Client>();
            CreateMap<Client, ViewClient>();
            CreateMap<Branch, ViewBranch>();
            CreateMap<ViewBranch, Branch>();
            CreateMap<Order, ViewOrder>();
            CreateMap<ViewOrder, Order>();
            CreateMap<ViewCreateProductionNoteModel, ProductionNote>();
            CreateMap<ProductionNote, ViewCreateProductionNoteModel>();
        }
    }
}
