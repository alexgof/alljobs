using AllJobsRestApi.Logger;
using AllJobsRestAPI.Helpers;
using AllJobsRestAPI.Models.Users;
using AllJobsRestAPI.Services;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using static AllJobsRestAPI.Models.Users.JsonResponse;
using static AllJobsRestAPI.Models.Users.JsonResponseWithUserID;

namespace AllJobsRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private const string SecretKey = "135789k@%Nlgfdysd";
        private IUserService _userService;
        private IMapper _mapper;

        public object BCryptNet { get; private set; }

        public UserController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(CreateRequest model)
        {
            _userService.Create(model);
            return Ok(new { message = "User created" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted" });
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JSONResponse</returns>        
        [Route("Register")]
        [HttpPost]
        public JSONResponse Register(CreateRequest model)
        {
            
            string myName = NlogLogger.InitMethodName();
            NlogLogger.Log.Info($"{myName} started...");

            var response = new JSONResponse() { Status = "0", Token = "{JWT Token created earlier}", Message = "" };

            try
            {
                var user = _userService.GetByFields(model);
                if(user != null)
                {
                    if (user.ID > 0)
                    {
                        response = new JSONResponse() { Status = "W", Token = "", Message = "user already exists" };
                    }
                }
                else
                {
                    //a. create a hash from the user password 
                    //b.call the stored procedure: sp_CreateNewUser with the following details: Email, Password, PhoneNumber, FullName, UserIP                    
                    var newUser = _userService.CreateNewUserBySP(model);

                    //c.generate JWT Token with the user.id that was retured from the store procedure                     
                    var token = TokenJWT.GenerateJwtToken(newUser.ID, SecretKey);
                    response.Token = token;
                }
            }
            catch (Exception ex)
            {
                //response.Message = ex.Message;
                NlogLogger.Log.Error($"{myName} ex.Message: {ex.Message}.");
            }

            return response;
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JSONResponse</returns>
        [Route("Login")]
        [HttpPost]
        public JSONResponse Login(LoginRequest model)
        {
            string myName = NlogLogger.InitMethodName();
            NlogLogger.Log.Info($"{myName} started...");

            var response = new JSONResponse() { Status = "O", Token = "", Message = "Some of the details was wrong" };

            try
            {
                // b.create a base64 string from the password hash
                // c.call the stored procedure: sp_CheckUserEmailPassword with the following details: Email, Password
                var user = _userService.CheckUserEmailPassword(model);

                // d. if user.id > 0 return the the following response
                // { "Status": "O", "Token": "{JWT Token created earlier}", "Message": "" }
                //   if not return the following response { "Status": "O", "Token": "", "Message": "Some of the details was wrong" }
                if (user != null)
                {
                    if (user.ID > 0)                    
                    {
                        var token = TokenJWT.GenerateJwtToken(user.ID, SecretKey);
                        response = new JSONResponse() { Status = "O", Token = token, Message = "" }; 
                    }
                }                
            }
            catch (Exception ex)
            {
                //response.Message = ex.Message;
                NlogLogger.Log.Error($"{myName} ex.Message: {ex.Message}.");
            }

            return response;
        }

        /// <summary>
        /// Set Bearer from previouse request
        /// </summary>
        /// <returns>JsonResponseWithUserID</returns>
        [Route("GetUserDetails")]
        [HttpPost]
        public JsonResponseWithUserID GetUserDetails()
        {
            string myName = NlogLogger.InitMethodName();
            NlogLogger.Log.Info($"{myName} started...");//check if folder exists C:/Log or edit nlog.config

            var response = new JsonResponseWithUserID() { Status = "E", Message = "UnAuthorized Request" };
            //create a new action: GetUserDetails the GetUserDetails
            //should receive an header called Bearer with the JWT Token created in the login or register actions
            //if the JWT Token is not valid or no token sent return the following response { "Status": "E", "Message": "UnAuthorized Request" }
            // otherwise return { "Status": "O", "User": { "ID": user.id from the JWT Token message } }

            try
            {
                string bearerToken = HttpContext.Request.Headers["Bearer"];
                               
                string userId = TokenJWT.GetIDFromJwtToken(bearerToken, SecretKey);
                if (!string.IsNullOrEmpty(userId))
                {
                    //{ "Status": "O", "User": { "ID": user.id from the JWT Token message } }
                    response.User = new UserData();
                    response.User.ID = userId;
                    response.Status = "O";
                    response.Message = null;
                }
            }
            catch (Exception ex)
            {
                //response.Message = ex.Message;
                NlogLogger.Log.Error($"{myName} ex.Message: {ex.Message}.");
            }

            return response;
        }
    }
}
