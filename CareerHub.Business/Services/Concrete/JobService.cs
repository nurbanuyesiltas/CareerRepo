using AutoMapper;
using CareerHub.Business.Services.Abstract;
using CareerHub.Business.Validators;
using CareerHub.Core.Common.Concrete;
using CareerHub.Core.Constants;
using CareerHub.Core.Parameters;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.Entities.Entities;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CareerHub.Business.Services.Concrete
{
    public class JobService : IJobService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticsearchService _elasticsearchService;
        public JobService(IMapper mapper
            , ICompanyRepository companyRepository
            , IJobRepository jobRepository
            , IRedisCacheService redisCacheService
            , IUnitOfWork unitOfWork
            , IElasticsearchService elasticsearchService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _jobRepository = jobRepository;
            _redisCacheService = redisCacheService;
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
        }
        public async Task<ApiResult<JobRequestModel>> AddJobAsync(JobRequestModel jobRequestModel)
        {
            try
            {
                #region Model Validate Process
                var modelValidator = new JobRequestParameterValidator();
                var modelValidatorResponse = await modelValidator.ValidateAsync(jobRequestModel);
                if (!modelValidatorResponse.IsValid)
                {
                    var errorMessages = string.Join(Environment.NewLine, modelValidatorResponse.Errors.Select(x => x.ErrorMessage));
                    return CreateErrorResult(HttpStatusCode.NotAcceptable, $"{MessageConstant.INVALID_VALIDATION}: {errorMessages}", jobRequestModel);
                }

                #endregion

                #region Remaining Job Posting Rights Check
                var getCompany = await _companyRepository.GetByIdAsync(jobRequestModel.CompanyId);
                if (getCompany.RemainingJobPostingRights < 1)
                {
                    return CreateErrorResult(HttpStatusCode.NotAcceptable, MessageConstant.REMAININGJOBPOSTINGRIGHTS_ERRORMESSAGE, jobRequestModel);
                }
                #endregion

                #region Update Company and Add Job & Mapping

                getCompany.RemainingJobPostingRights--;
                getCompany.ModifiedTime = DateTime.Now;
                _companyRepository.Update(getCompany);

                var mapper = _mapper.Map<Job>(jobRequestModel);
                mapper.PublicationDuration = DateTime.Now.AddDays(15);
                mapper.QualityScore = CalculateQualityScore(jobRequestModel);


                List<string> prohibitedWords = await InitializeProhibitedWordsAsync();

                if (!(ContainsProhibitedWords(prohibitedWords,jobRequestModel.Description, jobRequestModel.WorkingType, jobRequestModel.Benefits, jobRequestModel.Position)))
                {
                    mapper.QualityScore += 2;
                }

                await _jobRepository.AddAsync(mapper);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    await _elasticsearchService.IndexJobAsync(mapper);
                }
                // Return Success Result
                return new ApiResult<JobRequestModel>()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = MessageConstant.SUCCESSFUL_PROCESS,
                    Data = jobRequestModel
                };

            }

            #endregion

            catch (Exception ex)
            {
                return CreateErrorResult(HttpStatusCode.InternalServerError, $"{MessageConstant.UNSUCCESSFUL_PROCESS}: {ex.Message}", jobRequestModel);
            }

        }
        private ApiResult<JobRequestModel> CreateErrorResult(HttpStatusCode statusCode, string message, JobRequestModel jobRequestModel)
        {
            return new ApiResult<JobRequestModel>()
            {
                HttpStatusCode = statusCode,
                IsSuccess = false,
                Message = message,
                Data = jobRequestModel
            };
        }
        private int CalculateQualityScore(JobRequestModel jobRequestModel)
        {
            int score = 0;
            score += !string.IsNullOrEmpty(jobRequestModel.WorkingType) ? 1 : 0;
            score += !string.IsNullOrEmpty(jobRequestModel.Salary) ? 1 : 0;
            score += !string.IsNullOrEmpty(jobRequestModel.Benefits) ? 1 : 0;
            return score;
        }

        public async Task<ApiResult<IEnumerable<Job>>> SearchJobsByPublicationDurationAsync(JobSearchModel searchModel)
        {
            try
            {
                var jobs = await _elasticsearchService.SearchJobsAsync(searchModel.PublicationStart, searchModel.PublicationEnd);
                return new ApiResult<IEnumerable<Job>>()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = MessageConstant.SUCCESSFUL_PROCESS,
                    Data = _mapper.Map<IEnumerable<Job>>(jobs)
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<IEnumerable<Job>>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = $"{MessageConstant.UNSUCCESSFUL_PROCESS}: {ex.Message}",
                    Data = null
                };
            }

        }
        private async Task<List<string>> InitializeProhibitedWordsAsync()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "Resources", "prohibited_words.json");

            var prohibitedWordsJson = await _redisCacheService.GetValueAsync("ProhibitedWords");
            List<string> prohibitedWords = null;

            if (!string.IsNullOrEmpty(prohibitedWordsJson))
            {
                prohibitedWords = JsonSerializer.Deserialize<List<string>>(prohibitedWordsJson);
            }

            if (prohibitedWords == null || prohibitedWords.Count == 0)
            {
                if (File.Exists(jsonFilePath))
                {
                    var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                    prohibitedWords = JsonSerializer.Deserialize<List<string>>(jsonContent);
                    await _redisCacheService.SetValueAsync("ProhibitedWords", JsonSerializer.Serialize(prohibitedWords), TimeSpan.FromMinutes(15));
                }
                else
                {
                    prohibitedWords = new List<string>();
                }
            }

            return prohibitedWords;
        }

        private bool ContainsProhibitedWords(List<string> prohibitedWords,params string[] inputs)
        {           
            foreach (var input in inputs)
            {
                foreach (var word in prohibitedWords)
                {
                    if (input.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
