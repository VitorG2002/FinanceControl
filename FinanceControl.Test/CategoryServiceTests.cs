using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Application.Services;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using Moq;

namespace FinanceControl.Test
{
    public class CategoryServiceTests
    {
        private readonly Mock<IRepository<Category>> _mockRepo;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepo = new Mock<IRepository<Category>>();
            _service = new CategoryService(_mockRepo.Object);
        }

        //[Fact]
        //public async Task AddAsync_ShouldReturnCategory()
        //{
        //    var categoryDto = new CategoryCreateDto { Name = "Test" };
        //    var category = new Category { Name = "Test" };

        //    _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(category);

        //    var result = await _service.AddAsync(categoryDto);

        //    Assert.NotNull(result);
        //    Assert.Equal("Test", result.Name);
        //}
    }
}
