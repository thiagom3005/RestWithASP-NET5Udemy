using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
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

      var userUpdated = UpdateUser(userValidated, accessToken, refreshToken);
      return userUpdated; 
    }

    public TokenVO ValidateCredentials(TokenVO token)
    {
      var accessToken = token.AccessToken;
      var refreshToken = token.RefreshToken;
      var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
      var userName = principal.Identity.Name;
      var user = _repository.ValidaCredentials(userName);

      if (user == null ||
        user.RefreshToken != refreshToken ||
        user.RefreshTokenExpiryTime <= DateTime.Now) return null;

      accessToken = _tokenService.GenerateAccessToken(principal.Claims);
      refreshToken = _tokenService.GenerateRefreshToken();

      user.RefreshToken = refreshToken; 
      
      var userUpdated = UpdateUser(user, accessToken, refreshToken);
      return userUpdated;
    }

    public bool RevokeToken(string userName)
    {
      return _repository.RevokeToken(userName);
    }

    private TokenVO UpdateUser(User userValidated, string accessToken, string refreshToken)
    {
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
