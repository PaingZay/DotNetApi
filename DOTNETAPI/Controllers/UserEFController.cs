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
    public class UserEFController : Controller
    {
        DataContextEF _entityFramework;

        AutoMapper.IMapper _mapper;

        public UserEFController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);

            _mapper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
        }


        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers(string testValue)
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();

            return users;
        }

        [HttpGet("GetUsers/{userId}")]
        public User GetSingleUsers(int userId)
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)//If that row matches the userId on the row that the EntityFramework is checking
                .FirstOrDefault<User>();
            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to Get User");
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
            User userDb = new User();

            userDb.Active = user.Active;//Assign the userDb from user input
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            _entityFramework.Add(userDb);
            if (_entityFramework.SaveChanges() > 0)//If entityframework save chnages is successful at least zero row
            {
                return Ok();
            }
            throw new Exception("Failed to Get User");
        }

        /*WithAutoMapper
        With that we do not need to assign value from dto to newly created object */
        //[HttpPost("AddUser")]
        //public IActionResult AddUser([FromBody] UserToAddDto user)
        //{
        //    User userDb = _mapper.Map<User>(user);

        //    userDb.Active = user.Active;//Assign the userDb from user input
        //    userDb.FirstName = user.FirstName;
        //    userDb.LastName = user.LastName;
        //    userDb.Email = user.Email;
        //    userDb.Gender = user.Gender;
        //    _entityFramework.Add(userDb);
        //    if (_entityFramework.SaveChanges() > 0)//If entityframework save chnages is successful at least zero row
        //    {
        //        return Ok();
        //    }
        //    throw new Exception("Failed to Get User");
        //}

        // PUT api/values/5
        [HttpPut("EditUser")]
        public IActionResult EditUser([FromBody] User user)
        {
            User? userDb = _entityFramework.Users
                .Where(u => u.UserId == user.UserId)//If that row matches the userId on the row that the EntityFramework is checking
                .FirstOrDefault<User>();
            if (userDb != null)
            {
                userDb.Active = user.Active;//Assign the userDb from user input
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                if (_entityFramework.SaveChanges() > 0)//If entityframework save chnages is successful at least zero row
                {
                    return Ok();
                }
                throw new Exception("Failed to Update User");
            }
            throw new Exception("Failed to Get User");
        }



        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUsers(int userId)
        {
            User? userDb = _entityFramework.Users
                .Where(u => u.UserId == userId)//If that row matches the userId on the row that the EntityFramework is checking
                .FirstOrDefault<User>();
            if (userDb != null)
            {
                _entityFramework.Users.Remove(userDb);
                if (_entityFramework.SaveChanges() > 0)//If entityframework save chnages is successful at least zero row
                {
                    return Ok();
                }
                throw new Exception("Failed to Delete User");
            }
            throw new Exception("Failed to Get User");
        }
    }
}

