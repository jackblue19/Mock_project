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

        public async Task<IEnumerable<Feedback?>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }

        //public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksForItemAsync(int itemId)
        //{
        //    var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
        //    return feedbacks.Select(MapToDTO).ToList();
        //}
        public async Task<IEnumerable<Feedback>> GetFeedbacksByItemAsync(int itemId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
            
            return feedbacks;
        }

        public async Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto)
        {
            var account = await _accountRepository.GetAccountByUsnAsync(feedbackDto.Username);
            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (account == null || item == null)
            {
                throw new InvalidOperationException("Invalid Account or Item.");
            }
            var feedback = new Feedback
            {
                FbContent = feedbackDto.Content,
                FbDatetime = DateTime.Now,
                AccountId = account.AccountId,
                ItemId = feedbackDto.ItemId,
                Account = account,
                Item = item,
                ParentFbFlag = null
            };
            await _feedbackRepository.CreateAsync(feedback);
            feedbackDto.Id = feedback.FbId;
            return feedbackDto;
        }

        //public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync(int pageNumber, int pageSize)
        //{
        //    var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync(pageNumber, pageSize);
        //    return feedbacks.Select(MapToDTO).ToList();
        //}

        //public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId)
        //{
        //    var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
        //    return feedbacks.Select(MapToDTO).ToList();
        //}

        //public async Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto)
        //{
        //    var feedback = await _feedbackRepository.GetByIdAsync(feedbackDto.Id);
        //    if (feedback == null)
        //    {
        //        throw new InvalidOperationException("Feedback not found");
        //    }
        //    var account = await _accountRepository.GetByIdAsync(feedbackDto.AccountId);
        //    var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
        //    if (account == null || item == null)
        //    {
        //        throw new InvalidOperationException("Invalid Account or Item.");
        //    }

        //    feedback.FbContent = feedbackDto.Content;
        //    var updatedFeedback = await _feedbackRepository.UpdateAsync(feedback);
        //    return new FeedbackDTO
        //    {
        //        Id = updatedFeedback.FbId,
        //        Content = updatedFeedback.FbContent,
        //        AccountId = updatedFeedback.AccountId,
        //        Username = updatedFeedback.Account.Username,
        //        ItemId = updatedFeedback.ItemId,
        //        ItemName = updatedFeedback.Item.ItemName,
        //        ProfileImage = updatedFeedback.Account.ProfileImage
        //    };
        //}

        //public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        //{
        //    var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
        //    if (feedback != null)
        //    {
        //        await _feedbackRepository.DeleteAsync(feedback);
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<IEnumerable<ReplyDTO>> GetRepliesForFeedbackAsync(int parentFbFlag)
        //{
        //    var replies = await _feedbackRepository.GetFeedbackRepliesAsync(parentFbFlag);
        //    return replies.Select(MapToReplyDTO).ToList();
        //}

        //public async Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto)
        //{
        //    var reply = new Feedback
        //    {
        //        FbContent = replyDto.Content,
        //        FbDatetime = DateTime.Now,
        //        AccountId = replyDto.AccountId,
        //        ItemId = replyDto.ItemId,
        //        ParentFbFlag = parentFbFlag
        //    };

        //    var submittedReply = await _feedbackRepository.CreateAsync(reply);
        //    return MapToDTO(submittedReply);
        //}

        //public async Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto)
        //{
        //    var existingReply = await _feedbackRepository.GetByIdAsync(replyDto.Id) ?? throw new KeyNotFoundException("Reply not found.");
        //    existingReply.FbContent = replyDto.Content;
        //    existingReply.FbDatetime = DateTime.Now;
        //    var updatedReply = await _feedbackRepository.UpdateAsync(existingReply);
        //    return MapToDTO(updatedReply);
        //}

        //public async Task<bool> DeleteReplyAsync(int replyId)
        //{
        //    var existingReply = await _feedbackRepository.GetByIdAsync(replyId);
        //    if (existingReply != null)
        //    {
        //        await _feedbackRepository.DeleteAsync(existingReply);
        //        return true;
        //    }
        //    return false;
        //}

        // Mapping functions
        //private FeedbackDTO MapToDTO(Feedback feedback)
        //{
        //    if (feedback != null)
        //    {
        //        FeedbackDTO parentFeedbackDTO = feedback.ParentFbFlagNavigation != null ? MapToDTO(feedback.ParentFbFlagNavigation) : null;

        //        return new FeedbackDTO
        //        {
        //            Id = feedback.FbId,
        //            Content = feedback.FbContent,
        //            DateTime = feedback.FbDatetime,
        //            Username = feedback.Account.Username,
        //            ProfileImage = feedback.Account.ProfileImage,
        //            ItemId = feedback.ItemId,
        //            ItemName = feedback.Item.ItemName,
        //            ParentFb = feedback.ParentFbFlag,
        //            ParentFeedback = parentFeedbackDTO
        //        };
        //    }

        //    throw new ArgumentNullException(nameof(feedback));
        //}


        //private Feedback MapToitem(FeedbackDTO feedbackDTO)
        //{
        //    return new Feedback
        //    {
        //        FbId = feedbackDTO.Id,
        //        FbContent = feedbackDTO.Content,
        //        FbDatetime = feedbackDTO.DateTime,
        //        AccountId = feedbackDTO.AccountId,
        //        ItemId = feedbackDTO.ItemId,
        //        ParentFbFlag = feedbackDTO.ParentFb
        //    };
        //}

        //private ReplyDTO MapToReplyDTO(Feedback reply)
        //{
        //    return reply == null
        //        ? throw new ArgumentNullException(nameof(reply))
        //        : new ReplyDTO
        //    {
        //        Id = reply.FbId,
        //        Content = reply.FbContent,
        //        DateTime = reply.FbDatetime,
        //        AccountId = reply.AccountId,
        //        Username = reply.Account.Username ?? "Unknown",
        //        ProfileImage = reply.Account.ProfileImage,
        //        ItemId = reply.ItemId,
        //        ItemName = reply.Item.ItemName ?? "Unknown",
        //        ParentFb = reply.ParentFbFlag ?? 0
        //    };
        //}
    }
}
