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
        private FeedbackDTO MapToDTO(Feedback feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback));
            }

            FeedbackDTO parentFeedbackDTO = null;
            if (feedback.ParentFbFlagNavigation != null)
            {
                parentFeedbackDTO = MapToDTO(feedback.ParentFbFlagNavigation);
            }

            return new FeedbackDTO
            {
                Id = feedback.FbId,
                Content = feedback.FbContent,
                DateTime = feedback.FbDatetime,
                Username = feedback.Account?.Username ?? "Unknown",
                ProfileImage = feedback.Account?.ProfileImage,
                ItemId = feedback.ItemId,
                ItemName = feedback.Item?.ItemName ?? "Unknown",
                ParentFb = feedback.ParentFbFlag,
                ParentFeedback = parentFeedbackDTO,
                IsReply = feedback.ParentFbFlag != null // Set IsReply based on ParentFbFlag
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

        private ReplyDTO MapToReplyDTO(Feedback reply)
        {
            if (reply == null)
            {
                throw new ArgumentNullException(nameof(reply));
            }

            return new ReplyDTO
            {
                Id = reply.FbId,
                Content = reply.FbContent,
                DateTime = reply.FbDatetime,
                AccountId = reply.AccountId,
                Username = reply.Account?.Username ?? "Unknown",
                ProfileImage = reply.Account?.ProfileImage,
                ItemId = reply.ItemId,
                ItemName = reply.Item?.ItemName ?? "Unknown",
                ParentFb = reply.ParentFbFlag ?? 0
            };
        }

        // CRUD Feedback
        public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync(int pageNumber, int pageSize)
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync(pageNumber, pageSize);
            return feedbacks?.Select(MapToDTO).ToList() ?? new List<FeedbackDTO>();
        }

        public async Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            return feedbacks.Select(f => new FeedbackDTO
            {
                Id = f.FbId,
                Content = f.FbContent,
                DateTime = f.FbDatetime,
                Username = f.Account?.Username ?? "Unknown",
                ProfileImage = f.Account?.ProfileImage,
                ItemId = f.ItemId,
                ItemName = f.Item?.ItemName ?? "Unknown",
                ParentFb = f.ParentFbFlag,
                ParentFeedback = f.ParentFbFlag != null ? MapToDTO(f.ParentFbFlagNavigation) : null,
                IsReply = f.ParentFbFlag !=null
            }).ToList();
        }


        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
            return feedbacks?.Select(MapToDTO).ToList() ?? new List<FeedbackDTO>();
        }

        public async Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto)
        {
            var account = await _accountRepository.GetAccountByUsnAsync(feedbackDto.Username);
            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (account == null || item == null)
            {
                throw new InvalidOperationException("Invalid Account or Item.");
            }

            var feedback = MapToItem(feedbackDto);
            feedback.AccountId = account.AccountId;
            feedback.ItemId = feedbackDto.ItemId;
            feedback.Account = account;
            feedback.Item = item;

            await _feedbackRepository.CreateAsync(feedback);
            feedbackDto.Id = feedback.FbId;
            return feedbackDto;
        }

        public async Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackDto.Id);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found");
            }

            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (item == null)
            {
                throw new InvalidOperationException("Invalid Item.");
            }

            feedback = MapToItem(feedbackDto);
            feedback.Item = item;

            var updatedFeedback = await _feedbackRepository.UpdateAsync(feedback);
            return MapToDTO(updatedFeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback != null)
            {
                await _feedbackRepository.DeleteAsync(feedback);
                return true;
            }
            return false;
        }

        //CRUD Reply
        public async Task<IEnumerable<ReplyDTO>> GetRepliesForFeedbackAsync(int parentFbFlag)
        {
            var replies = await _feedbackRepository.GetFeedbackRepliesAsync(parentFbFlag);
            return replies.Select(MapToReplyDTO).ToList();
        }

        public async Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto)
        {
            var account = await _accountRepository.GetByIdAsync(replyDto.AccountId);
            var item = await _itemRepository.GetByIdAsync(replyDto.ItemId);
            if (account == null || item == null)
            {
                throw new InvalidOperationException("Invalid Account or Item.");
            }

            var reply = new Feedback
            {
                FbContent = replyDto.Content,
                FbDatetime = DateTime.Now,
                AccountId = replyDto.AccountId,
                ItemId = replyDto.ItemId,
                ParentFbFlag = parentFbFlag,
                Account = account,
                Item = item
            };

            var submittedReply = await _feedbackRepository.CreateReplyAsync(reply);
            return MapToDTO(submittedReply);
        }

        public async Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto)
        {
            var existingReply = await _feedbackRepository.GetByIdAsync(replyDto.Id) ?? throw new KeyNotFoundException("Reply not found.");
            existingReply.FbContent = replyDto.Content;
            existingReply.FbDatetime = DateTime.Now;

            var updatedReply = await _feedbackRepository.UpdateReplyAsync(existingReply);
            return MapToDTO(updatedReply);
        }

        public async Task<bool> DeleteReplyAsync(int replyId)
        {
            var existingReply = await _feedbackRepository.GetByIdAsync(replyId);
            if (existingReply != null)
            {
                await _feedbackRepository.DeleteReplyAsync(existingReply); // Pass the Feedback object
                return true;
            }
            return false;
        }
    }
}
