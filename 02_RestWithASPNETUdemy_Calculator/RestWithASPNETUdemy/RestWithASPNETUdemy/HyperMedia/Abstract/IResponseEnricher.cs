using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace RestWithASPNETUdemy.HyperMedia.Abstract
{
  public interface IResponseEnricher
  {
    bool CanEnrich(ResultExecutingContext context);
    Task Enrich(ResultExecutingContext context);
  }
}
