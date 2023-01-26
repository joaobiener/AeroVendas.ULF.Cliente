using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace AeroVendas.ULF.Cliente.HttpRepository
{
    public interface IAuthenticationService
	{
		Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto);
		Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
		Task Logout();
		Task<string> RefreshToken();
	}
}
