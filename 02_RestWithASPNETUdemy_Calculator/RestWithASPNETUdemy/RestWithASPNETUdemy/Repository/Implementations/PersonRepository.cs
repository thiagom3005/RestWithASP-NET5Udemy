using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNETUdemy.Repository.Implementations
{
  public class PersonRepository : IPersonRepository
  {
    private MySQLContext _context;

    public PersonRepository(MySQLContext context)
    {
      _context = context;
    }

    public List<Person> FindAll()
    {
      return _context.Persons.ToList();
    }

    public Person FindById(long id)
    {
      return _context.Persons.SingleOrDefault(p => p.Id.Equals(id));
    }

    public Person Create(Person person)
    {
      try
      {
        _context.Persons.Add(person);
        _context.SaveChanges();
      }
      catch (Exception)
      {
        throw;
      }
      return person;
    }

    public Person Update(Person person)
    {
      if (!NotExists(person.Id)) return null;

      var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(person.Id));
      if (result != null)
      {
        try
        {
          _context.Entry(result).CurrentValues.SetValues(person);
          _context.SaveChanges();
        }
        catch (Exception)
        {
          throw;
        }
      }

      return person;
    }

    public void Delete(long id)
    {
      var result = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));
      if (result != null)
      {
        try
        {
          _context.Persons.Remove(result);
          _context.SaveChanges();
        }
        catch (Exception)
        {
          throw;
        }
      }
    }

    public bool NotExists(object id)
    {
      return _context.Persons.Any(p => p.Id.Equals(id));
    }
  }
}
