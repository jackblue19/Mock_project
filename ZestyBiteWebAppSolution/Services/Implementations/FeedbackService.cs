using AutoMapper;
using ZestyBiteWebAppSolution.Models.DTOs;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Interfaces;

namespace ZestyBiteWebAppSolution.Services.Implementations {
    public class FeedbackService : IFeedbackService {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IAccountRepository accountRepository, IItemRepository itemRepository, IMapper mapper) {
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // CRUD Feedback
        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByPageAsync(int pageNumber, int pageSize) {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackDTO?>> GetAllFeedbacksAsync()
        {
            var usn = await _accountRepository.GetAllAsync();

            var feedbacks = await _feedbackRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByItemIdAsync(int itemId) {
            var feedbacks = await _feedbackRepository.GetFeedbacksByItemIdAsync(itemId);
            return _mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
        }

        //[AllowAnonymous]
        public async Task<FeedbackDTO> SubmitFeedbackAsync(FeedbackDTO feedbackDto, string usn) {
            if (feedbackDto == null) {
                throw new ArgumentNullException(nameof(feedbackDto));
            }

            var account = await _accountRepository.GetAccountByUsnAsync(usn);
            //var username = User.Identity.Name;
            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (account == null) throw new InvalidOperationException("Invalid Account.");
            if (item == null) throw new InvalidOperationException("Invalid Item.");

            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.Username = usn;
            feedback.UsernameNavigation = account;
            feedback.Item = item;

            var createdFeedback = await _feedbackRepository.CreateAsync(feedback);
            return _mapper.Map<FeedbackDTO>(createdFeedback);
        }

        public async Task<FeedbackDTO> UpdateFeedbackAsync(FeedbackDTO feedbackDto) {
            if (feedbackDto == null) {
                throw new ArgumentNullException(nameof(feedbackDto));
            }

            var feedback = await _feedbackRepository.GetByIdAsync(feedbackDto.Id);
            if (feedback == null) {
                throw new InvalidOperationException("Feedback not found.");
            }

            var item = await _itemRepository.GetByIdAsync(feedbackDto.ItemId);
            if (item == null) {
                throw new InvalidOperationException("Invalid Item.");
            }

            feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.Item = item;

            var updatedFeedback = await _feedbackRepository.UpdateAsync(feedback);
            return _mapper.Map<FeedbackDTO>(updatedFeedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId) {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null) return false;

            await _feedbackRepository.DeleteAsync(feedback);
            return true;
        }

        public async Task<IEnumerable<ItemDTO?>> GetAllItemsAsync() {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        // CRUD Reply
        public async Task<IEnumerable<ReplyDTO>> GetRepliesByFeedbackAsync(int parentFbFlag) {
            var replies = await _feedbackRepository.GetFeedbackRepliesAsync(parentFbFlag);
            return _mapper.Map<IEnumerable<ReplyDTO>>(replies);
        }
        public async Task<FeedbackDTO> SubmitReplyAsync(int parentFbFlag, ReplyDTO replyDto) {
            if (replyDto == null) {
                throw new ArgumentNullException(nameof(replyDto));
            }

            // Retrieve account using username
            var account = await _accountRepository.GetAccountByUsnAsync(replyDto.Username);
            var item = await _itemRepository.GetByIdAsync(replyDto.ItemId);

            if (account == null) throw new InvalidOperationException("Invalid Account.");
            if (item == null) throw new InvalidOperationException("Invalid Item.");

            var reply = new Feedback {
                FbContent = replyDto.Content,
                FbDatetime = DateTime.UtcNow,
                Username = replyDto.Username,
                ItemId = replyDto.ItemId,
                ParentFbFlag = parentFbFlag,
                UsernameNavigation = account,
                Item = item
            };

            var submittedReply = await _feedbackRepository.CreateReplyAsync(reply);
            return _mapper.Map<FeedbackDTO>(submittedReply);
        }

        public async Task<FeedbackDTO> UpdateReplyAsync(ReplyDTO replyDto) {
            if (replyDto == null) {
                throw new ArgumentNullException(nameof(replyDto));
            }

            var existingReply = await _feedbackRepository.GetByIdAsync(replyDto.Id);
            if (existingReply == null) {
                throw new KeyNotFoundException("Reply not found.");
            }

            existingReply.FbContent = replyDto.Content;
            existingReply.FbDatetime = DateTime.UtcNow;

            var updatedReply = await _feedbackRepository.UpdateReplyAsync(existingReply);
            return _mapper.Map<FeedbackDTO>(updatedReply);
        }

        public async Task<bool> DeleteReplyAsync(int replyId) {
            var existingReply = await _feedbackRepository.GetByIdAsync(replyId);
            if (existingReply == null) return false;

            await _feedbackRepository.DeleteReplyAsync(existingReply);
            return true;
        }
    }
}