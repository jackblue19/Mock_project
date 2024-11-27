using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZestyBiteWebAppSolution.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IItemRepository _itemRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IAccountRepository accountRepository, IItemRepository itemRepository)
        {
            _feedbackRepository = feedbackRepository;
            _accountRepository = accountRepository;
            _itemRepository = itemRepository;
        }

        // Mapping functions
        private FeedbackDTO MapToFeedbackDTO(Feedback? feedback)
        {
            if (feedback == null) throw new ArgumentNullException(nameof(feedback));

            return new FeedbackDTO
            {
                Id = feedback.FbId,
                Content = feedback.FbContent,
                DateTime = feedback.FbDatetime,
                Username = feedback.Username,
                ProfileImage = feedback.UsernameNavigation?.ProfileImage ?? string.Empty,
                ItemId = feedback.ItemId,
                ItemName = feedback.Item?.ItemName ?? "Unknown",
                ParentFb = feedback.ParentFbFlag,
                ParentFeedback = feedback.ParentFbFlagNavigation != null ? MapToFeedbackDTO(feedback.ParentFbFlagNavigation) : null,
                IsReply = feedback.ParentFbFlag != null
            };
        }

        private ItemDTO? MapToItemDTO(Item? item)
        {
            if (item == null) return null; return new ItemDTO
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemCategory = item.ItemCategory,
                ItemStatus = item.ItemStatus,
                ItemDescription = item.ItemDescription,
                SuggestedPrice = item.SuggestedPrice,
                ItemImage = item.ItemImage,
                IsServed = item.IsServed
            };
        }
        private Feedback MapToItem(FeedbackDTO feedbackDTO)
        {
            return new Feedback
            {
                FbId = feedbackDTO.Id,
                FbContent = feedbackDTO.Content,
                FbDatetime = feedbackDTO.DateTime,
                ItemId = feedbackDTO.ItemId,
                ParentFbFlag = feedbackDTO.ParentFb
            };
        }


        private Feedback MapToFeedback(FeedbackDTO feedbackDTO)
        {
            if (feedbackDTO == null) throw new ArgumentNullException(nameof(feedbackDTO));

            return new Feedback
            {
                FbId = feedbackDTO.Id,
                FbContent = feedbackDTO.Content,
                FbDatetime = feedbackDTO.DateTime,
                ItemId = feedbackDTO.ItemId,
                ParentFbFlag = feedbackDTO.ParentFb
            };
        }

        // CRUD Feedback
        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByPageAsync(int pageNumber, int pageSize)
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync(pageNumber, pageSize);
            return feedbacks?.Select(MapToFeedbackDTO) ?? Enumerable.Empty<FeedbackDTO>();
        }

        public async Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return feedbacks.Select(MapToFeedbackDTO).ToList();
        }

        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
            return feedbacks?.Select(MapToFeedbackDTO) ?? Enumerable.Empty<FeedbackDTO>();
        }

        public async Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto)
        {
            if (feedbackDto == null) throw new ArgumentNullException(nameof(feedbackDto));

            var account = await _accountRepository.GetAccountByUsnAsync(feedbackDto.Username);
            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (account == null) throw new InvalidOperationException("Invalid Account.");
            if (item == null) throw new InvalidOperationException("Invalid Item.");

            var feedback = MapToFeedback(feedbackDto);
            feedback.Username = account.Username;
            feedback.UsernameNavigation = account;
            feedback.Item = item;

            var createdFeedback = await _feedbackRepository.CreateAsync(feedback);
            return MapToFeedbackDTO(createdFeedback);
        }

        public async Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto)
        {
            if (feedbackDto == null) throw new ArgumentNullException(nameof(feedbackDto));

            var feedback = await _feedbackRepository.GetByIdAsync(feedbackDto.Id);
            if (feedback == null) throw new InvalidOperationException("Feedback not found.");

            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (item == null) throw new InvalidOperationException("Invalid Item.");

            feedback = MapToFeedback(feedbackDto);
            feedback.Item = item;

            var updatedFeedback = await _feedbackRepository.UpdateAsync(feedback);
            return MapToFeedbackDTO(updatedFeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null) return false;

            await _feedbackRepository.DeleteAsync(feedback);
            return true;
        }

        public async Task<IEnumerable<ItemDTO?>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(MapToItemDTO);
        }

        // CRUD Reply
        public async Task<IEnumerable<ReplyDTO>> GetRepliesByFeedbackAsync(int parentFbFlag)
        {
            var replies = await _feedbackRepository.GetFeedbackRepliesAsync(parentFbFlag);
            return replies.Select(reply => new ReplyDTO
            {
                Id = reply.FbId,
                Content = reply.FbContent,
                DateTime = reply.FbDatetime,
                Username = reply.Username,
                ProfileImage = reply.UsernameNavigation?.ProfileImage ?? string.Empty,
                ItemId = reply.ItemId,
                ItemName = reply.Item?.ItemName ?? "Unknown",
                ParentFb = reply.ParentFbFlag ?? 0
            }).ToList();
        }

        public async Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto)
        {
            if (replyDto == null) throw new ArgumentNullException(nameof(replyDto));

            var account = await _accountRepository.GetByIdAsync(replyDto.AccountId);
            var item = await _itemRepository.GetByIdAsync(replyDto.ItemId);
            if (account == null) throw new InvalidOperationException("Invalid Account.");
            if (item == null) throw new InvalidOperationException("Invalid Item.");

            var reply = new Feedback
            {
                FbContent = replyDto.Content,
                FbDatetime = DateTime.UtcNow,
                Username = replyDto.Username,
                ItemId = replyDto.ItemId,
                ParentFbFlag = parentFbFlag,
                UsernameNavigation = account,
                Item = item
            };

            var submittedReply = await _feedbackRepository.CreateReplyAsync(reply);
            return MapToFeedbackDTO(submittedReply);
        }

        public async Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto)
        {
            if (replyDto == null) throw new ArgumentNullException(nameof(replyDto));

            var existingReply = await _feedbackRepository.GetByIdAsync(replyDto.Id);
            if (existingReply == null) throw new KeyNotFoundException("Reply not found.");

            existingReply.FbContent = replyDto.Content;
            existingReply.FbDatetime = DateTime.UtcNow;

            var updatedReply = await _feedbackRepository.UpdateReplyAsync(existingReply);
            return MapToFeedbackDTO(updatedReply);
        }

        public async Task<bool> DeleteReplyAsync(int replyId)
        {
            var existingReply = await _feedbackRepository.GetByIdAsync(replyId);
            if (existingReply == null) return false;

            await _feedbackRepository.DeleteReplyAsync(existingReply);
            return true;
        }
    }
}