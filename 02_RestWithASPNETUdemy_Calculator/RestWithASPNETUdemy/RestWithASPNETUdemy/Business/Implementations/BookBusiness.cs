using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithASPNETUdemy.Business.Implementations
{
  public class BookBusiness : IBookBusiness
  {
    private readonly IBookRepository _bookRepository;

    public BookBusiness(IBookRepository context)
    {
      _bookRepository = context;
    }

    public List<Book> FindAll()
    {
      return _bookRepository.FindAll();
    }

    public Book FindById(long id)
    {
      return _bookRepository.FindById(id);
    }

    public Book Create(Book book)
    {

      return _bookRepository.Create(book);
    }

    public Book Update(Book book)
    {

      return _bookRepository.Update(book);
    }

    public void Delete(long id)
    {
      _bookRepository.Delete(id);
    }
  }
}
