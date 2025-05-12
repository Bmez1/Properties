using Bogus;

using MockQueryable;

using NSubstitute;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Owners.List;
using Properties.Domain.Entities;


namespace Properties.Unitests.UseCases.Owners
{
    public class ListTests
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ListQueryHandler _handler;

        public ListTests()
        {
            _ownerRepository = Substitute.For<IOwnerRepository>();
            _handler = new ListQueryHandler(_ownerRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedOwnerResponseDtos()
        {
            // Arrange
            var faker = new Faker();
            var owners = new List<Owner>
            {
                Owner.Create(faker.Name.FullName(), faker.Address.FullAddress(), DateOnly.FromDateTime(faker.Date.Past())),
                Owner.Create(faker.Name.FullName(), faker.Address.FullAddress(), DateOnly.FromDateTime(faker.Date.Past()))
            };

            owners[0].GetType().GetProperty("Properties")?.SetValue(owners[0], Enumerable.Empty<Property>());
            owners[1].GetType().GetProperty("Properties")?.SetValue(owners[1], Enumerable.Empty<Property>());

            var ownerQueryable = owners.AsQueryable().BuildMock();
            _ownerRepository.GetAll().Returns(ownerQueryable);

            var request = new ListQuery();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());

        }
    }
}
