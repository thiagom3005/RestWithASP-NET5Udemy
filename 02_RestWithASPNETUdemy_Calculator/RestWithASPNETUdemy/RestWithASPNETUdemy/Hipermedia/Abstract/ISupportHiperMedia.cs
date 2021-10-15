using System.Collections.Generic;

namespace RestWithASPNETUdemy.Hipermedia.Abstract
{
  public interface ISupportHiperMedia
  {
    List<HyperMediaLink> Links { get; set; }
  }
}
