using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Posts;
using FYPTourneyPro.Services.Dtos.Comments;
using Volo.Abp.Domain.Entities;
using OpenQA.Selenium.DevTools.V128.Input;
using Volo.Abp.Identity;
using Volo.Abp.Users;


namespace FYPTourneyPro.Services.Posts
{
    public class PostAppService : ApplicationService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly ICurrentUser _currentUser;

        public PostAppService(IRepository<Post, Guid> postRepository, ICurrentUser currentUser)
        {
            _postRepository = postRepository;
            _currentUser = currentUser;
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
            return posts
                .Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Upvotes = post.Upvotes,
                    Downvotes = post.Downvotes,
                    Comments = post.Comments.Select(comment => new CommentDto
                    {
                        Id = comment.Id,
                        PostId = comment.PostId,
                        Content = comment.Content,
                        CreatedByUserId = comment.CreatedByUserId,
                    }).ToList()
                }).ToList();
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

            await _postRepository.UpdateAsync(post);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _postRepository.DeleteAsync(id);
        }

        //public async Task UpvotePostAsync(Guid postId)
        //{
        //    var post = await _postRepository.GetAsync(postId);
        //    post.Upvotes++;
        //    await _postRepository.UpdateAsync(post);
        //}

        //public async Task DownvotePostAsync(Guid postId)
        //{
        //    var post = await _postRepository.GetAsync(postId);
        //    post.Downvotes++;
        //    await _postRepository.UpdateAsync(post);
        //}
        //public async Task<List<PostDto>> GetHotPostsAsync()
        //{
        //    var posts = await _postRepository.GetListAsync();
        //    var hotPosts = posts.OrderByDescending(p => (p.Upvotes - p.Downvotes) / (DateTime.Now - p.CreationTime).TotalHours).ToList();
        //    return ObjectMapper.Map<List<Post>, List<PostDto>>(hotPosts);
        //}

        //public async Task<List<PostDto>> GetBestPostsAsync()
        //{
        //    var posts = await _postRepository.GetListAsync();
        //    var bestPosts = posts.OrderByDescending(p => p.Upvotes - p.Downvotes).ToList();
        //    return ObjectMapper.Map<List<Post>, List<PostDto>>(bestPosts);
        //}

        //public async Task<List<PostDto>> GetLatestPostsAsync()
        //{
        //    var posts = await _postRepository.GetListAsync();
        //    var latestPosts = posts.OrderByDescending(p => p.CreationTime).ToList();
        //    return ObjectMapper.Map<List<Post>, List<PostDto>>(latestPosts);
        //}
    }
}

