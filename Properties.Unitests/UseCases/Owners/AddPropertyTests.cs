using Bogus;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Owners.AddProperty;
using Properties.Application.UseCases.Properties.Dtos;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Unitests.UseCases.Owners
{
    public class AddPropertyTests
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Faker _faker;

        public AddPropertyTests()
        {
            _ownerRepository = Substitute.For<IOwnerRepository>();
            _propertyRepository = Substitute.For<IPropertyRepository>();
            _propertyTraceRepository = Substitute.For<IPropertyTraceRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _faker = new Faker();
        }


        [Fact]
        public async Task Handle_Should_AddTrace_And_ChangeOwner_WhenValid()
        {
            // Arrange
            var owner = Owner.Create(_faker.Person.FullName, _faker.Address.FullAddress(), DateOnly.FromDateTime(_faker.Person.DateOfBirth));
            var property = Property.Create("Test", "Some address", 100000, 2023, owner.Id);

            _ownerRepository.GetByIdAsync(owner.Id, true).Returns(owner);
            _propertyRepository.GetByIdAsync(property.Id).Returns(property);

            var traceDto = new PropertyTraceCreateDto
            {
                Value = 123000,
                Tax = 5000
            };

            var command = new AddPropertyCommand(owner.Id, property.Id, traceDto);

            var handler = new AddPropertyCommandHandler(_ownerRepository, _propertyRepository, _propertyTraceRepository, _unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            await _propertyTraceRepository.Received(1).CreateAsync(Arg.Is<PropertyTrace>(t =>
                t.PropertyId == property.Id &&
                t.Value == traceDto.Value &&
                t.Tax == traceDto.Tax
            ));

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            Assert.True(result.IsSuccess);
            Assert.Equal(owner.Id, result.Value);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenOwnerNotFound()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            _ownerRepository.GetByIdAsync(ownerId, true).ReturnsNull();

            var command = new AddPropertyCommand(ownerId, Guid.NewGuid(), new PropertyTraceCreateDto { Value = 1000, Tax = 100 });
            var handler = new AddPropertyCommandHandler(_ownerRepository, _propertyRepository, _propertyTraceRepository, _unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(OwnerError.NotFoundById, result.Error);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenPropertyNotFound()
        {
            // Arrange
            var owner = Owner.Create(_faker.Person.FullName, _faker.Address.FullAddress(), DateOnly.FromDateTime(_faker.Person.DateOfBirth));
            _ownerRepository.GetByIdAsync(owner.Id, true).Returns(owner);
            _propertyRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            var command = new AddPropertyCommand(owner.Id, Guid.NewGuid(), new PropertyTraceCreateDto { Value = 1000, Tax = 100 });
            var handler = new AddPropertyCommandHandler(_ownerRepository, _propertyRepository, _propertyTraceRepository, _unitOfWork);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PropertyError.NotFoundById, result.Error);
        }
    }
}
