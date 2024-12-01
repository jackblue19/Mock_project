using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Feedback, FeedbackDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FbId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.FbContent))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.FbDatetime))
            .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.UsernameNavigation.ProfileImage))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.ItemName))
            .ForMember(dest => dest.ParentFb, opt => opt.MapFrom(src => src.ParentFbFlag))
            .ForMember(dest => dest.ParentFeedback, opt => opt.MapFrom(src => src.ParentFbFlagNavigation))
            .ForMember(dest => dest.IsReply, opt => opt.MapFrom(src => src.ParentFbFlag != null));

            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(dest => dest.FbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FbContent, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.FbDatetime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ParentFbFlag, opt => opt.MapFrom(src => src.ParentFb));
                // .ForMember(dest => dest.UsernameNavigation.Name, opt => opt.MapFrom(src => src.Username))
                // .ForMember(dest => dest.Item.ItemName, opt => opt.MapFrom(src => src.ItemName));

            CreateMap<Feedback, ReplyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FbId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.FbDatetime))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UsernameNavigation.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.UsernameNavigation.ProfileImage))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.ItemName))
                .ForMember(dest => dest.ParentFb, opt => opt.MapFrom(src => src.ParentFbFlag));

            // Map Item to ItemDTO
            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.ItemCategory, opt => opt.MapFrom(src => src.ItemCategory))
                .ForMember(dest => dest.ItemStatus, opt => opt.MapFrom(src => src.ItemStatus))
                .ForMember(dest => dest.ItemDescription, opt => opt.MapFrom(src => src.ItemDescription))
                .ForMember(dest => dest.SuggestedPrice, opt => opt.MapFrom(src => src.SuggestedPrice))
                .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.ItemImage))
                .ForMember(dest => dest.IsServed, opt => opt.MapFrom(src => src.IsServed));
        }
    }
}
