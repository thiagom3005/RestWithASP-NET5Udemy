using RestWithASPNETUdemy.HyperMedia;
using RestWithASPNETUdemy.HyperMedia.Abstract;
using System;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Data.VO
{
  public class BookVO : ISupportHyperMedia
  {
    public long Id { get; set; }  
    public string Author { get; set; }
    public DateTime LaunchDate { get; set; }
    public float Price { get; set; }
    public string Title { get; set; }
    public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
  }
}
