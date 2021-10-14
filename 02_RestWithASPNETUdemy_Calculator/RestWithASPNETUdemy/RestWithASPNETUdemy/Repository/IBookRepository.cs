using RestWithASPNETUdemy.Model;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Repository
{
  public interface IBookRepository
  {
    Book Create(Book person);
    Book FindById(long id);
    List<Book> FindAll();
    Book Update(Book person);
    void Delete(long id);
    bool NotExists(long id);
  }
}
