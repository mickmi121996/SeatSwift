namespace TestProject
{
    public class TestPassword
    {
        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var password = "ValidPassword123";
            var salt = PasswordTools.CreateSalt();
            var hash = PasswordTools.HashPassword(password, salt);

            var client = new Client
            {
                PasswordHash = hash,
                PasswordSalt = salt
            };

            var isValid = client.IsPasswordValide(password, hash, salt);

            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            var correctPassword = "ValidPassword123";
            var incorrectPassword = "InvalidPassword321";
            var salt = PasswordTools.CreateSalt();
            var hash = PasswordTools.HashPassword(correctPassword, salt);

            var client = new Client
            {
                PasswordHash = hash,
                PasswordSalt = salt
            };

            var isValid = client.IsPasswordValide(incorrectPassword, hash, salt);

            Assert.False(isValid);
        }
    }
}