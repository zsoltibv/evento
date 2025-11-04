using System.Collections.Concurrent;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Endpoints.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Evento.Endpoints.Hubs;

public sealed class ChatNotificationHub(IChatService chatService, IVenueAdminService venueAdminService) : Hub
{
    private static readonly ConcurrentDictionary<string, ChatUser> OnlineUsers = new();
    private static readonly ConcurrentDictionary<string, int> UnreadMessageCount = new();
    private const string UnknownUser = "Unknown";

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var username = Context.User?.GetUserName() ?? UnknownUser;

        if (!string.IsNullOrEmpty(userId))
        {
            var chatUser = new ChatUser(userId, username, Context.ConnectionId);
            OnlineUsers[userId] = chatUser;
            await Clients.All.SendAsync(ChatNotificationType.UserOnline,
                new ChatUserDto(chatUser.UserId, chatUser.Username));

            // Notify user of unread messages
            if (UnreadMessageCount.TryGetValue(userId, out var count) && count > 0)
            {
                await Clients.Client(chatUser.ConnectionId)
                    .SendAsync("UnreadMessagesNotification", count);

                // Optionally reset unread count after notifying
                UnreadMessageCount[userId] = 0;
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = OnlineUsers.Values.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
        if (user is not null)
        {
            OnlineUsers.TryRemove(user.UserId, out _);
            await Clients.All.SendAsync(ChatNotificationType.UserOffline, new ChatUserDto(user.UserId, user.Username));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string senderId, string receiverId, string messageText)
    {
        var existingClaimOwner = await chatService.GetChatClaimOwnerAsync(receiverId);
        var senderUser = OnlineUsers.GetValueOrDefault(senderId);
        var receiverUser = OnlineUsers.GetValueOrDefault(receiverId);

        if (existingClaimOwner is null && Context.User!.IsVenueAdmin())
        {
            var newClaimOwner = await chatService.TryClaimChatAsync(senderId, receiverId);
            if (newClaimOwner is not null && newClaimOwner.AgentId != senderId)
            {
                await Clients.Client(receiverUser!.ConnectionId)
                    .SendAsync(ChatNotificationType.ChatClaimed, senderId, newClaimOwner.AgentId,
                        newClaimOwner.AgentName);
            }
        }

        var message = await chatService.SendMessageAsync(senderId, receiverId, messageText);
        var messageDto = new ChatMessageDto(
            new ChatUserDto(senderId, senderUser?.Username ?? UnknownUser),
            new ChatUserDto(receiverId, receiverUser?.Username ?? UnknownUser),
            message.MessageText,
            message.SentAt
        );

        if (receiverUser is not null)
        {
            await Clients.Client(receiverUser.ConnectionId).SendAsync(ChatNotificationType.ReceiveMessage, messageDto);
        }
        else
        {
            UnreadMessageCount.AddOrUpdate(receiverId, 1, (_, count) => count + 1);
        }

        if (senderUser is not null)
        {
            await Clients.Client(senderUser.ConnectionId).SendAsync(ChatNotificationType.ReceiveMessage, messageDto);
        }
        else
        {
            UnreadMessageCount.AddOrUpdate(senderId, 1, (_, count) => count + 1);
        }
    }

    public async Task SendMessageToUsers(string senderId, string[] receiverIds, string messageText)
    {
        foreach (var receiverId in receiverIds)
        {
            await SendMessage(senderId, receiverId, messageText);
        }
    }

    public async Task<List<ChatMessageDto>> GetChatHistory(string userId1, string userId2)
    {
        var messages = await chatService.GetChatHistoryAsync(userId1, userId2);

        return messages.Select(m =>
        {
            var sender = new ChatUserDto(m.SenderId, m.Sender?.UserName ?? UnknownUser);
            var receiver = new ChatUserDto(m.ReceiverId, m.Receiver?.UserName ?? UnknownUser);
            return new ChatMessageDto(sender, receiver, m.MessageText, m.SentAt);
        }).ToList();
    }

    public Task<List<ChatUserDto>> GetOnlineUsers()
        => Task.FromResult(OnlineUsers.Values
            .Select(u => new ChatUserDto(u.UserId, u.Username))
            .ToList());
}