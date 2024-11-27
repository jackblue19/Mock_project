using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Feedback to FeedbackDTO
            CreateMap<Feedback, FeedbackDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FbId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.FbContent)) // Optional
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.ItemName)) // Handles nested Item.Name
                .ForMember(dest => dest.ParentFb, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.ParentFeedback, opt => opt.MapFrom(src => src.FbContent != null
                    ? new FeedbackDTO
                    {
                        Id = src.ParentFbFlagNavigation.FbId,
                        Content = src.ParentFbFlagNavigation.FbContent,
                        Username = src.ParentFbFlagNavigation.Username,
                        
                    }
                    : null)) // Map only specific properties of ParentFeedback
                .ForMember(dest => dest.IsReply, opt => opt.MapFrom(src => src.FbContent != null));

            // Map Feedback to ReplyDTO
            CreateMap<Feedback, ReplyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.FbContent)) // Optional
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.ItemName)) // Handles nested Item.Name
                .ForMember(dest => dest.ParentFb, opt => opt.MapFrom(src => src.ParentFbFlag ?? 0)); // Default to 0 if null

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
