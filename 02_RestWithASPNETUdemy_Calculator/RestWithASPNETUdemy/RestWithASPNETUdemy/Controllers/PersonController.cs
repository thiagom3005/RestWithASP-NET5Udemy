using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Business;

namespace RestWithASPNETUdemy.Controllers
{
  [ApiVersion("1")]
  [ApiController]
  [Route("api/[controller]/v{version:apiVersion}")]
  public class PersonController : ControllerBase
  {
    private readonly ILogger<PersonController> _logger;
    private IPersonBusiness _persoBusiness;

    public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
    {
      _logger = logger;
      _persoBusiness = personBusiness;
    }

    [HttpGet]
    public IActionResult Get()
    {
      return Ok(_persoBusiness.FindAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
      var person = _persoBusiness.FindById(id);

      if (person == null) return NotFound();

      return Ok(person);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Person person)
    {
      if (person == null) return BadRequest();

      return Ok(_persoBusiness.Create(person));
    }

    [HttpPut]
    public IActionResult Put([FromBody] Person person)
    {
      if (person == null) return BadRequest();

      return Ok(_persoBusiness.Update(person));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
      _persoBusiness.Delete(id);

      return NoContent();
    }
  }
}