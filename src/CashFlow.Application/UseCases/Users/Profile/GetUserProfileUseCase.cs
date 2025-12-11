using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;
using Microsoft.Extensions.Logging;

namespace CashFlow.Application.UseCases.Users.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {

        private readonly ILoggerUser _loggerUser;
        private readonly IMapper _mapper;

        public GetUserProfileUseCase(ILoggerUser loggerUser, IMapper mapper)
        {
            _loggerUser = loggerUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {
            var user = await _loggerUser.Get();

            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
