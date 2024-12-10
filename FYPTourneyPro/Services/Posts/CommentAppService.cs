using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Comments;
using FYPTourneyPro.Services.Dtos.Posts;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FYPTourneyPro.Services.Posts
{
    public class CommentAppService : ApplicationService
    {
            private readonly IRepository<Comment, Guid> _commentRepository;
            private readonly ICurrentUser _currentUser;

            public CommentAppService(IRepository<Comment, Guid> postRepository, ICurrentUser currentUser)
            {
                _commentRepository = postRepository;
                _currentUser = currentUser;
            }

            public async Task<CommentDto> CreateAsync(CommentDto input)
            {
                var comments = new Comment
                {
                    PostId = input.PostId,
                    Content = input.Content,
                    Upvotes = 0,
                    Downvotes = 0,
                    CreatedByUserId = _currentUser.Id.Value, // Get UserId from ICurrentUser}
                };

                var comment = await _commentRepository.InsertAsync(comments);


            return new CommentDto
            {
                Content = comment.Content,
                CreatedByUserId = comment.CreatedByUserId
            };
            }

            public async Task<List<CommentDto>> GetAllAsync(Guid postId)
            {
                var comments = await _commentRepository.GetListAsync();
            return comments
                .Where(c => c.PostId == postId)
                .Select(c => new CommentDto
                {
                    Content = c.Content,
                    CreatedByUserId = c.CreatedByUserId
                }).ToList();
        }

            public async Task<List<CommentDto>> GetListAsyncUid()
        {
            var comments = await _commentRepository.GetListAsync();
            return comments
                .Where(c => c.CreatedByUserId == _currentUser.Id.Value)
                .Select(c => new CommentDto
                {
                    PostId = c.PostId,
                    Content = c.Content,
                    CreatedByUserId = c.CreatedByUserId
                }).ToList();
        }

    }
    }
