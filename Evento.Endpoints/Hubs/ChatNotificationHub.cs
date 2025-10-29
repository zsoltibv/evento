using System.Collections.Concurrent;
using Evento.Application.Common.Dto;
using Evento.Application.Services.Interfaces;
using Evento.Endpoints.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Evento.Endpoints.Hubs;

public sealed class ChatNotificationHub(IChatService chatService) : Hub
{
    private static readonly ConcurrentDictionary<string, ChatUser> OnlineUsers = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var username = Context.User?.GetUserName() ?? "Unknown";

        if (!string.IsNullOrEmpty(userId))
        {
            var chatUser = new ChatUser(userId, username, Context.ConnectionId);
            OnlineUsers[userId] = chatUser;
            await Clients.All.SendAsync("UserOnline", new ChatUserDto(chatUser.UserId, chatUser.Username));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = OnlineUsers.Values.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
        if (user is not null)
        {
            OnlineUsers.TryRemove(user.UserId, out _);
            await Clients.All.SendAsync("UserOffline", new ChatUserDto(user.UserId, user.Username));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string senderId, string receiverId, string messageText)
    {
        var message = await chatService.SendMessageAsync(senderId, receiverId, messageText);
        
        var senderUser = OnlineUsers.GetValueOrDefault(senderId);
        var receiverUser = OnlineUsers.GetValueOrDefault(receiverId);

        var messageDto = new ChatMessageDto(
            new ChatUserDto(senderId, senderUser?.Username ?? "Unknown"),
            new ChatUserDto(receiverId, receiverUser?.Username ?? "Unknown"),
            message.MessageText,
            message.SentAt
        );
        
        if (receiverUser is not null)
            await Clients.Client(receiverUser.ConnectionId).SendAsync("ReceiveMessage", messageDto);
        
        if (senderUser is not null)
            await Clients.Client(senderUser.ConnectionId).SendAsync("ReceiveMessage", messageDto);
    }

    public async Task<List<ChatMessageDto>> GetChatHistory(string userId1, string userId2)
    {
        var messages = await chatService.GetChatHistoryAsync(userId1, userId2);
        
        var result = messages.Select(m =>
        {
            var sender = new ChatUserDto(m.SenderId, m.Sender?.UserName ?? "Unknown");
            var receiver = new ChatUserDto(m.ReceiverId, m.Receiver?.UserName ?? "Unknown");

            return new ChatMessageDto(sender, receiver, m.MessageText, m.SentAt);
        }).ToList();

        return result;
    }

    public Task<List<ChatUserDto>> GetOnlineUsers()
        => Task.FromResult(OnlineUsers.Values
            .Select(u => new ChatUserDto(u.UserId, u.Username))
            .ToList());
}