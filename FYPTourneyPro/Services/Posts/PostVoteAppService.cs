using FYPTourneyPro.Entities.DiscussionBoard;
using FYPTourneyPro.Services.Dtos.Posts;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;

namespace FYPTourneyPro.Services.Posts
{
    public class PostVoteAppService : ApplicationService
    {
        private readonly IRepository<PostVotes, Guid> _postVoteRepository;
        private readonly PostAppService _postAppService;

        public PostVoteAppService(PostAppService postAppService, IRepository<PostVotes, Guid> postVoteRepository)
        {
            _postAppService = postAppService;
            _postVoteRepository = postVoteRepository;
        }

        // Create or Update Vote
        public async Task<PostVoteDto> CreateOrUpdateVoteAsync(PostVotes input)
        {
            var userId = CurrentUser.Id; // Get the current user's ID
            if (userId == null)
            {
                throw new BusinessException("User must be logged in to vote.");
            }

            // Check if the user already voted on this post
            var existingVote = await _postVoteRepository.FirstOrDefaultAsync(
                v => v.PostId == input.PostId && v.CreatorId == userId);

            if (existingVote != null)
            {
                // Update existing vote
                existingVote.VoteType = input.VoteType;
                await _postVoteRepository.UpdateAsync(existingVote);

                return new PostVoteDto
                {
                    Id = existingVote.Id,
                    PostId = existingVote.PostId,
                    VoteType = existingVote.VoteType
                };
            }
            else
            {
                // Create new vote
                var newVote = new PostVotes
                {
                    PostId = input.PostId,
                    VoteType = input.VoteType
                };

                await _postVoteRepository.InsertAsync(newVote);
                return new PostVoteDto
                {
                    Id = newVote.Id,
                    PostId = newVote.PostId,
                    VoteType = newVote.VoteType
                };
            }
        }

        // Remove Vote
        public async Task DeleteVoteAsync(Guid postId)
        {
            var userId = CurrentUser.Id; // Get the current user's ID
            if (userId == null)
            {
                throw new BusinessException("User must be logged in to remove a vote.");
            }

            var vote = await _postVoteRepository.FirstOrDefaultAsync(
                v => v.PostId == postId && v.CreatorId == userId);

            if (vote != null)
            {
                await _postVoteRepository.DeleteAsync(vote);
            }
        }

        // Count Votes by Type
        public async Task<(int upvotes, int downvotes)> GetVoteCountAsync(Guid postId)
        {
            var upvotes = await _postVoteRepository.CountAsync(v => v.PostId == postId && v.VoteType == "Upvote");
            var downvotes = await _postVoteRepository.CountAsync(v => v.PostId == postId && v.VoteType == "Downvote");

            return (upvotes, downvotes);
        }

        public async Task<List<PostVoteDto>> GetFilteredPostsAsync(string filterType)
        {
            // Get all posts from PostAppService
            var posts = await _postAppService.GetAllAsync();

            // Get vote data for all posts
            var votes = await _postVoteRepository.GetListAsync();

            // Combine post and vote data
            var postVoteData = posts.Select(post =>
            {
                var postVotes = votes.Where(v => v.PostId == post.Id); // Get all votes for this post
                var upvotes = postVotes.Count(v => v.VoteType == "Upvote"); // Count upvotes
                var downvotes = postVotes.Count(v => v.VoteType == "Downvote"); // Count downvotes

                return new
                {
                    Post = post,
                    Upvotes = upvotes,
                    Downvotes = downvotes,
                    Score = upvotes - downvotes, // Calculate score
                    HotScore = (upvotes - downvotes) / Math.Pow((DateTime.Now - post.CreationTime).TotalHours + 2, 1.5) // Example hot score formula
                };
            });

            // Apply filtering
            switch (filterType.ToLower())
            {
                case "hot":
                    return postVoteData
                        .OrderByDescending(p => p.HotScore)
                        .Select(p => new PostVoteDto
                        {
                            PostId = p.Post.Id,
                            Title = p.Post.Title,
                            Content = p.Post.Content,
                            CreationTime = p.Post.CreationTime,
                            Upvotes = p.Upvotes,
                            Downvotes = p.Downvotes,
                            Score = p.Score
                        }).ToList();



                case "best":
                    return postVoteData
                        .OrderByDescending(p => p.Score)
                        .Select(p => new PostVoteDto
                        {
                            PostId = p.Post.Id,
                            Title = p.Post.Title,
                            Content = p.Post.Content,
                            CreationTime = p.Post.CreationTime,
                            Upvotes = p.Upvotes,
                            Downvotes = p.Downvotes,
                            Score = p.Score
                        })
                        .ToList();

                case "latest":
                    return postVoteData
                        .OrderByDescending(p => p.Post.CreationTime)
                        .Select(p => new PostVoteDto
                        {
                            PostId = p.Post.Id,
                            Title = p.Post.Title,
                            Content = p.Post.Content,
                            CreationTime = p.Post.CreationTime,
                            Upvotes = p.Upvotes,
                            Downvotes = p.Downvotes,
                            Score = p.Score
                        })
                        .ToList();
                default:
                    throw new ArgumentException("Invalid filter type.");
            }
        }
    }
}