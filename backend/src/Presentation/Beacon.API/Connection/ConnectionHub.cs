using Beacon.Application;
using Beacon.Application.Features.Calls.Commands;
using Beacon.Application.Features.Calls.Queries;
using Beacon.Application.Features.Users.Commands;
using Beacon.Application.Features.Users.Queries;
using Beacon.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Beacon.API.Connection
{
    public class ConnectionHub(IMediator mediator) : Hub<IConnectionHub>
    {
        public async Task Join(string username)
        {
            await mediator.Send(new AddUser(username, Context.ConnectionId));
            await SendUserListUpdateAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Hang up any calls the user is in
            await HangUpAsync(); // Gets the user from "Context" which is available in the whole hub

            await mediator.Send(new RemoveUser(Context.ConnectionId));
            await SendUserListUpdateAsync();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task CallUser(User targetConnectionId)
        {
            var callingUser = await mediator.Send(new GetUser(Context.ConnectionId));
            if (callingUser is null)
            {
                return;
            }

            var targetUser = await mediator.Send(new GetUser(targetConnectionId.ConnectionId));

            // Make sure the person we are trying to call is still here
            if (targetUser is null)
            {
                // If not, let the caller know
                await Clients.Caller.CallDeclined(targetConnectionId, "The user you called has left.");
                return;
            }

            // And that they aren't already in a call
            var currentCall = await mediator.Send(new GetUserCall(targetUser.ConnectionId));
            if (currentCall is not null)
            {
                await Clients.Caller.CallDeclined(targetConnectionId, $"{targetUser.Name} is already in a call.");
                return;
            }

            // They are here, so tell them someone wants to talk
            await Clients.Client(targetConnectionId.ConnectionId).IncomingCall(callingUser);
            // Create an offer
            await mediator.Send(new AddCallOffer(callingUser, targetUser));
        }

        public async Task AnswerCall(bool acceptCall, User target)
        {
            var callingUser = await mediator.Send(new GetUser(Context.ConnectionId));
            var targetUser = await mediator.Send(new GetUser(target.ConnectionId));

            // This can only happen if the server-side came down and clients were cleared, while the user
            // still held their browser session.
            if (callingUser == null)
            {
                return;
            }

            // Make sure the original caller has not left the page yet
            if (targetUser == null)
            {
                await Clients.Caller.CallEnded(target, "The other user in your call has left.");
                return;
            }

            // Send a decline message if the callee said no
            if (!acceptCall)
            {
                await Clients.Client(target.ConnectionId).CallDeclined(callingUser, $"{callingUser.Name} did not accept your call.");
                return;
            }

            // Make sure there is still an active offer. If there isn't, then the other use hung up before the Callee answered.
            var offerCount = await mediator.Send(new CheckActiveCallOffers(callingUser.ConnectionId, targetUser.ConnectionId));
            if (offerCount < 1)
            {
                await Clients.Caller.CallEnded(target, $"{targetUser.Name} has already hung up.");
                return;
            }

            // And finally... make sure the user hasn't accepted another call already
            var targetUserCall = await mediator.Send(new GetUserCall(targetUser.ConnectionId));
            if (targetUserCall is not null)
            {
                // And that they aren't already in a call
                await Clients.Caller.CallDeclined(target, $"{targetUser.Name} chose to accept someone else call instead of yours");
                return;
            }

            // Remove all the other offers for the call initiator, in case they have multiple calls out
            await mediator.Send(new RemoveCallOffers(targetUser.ConnectionId));

            // Create a new call to match these folks up
            await mediator.Send(new AddUserCall(callingUser, targetUser));

            // Tell the original caller that the call was accepted
            await Clients.Client(target.ConnectionId).CallAccepted(callingUser);

            // Update the user list, since these two are now in a call
            await SendUserListUpdateAsync();
        }

        public async Task HangUpAsync()
        {
            var callingUser = await mediator.Send(new GetUser(Context.ConnectionId));
            if (callingUser is null)
            {
                return;
            }

            var currentCall = await mediator.Send(new GetUserCall(callingUser.ConnectionId));

            // Send a hang up message to each user in the call, if there is one
            if (currentCall != null)
            {
                var callingUserConnectionId = callingUser.ConnectionId;
                foreach (var user in currentCall.Users.Where(user => user.ConnectionId != callingUserConnectionId))
                {
                    await Clients.Client(user.ConnectionId).CallEnded(callingUser, $"{callingUser.Name} has hung up.");
                }

                // Remove the call from the list if there is only one (or none) person left.  This should
                // always trigger now, but will be useful when we implement conferencing.
                currentCall.Users.RemoveAll(u => u.ConnectionId == callingUserConnectionId);
                if (currentCall.Users.Count < 2)
                {
                    await mediator.Send(new RemoveUserCall(currentCall));
                }
            }

            // Remove all offers initiating from the caller
            await mediator.Send(new RemoveCallOffers(callingUser.ConnectionId));
            await SendUserListUpdateAsync();
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string signal, string targetConnectionId)
        {
            var callingUser = await mediator.Send(new GetUser(Context.ConnectionId));
            var targetUser = await mediator.Send(new GetUser(targetConnectionId));

            // Make sure both users are valid
            if (callingUser == null || targetUser == null)
            {
                return;
            }

            // Make sure that the person sending the signal is in a call
            var userCall = await mediator.Send(new GetUserCall(callingUser.ConnectionId));

            // ...and that the target is the one they are in a call with
            if (userCall != null && userCall.Users.Exists(u => u.ConnectionId == targetUser.ConnectionId))
            {
                // These folks are in a call together, let's let em talk WebRTC
                await Clients.Client(targetConnectionId).ReceiveSignal(callingUser, signal);
            }
        }

        #region private

        private async Task SendUserListUpdateAsync()
        {
            var refreshedUsers = await mediator.Send(new RefreshUserList());
            await Clients.All.UpdateUserList(refreshedUsers);
        }

        #endregion
    }
}
