using Microsoft.Extensions.Logging;
using Moq;
using PracticalAssignment.Repositories;
using PracticalAssignment.Services;
using PracticalAssignment.Services.Exceptions;

namespace PracticalAssignment.Unit.Tests
{
    [TestFixture]
    public class NumberServiceTests
    {
        private Mock<INumberRepository> _mockRepository;
        private Mock<ILogger<NumberService>> _mockLogger;
        private NumberService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<INumberRepository>();
            _mockLogger = new Mock<ILogger<NumberService>>();
            _service = new NumberService(_mockRepository.Object, _mockLogger.Object);
        }

        [Test]
        public void Parse_ValidNumbers_ReturnsCorrectNumbers()
        {
            // Arrange
            var input = "1 2 3 4 5";

            // Act
            var results = _service.Parse(input);

            // Assert
            Assert.That(results, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Parse_InvalidNumber_ThrowsDomainServiceException()
        {
            // Arrange
            var input = "1 2 three 4 5";

            // Act & Assert
            var ex = Assert.Throws<DomainServiceException>(() => _service.Parse(input).ToList());
            Assert.That(ex.Message, Is.EqualTo("Cannot parse string because three is not a number."));
        }

        [Test]
        public void Parse_EmptyString_ThrowsDomainServiceException()
        {
            // Arrange
            var input = String.Empty;

            // Act & Assert
            var ex = Assert.Throws<DomainServiceException>(() => _service.Parse(input).ToList());
            Assert.That(ex.Message, Is.EqualTo("Cannot parse because it is empty."));
        }

        [Test]
        public async Task Create_ValidNumbers_CallsSaveWithSortedNumbers()
        {
            // Arrange
            var numbers = new List<int> { 5, 3, 1, 4, 2 };

            _mockRepository.Setup(r => r.Save(It.IsAny<IEnumerable<int>>()))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            await _service.Create(numbers);

            // Assert
            _mockRepository.Verify(r => r.Save(It.Is<IEnumerable<int>>(nums => nums.SequenceEqual(new[] { 1, 2, 3, 4, 5 }))));
        }

        [Test]
        public async Task GetLatest_CallsRepositoryGetLatest()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetLatest())
                .ReturnsAsync("latest data")
                .Verifiable();

            // Act
            var result = await _service.GetLatest();

            // Assert
            Assert.That(result, Is.EqualTo("latest data"));
            _mockRepository.Verify(r => r.GetLatest(), Times.Once);
        }
    }
}