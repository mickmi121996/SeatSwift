namespace TestProject
{
    public class EnumTest
    {
        [Fact]
        public void RepresentationStatus_Values_ShouldNotChange()
        {
            // Assert
            Assert.Equal(0, (int)RepresentationStatus.Available);
            Assert.Equal(1, (int)RepresentationStatus.Complete);
            Assert.Equal(2, (int)RepresentationStatus.Cancelled);
        }

        [Fact]
        public void EmployeeType_Values_ShouldNotChange()
        {
            Assert.Equal(0, (int)EmployeeType.Accountant);
            Assert.Equal(1, (int)EmployeeType.Administrator);
        }

        [Fact]
        public void SeatStatus_Values_ShouldNotChange()
        {
            Assert.Equal(0, (int)SeatStatus.InService);
            Assert.Equal(1, (int)SeatStatus.OutOfService);
        }

        [Fact]
        public void SectionName_Values_ShouldNotChange()
        {
            Assert.Equal(0, (int)SectionName.Balcon);
            Assert.Equal(1, (int)SectionName.Parterre);
            Assert.Equal(2, (int)SectionName.Loge);
        }

        [Fact]
        public void ShowType_Values_ShouldNotChange()
        {
            Assert.Equal(0, (int)ShowType.Movie);
            Assert.Equal(1, (int)ShowType.Theater);
            Assert.Equal(2, (int)ShowType.MusicalComedy);
            Assert.Equal(3, (int)ShowType.Concert);
            Assert.Equal(4, (int)ShowType.Humor);
            Assert.Equal(5, (int)ShowType.Dance);
            Assert.Equal(6, (int)ShowType.Conference);
            Assert.Equal(7, (int)ShowType.Variety);
        }

        [Fact]
        public void TicketStatus_Values_ShouldNotChange()
        {
            Assert.Equal(0, (int)TicketStatus.Available);
            Assert.Equal(1, (int)TicketStatus.Reserved);
            Assert.Equal(2, (int)TicketStatus.Purchased);
        }
    }
}
