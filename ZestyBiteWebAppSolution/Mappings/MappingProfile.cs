using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;

namespace ZestyBiteWebAppSolution.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            // Feedback to FeedbackDTO mapping
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

            // FeedbackDTO to Feedback mapping
            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(dest => dest.FbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FbContent, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.FbDatetime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ParentFbFlag, opt => opt.MapFrom(src => src.ParentFb));

            // Feedback to ReplyDTO mapping
            CreateMap<Feedback, ReplyDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FbId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.FbContent))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.FbDatetime))
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.UsernameNavigation.ProfileImage))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.ItemName))
                .ForMember(dest => dest.ParentFb, opt => opt.MapFrom(src => src.ParentFbFlag));

            // Item to ItemDTO mapping
            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.ItemCategory, opt => opt.MapFrom(src => src.ItemCategory))
                .ForMember(dest => dest.ItemStatus, opt => opt.MapFrom(src => src.ItemStatus))
                .ForMember(dest => dest.ItemDescription, opt => opt.MapFrom(src => src.ItemDescription))
                .ForMember(dest => dest.SuggestedPrice, opt => opt.MapFrom(src => src.SuggestedPrice))
                .ForMember(dest => dest.ItemImage, opt => opt.MapFrom(src => src.ItemImage))
                .ForMember(dest => dest.IsServed, opt => opt.MapFrom(src => src.IsServed));

            // Table to TableDTO mapping
            CreateMap<Table, TableDTO>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId))
                .ForMember(dest => dest.TableCapacity, opt => opt.MapFrom(src => src.TableCapacity))
                .ForMember(dest => dest.TableMaintenance, opt => opt.MapFrom(src => src.TableMaintenance))
                .ForMember(dest => dest.TableType, opt => opt.MapFrom(src => src.TableType))
                .ForMember(dest => dest.TableStatus, opt => opt.MapFrom(src => src.TableStatus))
                .ForMember(dest => dest.TableNote, opt => opt.MapFrom(src => src.TableNote))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId));

            // TableDTO to Table mapping
            CreateMap<TableDTO, Table>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId))
                .ForMember(dest => dest.TableCapacity, opt => opt.MapFrom(src => src.TableCapacity))
                .ForMember(dest => dest.TableMaintenance, opt => opt.MapFrom(src => src.TableMaintenance))
                .ForMember(dest => dest.TableType, opt => opt.MapFrom(src => src.TableType))
                .ForMember(dest => dest.TableStatus, opt => opt.MapFrom(src => src.TableStatus))
                .ForMember(dest => dest.TableNote, opt => opt.MapFrom(src => src.TableNote))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId));

            // TableDetail to TableDetailDTO mapping
            CreateMap<TableDetail, TableDetailDTO>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            //.ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item)); // Include item details if needed

            // TableDetailDTO to TableDetail mapping
            CreateMap<TableDetailDTO, TableDetail>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.TableId))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
                //.ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Item)); // Include item details if needed
        }
    }
}