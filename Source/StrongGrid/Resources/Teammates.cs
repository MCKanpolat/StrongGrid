﻿using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using StrongGrid.Models;
using StrongGrid.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrongGrid.Resources
{
	/// <summary>
	/// You can invite teammates or they can request access to certain scopes and you can accept
	/// or deny these requests
	/// </summary>
	/// <seealso cref="StrongGrid.Resources.ITeammates" />
	/// <remarks>
	/// See https://sendgrid.com/docs/API_Reference/Web_API_v3/teammates.html
	/// </remarks>
	public class Teammates : ITeammates
	{
		private const string _endpoint = "teammates";
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="Teammates" /> class.
		/// </summary>
		/// <param name="client">The HTTP client</param>
		internal Teammates(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Retrieve a list of all recent access requests.
		/// </summary>
		/// <param name="limit">The limit.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of access requests
		/// </returns>
		public Task<AccessRequest[]> GetAccessRequestsAsync(int limit = 10, int offset = 0, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.GetAsync("scopes/requests")
				.WithArgument("limit", limit)
				.WithArgument("offset", offset)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<AccessRequest[]>();
		}

		/// <summary>
		/// Deny an attempt to access your account.
		/// </summary>
		/// <param name="requestId">The ID of the request that you want to deny.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DenyAccessRequestAsync(string requestId, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.DeleteAsync($"scopes/requests/{requestId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Approve an attempt to access your account.
		/// </summary>
		/// <param name="requestId">The ID of the request that you want to approve.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task ApproveAccessRequestAsync(string requestId, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.PatchAsync($"scopes/requests/{requestId}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Resend a teammate invite
		/// </summary>
		/// <param name="token">The token for the invite that you want to resend.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// Teammate invitations will expire after 7 days.
		/// Resending an invite will reset the expiration date.
		/// </remarks>
		public Task ResendInvitationAsync(string token, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.PostAsync($"{_endpoint}/pending/{token}/resend")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Retrieve a list of all pending teammate invitations
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="TeammateInvitation" />.</returns>
		/// <remarks>
		/// Each teammate invitation is valid for 7 days.
		/// Users may resend the invite to refresh the expiration date.
		/// </remarks>
		public Task<TeammateInvitation[]> GetAllPendingInvitationsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.GetAsync($"{_endpoint}/pending")
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<TeammateInvitation[]>("result");
		}

		/// <summary>
		/// Delete a pending teammate invite
		/// </summary>
		/// <param name="token">The token for the invite that you want to delete.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// Each teammate invitation is valid for 7 days.
		/// Users may resend the invite to refresh the expiration date.
		/// </remarks>
		public Task DeleteInvitationAsync(string token, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.DeleteAsync($"{_endpoint}/pending/{token}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}

		/// <summary>
		/// Send a teammate invitation via email with a predefined set of scopes, or permissions.
		/// A teammate invite will expire after 7 days, but you may resend the invite at any time
		/// to reset the expiration date.
		/// </summary>
		/// <param name="email">The email address of the teammate</param>
		/// <param name="scopes">The scopes, or permissions, the teammate will be granted</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// Essentials, Legacy Lite, and Free Trial users may create up to one teammate per account.
		/// There is not a teammate limit for Pro and higher plans.
		/// </remarks>
		public Task<TeammateInvitation> InviteTeammateAsync(string email, IEnumerable<string> scopes, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject
			{
				{ "email", email },
				{ "scopes", JArray.FromObject(scopes.ToArray()) },
				{ "is_admin", false }
			};
			return _client
				.PostAsync(_endpoint)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<TeammateInvitation>();
		}

		/// <summary>
		/// Send a teammate invitation via email with the same "read" permissions that have been granted to you.
		/// A teammate invite will expire after 7 days, but you may resend the invite at any time
		/// to reset the expiration date.
		/// </summary>
		/// <param name="email">The email address of the teammate</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// Essentials, Legacy Lite, and Free Trial users may create up to one teammate per account.
		/// There is not a teammate limit for Pro and higher plans.
		/// </remarks>
		public async Task<TeammateInvitation> InviteTeammateWithReadOnlyPrivilegesAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
		{
			var scopes = await _client.GetCurrentScopes(true, cancellationToken).ConfigureAwait(true);
			scopes = scopes.Where(s => s.EndsWith(".read", System.StringComparison.OrdinalIgnoreCase)).ToArray();

			var data = new JObject
			{
				{ "email", email },
				{ "scopes", JArray.FromObject(scopes.ToArray()) },
				{ "is_admin", false }
			};
			return await _client
				.PostAsync(_endpoint)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<TeammateInvitation>()
				.ConfigureAwait(false);
		}

		/// <summary>
		/// Send a teammate invitation via email with admin permissions.
		/// A teammate invite will expire after 7 days, but you may resend the invite at any time
		/// to reset the expiration date.
		/// </summary>
		/// <param name="email">The email address of the teammate</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <remarks>
		/// Essentials, Legacy Lite, and Free Trial users may create up to one teammate per account.
		/// There is not a teammate limit for Pro and higher plans.
		/// </remarks>
		public Task<TeammateInvitation> InviteTeammateAsAdminAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject
			{
				{ "email", email },
				{ "is_admin", true }
			};
			return _client
				.PostAsync(_endpoint)
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<TeammateInvitation>();
		}

		/// <summary>
		/// Retrieve a list of all current teammates
		/// </summary>
		/// <param name="limit">The limit.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>An array of <see cref="Teammate" />.</returns>
		public Task<Teammate[]> GetAllTeammatesAsync(int limit = 10, int offset = 0, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.GetAsync(_endpoint)
				.WithArgument("limit", limit)
				.WithArgument("offset", offset)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<Teammate[]>("result");
		}

		/// <summary>
		/// Retrieve a specific teammate by username
		/// </summary>
		/// <param name="username">The teammate username</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="Teammate" />.</returns>
		public Task<Teammate> GetTeammateAsync(string username, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.GetAsync($"{_endpoint}/{username}")
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<Teammate>();
		}

		/// <summary>
		/// Retrieve a specific teammate by username
		/// </summary>
		/// <param name="username">The teammate username</param>
		/// <param name="scopes">The permissions to asign to the teammate</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The <see cref="Teammate" />.</returns>
		public Task<Teammate> UpdateTeammatePermissionsAsync(string username, IEnumerable<string> scopes, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject
			{
				{ "is_admin", false },
				{ "scopes", JArray.FromObject(scopes.ToArray()) }
			};
			return _client
				.PatchAsync($"{_endpoint}/{username}")
				.WithJsonBody(data)
				.WithCancellationToken(cancellationToken)
				.AsSendGridObject<Teammate>();
		}

		/// <summary>
		/// Delete a teammate
		/// </summary>
		/// <param name="username">The teammate username</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		public Task DeleteTeammateAsync(string username, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _client
				.DeleteAsync($"{_endpoint}/{username}")
				.WithCancellationToken(cancellationToken)
				.AsMessage();
		}
	}
}
