using Bogus;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Update;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Unitests.UseCases.Properties
{
    public class UpdateTests
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdatePropertyCommandHandler _handler;
        private readonly Faker _faker;

        public UpdateTests()
        {
            _propertyRepository = Substitute.For<IPropertyRepository>();
            _ownerRepository = Substitute.For<IOwnerRepository>();
            _propertyTraceRepository = Substitute.For<IPropertyTraceRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new UpdatePropertyCommandHandler(_propertyRepository, _ownerRepository, _propertyTraceRepository, _unitOfWork);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Property_Not_Found()
        {
            // Arrange
            var command = new UpdatePropertyCommad(
                Guid.NewGuid(),
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 200000),
                _faker.Random.Int(1990, 2024),
                null,
                null!
            );

            _propertyRepository.GetByIdAsync(command.PropertyId, true).ReturnsNull();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PropertyError.NotFoundById, result.Error);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Owner_Not_Exists()
        {
            // Arrange
            var oldOwnerId = Guid.NewGuid();
            var newOwnerId = Guid.NewGuid();
            var property = Property.Create(_faker.Company.CompanyName(), _faker.Address.FullAddress(), 150000, 2020, oldOwnerId);

            _propertyRepository.GetByIdAsync(property.Id, true).Returns(property);
            _ownerRepository.ExistsByIdAsync(newOwnerId).Returns(false);

            var command = new UpdatePropertyCommad(
                property.Id,
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 200000),
                _faker.Random.Int(1990, 2024),
                newOwnerId,
                new() { Value = 120000, Tax = 0 }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(OwnerError.NotFoundById, result.Error);
        }

        [Fact]
        public async Task Handle_Should_Update_Property_And_Create_Trace_When_Owner_Changes()
        {
            // Arrange
            var owner = Owner.Create(_faker.Person.FullName, _faker.Address.FullAddress(), DateOnly.FromDateTime(_faker.Date.Past(30)));
            var oldOwnerId = Guid.NewGuid();
            var newOwnerId = Guid.NewGuid();
            var property = Property.Create(_faker.Company.CompanyName(), _faker.Address.FullAddress(), 150000, 2020, oldOwnerId);

            _propertyRepository.GetByIdAsync(property.Id, true).Returns(property);
            _ownerRepository.GetByIdAsync(newOwnerId).Returns(owner);

            var command = new UpdatePropertyCommad(
                property.Id,
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                180000,
                2024,
                newOwnerId,
                new() { Value = 180000, Tax = 0 }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(property.Id, result.Value);
            await _propertyTraceRepository.Received(1).CreateAsync(Arg.Any<PropertyTrace>());
            _propertyRepository.Received(1).Update(property);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Update_Property_Without_Trace_When_Owner_Does_Not_Change()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var property = Property.Create(_faker.Company.CompanyName(), _faker.Address.FullAddress(), 150000, 2020, ownerId);

            _propertyRepository.GetByIdAsync(property.Id, true).Returns(property);

            var command = new UpdatePropertyCommad(
                property.Id,
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                170000,
                2022,
                ownerId,
                null!
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(property.Id, result.Value);
            await _propertyTraceRepository.DidNotReceive().CreateAsync(Arg.Any<PropertyTrace>());
            _propertyRepository.Received(1).Update(property);
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
