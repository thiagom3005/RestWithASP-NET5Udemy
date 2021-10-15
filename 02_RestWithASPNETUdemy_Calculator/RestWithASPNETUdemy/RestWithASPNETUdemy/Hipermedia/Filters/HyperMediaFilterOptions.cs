using RestWithASPNETUdemy.Hipermedia.Abstract;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Hipermedia.Filters
{
  public class HyperMediaFilterOptions
  {
    public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
  }
}
