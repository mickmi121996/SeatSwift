namespace TestGuichetAutonome
{
    public class TestOrder
    {
        [Fact]
        public void Constructor_Default_ShouldInitializePropertiesToDefaults()
        {
            var order = new Order();

            Assert.Equal(default(int), order.Id);
            Assert.True(order.IsActive);
            Assert.Empty(order.OrderNumber);
            Assert.Equal(default(decimal), order.TotalPrice);
            Assert.Equal(default(decimal), order.TPS);
            Assert.Equal(default(decimal), order.TVQ);
            Assert.Equal(default(decimal), order.TotalAmountAfterTaxe);
            Assert.Null(order.Client);
        }

        [Fact]
        public void Constructor_WithParameters_ShouldCorrectlyAssignPropertiesAndCalculateTaxes()
        {
            // Arrange
            var client = new Client();
            var totalPrice = 100m;

            // Act
            var order = new Order(1, true, "ON123456", DateTime.Now, totalPrice, client);

            // Assert
            Assert.Equal(1, order.Id);
            Assert.True(order.IsActive);
            Assert.Equal("ON123456", order.OrderNumber);
            Assert.Equal(totalPrice, order.TotalPrice);
            Assert.Equal(5m, order.TPS);
            Assert.Equal(9.975m, order.TVQ);
            Assert.Equal(114.975m, order.TotalAmountAfterTaxe);
        }

        [Fact]
        public void TpsCalculation_ShouldCalculateCorrectly()
        {
            var order = new Order();
            var totalPrice = 200m;

            var tps = order.TpsCalculation(totalPrice);

            Assert.Equal(10m, tps);
        }

        [Fact]
        public void TvqCalculation_ShouldCalculateCorrectly()
        {
            var order = new Order();
            var totalPrice = 200m;

            var tvq = order.TvqCalculation(totalPrice);

            Assert.Equal(19.95m, tvq);
        }

        [Fact]
        public void TotalAmountAfterTaxeCalculation_ShouldCalculateCorrectly()
        {
            var order = new Order();
            var totalPrice = 100m;
            var tps = order.TpsCalculation(totalPrice);
            var tvq = order.TvqCalculation(totalPrice);

            var totalAfterTaxes = order.TotalAmountAfterTaxeCalculation(totalPrice, tps, tvq);

            Assert.Equal(114.975m, totalAfterTaxes);
        }
    }
}
