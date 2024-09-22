using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context) : BaseApiController{
    
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> RegisterAsync(RegisterRequest request){ 
        
        if (await UserExistsAsync(request.UserName)){
            return BadRequest("Nombre de Usuario En Uso :(");
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser{
            UserName = request.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> LoginAsync(LoginRequest request){
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToUpper() == request.Username.ToUpper());

        if (user == null){
            return Unauthorized("Usuario o Contraseña Incorrecta");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        for (int i =0; i<computeHash.Length; i++){
            if (computeHash[i] != user.PasswordHash[i]){
                return Unauthorized("Usuario o Contraseña Incorrecta");
            }
        }
        return user;
    }

    private async Task<bool> UserExistsAsync(string username){
        return await context.Users.AnyAsync(
            user => user.UserName.ToUpper() == username.ToUpper()
        );
        
    }
}