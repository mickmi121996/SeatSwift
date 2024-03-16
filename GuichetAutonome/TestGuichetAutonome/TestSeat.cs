namespace TestGuichetAutonome
{
    public class TestSeat
    {
        [Fact]
        public void Constructor_Default_ShouldInitializePropertiesToDefaults()
        {
            // Arrange & Act
            var seat = new Seat();

            // Assert
            Assert.Equal(default(int), seat.Id);
            Assert.Equal(default(int), seat.SeatNumber);
            Assert.Equal(default(SeatStatus), seat.Status);
            Assert.Equal(default(SectionName), seat.SectionName);
            Assert.Equal(default(int), seat.XCoordinate);
            Assert.Equal(default(int), seat.YCoordinate);
            Assert.Empty(seat.RowName);
            Assert.Null(seat.Auditorium);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCorrectlyAssignProperties()
        {
            var auditorium = new Auditorium { Id = 1, Name = "Main Auditorium" };
            var seatNumber = 10;
            var status = SeatStatus.InService;
            var sectionName = SectionName.Balcon;
            var xCoordinate = 5;
            var yCoordinate = 10;
            var rowName = "A";

            var seat = new Seat(1, seatNumber, status, auditorium, sectionName, rowName, xCoordinate, yCoordinate);

            Assert.Equal(1, seat.Id);
            Assert.Equal(seatNumber, seat.SeatNumber);
            Assert.Equal(status, seat.Status);
            Assert.Same(auditorium, seat.Auditorium);
            Assert.Equal(sectionName, seat.SectionName);
            Assert.Equal(xCoordinate, seat.XCoordinate);
            Assert.Equal(yCoordinate, seat.YCoordinate);
            Assert.Equal(rowName, seat.RowName);
        }

        [Fact]
        public void Properties_ShouldBeModifiable()
        {
            var seat = new Seat();

            seat.SeatNumber = 20;
            seat.Status = SeatStatus.OutOfService;
            seat.XCoordinate = 15;
            seat.YCoordinate = 20;
            seat.RowName = "B";
            seat.SectionName = SectionName.Parterre;

            Assert.Equal(20, seat.SeatNumber);
            Assert.Equal(SeatStatus.OutOfService, seat.Status);
            Assert.Equal(15, seat.XCoordinate);
            Assert.Equal(20, seat.YCoordinate);
            Assert.Equal("B", seat.RowName);
            Assert.Equal(SectionName.Parterre, seat.SectionName);
        }
    }
}
