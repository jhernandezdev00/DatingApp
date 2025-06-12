using API.DataEntities;

namespace API.Services{
    public interface ITokenService{
        public Task<string> CreateToken(AppUser user);
    }
}