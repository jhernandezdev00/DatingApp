using API.Data;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper){
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync(){
        var users = await _repository.GetAllAsync();
        var response = _mapper.Map<IEnumerable<MemberResponse>>(users);
        return Ok(response);
    }

    [HttpGet("{id:int}")] //api/v1/users/2
    public async Task <ActionResult<MemberResponse>> GetByIDAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return _mapper.Map<MemberResponse>(user);
    }

    [HttpGet("{username}")] //api/v1/users/2
    public async Task <ActionResult<MemberResponse>> GetByUsernameAsync(string username){
        var user = await _repository.GetByUsernameAsync(username);

        if(user == null){
            return NotFound();
        } 

        return _mapper.Map<MemberResponse>(user);
    }

}