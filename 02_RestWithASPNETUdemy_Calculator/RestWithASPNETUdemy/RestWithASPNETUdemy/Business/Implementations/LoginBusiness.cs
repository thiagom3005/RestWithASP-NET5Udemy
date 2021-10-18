using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Token.Configurations;
using RestWithASPNETUdemy.Token.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestWithASPNETUdemy.Business.Implementations
{
  public class LoginBusiness : ILoginBusiness
  {
    private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    private TokenConfigurations _configuration;
    private IUserRepository _repository;
    private readonly ITokenService _tokenService;

    public LoginBusiness(TokenConfigurations configuration, IUserRepository repository, ITokenService tokenService)
    {
      _configuration = configuration;
      _repository = repository;
      _tokenService = tokenService;
    }

    public TokenVO ValidateCredentials(UserVO user)
    {
      var userValidated = _repository.ValidaCredentials(user);
      if (userValidated == null) return null;

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new Claim(JwtRegisteredClaimNames.UniqueName, userValidated.UserName)
      };

      var accessToken = _tokenService.GenerateAccessToken(claims);
      var refreshToken = _tokenService.GenerateRefreshToken();

      userValidated.RefreshToken = refreshToken;
      userValidated.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

      _repository.RefreshUserInfo(userValidated);

      DateTime createDate = DateTime.Now;
      DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

      return new TokenVO(
        authenticated: true,
        created: createDate.ToString(DATE_FORMAT),
        expiration: expirationDate.ToString(DATE_FORMAT),
        accessToken: accessToken,
        refreshToken: refreshToken);
    }
  }
}
