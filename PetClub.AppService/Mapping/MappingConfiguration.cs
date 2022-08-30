using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            ////Athletic
            //CreateMap<AthleticDTO, CreateAthleticViewModel>().ReverseMap();
            //CreateMap<Athletic, AthleticDTO>().ReverseMap();
            //CreateMap<Athletic, AthleticViewModel>().ReverseMap();


            ////Event
            //CreateMap<EventDTO, CreateEventViewModel>().ReverseMap();
            //CreateMap<Event, EventDTO>().ForMember(e => e.PassaportTicketLot, opt => opt.MapFrom(p => p.PassaportTicketLot)).ReverseMap();
            //CreateMap<EventAthleticDTO, CreateAthleticViewModel>().ReverseMap();
            //CreateMap<EventAthletic, EventAthleticDTO>().ReverseMap();
            //CreateMap<EventItemDTO, CreateEventItemViewModel>().ReverseMap();
            //CreateMap<EventItem, EventItemDTO>().ReverseMap();
            //CreateMap<EventDTO, UpdateEventViewModel>().ReverseMap();
            //CreateMap<EventAthleticDTO, UpdateEventAthleticViewModel>().ReverseMap();
            //CreateMap<EventItemDTO, UpdateEventItemViewModel>().ReverseMap();





            //CreateMap<BankAccountDTO, CreateBankAccountViewModel>().ReverseMap();
            //CreateMap<BankAccount, BankAccountDTO>().ReverseMap();


            ////Lot
            //CreateMap<PassaportTicketLotDTO, CreatePassaportTicketLotViewModel>().ReverseMap();
            //CreateMap<PassaportTicketLot, PassaportTicketLotDTO>().ReverseMap();


            //CreateMap<PaymentMethodViewModel, PaymentMethod>().ReverseMap();

            ////Mapeamento Contas a receber
            //CreateMap<ReceivableBills, ReceivableBillsViewModel>().ForMember(c => c.AthleticViewModel, opt => opt.MapFrom(a => a.Athletic))
            //                                                      .ForMember(c => c.PurchaseOrderViewModel, opt => opt.MapFrom(a => a.PurchaseOrder));


            //CreateMap<RequestAssociate, CreateRequestAssociateViewModel>().ReverseMap();

            //CreateMap<ProductStock, ProductStockViewModel>().ReverseMap();
            //CreateMap<ProductStock, CreateProductStockViewModel>().ReverseMap();
            //CreateMap<ProductStock, UpdateProductStockViewModel>().ReverseMap();
            //CreateMap<UpdateProductViewModel, Product>().ReverseMap();

            ////CreateMap<Product, ListProductViewModel>().ForMember(p => p.ProductStockViewModel, opt => opt.MapFrom(a => a.ProductStock));


            //CreateMap<PassaportTicketLotStock, CreateTicketLotStockViewModel>().ReverseMap();
            //CreateMap<PagingParametersViewModel, PaginationFilter>().ReverseMap();


            //CreateMap<TicketViewModel, Ticket>().ReverseMap();

            ////CreateMap<UserCard, CreateUserCardViewModel>().ReverseMap();
            //CreateMap<UserCard, ListUserCardViewModel>();

            //CreateMap<CreatePurchaseOrderProductViewModel, PurchaseOrder>().ReverseMap();
            //CreateMap<CreatePurchaseOrderItemTicketViewModel, PurchaseOrderItem>().ReverseMap();
            //CreateMap<CreatePurchaseOrderItemProductViewModel, PurchaseOrderItem>().ReverseMap();
        }
    }
}
