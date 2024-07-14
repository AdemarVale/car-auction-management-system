using Xunit;

namespace CarAuctionManagementSystem.IntegrationTests.Fixtures;

[CollectionDefinition(Name)]
public class InfrastructureCollection : ICollectionFixture<DatabaseFixture>
{
    public const string Name = "Infrastructure collection";
}
