using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using postwebAPI.Model;

namespace postwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpPost("PostResult")]
        public IActionResult PostResult( Request req)
        {
            if (req == null || req.Operand == null || req.Numbers == null || req.Numbers.Count == 0)
            {
                return BadRequest("Please provide correct input .Input field is not valid");
            }

            var operand = req.Operand;
            var numbers = req.Numbers;

            string[] validinput = { "+", "-", "*", "/" };

            if (!validinput.Contains(operand))
            {
                return BadRequest($"Input filed is not validf validinput '{operand}'. Valid operands are: {string.Join(", ", validinput)}.");
            }
            double result = 0;

            try
            {
               

                if (operand == "+")
                {
                    result = numbers.Sum();
                }
                else if (operand == "-")
                {
                    result = numbers.First() - numbers.Skip(1).Sum();
                }
                else if (operand == "*")
                {
                    result = numbers.Aggregate((a, b) => a * b);
                }
                else if (operand == "/")
                {
                    result = numbers.First();

                    foreach (var number in numbers.Skip(1))
                    {
                        if (number == 0)
                        {
                            return BadRequest(new
                            { Error = "DivisionByZero", Message = "Cannot divide by zero"
                            });
                        }

                        result /= number;
                    }
                }
                else
                {
                    return BadRequest(new
                    { Error = "InvalidOperand", Message = $"Invalid operand '" +
                    $"{operand}'. Valid operands are:" +
                    $" {string.Join(", ", validinput)}." 
                    });
                }
            }
            catch
            {
                return BadRequest(new
                { Error = "operationalError", Message = "Problem during performing task"
                });
            }

            return Ok(new { result });
        }

    }
}
