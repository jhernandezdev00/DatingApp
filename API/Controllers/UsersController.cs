using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController{
    private readonly IUserRepository _repository;

    public UsersController(UserRepository repository){
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetAllAsync(){
        var  users = await _repository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")] //api/v1/users/2
    public async Task <ActionResult<AppUser>> GetByIDAsync(int id){
        var user = await _repository.GetByIdAsync(id);

        if(user == null){
            return NotFound();
        } 

        return user;
    }


    [HttpGet("{username}")] //api/v1/users/2
    public async Task <ActionResult<AppUser>> GetByUsernameAsync(string username){
        var user = await _repository.GetByUsernameAsync(username);

        if(user == null){
            return NotFound();
        } 

        return user;
    }

}