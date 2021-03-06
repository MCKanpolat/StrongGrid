﻿using StrongGrid.Models;
using StrongGrid.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StrongGrid.Resources
{
	/// <summary>
	/// Allows you to manage contacts (which are sometimes refered to as 'recipients').
	/// </summary>
	/// <remarks>
	/// See <a href="https://sendgrid.com/docs/API_Reference/Web_API_v3/Marketing_Campaigns/contactdb.html">SendGrid documentation</a> for more information.
	/// </remarks>
	public interface IContacts
	{
		/// <summary>
		/// Creates a contact.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="firstName">The first name.</param>
		/// <param name="lastName">The last name.</param>
		/// <param name="customFields">The custom fields.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The identifier of the new contact.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while creating the contact.</exception>
		Task<string> CreateAsync(
			string email,
			Parameter<string> firstName = default(Parameter<string>),
			Parameter<string> lastName = default(Parameter<string>),
			Parameter<IEnumerable<Field>> customFields = default(Parameter<IEnumerable<Field>>),
			string onBehalfOf = null,
			CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Updates the contact.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="firstName">The first name.</param>
		/// <param name="lastName">The last name.</param>
		/// <param name="customFields">The custom fields.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		/// <exception cref="System.Exception">Thrown when an exception occured while updating the contact.</exception>
		Task UpdateAsync(
			string email,
			Parameter<string> firstName = default(Parameter<string>),
			Parameter<string> lastName = default(Parameter<string>),
			Parameter<IEnumerable<Field>> customFields = default(Parameter<IEnumerable<Field>>),
			string onBehalfOf = null,
			CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Import contacts.
		/// </summary>
		/// <param name="contacts">The contacts.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="ImportResult">result</see> of the operation.
		/// </returns>
		Task<ImportResult> ImportAsync(IEnumerable<Contact> contacts, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Delete a contact.
		/// </summary>
		/// <param name="contactId">The contact identifier.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAsync(string contactId, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Delete contacts.
		/// </summary>
		/// <param name="contactId">The contact identifier.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The async task.
		/// </returns>
		Task DeleteAsync(IEnumerable<string> contactId, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Retrieve a contact.
		/// </summary>
		/// <param name="contactId">The contact identifier.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The <see cref="Contact" />.
		/// </returns>
		Task<Contact> GetAsync(string contactId, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Retrieve multiple contacts.
		/// </summary>
		/// <param name="recordsPerPage">The records per page.</param>
		/// <param name="page">The page.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact" />.
		/// </returns>
		Task<Contact[]> GetAsync(int recordsPerPage = 100, int page = 1, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Gets the billable count.
		/// </summary>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The number of billable contacts.
		/// </returns>
		Task<long> GetBillableCountAsync(string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Gets the total count.
		/// </summary>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// The total number of contacts.
		/// </returns>
		Task<long> GetTotalCountAsync(string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Searches for contacts matching the specified conditions.
		/// </summary>
		/// <param name="conditions">The conditions.</param>
		/// <param name="listId">The list identifier.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="Contact" />.
		/// </returns>
		Task<Contact[]> SearchAsync(IEnumerable<SearchCondition> conditions, long? listId = null, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Retrieve the lists that a recipient is on.
		/// </summary>
		/// <param name="contactId">The contact identifier.</param>
		/// <param name="onBehalfOf">The user to impersonate.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="List" />.
		/// </returns>
		Task<List[]> GetListsAsync(string contactId, string onBehalfOf = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
