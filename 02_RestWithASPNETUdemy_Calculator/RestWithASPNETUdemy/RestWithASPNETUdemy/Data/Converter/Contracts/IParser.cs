using System.Collections.Generic;

namespace RestWithASPNETUdemy.Data.Converter.Contracts
{
  public interface IParser<O,D>
  {
    D Parse(O origin);
    List<D> Parse(List<O> origin);
  }
}
