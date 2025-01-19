using Duende.IdentityServer.Models;

namespace IdentityService
{
    public static class Config
    {
        // Identity resources (OpenID Connect)
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        // Define API scopes for each service
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("user-service", "User Service"),
                new ApiScope("property-service", "Property Service"),
                new ApiScope("search-service", "Search Service"),
                new ApiScope("chat-service", "Chat Service"),
                new ApiScope("notification-service", "Notification Service"),
                new ApiScope("action-tracking-service", "Action Tracking Service")
            };

        // Define clients for each service
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // Client for Chat Service
                new Client
                {
                    ClientId = "chat-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("chat-service-secret".Sha256()) },
                    AllowedScopes = { "chat-service" }
                },

                // Client for Property Service
                new Client
                {
                    ClientId = "property-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("property-service-secret".Sha256()) },
                    AllowedScopes = { "property-service" }
                },

                // Client for Search Service
                new Client
                {
                    ClientId = "search-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("search-service-secret".Sha256()) },
                    AllowedScopes = { "search-service" }
                },

                // Client for Notification Service
                new Client
                {
                    ClientId = "notification-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("notification-service-secret".Sha256()) },
                    AllowedScopes = { "notification-service" }
                },

                // Client for Action Tracking Service
                new Client
                {
                    ClientId = "action-tracking-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("action-tracking-service-secret".Sha256()) },
                    AllowedScopes = { "action-tracking-service" }
                },

                // Client for User Service
                new Client
                {
                    ClientId = "user-service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("user-service-secret".Sha256()) },
                    AllowedScopes = { "user-service" }
                },

                // User-facing client for authentication (User Login)
                new Client
                {
                    ClientId = "user-client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("user-client-secret".Sha256()) },
                    AllowedScopes =
                    {
                        "user-service",
                        "property-service",
                        "search-service",
                        "chat-service",
                        "notification-service",
                        "action-tracking-service"
                    }
                }
            };
    }
}