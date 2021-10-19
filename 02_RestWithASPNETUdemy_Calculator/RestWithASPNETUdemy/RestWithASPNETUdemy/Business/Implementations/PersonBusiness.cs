using RestWithASPNETUdemy.Data.Converter.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.HyperMedia.Utils;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Repository;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business.Implementations
{
  public class PersonBusiness : IPersonBusiness
  {
    private readonly IPersonRepository _personRepository;
    private readonly PersonConverter _converter;

    public PersonBusiness(IPersonRepository context)
    {
      _personRepository = context;
      _converter = new PersonConverter();
    }

    public List<PersonVO> FindAll()
    {
      return _converter.Parse(_personRepository.FindAll());
    }

    public PersonVO FindById(long id)
    {
      return _converter.Parse(_personRepository.FindById(id));
    }
    public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
    {
      throw new System.NotImplementedException();
    }

    public List<PersonVO> FindByName(string firstName, string lastName)
    {
      return _converter.Parse(_personRepository.FindByName(firstName, lastName));
    }

    public PersonVO Create(PersonVO person)
    {
      var personEntity = _converter.Parse(person);
      personEntity = _personRepository.Create(personEntity);
      return _converter.Parse(personEntity);
    }

    public PersonVO Update(PersonVO person)
    {
      var personEntity = _converter.Parse(person);
      personEntity = _personRepository.Update(personEntity);
      return _converter.Parse(personEntity);
    }

    public PersonVO Disable(long id)
    {
      var personEntity = _personRepository.Disable(id);
      return _converter.Parse(personEntity);
    }

    public void Delete(long id)
    {
      _personRepository.Delete(id);
    }
  }
}
