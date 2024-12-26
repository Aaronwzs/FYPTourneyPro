using FYPTourneyPro.Entities.Chatroom;
using FYPTourneyPro.Entities.Notification;
using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Entities.UserM;
using FYPTourneyPro.Services.Dtos.Notifications;
using FYPTourneyPro.Services.Dtos.Organizer;
using Microsoft.CodeAnalysis.CSharp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using static Volo.Abp.Identity.IdentityPermissions;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace FYPTourneyPro.Services.Notifications
{
    public class NotificationAppService : ApplicationService
    {
        private readonly IRepository<Notification, Guid> _notificationRepository;
        private readonly IRepository<ChatRoomParticipant, Guid> _chatRoomParticipantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Tournament, Guid> _tournamentRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<ChatRoom, Guid> _chatRoomRepository;
        private readonly IRepository<CustomUser, Guid> _custUserRepository;

        public NotificationAppService(IRepository<Notification, Guid> notificationRepository, IRepository<ChatRoomParticipant, Guid> chatRoomParticipantRepository,
            IUnitOfWorkManager unitOfWorkManager, IRepository<Tournament, Guid> tournamentRepository, IIdentityUserRepository userRepository, IRepository<Category, Guid> categoryRepository
            , ICurrentUser currentUser, IRepository<ChatRoom, Guid> chatroomRepository, IRepository<CustomUser, Guid> custUserRepository)
        {
            _notificationRepository = notificationRepository;
            _chatRoomParticipantRepository = chatRoomParticipantRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _currentUser = currentUser;
            _chatRoomRepository = chatroomRepository;
            _custUserRepository = custUserRepository;
        }


        public async Task SaveChatNotification(Guid chatRoomId, Guid userId)
        {
            var groupUsers = await _chatRoomParticipantRepository.GetListAsync(cp => cp.ChatRoomId == chatRoomId && cp.UserId != userId);

            var chatroom = await _chatRoomRepository.GetAsync(chatRoomId);

            var userFull = await _custUserRepository.GetAsync(cp => cp.UserId == userId);

            foreach (var user in groupUsers)
            {
               

                if (user.UserId != userId)
                {
                    var content = $"New message from {userFull.FullName} in Chatroom {chatroom.Name}";

                    var notification = new Notification
                    {
                        Type = "ChatMessage",
                        UserId = user.UserId,
                        Content = content,
                        RelatedEntityId = chatRoomId
                    };

                    await _notificationRepository.InsertAsync(notification);
                }
            }

            await _unitOfWorkManager.Current.SaveChangesAsync();

        }

        public async Task SavePlayerRegistrationNotification(string username1, string? username2, Guid tournamentId)
        {
            var tournament = await _tournamentRepository.GetAsync(tournamentId);

            var users = new List<IdentityUser>();

            var userList1 = await _userRepository.GetListAsync(userName: username1);
            var user1 = userList1.FirstOrDefault();
            users.Add(user1);


            if (username2 != null)
            {
                var userList2 = await _userRepository.GetListAsync(userName: username2);
                var user2 = userList2.FirstOrDefault();
                users.Add(user2);
            }

            var content = $"You have registered for Tournament {tournament.Name}";
            foreach (var user in users)
            {
                var notification = new Notification
                {
                    Type = "Tournament",
                    UserId = user.Id,
                    Content = content,
                    RelatedEntityId = tournamentId,
                };
                await _notificationRepository.InsertAsync(notification);
            }
        }

        public async Task SavePlayerJoinTourNotification(RegistrationDto input)
        {
            var tournament = await _tournamentRepository.GetAsync(input.tournamentId);

            var category = await _categoryRepository.GetAsync(input.CategoryId);


            var content = $"{input.FullName1} has joined category {category.Name} in tournament {tournament.Name}";


            if (input.FullName2 != null)
            {
                content = $"{input.FullName1} and {input.FullName2} have joined category {category.Name} your tournament {tournament.Name}";
            }

            var notification = new Notification
            {
                Type = "Tournament",
                UserId = input.organizerId,
                Content = content,
                RelatedEntityId = input.tournamentId,
            };

            await _notificationRepository.InsertAsync(notification);
        }

        public async Task<List<NotificationDto>> GetTournamentNotification()
        {

            var notifications = await _notificationRepository.GetListAsync(n => n.Type == "Tournament" && n.UserId == _currentUser.Id);

            var notificationDtos = new List<NotificationDto>();

            foreach (var notification in notifications)
            {
                notificationDtos.Add(new NotificationDto
                {
                    Id = notification.Id,
                    Content = notification.Content,
                    RelatedEntityId = notification.RelatedEntityId,
                    Type = notification.Type,
                    CreationTime = notification.CreationTime,
                    CreatorId = notification.CreatorId.Value,
                });
            }

            return notificationDtos;
        }

        public async Task<List<NotificationDto>> GetChatNotification()
        {

            var notifications = await _notificationRepository.GetListAsync(n => n.Type == "ChatMessage" && n.UserId == _currentUser.Id);

            var notificationDtos = new List<NotificationDto>();

            foreach (var notification in notifications)
            {
                notificationDtos.Add(new NotificationDto
                {
                    Id = notification.Id,
                    Content = notification.Content,
                    RelatedEntityId = notification.RelatedEntityId,
                    Type = notification.Type,
                    CreationTime = notification.CreationTime,
                    CreatorId = notification.CreatorId.Value,
                });
            }

            return notificationDtos;
        }
    }
}