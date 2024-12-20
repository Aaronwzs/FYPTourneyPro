using FYPTourneyPro.Entities.Chatroom;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp;
using Volo.Abp.Users;
using FYPTourneyPro.Services.Dtos.Chat;
using System.Linq;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Dtos.Comments;
using FYPTourneyPro.Services.Dtos.Posts;
using Microsoft.AspNetCore.Mvc;



namespace FYPTourneyPro.Services.Chat
{
    public class ChatAppService : ApplicationService
    {
        private readonly IRepository<ChatRoom, Guid> _chatRoomRepository;
        private readonly IRepository<ChatMessage, Guid> _chatMessageRepository;
        private readonly IRepository<ChatRoomParticipant, Guid> _chatRoomParticipantRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;

        public ChatAppService(IRepository<ChatRoom, Guid> chatRoomRepository, IRepository<ChatRoomParticipant, Guid> chatRoomParticipantRepository, IRepository<IdentityUser, Guid> userRepository
            , IRepository<ChatMessage, Guid> chatMessageRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatRoomParticipantRepository = chatRoomParticipantRepository;
            _userRepository = userRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            var currentUserId = CurrentUser.Id; // Get the ID of the logged-in user

            if (currentUserId == null)
            {
                throw new UserFriendlyException("User is not logged in.");
            }

            var users = await _userRepository.GetListAsync();

            // Filter out the current user
            var filteredUsers = users.Where(user => user.Id != currentUserId).ToList();

            return filteredUsers;
        }


        //public async Task<Guid> CreateDirectChatAsync(Guid userId)
        //{
        //    try
        //    {
        //        var receiver = await _userRepository.GetAsync(userId);
        //        var currentUserId = CurrentUser.Id; // Get the ID of the logged-in user

        //        var existingChatRoom = await _chatRoomParticipantRepository.GetListAsync(cp => cp.UserId == currentUserId);

        //        var chatRoom = existingChatRoom.FirstOrDefault(cp => cp.UserId == userId);

        //        if (chatRoom != null && chatRoom.ChatRoom.ChatMessages.ChatType == ChatType.Direct)
        //        {
        //            return chatRoom.ChatRoomId;
        //        }



        //    }
        //}

        public async Task<ChatRoomDto> CreateGroupChatAsync(string groupName, List<Guid> participantIds)
        {
            bool isDuplicate;
            try
            {
                var currentUserId = CurrentUser.Id; // Get the ID of the logged-in user

                if (currentUserId.HasValue && !participantIds.Contains(currentUserId.Value))
                {
                    participantIds.Add(currentUserId.Value);

                }
                var existingChatRooms = await _chatRoomParticipantRepository.GetListAsync(cp => participantIds.Contains(cp.UserId));

                var duplicateChatRoom = existingChatRooms
                   .GroupBy(cp => cp.ChatRoomId) // Group by chat room ID
                   .FirstOrDefault(g =>
                       g.Count() == participantIds.Count && // Match participant count
                       participantIds.All(pid => g.Any(p => p.UserId == pid)) // Check if all participant IDs match
                   )?.Key;


                if (duplicateChatRoom.HasValue)
                {
                    // If a duplicate exists, return its ID
                    return new ChatRoomDto
                    {
                        Id = duplicateChatRoom.Value,
                        isDuplicate = true
                    };
                }

                // Create the new chat room
                var newChatRoom = new ChatRoom
                {
                    Name = groupName,
                };

                await _chatRoomRepository.InsertAsync(newChatRoom);

                // Add participants
                foreach (var userId in participantIds)
                {
                    var chatRoomParticipant = new ChatRoomParticipant
                    {
                        ChatRoomId = newChatRoom.Id,
                        UserId = userId,
                    };
                    await _chatRoomParticipantRepository.InsertAsync(chatRoomParticipant);
                }

                return new ChatRoomDto
                {
                    Id = newChatRoom.Id,
                    isDuplicate = false
                }; // Return the generated ID to the frontend
            }
            catch (Exception ex)
            {
                // Log the exception and handle accordingly
                Console.WriteLine($"Error creating group chat: {ex.Message}");
                throw; // Rethrow or handle error as needed
            }
        }

        public async Task<List<ChatRoomParticipantDto>> GetUserChatRoomsAsync()
        {
            var chatRoomParticipants = await _chatRoomParticipantRepository.GetListAsync(cp => cp.UserId == CurrentUser.Id);

            var chatRoomIds = chatRoomParticipants.Select(cp => cp.ChatRoomId).ToList();

            var chatRooms = await _chatRoomRepository.GetListAsync(cr => chatRoomIds.Contains(cr.Id));

            var chatRoomParticipantData = chatRoomParticipants.Select(participant =>
            {
                // Assuming chatRooms is available and contains all chat rooms
                var chatRoom = chatRooms.FirstOrDefault(cr => cr.Id == participant.ChatRoomId);

                // Manually combine ChatRoom and ChatRoomParticipant information
                return new ChatRoomParticipantDto
                {
                    Id = participant.Id,                          // From ChatRoomParticipant
                    ChatRoomId = participant.ChatRoomId,          // From ChatRoomParticipant
                    Name = chatRoom.Name,         // From ChatRoom (Name)
                    LastActivity = chatRoom?.LastModificationTime ?? participant.CreationTime
                };
            }).ToList();

            return chatRoomParticipantData;
        }

        public async Task<List<ChatMessageDto>> GetChatMessagesAsync(Guid chatRoomId)
        {
            var chatMessages = await _chatMessageRepository.GetListAsync(cr => cr.ChatRoomId == chatRoomId);


            return chatMessages
                .Select(message => new ChatMessageDto
                {
                    CreatorId = message.CreatorId.Value,
                    CreationTime = message.CreationTime,
                    Content = message.Content
                }).ToList();
        }

        public async Task<ChatMessageDto> CreateChatMessagesAsync(ChatMessageDto input)
        {
            try
            {
                var chatType = input.ReceiverId.HasValue ? ChatType.Direct : ChatType.Group;
                var chatMessages = new ChatMessage
                {
                    ChatRoomId = input?.ChatRoomId,
                    ReceiverId = input?.ReceiverId,
                    ChatType = chatType,
                    Content = input.Content,
                    IsSeen = false,
                };

                var message = await _chatMessageRepository.InsertAsync(chatMessages);

                return new ChatMessageDto
                {
                    ChatRoomId = message.ChatRoomId,
                    Content = message.Content,
                    IsSeen = message.IsSeen,
                    CreationTime = message.CreationTime,
                    CreatorId = message.CreatorId.Value,
                };
            }
            catch (Exception ex)
            {
                // Log the exception and handle accordingly
                Console.WriteLine($"Error inserting message: {ex.Message}");
                throw;
            }

        }
    }
}
