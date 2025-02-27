using API.Data;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Extensions;
using API.DataEntities;
using API.Helpers;
namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController{
    private readonly IUserRepository _repository;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IPhotoService photoService, IMapper mapper){
        _repository = repository;
        _photoService = photoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync([FromQuery] UserParams userParams){
        userParams.CurrentUsername = User.GetUserName();
        var members = await _repository.GetMembersAsync(userParams);

        Response.AddPaginationHeader(members);
        return Ok(members);
    }

    [HttpGet("{username}", Name="GetByUsername")] //api/v1/users/2
    public async Task <ActionResult<MemberResponse>> GetByUsernameAsync(string username){
        var member = await _repository.GetMemberAsync(username);

        if (member == null){
            return NotFound();
        }

        return member;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateRequest request)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        
        if (user == null)
        {
            return BadRequest("Could not find user");
        }

        _mapper.Map(request, user);
        _repository.Update(user);

        if (await _repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Update user failed!");
    }

    [HttpPost("photo")]
    public async Task<ActionResult<PhotoResponse>> AddPhoto(IFormFile file){
        var user = await _repository.GetByUsernameAsync(User.GetUserName());

        if(user == null){
            return BadRequest("No se pudo actualizar el usuario");
        }

        var result = await _photoService.AddPhotoAsync(file);

        if(result.Error != null){
            return BadRequest(result.Error.Message);
        }

        var photo = new Photo{
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if (await _repository.SaveAllAsync()){
            return CreatedAtAction("GetByUsername",
            new {username = user.UserName}, _mapper.Map<PhotoResponse>(photo));
        }

        return BadRequest("Problema al agregar la foto");
    
    }

    [HttpPut("photo/{photoId:int}")]
    public async Task<ActionResult> SetPhotoAsMain(int photoId){
        var user = await _repository.GetByUsernameAsync(User.GetUserName());

        if(user == null) return BadRequest("Usuarion No Encontrado");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
    
        if (photo == null || photo.IsMain) return BadRequest("No se puede establecer esta foto como principal");

        var currentMain = user.Photos.FirstOrDefault(p=> p.IsMain);

        if (currentMain != null) currentMain.IsMain = false;
        
        photo.IsMain = true;

        if(await _repository.SaveAllAsync()) return NoContent();

        return BadRequest("No hubo problema");
    }

    [HttpDelete("photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("This photo can't be deleted");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _repository.SaveAllAsync()) return Ok();

        return BadRequest("There was a problem when deleting the photo");
    }

}