namespace TestGuichetAutonome
{
    public class TestTicket
    {
        [Fact]
        public void Constructor_Default_ShouldInitializePropertiesToDefaults()
        {
            var ticket = new Ticket();

            Assert.Equal(default(int), ticket.Id);
            Assert.True(ticket.IsActive);
            Assert.Empty(ticket.ReservationNumber);
            Assert.Equal(default(TicketStatus), ticket.TicketStatus);
            Assert.Null(ticket.Representation);
            Assert.Null(ticket.Seat);
            Assert.Null(ticket.Order);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCorrectlyAssignProperties()
        {
            var representation = new Representation { Id = 1, Date = DateTime.Now };
            var seat = new Seat { Id = 1, SeatNumber = 10 };
            var order = new Order { Id = 1 };
            var reservationNumber = "R123456789";
            var ticketStatus = TicketStatus.Available;

            var ticket = new Ticket(1, true, reservationNumber, ticketStatus, representation, seat, order);

            Assert.Equal(1, ticket.Id);
            Assert.True(ticket.IsActive);
            Assert.Equal(reservationNumber, ticket.ReservationNumber);
            Assert.Equal(ticketStatus, ticket.TicketStatus);
            Assert.Same(representation, ticket.Representation);
            Assert.Same(seat, ticket.Seat);
            Assert.Same(order, ticket.Order);
        }

        [Fact]
        public void QRCodeData_ShouldGenerateCorrectly()
        {
            var representation = new Representation { Id = 1, Date = new DateTime(2023, 4, 15, 20, 0, 0) };
            var seat = new Seat { Id = 1, SeatNumber = 10, RowName = "A" };
            var reservationNumber = "R123456789";
            var ticket = new Ticket { ReservationNumber = reservationNumber, Representation = representation, Seat = seat };

            var qrCodeData = ticket.QRCodeData;

            var expectedQRCodeData = $"R123456789 ; {representation.Date} ; A ; 10";

            Assert.Equal(expectedQRCodeData, qrCodeData);
        }
    }
}
