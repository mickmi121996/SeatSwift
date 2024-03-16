namespace TestProject
{
    public class TestAuditorium
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            int id = 1;
            bool isActive = true;
            string name = "Main Auditorium";
            int rows = 20;
            int columns = 30;

            Auditorium auditorium = new Auditorium(id, isActive, name, rows, columns);

            Assert.Equal(id, auditorium.Id);
            Assert.Equal(isActive, auditorium.IsActive);
            Assert.Equal(name, auditorium.Name);
            Assert.Equal(rows, auditorium.NumberOfRows);
            Assert.Equal(columns, auditorium.NumberOfColumns);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            Auditorium auditorium = new Auditorium();

            auditorium.Id = 10;
            auditorium.IsActive = false;
            auditorium.Name = "Updated Auditorium";
            auditorium.NumberOfRows = 25;
            auditorium.NumberOfColumns = 35;

            Assert.Equal(10, auditorium.Id);
            Assert.False(auditorium.IsActive);
            Assert.Equal("Updated Auditorium", auditorium.Name);
            Assert.Equal(25, auditorium.NumberOfRows);
            Assert.Equal(35, auditorium.NumberOfColumns);
        }
    }
}
