using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Posts;
using FYPTourneyPro.Services.Dtos.Comments;
using Volo.Abp.Domain.Entities;
using OpenQA.Selenium.DevTools.V128.Input;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using FYPTourneyPro.Services.Dtos.Organizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace FYPTourneyPro.Services.Posts
{
    public class PostAppService : ApplicationService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<IdentityUser, Guid> _userRepository;


        public PostAppService(IRepository<Post, Guid> postRepository, ICurrentUser currentUser, IRepository<IdentityUser, Guid> userRepository)
        {
            _postRepository = postRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        public async Task<PostDto> CreateAsync(PostDto input)
        {
            var posts = new Post
            {
                Title = input.Title,
                Content = input.Content,
                Upvotes = 0,
                Downvotes = 0,
                CreationTime = DateTime.Now,
                CreatedByUserId = _currentUser.Id.Value, // Get UserId from ICurrentUser
                Comments = new List<Comment>()
            };

            var post = await _postRepository.InsertAsync(posts);


            return new PostDto
            {
                Title = post.Title,
                Content = post.Content,             
            };
        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = await _postRepository.GetListAsync();

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var userFound = await _userRepository.GetAsync(post.CreatedByUserId);

                var postDto = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    CreationTime = post.CreationTime,
                    CreatedByUserId = post.CreatedByUserId,
                    CreatedByUsername = userFound.UserName, // Populate username here
                    Upvotes = post.Upvotes,
                    Downvotes = post.Downvotes,
                    Comments = post.Comments.Select(comment => new CommentDto
                    {
                        Id = comment.Id,
                        PostId = comment.PostId,
                        Content = comment.Content,
                        CreatedByUserId = comment.CreatedByUserId
                    }).ToList()
                };

                postDtos.Add(postDto); // Add the populated PostDto to the list
            }

            return postDtos;
        }

        public async Task<PostDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), id);
            }

            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreationTime = post.CreationTime,
                Upvotes = post.Upvotes,
                Downvotes = post.Downvotes
            };
        }

        public async Task UpdateAsync(Guid id, PostDto input)
        {
            var post = await _postRepository.GetAsync(id);
            if (post == null)
            {
                throw new EntityNotFoundException(typeof(Post), id);
            }

            post.Title = input.Title;
            post.Content = input.Content;
            post.Comments = input.Comments.Select(c => new Comment
            {
                PostId = c.PostId,
                Content = c.Content,
                CreatedByUserId = c.CreatedByUserId
            }).ToList();

            await _postRepository.UpdateAsync(post);
        }
        public async Task<List<PostDto>> GetListAsyncUid()
        {
            // Fetch tournaments created by the current logged-in user
            var posts = await _postRepository.GetListAsync(p => p.CreatedByUserId == _currentUser.Id); //filter by userid

            return posts
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreationTime = p.CreationTime
                }).ToList();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _postRepository.DeleteAsync(id);
        }
    }
}

