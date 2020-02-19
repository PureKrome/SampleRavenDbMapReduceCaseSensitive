namespace SampleRavenDbMapReduceCaseSensitive
{
    public class Order
    {

        public Order(string orderName, string userId)
        {
            OrderName = orderName;
            UserId = userId;
        }

        public string Id { get; set; }
        public string OrderName { get; set; }

        /// <summary>
        /// User who made the order
        /// </summary>
        public string UserId { get; set; }
    }
}
