using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Application.Services;
using Discount.Domain.Entities;
using Discount.Domain.Enums;
using Discount.Domain.Models;
using Discount.Infrastructure.Contracts;
using Discount.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Discount.Infrastructure.Tests.Services
{
    public class DiscountServiceTests
    {
        [Fact]
        public async Task GenerateInvoiceWithDiscount_ValidBill_ShouldCalculateInvoiceWithDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data
            var customer = new Customer
            {
                Id = 1,
                Name = "John Doe",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-1)
            };

            // Segment data
            var segment = new Segment
            {
                Type = "Employee",
                DiscountRate = 30
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 1,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 50 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 100 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(segment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * segment.DiscountRate / 100), result.Discount);
            Assert.Equal((decimal)Math.Floor((double)(bill.Products.Sum(p => p.Price) - result.Discount) / 100) * 5, result.ExtraDiscount);
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithoutLoyalty_ShouldCalculateInvoiceWithoutLoyaltyDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data without a loyalty discount
            var customer = new Customer
            {
                Id = 2,
                Name = "Jane Smith",
                SegmentType = "Regular", // Example segment type for a regular customer
                CreatedDate = DateTime.UtcNow.AddYears(-2) // Customer created two years ago
            };

            // Regular segment data
            var regularSegment = new Segment
            {
                Type = "Regular",
                DiscountRate = 8 // Example discount rate for regular customers
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 2,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 70 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 90 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(regularSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * regularSegment.DiscountRate / 100), result.Discount);
            Assert.Equal(0, result.ExtraDiscount); // Customer without loyalty status does not get loyalty discount
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_EmptyBill_ShouldCalculateInvoiceWithNoDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data
            var customer = new Customer
            {
                Id = 3,
                Name = "Empty Bill Customer",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Segment data
            var segment = new Segment
            {
                Type = "Employee",
                DiscountRate = 15
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data with no products
            var bill = new Bill
            {
                CustomerId = 3,
                Products = new List<Product>() // Empty product list
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(segment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Empty(result.Products); // No products in the invoice
            Assert.Equal(0, result.Amount); // No products, so the amount should be zero
            Assert.Equal(0, result.Discount); // No products, so no discount
            Assert.Equal(0, result.ExtraDiscount); // No products, so no extra discount
            Assert.Equal(0, result.TotalAmount); // No products, so the total amount should be zero
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithUnknownSegment_ShouldUseDefaultSegment()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with an unknown segment type
            var customer = new Customer
            {
                Id = 4,
                Name = "Unknown Customer",
                SegmentType = "Unknown", // Unknown segment type
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Default segment data
            var defaultSegment = new Segment
            {
                Type = "Default",
                DiscountRate = 10 // Example discount rate for the default segment
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty", // Example loyalty segment type
                DiscountRate = 5 // Example loyalty discount rate
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 4,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 60 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 140 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(defaultSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * defaultSegment.DiscountRate / 100), result.Discount);
            Assert.Equal((decimal)Math.Floor((double)(bill.Products.Sum(p => p.Price) - result.Discount) / 100) * 5, result.ExtraDiscount);
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_EmployeeCustomerWithLoyaltyDiscount_ShouldCalculateInvoiceWithLoyaltyDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with Employee segment type and loyalty status
            var customer = new Customer
            {
                Id = 5,
                Name = "Employee Customer",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Employee segment data
            var EmployeeSegment = new Segment
            {
                Type = "Employee",
                DiscountRate = 20 // Example discount rate for Employee customers
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 10 // Example loyalty discount rate
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 5,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 80 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 120 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(EmployeeSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * EmployeeSegment.DiscountRate / 100), result.Discount);
            Assert.Equal((decimal)Math.Floor((double)(bill.Products.Sum(p => p.Price) - result.Discount) / 100) * 5, result.ExtraDiscount);
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithNoSegment_ShouldCalculateInvoiceWithoutDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with no segment type
            var customer = new Customer
            {
                Id = 6,
                Name = "No Segment Customer",
                SegmentType = null, // No segment type
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Default segment data
            var defaultSegment = new Segment
            {
                Type = "Default",
                DiscountRate = 10 // Example discount rate for the default segment
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 6,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 60 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 140 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(defaultSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(0, result.Discount); // No segment, so no discount
            Assert.Equal(0, result.ExtraDiscount); // No segment, so no extra discount
            Assert.Equal(bill.Products.Sum(p => p.Price), result.TotalAmount); // No segment, so the total amount should be the same as the amount
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithExpiredLoyalty_ShouldCalculateInvoiceWithoutLoyaltyDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with Employee segment type but expired loyalty status
            var customer = new Customer
            {
                Id = 7,
                Name = "Expired Loyalty Customer",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Employee segment data
            var EmployeeSegment = new Segment
            {
                Type = "Employee",
                DiscountRate = 20 // Example discount rate for Employee customers
            };

            // Expired loyalty discount data (expired two years ago)
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 7,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 80 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 120 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(EmployeeSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * EmployeeSegment.DiscountRate / 100), result.Discount);
            Assert.Equal((decimal)Math.Floor((double)(bill.Products.Sum(p => p.Price) - result.Discount) / 100) * 5, result.ExtraDiscount);
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithExpiredSegment_ShouldUseDefaultSegment()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with an expired Employee segment
            var customer = new Customer
            {
                Id = 8,
                Name = "Expired Segment Customer",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Expired Employee segment data (expired two years ago)
            var expiredEmployeeSegment = new Segment
            {
                Type = "Employee",
                DiscountRate = 20
            };

            // Default segment data
            var defaultSegment = new Segment
            {
                Type = "Default",
                DiscountRate = 10 // Example discount rate for the default segment
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data
            var bill = new Bill
            {
                CustomerId = 8,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 80 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 120 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(expiredEmployeeSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * defaultSegment.DiscountRate / 100), result.Discount);
            Assert.Equal((decimal)Math.Floor((double)(bill.Products.Sum(p => p.Price) - result.Discount) / 100) * 5, result.ExtraDiscount);
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

        [Fact]
        public async Task GenerateInvoiceWithDiscount_CustomerWithHighTotalAmount_ShouldApplyExtraDiscount()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockSegmentRepository = new Mock<ISegmentRepository>();

            // Customer data with Employee segment type
            var customer = new Customer
            {
                Id = 9,
                Name = "High Amount Customer",
                SegmentType = "Employee",
                CreatedDate = DateTime.UtcNow.AddYears(-3)
            };

            // Employee segment data
            var EmployeeSegment = new Segment
            {
                Type = "Employee",
                DiscountRate = 20 // Example discount rate for Employee customers
            };

            // Loyalty discount data
            var loyaltyDiscount = new Segment
            {
                Type = "Loyalty",
                DiscountRate = 5
            };

            // Bill data with high total amount
            var bill = new Bill
            {
                CustomerId = 9,
                Products = new List<Product>
                {
                    new Product { Category = ProductCategoryEnum.Groceries, Price = 200 }, // Groceries product
                    new Product { Category = ProductCategoryEnum.Electronics, Price = 300 }, // Non-groceries product
                    new Product { Category = ProductCategoryEnum.Clothing, Price = 400 } // Non-groceries product
                }
            };

            // Set up the mock repository behavior
            mockCustomerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(customer);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(It.IsAny<string>()))
                                 .ReturnsAsync(EmployeeSegment);

            mockSegmentRepository.Setup(repo => repo.GetSegmentByType(SegmentTypesEnum.Loyalty.ToString()))
                                 .ReturnsAsync(loyaltyDiscount);

            // Create the DiscountService with the mocked dependencies
            var discountService = new DiscountService(mockCustomerRepository.Object, mockSegmentRepository.Object);

            // Act
            var result = await discountService.GenerateInvoiceWithDiscount(bill);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Name, result.CustomerName);
            Assert.Equal(customer.Id, result.CustomerId);
            Assert.Equal(bill.Products.Count, result.Products.Count);
            Assert.Equal(bill.Products.Sum(p => p.Price), result.Amount);
            Assert.Equal(bill.Products.Sum(p => p.Price * EmployeeSegment.DiscountRate / 100), result.Discount);
            Assert.Equal(2 * 5, result.ExtraDiscount); // Two extra discounts of $5 each
            Assert.Equal(bill.Products.Sum(p => p.Price) - result.Discount - result.ExtraDiscount, result.TotalAmount);
        }

    }
}
