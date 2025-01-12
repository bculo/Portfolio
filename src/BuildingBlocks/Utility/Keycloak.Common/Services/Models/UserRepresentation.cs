using System.Text.Json.Serialization;

namespace Keycloak.Common.Services.Models
{
    public partial class UserRepresentation
    {

        [JsonPropertyName("self")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Self { get; set; }

        [JsonPropertyName("id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Id { get; set; }

        [JsonPropertyName("origin")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Origin { get; set; }

        [JsonPropertyName("createdTimestamp")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public long CreatedTimestamp { get; set; }

        [JsonPropertyName("username")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Username { get; set; }

        [JsonPropertyName("enabled")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public bool Enabled { get; set; }

        [JsonPropertyName("totp")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public bool Totp { get; set; }

        [JsonPropertyName("emailVerified")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public bool EmailVerified { get; set; }

        [JsonPropertyName("firstName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string LastName { get; set; }

        [JsonPropertyName("email")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string Email { get; set; }

        [JsonPropertyName("federationLink")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string FederationLink { get; set; }

        [JsonPropertyName("serviceAccountClientId")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string ServiceAccountClientId { get; set; }

        [JsonPropertyName("attributes")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public IDictionary<string, ICollection<object>> Attributes { get; set; }

        [JsonPropertyName("credentials")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<CredentialRepresentation> Credentials { get; set; }

        [JsonPropertyName("disableableCredentialTypes")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<string> DisableableCredentialTypes { get; set; }

        [JsonPropertyName("requiredActions")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<string> RequiredActions { get; set; }

        [JsonPropertyName("federatedIdentities")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<FederatedIdentityRepresentation> FederatedIdentities { get; set; }

        [JsonPropertyName("realmRoles")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<string> RealmRoles { get; set; }

        [JsonPropertyName("clientRoles")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public IDictionary<string, ICollection<object>> ClientRoles { get; set; }

        [JsonPropertyName("clientConsents")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<UserConsentRepresentation> ClientConsents { get; set; }

        [JsonPropertyName("notBefore")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int NotBefore { get; set; }

        [JsonPropertyName("applicationRoles")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public IDictionary<string, ICollection<object>> ApplicationRoles { get; set; }

        [JsonPropertyName("socialLinks")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<SocialLinkRepresentation> SocialLinks { get; set; }

        [JsonPropertyName("groups")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public ICollection<string> Groups { get; set; }

        [JsonPropertyName("access")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public IDictionary<string, bool> Access { get; set; }

        [JsonPropertyName("userProfileMetadata")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public UserProfileMetadata UserProfileMetadata { get; set; }

        private IDictionary<string, object> _additionalProperties;

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ??= new Dictionary<string, object>(); }
            set { _additionalProperties = value; }
        }

    }
}
