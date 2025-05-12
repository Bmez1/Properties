using Bogus;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.ChangePrice;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Unitests.UseCases.Properties
{
    public class ChangePriceTests
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ChangePriceCommandHandler _handler;

        public ChangePriceTests()
        {
            _propertyRepository = Substitute.For<IPropertyRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new ChangePriceCommandHandler(_propertyRepository, _unitOfWork);
        }

        [Fact]
        public async Task Handle_Should_Update_Price_And_Return_Id_When_Property_Exists()
        {
            // Arrange
            var faker = new Faker();
            var property = Property.Create(faker.Address.StreetName(), faker.Address.FullAddress(), faker.Random.Decimal(100000, 200000), 2020);
            var newPrice = faker.Random.Decimal(250000, 300000);

            _propertyRepository.GetByIdAsync(property.Id).Returns(property);

            var command = new ChangePriceCommand(property.Id, newPrice);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(property.Id, result.Value);
            Assert.Equal(newPrice, property.Price);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Property_Not_Found()
        {
            // Arrange
            var command = new ChangePriceCommand(Guid.NewGuid(), 500000);
            _propertyRepository.GetByIdAsync(command.PropertyId).ReturnsNull();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PropertyError.NotFoundById, result.Error);
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Throw_When_NewPrice_Is_Invalid()
        {
            // Arrange
            var property = Property.Create("Casa", "Calle 1", 100000, 2022);
            var command = new ChangePriceCommand(property.Id, 0);

            _propertyRepository.GetByIdAsync(property.Id).Returns(property);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
