using System.Collections.Generic;

namespace RestWithASPNETUdemy.HyperMedia.Abstract
{
  public interface ISupportHyperMedia
  {
    List<HyperMediaLink> Links { get; set; }
  }
}
