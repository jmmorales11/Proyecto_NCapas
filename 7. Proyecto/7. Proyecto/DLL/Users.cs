using DAL;
using Entities;
using SL.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    // Validar la contraseña
                    if (!ValidatePassword(newUser, newUser.PasswordHash))
                    {
                        throw new Exception("La contraseña no cumple con los requisitos de seguridad.");
                    }

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
                    // Validar la contraseña
                    if (!ValidatePassword(userToUpdate, userToUpdate.PasswordHash))
                    {
                        throw new Exception("La contraseña no cumple con los requisitos de seguridad.");
                    }
                    // Hashear la contraseña antes de guardar el usuario
                    userToUpdate.PasswordHash = PasswordHasher.HashPassword(userToUpdate.PasswordHash);
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
                    throw new System.UnauthorizedAccessException("Usuario no encontrado.");
                }

                // Verificar si la cuenta está bloqueada
                if (user.AccountLockedUntil.HasValue && user.AccountLockedUntil > DateTime.Now)
                {
                    throw new System.UnauthorizedAccessException("La cuenta está bloqueada. Intenta nuevamente después de unos minutos.");
                }

                // Verificar si la contraseña coincide con el hash
                if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    // Aumentar el contador de intentos fallidos
                    if (user.FailedLoginAttempts == null)
                    {
                        user.FailedLoginAttempts = 1;
                    }
                    else
                    {
                        user.FailedLoginAttempts += 1;
                    }

                    // Si el número de intentos fallidos alcanza 3, bloquear la cuenta durante 2 minutos
                    if (user.FailedLoginAttempts >= 3)
                    {
                        user.AccountLockedUntil = DateTime.Now.AddMinutes(2);
                        r.Update(user); // Actualizar el usuario con la fecha de bloqueo
                        throw new System.UnauthorizedAccessException("Has excedido los intentos de inicio de sesión. La cuenta está bloqueada por 2 minutos.");
                    }

                    // Guardar los cambios de intentos fallidos
                    r.Update(user);

                    throw new System.UnauthorizedAccessException("Contraseña incorrecta.");
                }

                // Si la autenticación fue exitosa, reiniciar los intentos fallidos y la fecha de bloqueo
                user.FailedLoginAttempts = 0;
                user.AccountLockedUntil = null;
                r.Update(user);

                return user; // Usuario autenticado correctamente
            }
        }

        private bool ValidatePassword(User user, string password)
        {
            // Verificar longitud mínima
            if (password.Length < 13)
                return false;

            // Verificar que contenga al menos un carácter especial
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?\:{ }|<>]"))
                return false;

            // Verificar que no contenga partes del nombre de usuario o correo electrónico
            var userNameParts = user.UserName.Split(new char[] { ' ', '.', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in userNameParts)
            {
                if (password.Contains(part))
                    return false;
            }

            // Verificar que no contenga partes del correo electrónico
            var emailParts = user.Email.Split(new char[] { '@', '.', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in emailParts)
            {
                if (password.Contains(part))
                    return false;
            }

            return true;
        }
        //buscar por email
        public User GetUserByEmail(string email)
        {
            User Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Recuperar el usuario por correo electrónico
                Result = r.Retrieve<User>(u => u.Email == email);
            }

            if (Result == null)
            {
                // Si no se encuentra el usuario, puedes lanzar una excepción o devolver null
                throw new Exception("Usuario no encontrado.");
            }

            return Result;
        }


    }
}
