namespace Iris.Controllers
{
    using Businnes.Interfaces;
    using Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoBusiness _toDo;
        private readonly IConfiguration _configuration;

        public ToDoController(IToDoBusiness toDo, IConfiguration configuration)
        {
            _toDo = toDo;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<TaskToDo>> Get()
        {
            try
            {
                var tasks = _toDo.Get();
                if (tasks == null || tasks.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<bool> Insertar(TaskToDo toDo)
        {
            try
            {
                if (toDo == null)
                {
                    return BadRequest();
                }
                var inserted = _toDo.Insert(toDo);
                if (!inserted)
                {
                    return StatusCode(500, "No se pudo insertar");
                }
                return CreatedAtAction(nameof(Get), toDo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut]
        public ActionResult<bool> Actualizar(TaskToDo toDo)
        {
            try
            {
                if (toDo == null)
                {
                    return BadRequest();
                }
                var updated = _toDo.Update(toDo);
                if (!updated)
                {
                    return NotFound();
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var deleted = _toDo.Delete(id);
                if (!deleted)
                {
                    return NotFound(); 
                }
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        private string GenerateJWT(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("token")]
        public IActionResult GetToken(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }
            var token = GenerateJWT(username);
            return Ok(new { Token = token });
        }
    }
}
