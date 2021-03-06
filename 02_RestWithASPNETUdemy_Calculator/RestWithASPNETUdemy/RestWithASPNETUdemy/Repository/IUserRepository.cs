using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNETUdemy.Repository
{
  public interface IUserRepository
  {
    User ValidaCredentials(UserVO user);
    User ValidaCredentials(string userName);
    User RefreshUserInfo(User user);
    bool RevokeToken(string userName);
  }
}
