using System.Collections.Concurrent;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Endpoints.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Evento.Endpoints.Hubs;

public sealed class ChatNotificationHub(IChatService chatService, IVenueAdminService venueAdminService) : Hub
{
    private static readonly ConcurrentDictionary<string, ChatUser> OnlineUsers = new();

    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> UserConnectionUnreadCounts =
        new();

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

            var connCounts = UserConnectionUnreadCounts.GetOrAdd(userId, _ => new());
            connCounts.TryAdd(Context.ConnectionId, 0);

            await Clients.All.SendAsync(ChatNotificationType.UserOnline,
                new ChatUserDto(chatUser.UserId, chatUser.Username));
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
            var receiverConnections = OnlineUsers
                .Where(kv => kv.Key == receiverId)
                .Select(kv => kv.Value.ConnectionId)
                .ToList();

            foreach (var connId in receiverConnections)
            {
                await Clients.Client(connId)
                    .SendAsync(ChatNotificationType.ReceiveMessage, messageDto);

                var connCounts =
                    UserConnectionUnreadCounts.GetOrAdd(receiverId, _ => new ConcurrentDictionary<string, int>());
                connCounts.AddOrUpdate(connId, 1, (_, c) => c + 1);

                await Clients.Client(connId)
                    .SendAsync(ChatNotificationType.UnreadMessagesNotification, connCounts[connId]);
            }
        }

        if (senderUser is not null)
        {
            await Clients.Client(senderUser.ConnectionId)
                .SendAsync(ChatNotificationType.ReceiveMessage, messageDto);
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