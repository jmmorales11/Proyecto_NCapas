using BLL;
using Entities;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Service.WebApiConfig;

namespace Service.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController, IUser
    {
        [HttpPost]
        [RoleAuthorize("Admin", "Editor")]
        [Route("create-user")]
        public User CreateUser(User newUser)
        {
            var BL = new Users();
            var NewUser = BL.Create(newUser);
            return NewUser;
        }

        [HttpGet]
        [RoleAuthorize("Admin")]
        [Route("delete-user/{id}")]
        public bool DeleteUser(int id)
        {
            var BL = new Users();
            var Result = BL.Delete(id);
            return Result;
        }

        [HttpGet]
        [RoleAuthorize("Admin", "Viewer")]
        [Route("filter-user")]
        public List<User> GetUsers()
        {
            var BL = new Users();
            var Result = BL.GetAllUser();
            foreach (var user in Result)
            {
                user.PasswordHash = null;
            }
            return Result;
        }

        [HttpGet]
        [RoleAuthorize("Admin", "Viewer")]
        [Route("retrieve-user/{id}")]
        public User RetrieveUserByID(int id)
        {
            var BL = new Users();
            var Result = BL.RetrieveByID(id);
            return Result;
        }

        [HttpPost]
        [RoleAuthorize("Admin", "Editor")]
        [Route("update-user")]
        public bool UpdateUser(User UserToUpdate)
        {
            var BL = new Users();
            var Result = BL.Update(UserToUpdate);
            return Result;
        }
    }
}
