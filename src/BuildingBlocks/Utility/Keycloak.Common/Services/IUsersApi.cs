using Keycloak.Common.Models;
using Refit;

namespace Keycloak.Common.Services;

public interface IUsersApi
{
    /// <summary>
    /// Get users Returns a stream of users, filtered according to query parameters.
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users")]
    Task<List<UserRepresentation>> GetUsersByRealm(string realm, [Query] string briefRepresentation = null, 
        [Query] string email = null, [Query] string emailVerified = null, [Query] string enabled = null, 
        [Query] string exact = null, [Query] string first = null, [Query] string firstName = null, 
        [Query] string idpAlias = null, [Query] string idpUserId = null, [Query] string lastName = null, 
        [Query] string max = null, [Query] string q = null, [Query] string search = null, [Query] string username = null);

    /// <summary>
    /// Create a new user Username must be unique.
    /// </summary>
    [Post("/{realm}/users")]
    Task<IApiResponse<string>> PostUsers(string realm, [Body] UserRepresentation body);

    /// <summary>
    /// Returns the number of users that match the given criteria.
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/count")]
    Task<IApiResponse<int?>> GetUsersCount(string realm, [Query] string email, [Query] string emailVerified, [Query] string enabled, [Query] string firstName, [Query] string lastName, [Query] string q, [Query] string search, [Query] string username);

    [Headers("Accept: application/json")]
    [Get("/{realm}/users/profile")]
    Task<IApiResponse<string>> GetProfile(string realm);

    [Put("/{realm}/users/profile")]
    Task PutProfile(string realm, [Body] string body);

    /// <summary>
    /// Get representation of the user
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}")]
    Task<IApiResponse<UserRepresentation>> GetUserByRealmById(string realm, string id, [Query] string userProfileMetadata);

    /// <summary>
    /// Update the user
    /// </summary>
    [Put("/{realm}/users/{id}")]
    Task<IApiResponse> PutUser(string realm, string id, [Body] UserRepresentation body);

    /// <summary>
    /// Delete the user
    /// </summary>
    [Delete("/{realm}/users/{id}")]
    Task DeleteUserByRealmById(string realm, string id);

    /// <summary>
    /// Return credential types, which are provided by the user storage where user is stored.
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/configured-user-storage-credential-types")]
    Task<IApiResponse<object>> GetConfiguredUserStorageCredentialTypes(string realm, string id);

    /// <summary>
    /// Get consents granted by the user
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/consents")]
    Task<IApiResponse<object>> GetConsents(string realm, string id);

    /// <summary>
    /// Revoke consent and offline tokens for particular client from user
    /// </summary>
    [Delete("/{realm}/users/{id}/consents/{client}")]
    Task DeleteConsent(string realm, string id, string client);

    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/credentials")]
    Task<IApiResponse<object>> GetCredentials(string realm, string id);

    /// <summary>
    /// Remove a credential for a user
    /// </summary>
    [Delete("/{realm}/users/{id}/credentials/{credentialId}")]
    Task DeleteCredential(string realm, string id, string credentialId);

    /// <summary>
    /// Move a credential to a position behind another credential
    /// </summary>
    [Post("/{realm}/users/{id}/credentials/{credentialId}/moveAfter/{newPreviousCredentialId}")]
    Task PostMoveAfter(string realm, string id, string credentialId, string newPreviousCredentialId);

    /// <summary>
    /// Move a credential to a first position in the credentials list of the user
    /// </summary>
    [Post("/{realm}/users/{id}/credentials/{credentialId}/moveToFirst")]
    Task PostMoveToFirst(string realm, string id, string credentialId);

    /// <summary>
    /// Update a credential label for a user
    /// </summary>
    [Put("/{realm}/users/{id}/credentials/{credentialId}/userLabel")]
    Task PutUserLabel(string realm, string id, string credentialId, [Body] string body);

    /// <summary>
    /// Disable all credentials for a user of a specific type
    /// </summary>
    [Put("/{realm}/users/{id}/disable-credential-types")]
    Task PutDisableCredentialTypes(string realm, string id, [Body] string body);

    /// <summary>
    /// Send an email to the user with a link they can click to execute particular actions.
    /// </summary>
    [Put("/{realm}/users/{id}/execute-actions-email")]
    Task PutExecuteActionsEmail(string realm, string id, [Query] string client_id, [Query] string lifespan, [Query] string redirect_uri, [Body] string body);

    /// <summary>
    /// Get social logins associated with the user
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/federated-identity")]
    Task<IApiResponse<object>> GetFederatedIdentity(string realm, string id);

    /// <summary>
    /// Add a social login provider to the user
    /// </summary>
    [Post("/{realm}/users/{id}/federated-identity/{provider}")]
    Task PostFederatedIdentity(string realm, string id, string provider);

    /// <summary>
    /// Remove a social login provider from user
    /// </summary>
    [Delete("/{realm}/users/{id}/federated-identity/{provider}")]
    Task DeleteFederatedIdentity(string realm, string id, string provider);

    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/groups")]
    Task<IApiResponse<object>> GetUserGroups(string realm, string id, [Query] string briefRepresentation, [Query] string first, [Query] string max, [Query] string search);

    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/groups/count")]
    Task<IApiResponse<IDictionary<string, long>>> GetUserGroupsCount(string realm, string id, [Query] string search);

    [Put("/{realm}/users/{id}/groups/{groupId}")]
    Task PutUserGroup(string realm, string id, string groupId);

    [Delete("/{realm}/users/{id}/groups/{groupId}")]
    Task DeleteUserGroup(string realm, string id, string groupId);

    /// <summary>
    /// Impersonate the user
    /// </summary>
    [Headers("Accept: application/json")]
    [Post("/{realm}/users/{id}/impersonation")]
    Task<IApiResponse<IDictionary<string, object>>> PostImpersonation(string realm, string id);

    /// <summary>
    /// Remove all user sessions associated with the user Also send notification to all clients that have an admin URL to invalidate the sessions for the particular user.
    /// </summary>
    [Post("/{realm}/users/{id}/logout")]
    Task PostLogout(string realm, string id);

    /// <summary>
    /// Get offline sessions associated with the user and client
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/offline-sessions/{clientUuid}")]
    Task<IApiResponse<object>> GetOfflineSession(string realm, string id, string clientUuid);

    /// <summary>
    /// Set up a new password for the user.
    /// </summary>
    [Put("/{realm}/users/{id}/reset-password")]
    Task PutResetPassword(string realm, string id, [Body] CredentialRepresentation body);

    /// <summary>
    /// Send an email to the user with a link they can click to reset their password.
    /// </summary>
    [Put("/{realm}/users/{id}/reset-password-email")]
    Task PutResetPasswordEmail(string realm, string id, [Query] string client_id, [Query] string redirect_uri);

    /// <summary>
    /// Send an email-verification email to the user An email contains a link the user can click to verify their email address.
    /// </summary>
    [Put("/{realm}/users/{id}/send-verify-email")]
    Task PutSendVerifyEmail(string realm, string id, [Query] string client_id, [Query] string redirect_uri);

    /// <summary>
    /// Get sessions associated with the user
    /// </summary>
    [Headers("Accept: application/json")]
    [Get("/{realm}/users/{id}/sessions")]
    Task<IApiResponse<object>> GetSessions(string realm, string id);
}