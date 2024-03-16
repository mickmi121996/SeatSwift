namespace TestGuichetAutonome
{
    public class TestClient
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var client = new Client(1, true, "John", "Doe", "john.doe@example.com", "1234567890", "New York");

            Assert.Equal(1, client.Id);
            Assert.True(client.IsActive);
            Assert.Equal("John", client.FirstName);
            Assert.Equal("Doe", client.LastName);
            Assert.Equal("john.doe@example.com", client.Email);
            Assert.Equal("1234567890", client.PhoneNumber);
            Assert.Equal("New York", client.City);
        }

        [Fact]
        public void IsPasswordValid_ShouldReturnTrueForCorrectPassword()
        {
            var client = new Client();
            var password = "ValidPassword123";
            var salt = PasswordTools.CreateSalt();
            var hash = PasswordTools.HashPassword(password, salt);

            client.PasswordHash = hash;
            client.PasswordSalt = salt;

            var result = client.IsPasswordValide(password, hash, salt);

            Assert.True(result);
        }
    }
}