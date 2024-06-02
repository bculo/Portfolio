namespace User.Application.Entities
{
    public class PortfolioUser
    {
        public long Id { get; set; }
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        
        public DateTime BornOn { get; set; }

        /// <summary>
        /// Keycloak ID in this case
        /// </summary>
        public Guid? ExternalId { get; set; }
    }
}
