using HR_Management_System.Application.Dtos;
using HR_Management_System.Repositories;
using HR_Management_System.Application.Validators; 
using Moq;

namespace HR_Management_System.Tests.Application.Validators
{
    [TestFixture]
    public class UserValidatorTest
    {
        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginIsEmpty()
        {
            var repositoryMock = new Mock<ISystemUserRepository>();
            repositoryMock.Setup(r => r.LoginExists(It.IsAny<string>()))
                          .ReturnsAsync(false);

            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            var request = new UserRequestDto
            {
                Login = "",
                Password = "123",
                Role = "Admin"
            };

            var result = await validator.ValidateAsync(request);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e => e.PropertyName == "Login"), Is.True);
        }

        [Test]
        public async Task Create_ShouldHaveValidationErrors_WhenLoginIsTooShort()
        {
            var repositoryMock = new Mock<ISystemUserRepository>();
            repositoryMock.Setup(r => r.LoginExists(It.IsAny<string>()))
                          .ReturnsAsync(false);

            var validator = new UserRequestDtoValidator(repositoryMock.Object);

            var request = new UserRequestDto
            {
                Login = "abc",
                Password = "123",
                Role = "Admin"
            };

            var result = await validator.ValidateAsync(request);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Any(e =>
                e.ErrorMessage.Contains("mínimo 4")), Is.True);
        }
    }
}
