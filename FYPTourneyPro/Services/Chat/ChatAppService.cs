﻿using FYPTourneyPro.Entities.Chatroom;
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
using Microsoft.VisualBasic;
using FYPTourneyPro.Entities.Organizer;
using JetBrains.Annotations;
using FYPTourneyPro.Entities.UserM;



namespace FYPTourneyPro.Services.Chat
{
    public class ChatAppService : ApplicationService
    {
        private readonly IRepository<ChatRoom, Guid> _chatRoomRepository;
        private readonly IRepository<ChatMessage, Guid> _chatMessageRepository;
        private readonly IRepository<ChatRoomParticipant, Guid> _chatRoomParticipantRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<CustomUser, Guid> _custUserRepository;

        public ChatAppService(IRepository<ChatRoom, Guid> chatRoomRepository, IRepository<ChatRoomParticipant, Guid> chatRoomParticipantRepository, IRepository<IdentityUser, Guid> userRepository
            , IRepository<ChatMessage, Guid> chatMessageRepository, IRepository<CustomUser, Guid> custUserRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatRoomParticipantRepository = chatRoomParticipantRepository;
            _userRepository = userRepository;
            _chatMessageRepository = chatMessageRepository;
            _custUserRepository = custUserRepository;
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



        public async Task<ChatRoomDto> CreateGroupChatAsync(string? groupName, List<Guid> participantIds)
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

                var chatRoomIds = existingChatRooms.Select(cr => cr.ChatRoomId).Distinct().ToList();

                

                foreach (var crIds in chatRoomIds)
                {
                    var chatRoomParticipants = await _chatRoomParticipantRepository.GetListAsync(cp => cp.ChatRoomId == crIds);

                    var userIdList = chatRoomParticipants.Select(cr => cr.UserId).ToList(); 

                    if(userIdList.Count == participantIds.Count && !participantIds.Except(userIdList).Any())
                    {
                        return new ChatRoomDto
                        {
                            Id = crIds,
                            isDuplicate = true
                        };
                    }

                }


                // If the group name is empty and only two participants, use their names
                if (participantIds.Count == 2)
                {
                    var participants = await _userRepository.GetListAsync(user => participantIds.Contains(user.Id));

                    var participantsIds = participants.Select(p => p.Id).ToList(); 

                    var participantFound = await _custUserRepository.GetListAsync(user => participantsIds.Contains(user.UserId));

                        groupName = $"{participantFound[0].FullName} and {participantFound[1].FullName}";
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

        public async Task EditGroupNameAsync(Guid groupId, string newGroupName)
        {
            // Fetch the group
            var group = await _chatRoomRepository.GetAsync(groupId);

            // Update the group name
            group.Name = newGroupName;
            await _chatRoomRepository.UpdateAsync(group);
        }

        public async Task DeleteGroupAsync(Guid groupId)
        {
            // Fetch the group
            var group = await _chatRoomRepository.GetAsync(groupId);

       
            // Delete the group
            await _chatRoomRepository.DeleteAsync(groupId);
        }


        public async Task<List<ChatRoomParticipantDto>> GetUserChatRoomsAsync()
        {
            var chatRoomParticipants = await _chatRoomParticipantRepository.GetListAsync(cp => cp.UserId == CurrentUser.Id);

            var chatRoomIds = chatRoomParticipants.Select(cp => cp.ChatRoomId).ToList();

            var chatRooms = await _chatRoomRepository.GetListAsync(cr => chatRoomIds.Contains(cr.Id));

            var lastChatMessage = await _chatMessageRepository.GetListAsync(cmsg => chatRoomIds.Contains(cmsg.ChatRoomId.Value));

            var chatRoomParticipantData = new List<ChatRoomParticipantDto>();

           foreach(var participant in chatRoomParticipants)
            {
                // Assuming chatRooms is available and contains all chat rooms
                var chatRoom = chatRooms.FirstOrDefault(cr => cr.Id == participant.ChatRoomId);
                
                var lastMessage = lastChatMessage.Where(cm => cm.ChatRoomId == participant.ChatRoomId).OrderByDescending(msg => msg.CreationTime).FirstOrDefault();

                var lastMessageUser = (IdentityUser)null;

                if (lastMessage != null)
                {
                    lastMessageUser = await _userRepository.GetAsync(lastMessage.CreatorId.Value);
                }

                // Manually combine ChatRoom and ChatRoomParticipant information
                chatRoomParticipantData.Add(new ChatRoomParticipantDto
                {
                    Id = participant.Id,                          // From ChatRoomParticipant
                    ChatRoomId = participant.ChatRoomId,          // From ChatRoomParticipant
                    Name = chatRoom.Name,         // From ChatRoom (Name)
                    LastMessage = lastMessage?.Content ?? "",
                    Username = lastMessageUser?.UserName ?? "No messages yet",
                    CreationTime = lastMessage?.CreationTime ?? chatRoom.CreationTime,
                    CreatorId = chatRoom.CreatorId,
                    CurrentUserId = CurrentUser.Id
                });
            }

            return chatRoomParticipantData;
        }

        public async Task<List<ChatMessageDto>> GetChatMessagesAsync(Guid chatRoomId)
        {
            var chatMessages = await _chatMessageRepository.GetListAsync(cr => cr.ChatRoomId == chatRoomId);

            var chatMessageDtos = new List<ChatMessageDto>();


            var chatRoom = await _chatRoomRepository.GetAsync(chatRoomId);

            foreach (var message in chatMessages)
            {
                var user = await _userRepository.GetAsync(message.CreatorId.Value);

                chatMessageDtos.Add(new ChatMessageDto
                {
                    CreatorId = message.CreatorId.Value,
                    CreationTime = message.CreationTime,
                    ChatRoomName = chatRoom.Name,
                    Content = message.Content,
                    Username = user.UserName // Assuming user has a UserName property
                });
            }

            return chatMessageDtos;
        }
        public async Task<List<ChatRoomParticipantDto>> GetUsersInChatroomAsync(Guid chatRoomId)
        {
            var userFound = await _chatRoomParticipantRepository.GetListAsync(crp => crp.ChatRoomId == chatRoomId);

            var usersInChatRoom = new List<ChatRoomParticipantDto>();

            foreach(var user in userFound)
            {
                var usernameSearch = await _userRepository.GetAsync(user.UserId);

                usersInChatRoom.Add(new ChatRoomParticipantDto
                {
                    Username = usernameSearch.UserName
                });
                
            }
            return usersInChatRoom;
        }
        public async Task<ChatMessageDto> CreateChatMessagesAsync(Guid chatRoomId, string content)
        {
            try
            {
                var chatMessages = new ChatMessage
                {
                    ChatRoomId = chatRoomId,
                    Content = content,
                    IsSeen = false,
                };

                var message = await _chatMessageRepository.InsertAsync(chatMessages);

                var userFound = await _userRepository.GetAsync(message.CreatorId.Value);

                return new ChatMessageDto
                {
                    ChatRoomId = message.ChatRoomId.Value,
                    Content = message.Content,
                    IsSeen = message.IsSeen,
                    CreationTime = message.CreationTime,
                    Username = userFound.UserName,
                    CreatorId = message.CreatorId.Value
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
