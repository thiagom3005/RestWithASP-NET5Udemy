using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithASPNETUdemy.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CalculatorController : ControllerBase
  {
    
    private readonly ILogger<CalculatorController> _logger;
    public CalculatorController(ILogger<CalculatorController> logger)
    {
      _logger = logger;
    }

    [HttpGet("sum/{firstNumber}/{secondNumber}")]
    public IActionResult Sum(string firstNumber, string secondNumber)
    {
      if (IsNumeric(firstNumber) && IsNumeric(secondNumber)) 
      {
        var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);
        return Ok(sum.ToString());
      }

      return BadRequest("Invalid input");
    }

    [HttpGet("multiplication/{firstNumber}/{secondNumber}")]
    public IActionResult Multiplication(string firstNumber, string secondNumber)
    {
      if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
      {
        var product = ConvertToDecimal(firstNumber) * ConvertToDecimal(secondNumber);
        return Ok(product.ToString());
      }

      return BadRequest("Invalid input");
    }

    [HttpGet("division/{firstNumber}/{secondNumber}")]
    public IActionResult Division(string firstNumber, string secondNumber)
    {
      if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
      {
        var quotient = ConvertToDecimal(firstNumber) / ConvertToDecimal(secondNumber);
        return Ok(quotient.ToString());
      }

      return BadRequest("Invalid input");
    }

    [HttpGet("avarage/{firstNumber}/{secondNumber}")]
    public IActionResult Avarage(string firstNumber, string secondNumber)
    {
      if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
      {
        var avg = (ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber)) / 2;
        return Ok(avg.ToString());
      }

      return BadRequest("Invalid input");
    }

    [HttpGet("square-root/{firstNumber}")]
    public IActionResult SquareRoot(string firstNumber)
    {
      if (IsNumeric(firstNumber))
      {
        var squareRoot = Math.Sqrt((double)ConvertToDecimal(firstNumber));
        return Ok(squareRoot.ToString());
      }

      return BadRequest("Invalid input");
    }

    private bool IsNumeric(string strNumber)
    {
      double number;
      bool isNumber = double.TryParse(
        strNumber,
        System.Globalization.NumberStyles.Any,
        System.Globalization.NumberFormatInfo.InvariantInfo,
        out number);
      return isNumber;
    }

    private decimal ConvertToDecimal(string strNumber)
    {
      decimal decimalValue;

      if (decimal.TryParse(strNumber, out decimalValue))
        return decimalValue;

      return 0;
    }
  }
}
