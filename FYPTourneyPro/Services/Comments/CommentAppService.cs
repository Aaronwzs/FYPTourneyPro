using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Comments;

namespace FYPTourneyPro.Services.Comments
{
    public class CommentAppService : ApplicationService
    {
        private readonly IRepository<Comment, Guid> _commentRepository;

        public CommentAppService(IRepository<Comment, Guid> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> CreateAsync(CreateCommentDto input)
        {
            var comment = ObjectMapper.Map<CreateCommentDto, Comment>(input);
            await _commentRepository.InsertAsync(comment);
            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<List<CommentDto>> GetAllByPostIdAsync(Guid postId)
        {
            var comments = await _commentRepository.GetListAsync(c => c.PostId == postId);
            return ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments);
        }

        public async Task<CommentDto> GetAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);
            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await _commentRepository.GetAsync(id);
            ObjectMapper.Map(input, comment);
            await _commentRepository.UpdateAsync(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _commentRepository.DeleteAsync(id);
        }

        public async Task UpvoteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetAsync(commentId);
            comment.Upvotes++;
            await _commentRepository.UpdateAsync(comment);
        }

        public async Task DownvoteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetAsync(commentId);
            comment.Downvotes++;
            await _commentRepository.UpdateAsync(comment);
        }
    }
}