using AutoMapper;
using Aviasales.DAL.Models;
using Aviasales.Web.Models;

namespace Aviasales.Web.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(r => r.UserName, o => o.MapFrom(u => u.UserName))
                .ForMember(r => r.Email, o => o.MapFrom(u => u.Email));
            CreateMap<TicketModel, Ticket>()
                .ForMember(m => m.origin, o => o.MapFrom(t => t.origin))
                .ForMember(m => m.origin_name, o => o.MapFrom(t => t.origin_name))
                .ForMember(m => m.destination, o => o.MapFrom(t => t.destination))
                .ForMember(m => m.destination_name, o => o.MapFrom(t => t.destination_name))
                .ForMember(m => m.carrier, o => o.MapFrom(t => t.carrier))
                .ForMember(m => m.departure_date, o => o.MapFrom(t => t.departure_date))
                .ForMember(m => m.departure_time, o => o.MapFrom(t => t.departure_time))
                .ForMember(m => m.arrival_date, o => o.MapFrom(t => t.arrival_date))
                .ForMember(m => m.arrival_time, o => o.MapFrom(t => t.arrival_time))
                .ForMember(m => m.stops, o => o.MapFrom(t => t.stops))
                .ForMember(m => m.price, o => o.MapFrom(t => t.price));
        }
    }
}
