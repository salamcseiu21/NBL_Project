using AutoMapper;
using NBL.Models.ViewModels;

namespace NBL.Models.AutoMapper
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
