using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Dtos.Posts;
using FYPTourneyPro.Services.Organizer;
using FYPTourneyPro.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace FYPTourneyPro.Pages.DiscussionBoard
{
    public class indexModel : PageModel
    {
        private readonly PostAppService _postAppService;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public indexModel(PostAppService postAppService, IIdentityUserRepository userRepository, ICurrentUser currentUser)
        {
            _postAppService = postAppService;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        public List<PostDto> Posts { get; set; }
        public List<PostDto> YourPosts { get; set; }
        public List<IdentityUser> Users { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        

        public async Task OnGetAsync()
        {
            if (_currentUser.IsAuthenticated)
            {
                UserId = _currentUser.Id.Value; // Get UserId from ICurrentUser
                var currentUser = await _userRepository.GetAsync(UserId);
                UserName = currentUser.UserName;
            }

            Posts = await _postAppService.GetAllAsync();

            YourPosts = await _postAppService.GetListAsyncUid();
        }
    }
}
