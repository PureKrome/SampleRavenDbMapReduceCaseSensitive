using Raven.Client.Documents;
using Raven.TestDriver;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace SampleRavenDbMapReduceCaseSensitive
{
    public class MapReduceCaseSensitiveTest : RavenTestDriver
    {
        [Fact]
        public void GivenSomeDocumentsWithDifferentIdCasings_GetAll_ReturnsDuplicates()
        {
            using (var store = GetDocumentStore())
            {
                store.ExecuteIndex(new UserOrdersSummary());

                SeedData(store);

                WaitForIndexing(store);

                // Check out the results in the INDEX.
                WaitForUserToContinueTheTest(store);

                using (var session = store.OpenSession())
                {
                    var results = session.Query<UserOrdersSummary.UserOrderSummaryResult, UserOrdersSummary>()
                                         .ToList();

                    // I should expect to have only 10 users.
                    results.Count.ShouldBe(10);
                }
            }
        }

        private void SeedData(IDocumentStore store)
        {
            var users = new[]
            {
                new User("Jane"),
                new User("Jill"),
                new User("Alice"),
                new User("Lily"),
                new User("Anabel"),
                new User("Ada"),
                new User("Grace"),
                new User("Margret"),
                new User("Anna"),
                new User("Jade")
            };

            var orders = new[]
            {
                new Order("First order", "users/1-A"),
                new Order("Second order", "Users/1-A"),
                new Order("Third order", "users/1-A"),
                new Order("Forth order", "users/2-A"),
                new Order("Fifth order", "Users/2-A"),
                new Order("AAA order", "Users/2-A"),
                new Order("BBBB order", "Users/3-A"),
                new Order("CCCC order", "Users/4-A"),
                new Order("DdDdDd order", "Users/5-A"),
            };

            using (var session = store.OpenSession())
            {
                foreach(var user in users)
                {
                    session.Store(user);
                }

                foreach(var order in orders)
                {
                    session.Store(order);
                }

                session.SaveChanges();
            }

        }
    }
}
