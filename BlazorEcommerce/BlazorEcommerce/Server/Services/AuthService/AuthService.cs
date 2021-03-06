using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var isUser = await UserExists(user.Email);

            if (isUser)
            {
                var errorResponse = new ServiceResponse<int>()
                {
                    Success = false,
                    Message = "User already exists."
                };

                return errorResponse;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<int>()
            {
                Data = user.Id,
                Message = "Registration successful!"
            };

            return response;
        }

        public async Task<ServiceResponse<int>> RemoveUser(User user)
        {
            var isUser = await UserExists(user.Email);

            if (!isUser)
            {
                var errorResponse = new ServiceResponse<int>()
                {
                    Success = false,
                    Message = "User doesn't exist."
                };

                return errorResponse;
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<int>()
            {
                Data = user.Id,
                Message = "Deleted successful!"
            };

            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            var isUser = await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));

            if (isUser)
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token");

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken.Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                var errorResponse = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };

                return errorResponse;
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            var respone = new ServiceResponse<bool>()
            {
                Data = true,
                Message = "Password has been changed."
            };

            return respone;
        }

        public int GetUserId()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                var user = context.User;

                if (user != null)
                {
                    var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (int.TryParse(claim, out var id))
                    {
                        return id;
                    }
                }
            }

            return 0;
        }

        public string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));

            return user;
        }

        public async Task<ServiceResponse<UserProfile>> GetFullUserByIdAsync(int id)
        {
            var response = new ServiceResponse<UserProfile>();

            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (dbUser == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else
            {
                var userProfile = new UserProfile()
                {
                    Id = id,
                    Email = dbUser.Email,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    PhoneNumber = dbUser.PhoneNumber,
                    ImageUrl = dbUser.ImageUrl,
                };

                response.Data = userProfile;
            }

            return response;
        }

        public async Task<ServiceResponse<UserProfile>> UpdateUserAsync(UserProfile user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser == null)
            {
                var errorResponse = new ServiceResponse<UserProfile>
                {
                    Success = false,
                    Message = "User not found."
                };

                return errorResponse;
            }

            dbUser.ImageUrl = user.ImageUrl;
            dbUser.Email = user.Email;
            dbUser.LastName = user.LastName;
            dbUser.FirstName = user.FirstName;
            dbUser.PhoneNumber = user.PhoneNumber;

            await _context.SaveChangesAsync();

            var response = new ServiceResponse<UserProfile>
            {
                Data = user
            };

            return response;
        }
    }
}
