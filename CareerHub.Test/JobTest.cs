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
    public class JobTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IElasticsearchService> _elasticsearchServiceMock;
        private readonly JobService _jobService;
        public JobTest()
        {
            _mapperMock = new Mock<IMapper>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _jobRepositoryMock = new Mock<IJobRepository>();
            _redisCacheServiceMock = new Mock<IRedisCacheService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _elasticsearchServiceMock = new Mock<IElasticsearchService>();

            _jobService = new JobService(
                _mapperMock.Object,
                _companyRepositoryMock.Object,
                _jobRepositoryMock.Object,
                _redisCacheServiceMock.Object,
                _unitOfWorkMock.Object,
                _elasticsearchServiceMock.Object
            );
        }
        [Fact]
        public async Task AddJobAsync_InvalidModel_ReturnsNotAcceptable()
        {
            // Arrange
            var jobRequestModel = new JobRequestModel
            {
                CompanyId = 1,
                Benefits="",
                Description="",
                Position="",
                Salary="",
                WorkingType=""
            };
            var validator = new JobRequestParameterValidator();
            var validationResult = await validator.ValidateAsync(jobRequestModel);

            var company = new Company
            {
                RemainingJobPostingRights = 2
            };
            var job = new Job();

            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequestModel.CompanyId))
      .ReturnsAsync(company);
            _jobRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Job>())).Returns(Task.FromResult<Job>(null));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<Job>(jobRequestModel)).Returns(job);
            _redisCacheServiceMock.Setup(redis => redis.GetValueAsync("ProhibitedWords")).ReturnsAsync("");
            _elasticsearchServiceMock.Setup(es => es.IndexJobAsync(It.IsAny<Job>())).Returns(Task.CompletedTask);

            // Act
            var result = await _jobService.AddJobAsync(jobRequestModel);

            // Assert
            result.HttpStatusCode.Should().Be(HttpStatusCode.NotAcceptable);
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain(MessageConstant.INVALID_VALIDATION);
        }
        [Fact]
        public async Task AddJobAsync_ValidModel_ReturnsOk()
        {
            // Arrange
            var jobRequestModel = new JobRequestModel
            {
                CompanyId = 5,
                Benefits = "test",
                Description = "test",
                Position = "test",
                Salary = "100",
                WorkingType = "test"
            };
            var company = new Company
            {
                RemainingJobPostingRights = 2
            };
            var job = new Job();

            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(jobRequestModel.CompanyId))
                .ReturnsAsync(company);
            _jobRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Job>())).Returns(Task.FromResult<Job>(null));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<Job>(jobRequestModel)).Returns(job);
            _redisCacheServiceMock.Setup(redis => redis.GetValueAsync("ProhibitedWords")).ReturnsAsync("");
            _elasticsearchServiceMock.Setup(es => es.IndexJobAsync(It.IsAny<Job>())).Returns(Task.CompletedTask);

            // Act
            var result = await _jobService.AddJobAsync(jobRequestModel);

            // Assert
            result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(MessageConstant.SUCCESSFUL_PROCESS);
        }

        [Fact]
        public async Task SearchJobsByPublicationDurationAsync_ValidModel_ReturnsOk()
        {
            // Arrange
            var searchModel = new JobSearchModel
            {
                PublicationStart = DateTime.Now,
                PublicationEnd = DateTime.Now.AddMonths(1)
            };
            var jobs = new List<Job>
    {
        new Job { JobId = 11, Position= "Yazýlým Uzmaný", CompanyId = 1, Description = "Yazýlým geliþtirme", Salary = "6000" },
        new Job { JobId = 12, Position = "Sistem Uzmaný", CompanyId = 1, Description = "Sistem", Salary = "6000" }
    };

            _elasticsearchServiceMock.Setup(es => es.SearchJobsAsync(searchModel.PublicationStart, searchModel.PublicationEnd))
                .ReturnsAsync(jobs);
            _mapperMock.Setup(m => m.Map<IEnumerable<Job>>(jobs)).Returns(jobs);

            // Act
            var result = await _jobService.SearchJobsByPublicationDurationAsync(searchModel);

            // Assert
            result.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(MessageConstant.SUCCESSFUL_PROCESS);
            result.Data.Should().BeEquivalentTo(jobs);
        }
    }
}
