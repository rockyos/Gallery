using AutoMapper;
using CoreTest.Models;
using CoreTest.Models.Dto;
using CoreTest.Models.Entity;

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
