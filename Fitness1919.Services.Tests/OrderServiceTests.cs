using Fitness1919.Data;
using Fitness1919.Data.Models;
using Fitness1919.Services.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fitness1919.Tests.Services.Data
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public async Task All_ShouldReturnAllOrders()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: "AllOrdersDatabase")
                .Options;

            using (var context = new Fitness1919DbContext(options))
            {
                context.Orders.Add(new Order { Id = 1.ToString(), FullName = "John Doe", Address = "123 Main St", PhoneNumber = "+359123456789" });
                context.Orders.Add(new Order { Id = 2.ToString(), FullName = "Jane Smith", Address = "456 Elm St", PhoneNumber = "+359987654321" });
                context.SaveChanges();
            }

            using (var context = new Fitness1919DbContext(options))
            {
                var orderService = new OrderService(context);

                var result = await orderService.All();

                Assert.AreEqual(2, result.Count());
                Assert.IsTrue(result.Any(o => o.Name == "John Doe"));
                Assert.IsTrue(result.Any(o => o.Name == "Jane Smith"));
            }
        }

        [Test]
        public async Task My_ShouldReturnUserOrders()
        {
            var options = new DbContextOptionsBuilder<Fitness1919DbContext>()
                .UseInMemoryDatabase(databaseName: "MyOrdersDatabase")
                .Options;

            var customerId = Guid.NewGuid().ToString(); 

            using (var context = new Fitness1919DbContext(options))
            {
                context.Orders.Add(new Order { Id = 1.ToString(), UserId = new Guid(customerId), FullName = "John Doe", Address = "123 Main St", PhoneNumber = "+359123456789" });
                context.Orders.Add(new Order { Id = 2.ToString(), UserId = new Guid(customerId), FullName = "Jane Smith", Address = "456 Elm St", PhoneNumber = "+359987654321" });
                context.Orders.Add(new Order { Id = 3.ToString(), UserId = Guid.NewGuid(), FullName = "Another User", Address = "333 Hall St", PhoneNumber = "+359111111111" });
                context.SaveChanges();
            }

            using (var context = new Fitness1919DbContext(options))
            {
                var orderService = new OrderService(context);

                var result = await orderService.My(customerId);

                Assert.AreEqual(2, result.Count());
                Assert.IsTrue(result.All(f => f.Name == "John Doe" || f.Name == "Jane Smith"));
            }
        }
    }
}
