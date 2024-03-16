namespace TestProject
{
    public class TestRepresentation
    {
        [Fact]
        public void Constructor_Default_ShouldInitializePropertiesToDefaults()
        {
            var representation = new Representation();

            Assert.Equal(default(int), representation.Id);
            Assert.True(representation.IsActive);
            Assert.Equal(default(RepresentationStatus), representation.Status);
            Assert.True((DateTime.Now - representation.Date).TotalSeconds < 1);
            Assert.Null(representation.Show);
            Assert.Null(representation.Auditorium);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCorrectlyAssignProperties()
        {
            var show = new Show { Id = 1, Name = "Show Name" };
            var auditorium = new Auditorium { Id = 1, Name = "Auditorium Name" };
            var date = DateTime.Now.AddDays(5);
            var status = RepresentationStatus.Available;

            var representation = new Representation(1, true, date, status, show, auditorium);

            Assert.Equal(1, representation.Id);
            Assert.True(representation.IsActive);
            Assert.Equal(date, representation.Date);
            Assert.Equal(status, representation.Status);
            Assert.Same(show, representation.Show);
            Assert.Same(auditorium, representation.Auditorium);
        }

        [Fact]
        public void Properties_ShouldBeModifiable()
        {
            var representation = new Representation();

            var newDate = DateTime.Now.AddDays(10);
            representation.Date = newDate;
            representation.Status = RepresentationStatus.Cancelled;

            Assert.Equal(newDate, representation.Date);
            Assert.Equal(RepresentationStatus.Cancelled, representation.Status);
        }
    }
}
