using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Comments;
using FYPTourneyPro.Services.Dtos.Posts;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace FYPTourneyPro.Services.Posts
{
    public class CommentAppService : ApplicationService
    {
            private readonly IRepository<Comment, Guid> _commentRepository;
            private readonly ICurrentUser _currentUser;
            private readonly IIdentityUserRepository _userRepository;
        public CommentAppService(IRepository<Comment, Guid> postRepository, ICurrentUser currentUser, IIdentityUserRepository userRepository)
            {
                _commentRepository = postRepository;
                _currentUser = currentUser;
                _userRepository = userRepository;
             }

            public async Task<CommentDto> CreateAsync(CommentDto input)
            {
                var comments = new Comment
                {
                    PostId = input.PostId,
                    Content = input.Content,
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

                var result = new List<CommentDto>();
           
            foreach (var comment in comments.Where(c => c.PostId == postId))
            {
                // Fetch the username for the comment's creator
                var user = await _userRepository.GetAsync(comment.CreatedByUserId);

                // Map the comment to a CommentDto
                result.Add(new CommentDto
                {
                    Content = comment.Content,
                    CreatedByUsername = user.UserName,
                    CreationTime = user.CreationTime
                });
            }

            return result;
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
