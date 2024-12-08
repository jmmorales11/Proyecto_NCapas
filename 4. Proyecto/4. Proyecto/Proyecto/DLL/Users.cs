using DAL;
using Entities;
using BLL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Users
    {
        public User Create(User newUser)
        {
            User Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Verificar si el correo ya está registrado
                User res = r.Retrieve<User>(u => u.Email == newUser.Email);
                if (res == null)
                {
                    // Hashear la contraseña antes de guardar el usuario
                    newUser.PasswordHash = PasswordHasher.HashPassword(newUser.PasswordHash);

                    // Crear el usuario
                    Result = r.Create(newUser);
                }
                else
                {
                    throw new Exception("El correo electrónico ya está registrado.");
                }
            }
            return Result;
        }
        public User RetrieveByID(int ID)
        {
            User Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                Result = r.Retrieve<User>(u => u.UserID == ID);
            }
            return Result;
        }

        public bool Update(User userToUpdate)
        {
            bool Result = false;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el correo no esté en uso por otro usuario
                User temp = r.Retrieve<User>(u =>
                    u.Email == userToUpdate.Email &&
                    u.UserID != userToUpdate.UserID);
                if (temp == null)
                {
                    Result = r.Update(userToUpdate);
                }
                else
                {
                    // Implementar lógica para notificar que no se puede actualizar
                }
            }
            return Result;
        }

        public bool Delete(int ID)
        {
            bool Result = false;
            var user = RetrieveByID(ID);
            if (user != null)
            {
                using (var r = RepositoryFactory.CreateRepository())
                {
                    Result = r.Delete(user);
                }
            }
            else
            {
                // Implementar lógica para indicar que el usuario no existe
            }
            return Result;
        }

        public List<User> GetAllUser()
        {
            List<User> users = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Recuperar todos los usuarios
                users = r.GetAll<User>();
            }
            return users;
        }

        public User Authenticate(string email, string password)
        {
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Recuperar el usuario por correo electrónico
                User user = r.Retrieve<User>(u => u.Email == email);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("Usuario no encontrado.");
                }

                // Verificar si la contraseña coincide con el hash
                if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Contraseña incorrecta.");
                }

                return user; // Usuario autenticado correctamente
            }
        }



    }
}
