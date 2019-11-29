using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Identity;
using ee.itcollege.dauuka.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// Controller for managing account
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for AccountController
        /// </summary>
        /// <param name="signInManager">SignInManager.</param>
        /// <param name="configuration">IConfiguration.</param>
        /// <param name="userManager">UserManager.</param>
        /// <param name="logger">ILogger.</param>
        /// <param name="emailSender">IEmailSender.</param>
        public AccountController(SignInManager<AppUser> signInManager, IConfiguration configuration, 
            UserManager<AppUser> userManager, ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Method for managing login
        /// </summary>
        /// <param name="model">LoginDTO.</param>
        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO model)
        {
            var appUser = await _userManager.FindByEmailAsync(model.Email);
            
            if (appUser == null)
            {
                // user is not found, return 403
                _logger.LogInformation("User not found.");
                return StatusCode(403);
            }
            
            // do not log user in, just check that the password is ok
            var result = await _signInManager.CheckPasswordSignInAsync(appUser, model.Password, false);

            if (result.Succeeded)
            {
                // create claims based user 
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
          
                // get the Json Web Token
                var jwt = JwtHelper.GenerateJwt(
                    claimsPrincipal.Claims, 
                    _configuration["JWT:Key"], 
                    _configuration["JWT:Issuer"], 
                    int.Parse(_configuration["JWT:ExpireDays"]));
                _logger.LogInformation("Token generated for user");
                return Ok(new {token = jwt});
            }

            return StatusCode(403);
        }
        
        /// <summary>
        /// Method for managing register
        /// </summary>
        /// <param name="model">RegisterDTO.</param>
        [HttpPost]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("New user created.");
                    
                    // create claims based user 
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
          
                    // get the Json Web Token
                    var jwt = JwtHelper.GenerateJwt(
                        claimsPrincipal.Claims, 
                        _configuration["JWT:Key"], 
                        _configuration["JWT:Issuer"], 
                        int.Parse(_configuration["JWT:ExpireDays"]));
                    _logger.LogInformation("Token generated for user");
                    return Ok(new {token = jwt});
                }
                return StatusCode(406); //406 Not Acceptable
            }
            
            return StatusCode(400); //400 Bad Request
        }
        
        /// <summary>
        /// Class for login data
        /// </summary>
        public class LoginDTO
        {
            /// <summary>
            /// Email
            /// </summary>
            public string Email { get; set; }
            
            /// <summary>
            /// Password
            /// </summary>
            public string Password { get; set; }
        }

        /// <summary>
        /// Class for register data
        /// </summary>
        public class RegisterDTO
        {
            /// <summary>
            /// Email
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Password
            /// </summary>
            [Required]
            [MinLength(6)]
            public string Password { get; set; }
        }
    }
}