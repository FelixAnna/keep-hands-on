using EStore.Common.Entities;
using EStore.SharedServices.Orders.Repositories;
using EStore.SharedServices.Orders.Services;
using EStore.SharedServices.Products.Repositories;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.SharedServices.Tests.Orders
{
    public class OrdersServiceTests
    {
        OrdersService service;
        private IOrdersRepository orderRepository;
        private IProductRepository productRepository;

        [SetUp]
        public void Setup()
        {
            orderRepository = A.Fake<IOrdersRepository>();
            productRepository = A.Fake<IProductRepository>();
            service = new OrdersService(orderRepository, productRepository);
        }

        [Test]
        public void TestGetByIdAsyncWhenItemEmpty()
        {
            //Arrange
            var fakeOrder = new OrderEntity()
            {
                Items = new List<OrderItemEntity>()
            };

            A.CallTo(() => orderRepository.GetByIdAsync(A<int>.Ignored)).Returns(fakeOrder);

            var result = service.GetByIdAsync(-1).Result;

            A.CallTo(() => orderRepository.GetByIdAsync(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => productRepository.GetByIdsAsync(A<int[]>.Ignored)).MustNotHaveHappened();
            Assert.Multiple(() =>
            {
                Assert.That(!result.Products.Any(), Is.True);
                Assert.That(!result.Order.Items.Any(), Is.True);
            });
            Assert.Pass();
        }

        [Test]
        public void TestGetByIdAsyncWhenItemNotEmpty()
        {
            //Arrange
            var fakeOrder = new OrderEntity()
            {
                Items = new List<OrderItemEntity>()
                {
                    new OrderItemEntity() {}
                }
            };
            var fakeProducts = new List<ProductEntity>()
            {
                new ProductEntity(),
            };

            A.CallTo(() => orderRepository.GetByIdAsync(A<int>.Ignored)).Returns(fakeOrder);
            A.CallTo(() => productRepository.GetByIdsAsync(A<int[]>.Ignored)).Returns(fakeProducts);

            var result = service.GetByIdAsync(-1).Result;

            A.CallTo(() => orderRepository.GetByIdAsync(A<int>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => productRepository.GetByIdsAsync(A<int[]>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Multiple(() =>
            {
                Assert.That(result.Products.Any(), Is.True);
                Assert.That(result.Order.Items.Any(), Is.True);
            });
            Assert.Pass();
        }
    }
}
