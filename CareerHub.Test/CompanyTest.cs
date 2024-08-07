using AutoMapper;
using CareerHub.Business.Services.Abstract;
using CareerHub.Business.Services.Concrete;
using CareerHub.Business.Validators;
using CareerHub.Core.Constants;
using CareerHub.Core.Parameters;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.Entities.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace CareerHub.Test
{
    public class CompanyTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CompanyService _companyService;
        public CompanyTest()
        {
            _mapperMock = new Mock<IMapper>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _companyService = new CompanyService(
                _mapperMock.Object,
                _companyRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }
        [Fact]
        public async Task AddCompanyAsync_InvalidModel_ReturnsNotAcceptable()
        {
            // Arrange
            var companyRequestModel = new CompanyRequestModel
            {
                PhoneNumber = "abcd"
            };
            var validator = new CompanyRequestParameterValidator();
            var validationResult = await validator.ValidateAsync(companyRequestModel);

            // Act
            var result = await _companyService.AddCompanyAsync(companyRequestModel);

            // Assert
            result.HttpStatusCode.Should().Be(HttpStatusCode.NotAcceptable);
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain(MessageConstant.INVALID_VALIDATION);
        }

        [Fact]
        public async Task AddCompanyAsync_ValidModel_ReturnsOk()
        {
            // Arrange
            var companyRequestModel = new CompanyRequestModel
            {
                PhoneNumber = "05467425858",
                CompanyName = "Nurbanu Yeþiltaþ Yazýlým",
                Address = "Ýstanbul"
            };
            var company = new Company();

            _companyRepositoryMock.Setup(repo => repo.GetByPhoneNumberAsync(companyRequestModel.PhoneNumber))
                .ReturnsAsync((Company)null);
            _companyRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Company>())).Returns(Task.FromResult<Company>(null));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<Company>(companyRequestModel)).Returns(company);

            // Act
            var result = await _companyService.AddCompanyAsync(companyRequestModel);

            // Assert
            result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(MessageConstant.SUCCESSFUL_PROCESS);
        }

    }
}