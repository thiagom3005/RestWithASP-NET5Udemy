using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNETUdemy.Business.Implementations
{
  public class PersonBusiness : IPersonBusiness
  {
    private readonly IPersonRepository _personRepository;

    public PersonBusiness(IPersonRepository context)
    {
      _personRepository = context;
    }

    public List<Person> FindAll()
    {
      return _personRepository.FindAll();
    }

    public Person FindById(long id)
    {
      return _personRepository.FindById(id);
    }

    public Person Create(Person person)
    {

      return _personRepository.Create(person);
    }

    public Person Update(Person person)
    {

      return _personRepository.Update(person);
    }

    public void Delete(long id)
    {
      _personRepository.Delete(id);
    }
  }
}
