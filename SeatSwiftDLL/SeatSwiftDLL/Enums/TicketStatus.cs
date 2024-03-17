namespace SeatSwiftDLL.Enums
{
    /// <summary>
    /// The status of a ticket
    /// </summary>
    /// <remarks>
    /// Available: The ticket is available for purchase
    /// Reserved: The ticket is reserved for purchase
    /// Purchased: The ticket has been purchased
    /// </remarks>
    public enum TicketStatus
    {
        Available = 0,
        Reserved = 1,
        Purchased = 2,
    }
}
