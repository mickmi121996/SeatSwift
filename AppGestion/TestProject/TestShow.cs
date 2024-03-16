namespace TestProject
{
    public class TestShow
    {
        [Fact]
        public void Constructor_Default_ShouldInitializePropertiesToDefaults()
        {
            var show = new Show();

            Assert.Equal(default(int), show.Id);
            Assert.True(show.IsActive);
            Assert.Empty(show.Name);
            Assert.Empty(show.Artist);
            Assert.Empty(show.Description);
            Assert.Equal(default(ShowType), show.ShowType);
            Assert.Empty(show.ImageUrl);
            Assert.Equal(default(int), show.MaxTicketsByClient);
            Assert.Equal(default(decimal), show.BasePrice);
            Assert.Null(show.User);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCorrectlyAssignProperties()
        {
            var user = new User();
            var id = 1;
            var isActive = true;
            var name = "Test Show";
            var artist = "Test Artist";
            var description = "Test Description";
            var showType = ShowType.Theater;
            var imageUrl = "http://example.com/image.jpg";
            var maxTicketsByClient = 4;
            var basePrice = 25.00m;

            var show = new Show(id, isActive, name, artist, description, showType, imageUrl, maxTicketsByClient, basePrice, user);

            Assert.Equal(id, show.Id);
            Assert.Equal(isActive, show.IsActive);
            Assert.Equal(name, show.Name);
            Assert.Equal(artist, show.Artist);
            Assert.Equal(description, show.Description);
            Assert.Equal(showType, show.ShowType);
            Assert.Equal(imageUrl, show.ImageUrl);
            Assert.Equal(maxTicketsByClient, show.MaxTicketsByClient);
            Assert.Equal(basePrice, show.BasePrice);
            Assert.Same(user, show.User);
        }

        [Fact]
        public void Properties_ShouldBeModifiable()
        {
            var show = new Show();

            show.Name = "Updated Show";
            show.IsActive = false;
            show.Description = "Updated Description";
            show.MaxTicketsByClient = 5;

            Assert.Equal("Updated Show", show.Name);
            Assert.False(show.IsActive);
            Assert.Equal("Updated Description", show.Description);
            Assert.Equal(5, show.MaxTicketsByClient);
        }
    }
}
