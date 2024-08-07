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

namespace CareerHub.Business.Services.Concrete
{
    public class CompanyService: ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IMapper mapper
            , ICompanyRepository companyRepository
            , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResult<CompanyRequestModel>> AddCompanyAsync(CompanyRequestModel companyRequestModel)
        {
            try
            {
                #region Model Validate Process
                var modelValidator = new CompanyRequestParameterValidator();
                var modelValidatorResponse = await modelValidator.ValidateAsync(companyRequestModel);
                if (!modelValidatorResponse.IsValid)
                {
                    var errorMessages = new StringBuilder();
                    modelValidatorResponse.Errors.ForEach(x => errorMessages.AppendLine(x.ErrorMessage));
                    return new ApiResult<CompanyRequestModel>()
                    {
                        HttpStatusCode = HttpStatusCode.NotAcceptable,
                        IsSuccess = false,
                        Message = $"{MessageConstant.INVALID_VALIDATION}: {errorMessages.ToString()}",
                        Data = companyRequestModel
                    };
                }

                #endregion

                #region Check Phone Number

                if ((await _companyRepository.GetByPhoneNumberAsync(companyRequestModel.PhoneNumber)) != null)
                {
                    return new ApiResult<CompanyRequestModel>()
                    {
                        HttpStatusCode = HttpStatusCode.NotAcceptable,
                        IsSuccess = false,
                        Message = MessageConstant.EXIST_PHONENUMBER,
                        Data = companyRequestModel
                    };
                }
                #endregion

                #region Add Data & Mapping
                companyRequestModel.RemainingJobPostingRights = 2;
                var mapper = _mapper.Map<Company>(companyRequestModel);
                await _companyRepository.AddAsync(mapper);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResult<CompanyRequestModel>()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = MessageConstant.SUCCESSFUL_PROCESS,
                    Data = companyRequestModel
                };

                #endregion
            }
            catch (Exception ex)
            {
                return new ApiResult<CompanyRequestModel>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = $"{MessageConstant.UNSUCCESSFUL_PROCESS}: {ex.Message}",
                    Data = companyRequestModel
                };
            }
        }

    }
}
