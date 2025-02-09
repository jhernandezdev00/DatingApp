using API.DataEntities;

namespace API.Services{
    public interface ITokenService{
        string CreateToken(AppUser user);
    }
}