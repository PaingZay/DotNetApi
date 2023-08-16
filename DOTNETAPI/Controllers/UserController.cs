using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNETAPI.Data;
using DOTNETAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DOTNETAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        //Testing Database Connection
        //public UserController(IConfiguration config)
        //{
        //    Console.WriteLine(config.GetConnectionString("DefaultConnection"));
        //}

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }


        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers(string testValue)
        {

            string sql = @"
                SELECT [UserId],
                        [FirstName],
                        [LastName], 
                        [Email],
                        [Gender],
                        [Active] From tutorialAppSchema.Users
            ";
            IEnumerable<User> users = _dapper.LoadData<User>(sql);

            return users;
        }

        [HttpGet("GetUsers/{userId}")]
        public User GetSingleUsers(int userId)
        {
            string sql = @"
                SELECT [UserId],
                        [FirstName],
                        [LastName], 
                        [Email],
                        [Gender],
                        [Active] From tutorialAppSchema.Users
                        Where UserId = " + userId.ToString();
            User user = _dapper.LoadDataSingle<User>(sql);
            return user;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserToAddDto user)
        {
            string sql = @"
                        Insert Into TutorialAppSchema.Users(
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active]
                    ) VALUES (" +
                            "'" + user.FirstName +
                            "','" + user.LastName +
                            "','" + user.Email +
                            "','" + user.Gender +
                            "','" + user.Active +
                            "')";

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Add User");
        }

        // PUT api/values/5
        [HttpPut("EditUser{userId}")]
        public IActionResult EditUser(int userId, [FromBody] User user)
        {
            string sql = @"
                            UPDATE TutorialAppSchema.Users SET
                            [FirstName] = '" + user.FirstName +
                            "', [LastName] = '" + user.LastName +
                            "', [Email] = '" + user.Email +
                            "', [Gender] = '" + user.Gender +
                            "', [Active] = '" + user.Active +
                            "' WHERE UserId = " + user.UserId;

            Console.WriteLine(sql);
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to update User");
        }



        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUsers(int userId)
        {
            string sql = @"
                        Delete from tutorialAppSchema.Users
                        Where userid = " + userId.ToString();

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete User");
        }
    }
}

