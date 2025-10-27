using System.Collections.Concurrent;
using Evento.Endpoints.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Evento.Endpoints.Hubs;

public sealed class ChatNotificationHub : Hub
{
    // Store ChatUser including ConnectionId
    private static readonly ConcurrentDictionary<string, ChatUser> OnlineUsers = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var username = Context.User?.GetUserName() ?? "Unknown";

        if (!string.IsNullOrEmpty(userId))
        {
            var chatUser = new ChatUser(userId, username, Context.ConnectionId);
            OnlineUsers[userId] = chatUser;

            // Notify everyone that a new user is online
            await Clients.All.SendAsync("UserOnline", chatUser);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = OnlineUsers.Values.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
        if (user != null)
        {
            OnlineUsers.TryRemove(user.UserId, out _);
            await Clients.All.SendAsync("UserOffline", user);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string senderId, string receiverId, string messageText)
    {
        if (!OnlineUsers.TryGetValue(senderId, out var sender) ||
            !OnlineUsers.TryGetValue(receiverId, out var receiver))
            return;

        var chatMessage = new ChatMessage(sender, receiver, messageText);

        // Send message to both sender and receiver
        await Clients.Client(receiver.ConnectionId).SendAsync("ReceiveMessage", chatMessage);
        await Clients.Client(sender.ConnectionId).SendAsync("ReceiveMessage", chatMessage);
    }

    public Task<List<ChatUser>> GetOnlineUsers()
        => Task.FromResult(OnlineUsers.Values.ToList());
}