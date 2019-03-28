using AutoMapper;
using CoreTest.Models;

namespace CoreTest.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Photo, PhotoDto>();
        }
    }
}
