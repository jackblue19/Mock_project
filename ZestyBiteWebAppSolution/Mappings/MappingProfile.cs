using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Mappings {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Feedback, FeedbackDTO>();
            CreateMap<Feedback, ReplyDTO>();
            CreateMap<Item, ItemDTO>();
        }
    }
}


