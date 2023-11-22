using AutoMapper;
using TreinShop.Domain.Entities;
using TreinShop.ViewModels;

namespace TreinShop.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Trein, TreinVM>().ForMember(dest => dest.AankomstNaam,
                opts => opts.MapFrom(
                    src => src.Aankomst.Name
                ))
                                .ForMember(dest => dest.VertrekNaam,
                    opts => opts.MapFrom(
                        src => src.Vertrek.Name
                    ));
            CreateMap<TreinVM, TreinVM>();
            CreateMap<Station, StationVM>().ForMember(dest => dest.Name,       //manueel mappen omdat datatypes verschillen
                            opts => opts.MapFrom(
                                src => src.Name
                            ));

            CreateMap<Ticket, TicketVM>();
            CreateMap<TicketItem, TicketItemVM>();

            CreateMap<TicketCreateVM, Ticket>();
            CreateMap<Ticket, TicketCreateVM>();

            CreateMap<TicketItemCreateVM, TicketItem>();
            CreateMap<TicketItem, TicketItemCreateVM>();

            CreateMap<AspNetUser, UserVM>();

        }
    }
}
