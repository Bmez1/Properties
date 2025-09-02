using Bogus;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.Create;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Unitests.UseCases.Properties
{
    public class CreateTests
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreatePropertyCommandHandler _handler;
        private readonly Faker _faker;

        public CreateTests()
        {
            _propertyRepository = Substitute.For<IPropertyRepository>();
            _ownerRepository = Substitute.For<IOwnerRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CreatePropertyCommandHandler(_propertyRepository, _ownerRepository, _unitOfWork);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_Should_Create_Property_With_Trace_When_Owner_Exists()
        {
            // Arrange
            var owner = Owner.Create(_faker.Person.FullName, _faker.Address.FullAddress(), DateOnly.FromDateTime(_faker.Date.Past(30)));
            _ownerRepository.GetByIdAsync(owner.Id).Returns(owner);

            var command = new CreatePropertyCommand(
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 300000),
                _faker.Random.Int(1990, 2024),
                owner.Id,
                new PropertyTraceCreateDto
                {
                    Value = _faker.Random.Decimal(100000, 200000),
                    Tax = _faker.Random.Decimal(1000, 10000)
                }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Address, result.Value.Address);
            await _propertyRepository.Received(1).CreateAsync(Arg.Any<Property>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Owner_Not_Exists()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            _ownerRepository.GetByIdAsync(ownerId).ReturnsNull();

            var command = new CreatePropertyCommand(
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 300000),
                _faker.Random.Int(1990, 2024),
                ownerId,
                new PropertyTraceCreateDto { Value = 100000, Tax = 5000 }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(OwnerError.NotFoundById, result.Error);
            await _propertyRepository.DidNotReceive().CreateAsync(Arg.Any<Property>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Create_Property_Without_Trace_When_OwnerId_Is_Null()
        {
            // Arrange
            var command = new CreatePropertyCommand(
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 300000),
                _faker.Random.Int(1990, 2024),
                Guid.NewGuid(),
                new PropertyTraceCreateDto { Value = 100000, Tax = 5000 }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Domain.Errors.OwnerError.NotFoundById.Description, result.Error.Description);
            await _propertyRepository.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Property>());
            await _unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
