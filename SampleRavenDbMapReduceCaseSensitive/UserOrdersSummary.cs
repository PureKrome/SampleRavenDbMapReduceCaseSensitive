using Raven.Client.Documents.Indexes;
using System.Linq;

namespace SampleRavenDbMapReduceCaseSensitive
{
    public class UserOrdersSummary : AbstractMultiMapIndexCreationTask<UserOrdersSummary.UserOrderSummaryResult>
    {
        public class UserOrderSummaryResult
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public int OrdersCount { get; set; }
        }

        public UserOrdersSummary()
        {
            AddMap<User>(users => from user in users
                                  select new UserOrderSummaryResult
                                  {
                                      UserId = user.Id,
                                      UserName = user.Name,
                                      OrdersCount = 0
                                  });

            AddMap<Order>(orders => from order in orders
                                    select new UserOrderSummaryResult
                                    {
                                        UserId = order.UserId,
                                        UserName = null,
                                        OrdersCount = 1
                                    });

            Reduce = results => from result in results
                                group result by result.UserId
                                into g
                                select new UserOrderSummaryResult
                                {
                                    UserId = g.Key,
                                    UserName = g.Select(x => x.UserName).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),
                                    OrdersCount = g.Sum(x => x.OrdersCount)
                                };
        }


    }

    
}
