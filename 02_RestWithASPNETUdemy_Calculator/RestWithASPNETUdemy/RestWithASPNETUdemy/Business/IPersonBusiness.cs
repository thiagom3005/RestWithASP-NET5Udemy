﻿using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.HyperMedia.Utils;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business
{
  public interface IPersonBusiness
  {
    PersonVO Create(PersonVO person);
    PersonVO FindById(long id);
    List<PersonVO> FindByName(string firstName, string lastName);
    List<PersonVO> FindAll();
    PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    PersonVO Update(PersonVO person);
    PersonVO Disable(long id);
    void Delete(long id);
  }
}
