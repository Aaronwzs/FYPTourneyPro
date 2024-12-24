using FYPTourneyPro.Services.Dtos.User;
using FYPTourneyPro.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace FYPTourneyPro.Pages.User
{
    public class WalletModel : PageModel
    {
        private readonly WalletAppService _walletAppService;

        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        //public WalletModel(WalletAppService walletAppService, IIdentityUserRepository userRepository,
        //    ICurrentUser currentUser)
        //{
        //    _walletAppService = walletAppService;
        //    _userRepository = userRepository;
        //    _currentUser = currentUser;
        //}

        public WalletModel(IIdentityUserRepository userRepository,
            ICurrentUser currentUser,WalletAppService walletAppService)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _walletAppService = walletAppService;
        }

        [BindProperty]
        public Guid UserId { get; set; }

        [BindProperty]
        public WalletDto Wallet { get; set; } = new WalletDto();

        [BindProperty]
        public decimal TopUpAmount { get; set; }

        [BindProperty]
        public decimal WithdrawAmount { get; set; }

        public string UserName { get; set; }

        public async Task OnGetAsync()
        {
            UserId = _currentUser.Id.Value; // Get UserId from ICurrentUser

            // Get current user information
            if (_currentUser.IsAuthenticated)
            {
                var currentUser = await _userRepository.GetAsync(UserId);
                UserName = currentUser.UserName;
            }

            // Get the list of wallets
            var wallets = await _walletAppService.GetListAsync();

            // Check if the current user has a wallet
            var existingWallet = wallets.FirstOrDefault(w => w.UserId == UserId);

            if (existingWallet != null)
            {
                // If the wallet exists, retrieve it
                Wallet = await _walletAppService.GetWalletAsync(UserId);
            }
            else
            {
                // If the wallet does not exist, create a new one
                await _walletAppService.CreateAsync(UserId);
                // Optionally retrieve the new wallet to use in the UI
               
            }

           
        }

        

    }
}
