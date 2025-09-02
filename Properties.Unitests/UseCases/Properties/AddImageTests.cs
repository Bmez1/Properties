using Bogus;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Properties.AddImage;
using Properties.Domain.Entities;
using Properties.Domain.Errors;

namespace Properties.Unitests.UseCases.Properties
{
    public class AddImageTests
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;
        private readonly AddImageCommandHandler _handler;
        private readonly Faker _faker;

        public AddImageTests()
        {
            _propertyRepository = Substitute.For<IPropertyRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _blobStorageService = Substitute.For<IBlobStorageService>();
            _handler = new AddImageCommandHandler(_propertyRepository, _unitOfWork, _blobStorageService);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_Should_Add_Image_When_Property_Exists()
        {
            // Arrange
            var property = Property.Create(
                _faker.Address.StreetName(),
                _faker.Address.FullAddress(),
                _faker.Random.Decimal(100000, 300000),
                _faker.Random.Int(1990, 2024),
                Guid.NewGuid()
            );

            var fileUpload = Substitute.For<IFileUpload>();
            fileUpload.FileName.Returns("photo.jpg");
            fileUpload.OpenReadStream().Returns(new MemoryStream(new byte[10]));

            var command = new AddImageCommand(fileUpload, property.Id);

            _propertyRepository.GetByIdAsync(property.Id).Returns(property);
            _blobStorageService.UploadFileAsync(
                Arg.Any<Func<Stream>>(),
                Arg.Any<string>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns("http://blobstorage.com/photo.jpg");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(property.Id, result.Value.PropertyId);
            Assert.Equal("http://blobstorage.com/photo.jpg", result.Value.Image);
            await _blobStorageService.Received(1)
                .UploadFileAsync(Arg.Any<Func<Stream>>(), Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>());

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Property_Not_Found()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var fileUpload = Substitute.For<IFileUpload>();

            var command = new AddImageCommand(fileUpload, propertyId);

            _propertyRepository.GetByIdAsync(propertyId).ReturnsNull();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PropertyError.NotFoundById, result.Error);
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
