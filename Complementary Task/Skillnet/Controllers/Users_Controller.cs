using Microsoft.AspNetCore.Mvc;
using SkillNet.Models.Entities;
using SkillNet.Repositories.Contracts;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUser_Repository _repository;
    public UsersController(IUser_Repository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Post(User user)
    {
        await _repository.CreateAsync(user);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}