using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface IUser
    {
        User CreateUser(User newUser);
        User RetrieveUserByID(int ID);
        bool UpdateUser(User UserToUpdate);
        bool DeleteUser(int ID);
        List<User> GetUsers();
        User GetUserByEmail(string email);
    }
}