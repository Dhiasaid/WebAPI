using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebAPI.Auth;
using WebAPI.Modeles;
using WebAPIl.Auth;
using WebAPI.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationsDbContext databaseContext;

        public AuthenticationController(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IConfiguration configuration
            , SignInManager<ApplicationUser> signInManager
            , ApplicationsDbContext applicationdbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            databaseContext = applicationdbContext;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel jsonData)
        {
            try
            {


                var HttpRequest = HttpContext.Request;
                var userExists = await _userManager.FindByNameAsync(jsonData.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
                //////////////
                ///
              /*  string CIN;
                DateTime DateNais;
                 var list = new List<ParamUserModel>();
                list = DbClientFactory<ResultatDbClient>.Instance.GetParmUser(jsonData.Username, jsonData.Role, _configuration.GetSection("ConnectionStrings").GetSection("ConnStr").Value);
                if (list != null && list.Count != 0)
                {
                    CIN = list[0].CIN;
                    DateNais = list[0].DateNais;
                }
                else
                {
                    _logger.LogInformation("Register er Id FAil:{Username}-Role{UserRole}, {Name}!", jsonData.Username, jsonData.Role, "Register");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "unknown user, check your Id" });
                }
                // UserManager.UserValidator = new UserValidator<TUser>(UserManager) { AllowOnlyAlphanumericUserNames = false }

                if (jsonData.CIN != list[0].CIN)
                {
                    _logger.LogInformation("Register er cin FAil:{Username}-Role{UserRole}, {Name}!", jsonData.Username, jsonData.Role, "Register");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "unknown user, check your CIN" });
                }
                if (jsonData.DateNais != list[0].DateNais)
                {
                    _logger.LogInformation("Register er DateNais  FAil:{Username}-Role{UserRole}, {Name}!", jsonData.Username, jsonData.Role, "Register");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "unknown user, check your date of birth" });
                }
                */

                /*
                Byte[] bytes;
                if (jsonData.FileContent != null)
                {
                    bytes = Convert.FromBase64String(jsonData.FileContent);
                    var fileExtension = MimeTypeMap.GetExtension(jsonData.ContentType);
                    System.IO.File.WriteAllBytes(GetFilePath(jsonData.Username, fileExtension, jsonData.Role, fileUploadPath), bytes);
                }
                else
                    bytes = null;

                */

                ApplicationUser user = new ApplicationUser()
                {
                    Email = jsonData.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = jsonData.Username,
                    CIN = jsonData.CIN,
                    // IdDevice = jsonData.IdDevice,
                    // UserMatricule = model.UserMatricule
                    //  ProfilePicture = bytes,

                };


                var result = await _userManager.CreateAsync(user, jsonData.Password);
                if (!result.Succeeded)
                {
                    //  _logger.LogInformation("Register creation FAil:{Username}-Role{UserRole}, {Name}!", jsonData.Username, jsonData.Role, "Register");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result + ".....User creation failed! Vérifiez vos informations et réessayez." });
                }

                // Checking roles in database and creating if not exists
                if (!await _roleManager.RoleExistsAsync(ApplicationUser.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(ApplicationUser.Admin));
                if (!await _roleManager.RoleExistsAsync(ApplicationUser.User))
                    await _roleManager.CreateAsync(new IdentityRole(ApplicationUser.User));


                // Add role to user
                if (!string.IsNullOrEmpty(jsonData.Role) && jsonData.Role == ApplicationUser.Admin)
                {
                    await _userManager.AddToRoleAsync(user, userRole.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, ApplicationUser.User);
                }
                /*
                                var profile = new Profile()
                                {
                                    Address1 = jsonData.Address1,
                                    //Address2 = registerModel.Address2,
                                    //City = registerModel.City,
                                    // Landmark = registerModel.Landmark,
                                    //CountryCode = registerModel.CountryCode,
                                    //Pin = registerModel.Pin,
                                    //State = registerModel.State,
                                    UserId = user.Id
                                };

                                await databaseC
                ontext.AddAsync(profile);
                                await databaseContext.SaveChangesAsync();
                                */

                // _logger.LogInformation("Register Success:{Username}-Role{UserRole}, {Name}!", jsonData.Username, jsonData.Role, "Register");
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ".....User creation failed! " });


            }

        }

        private async Task<RefreshTokenModel> GenerateAccessToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("id", user.Id)
    };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddSeconds(600),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var refreshTokenModel = new RefreshTokenModel
            {
                RefreshToken = (await GenerateRefreshToken(user.Id, token.Id)).Token,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return refreshTokenModel;
        }

        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TokenId"></param>
        /// <returns><see cref="Task{RefreshToken}"/></returns>
        private async Task<RefreshToken> GenerateRefreshToken(string UserId, string TokenId)
        {
            var refreshToken = new RefreshToken();
            var randomNumber = new byte[32];

            using (var randomNumerGenerator = RandomNumberGenerator.Create())
            {
                randomNumerGenerator.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
                refreshToken.ExpiryDateTimeUtc = DateTime.UtcNow.AddMonths(6);
                refreshToken.CreatedDateTimeUtc = DateTime.UtcNow;
                refreshToken.UserId = UserId;
                refreshToken.JwtId = TokenId;
            }

            await databaseContext.AddAsync(refreshToken);
            await databaseContext.SaveChangesAsync();

            return refreshToken;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            try
            {
                /*
                if (_configuration["ParamAPP:VersionApp"] != loginModel.VersionApp.ToString())
                {
                  //  _logger.LogInformation("Login Er Version:{Username} {Name}!", loginModel.Username, "Login");
                    return Unauthorized(new
                    {
                        Status = "Error",
                        Message = "Error, Version obsolète, Veuillez installer la nouvelle version."
                    });
                }
                */

                var user = await _userManager.FindByNameAsync(loginModel.Username);
                if (user != null
                    && await _userManager.CheckPasswordAsync(user, loginModel.Password)
                    && (await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                {
                    RefreshTokenModel refreshTokenModel = await GenerateAccessToken(user);
                    var rolesUser = await _userManager.GetRolesAsync(user);
                    string Role = rolesUser.First();
                    /*
                     if (user.IdDevice != loginModel.IdDevice && Role == "Admin")
                     {
                          // _logger.LogError("Login User:{Username}-Role{UserRole}, {Name}!", loginModel.Username, rolesUser, "Login");
                         return Unauthorized(new
                         {
                             Status = "Error",
                             Message = "Error Device ID."
                         });
                     }
                     else
                     {*/

                    //  _logger.LogInformation("Login Access:{Username}-Role{UserRole}, {Name}!", loginModel.Username, rolesUser, "Login");
                    return Ok(new
                    {
                        Status = "Success",
                        token = refreshTokenModel.Token,
                        refreshToken = refreshTokenModel.RefreshToken,
                        expiration = refreshTokenModel.Expiration,
                        role = Role

                    });
                    //  }
                }
                //  _logger.LogInformation("Login Unauthorized:{Username} {Name}!", loginModel.Username, "Login");
                return Unauthorized(new
                {
                    Status = "Error",
                    Message = "Error: Login ou mot de passe incorrect."

                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ".....Error login! " });


            }
        }

    }
}
