using FYPTourneyPro.Entities.TodoList;
using FYPTourneyPro.Entities.User;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Dtos.TodoItems;
using FYPTourneyPro.Services.Dtos.User;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Users
{
    public class WalletAppService : ApplicationService
    {
        private readonly IRepository<Wallet, Guid> _walletRepository;

        public WalletAppService(IRepository<Wallet, Guid> walletAppService)
        {
            _walletRepository = walletAppService;
        }

        public async Task<WalletDto> GetWalletAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetAsync(w => w.UserId == userId);
            var walletBal = wallet.Balance;
            if (wallet == null)
            {
                return null; // Or throw an exception based on your needs
            }
            return new WalletDto
            {
                UserId = wallet.UserId,
                Balance = walletBal
            };
        }

        public async Task<List<WalletDto>> GetListAsync()
        {
            var wallets = await _walletRepository.GetListAsync();
            return wallets
                .Select(w => new WalletDto
                {
                    Id = w.Id,
                    UserId = w.UserId,
                   Balance = w.Balance
                }).ToList();
        }

        public async Task<WalletDto> CreateAsync(Guid userId)
        {


            var wallet = await _walletRepository.InsertAsync(
                new Wallet { UserId = userId, Balance = 0}
            );

            return new WalletDto
            {
                UserId = userId,
                Balance = wallet.Balance
            };
        }

        public async Task TopUpAsync(WalletDto input)
        {
            

            var wallet = await _walletRepository.FirstOrDefaultAsync(w => w.UserId == input.UserId);
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = input.UserId,
                    Balance = input.TopUpAmount
                };
               
                await _walletRepository.InsertAsync(wallet);
            }
            else
            {
                wallet.Balance += input.TopUpAmount;
                await _walletRepository.UpdateAsync(wallet);
            }
        }

        public async Task WithdrawAsync(WalletDto input)
        {
           

            var wallet = await _walletRepository.FirstOrDefaultAsync(w => w.UserId == input.UserId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found for the user.");
            }

            if (wallet.Balance < input.WithdrawAmount)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }

            wallet.Balance -= input.WithdrawAmount;
            await _walletRepository.UpdateAsync(wallet);
        }

        public async Task PaymentAsync(WalletDto input)
        {


            var wallet = await _walletRepository.FirstOrDefaultAsync(w => w.UserId == input.UserId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found for the user.");
            }

            if (wallet.Balance < input.RegFee)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }

            wallet.Balance -= input.RegFee;
            await _walletRepository.UpdateAsync(wallet);
        }
    }
}
