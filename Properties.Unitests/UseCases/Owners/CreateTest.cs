using Bogus;

using NSubstitute;

using Properties.Application.Interfaces;
using Properties.Application.UseCases.Owners.Create;
using Properties.Domain.Entities;

namespace Properties.Unitests.UseCases.Owners
{
    public class CreateTest
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;
        private readonly Faker _faker;

        public CreateTest()
        {
            _ownerRepository = Substitute.For<IOwnerRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _blobStorageService = Substitute.For<IBlobStorageService>();
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_WithPhoto_ShouldUploadPhotoAndCreateOwner()
        {
            // Arrange
            var stream = new MemoryStream([1, 2, 3]);
            var fakeFile = Substitute.For<IFileUpload>();
            fakeFile.FileName.Returns("photo.jpg");
            fakeFile.Extension.Returns(".jpg");
            fakeFile.Size.Returns(1000);
            fakeFile.OpenReadStream().Returns(stream);

            var command = new CreateOwnerCommand(
                _faker.Person.FullName,
                _faker.Address.FullAddress(),
                DateOnly.FromDateTime(_faker.Person.DateOfBirth),
                fakeFile
            );

            var expectedPath = "owners/20250101120000photo.jpg";
            _blobStorageService
                .UploadFileAsync(Arg.Any<Func<Stream>>(), Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
                .Returns(expectedPath);

            var handler = new CreateOwnerCommandHandler(_ownerRepository, _unitOfWork, _blobStorageService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            await _ownerRepository.Received(1).CreateAsync(Arg.Is<Owner>(o =>
                o.Name == command.Name &&
                o.Address == command.Address &&
                o.Photo == expectedPath
            ));

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _blobStorageService.Received(1).UploadFileAsync(Arg.Any<Func<Stream>>(), Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>());
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public async Task Handle_WithoutPhoto_ShouldCreateOwnerWithEmptyPhotoPath()
        {
            // Arrange
            var command = new CreateOwnerCommand(
                _faker.Person.FullName,
                _faker.Address.FullAddress(),
                DateOnly.FromDateTime(_faker.Person.DateOfBirth),
                null
            );

            var handler = new CreateOwnerCommandHandler(_ownerRepository, _unitOfWork, _blobStorageService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            await _ownerRepository.Received(1).CreateAsync(Arg.Is<Owner>(o =>
                o.Name == command.Name &&
                o.Photo == string.Empty
            ));

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

    }
}
