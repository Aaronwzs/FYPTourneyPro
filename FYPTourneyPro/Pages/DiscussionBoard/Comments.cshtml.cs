using FYPTourneyPro.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Users;   
using FYPTourneyPro.Services.Dtos.Comments;
using FYPTourneyPro.Services.Dtos.Posts;    
using FYPTourneyPro.Entities.DiscussionBoard;

namespace FYPTourneyPro.Pages.DiscussionBoard
{
    public class CommentsModel : PageModel
    {
        private readonly CommentAppService _commentAppService;
        private readonly PostAppService _postAppService;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;



        public CommentsModel(CommentAppService commentAppService, PostAppService postAppService,IIdentityUserRepository userRepository, ICurrentUser currentUser)
        {
            _commentAppService = commentAppService;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _postAppService = postAppService;
        }

        public List<CommentDto> Comments { get; set; }
        public List<CommentDto> YourComments { get; set; }
        public PostDto Post { get; set; }
        public Guid UserId { get; set; }
        public String UserName { get; set; }

        public async Task OnGetAsync(Guid postId)
        {
            if (_currentUser.IsAuthenticated)
            {
                UserId = _currentUser.Id.Value; // Get UserId from ICurrentUser
                var currentUser = await _userRepository.GetAsync(UserId);
                UserName = currentUser.UserName;
            }

            Comments = await _commentAppService.GetAllAsync(postId);

            Post = await _postAppService.GetAsync(postId);

            YourComments = await _commentAppService.GetListAsyncUid();
        }
    }
}
