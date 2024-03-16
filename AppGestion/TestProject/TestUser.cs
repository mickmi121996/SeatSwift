namespace TestProject
{
    public class TestUser
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var user = new User(1, true, "John", "Doe", "123456", EmployeeType.Accountant, "john.doe@example.com", "1234567890");

            Assert.Equal(1, user.Id);
            Assert.True(user.IsActive);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("123456", user.EmployeeNumber);
            Assert.Equal(EmployeeType.Accountant, user.Type);
            Assert.Equal("john.doe@example.com", user.Email);
            Assert.Equal("1234567890", user.PhoneNumber);
        }

        [Fact]
        public void IsPasswordValid_ShouldReturnTrueForCorrectPassword()
        {
            var user = new User();
            var password = "ValidPassword123";
            var salt = PasswordTools.CreateSalt();
            var hash = PasswordTools.HashPassword(password, salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            var result = user.IsPasswordValide(password, hash, salt);

            Assert.True(result);
        }
    }
}
