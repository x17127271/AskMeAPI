using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class UserService : IUserService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public UserService(
            IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method returns an existing User.
        /// </summary>
        /// <param name="userId">Integer</param>
        /// <returns>User</returns>
        public async Task<User> GetUserById(int userId)
        {
            // calls repository to search a User by id
            return await _askMeRepository.GetUserById(userId);
        }
        /// <summary>
        /// This method returns a list of Users
        /// </summary>
        /// <returns>List<User>()</returns>
        public async Task<List<User>> GetUsers()
        {
            // calls repository to get list of users
            return await _askMeRepository.GetUsers();
        }
        /// <summary>
        /// This method authenticates an existing User.
        /// </summary>
        /// <param name="username">String</param>
        /// <param name="password">String</param>
        /// <returns>User</returns>
        public async Task<User> Authenticate(string username, string password)
        {
            // validates parameters are not empty or null
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            // calls repository to search a user by name
            var user = await _askMeRepository.GetUserByUserName(username);

            // check if username exists
            if (user == null)
            {
                return null;
            }
            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            // authentication successful
            return user;
        }
        /// <summary>
        /// This method creates a new User.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="password">String</param>
        /// <returns>User</returns>
        public async Task<User> Create(User user, string password)
        {
            //validates password is not empty or null
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required");
            }
            // encrypt the passwors suing has and salt
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            // calls repository to create a new user
            var userCreated = await _askMeRepository.AddUser(user);
           // returns the created user
            return userCreated;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty.", nameof(password));
            }
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty.", nameof(password));
            }
            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length for password hash.", "passwordHash");
            }
            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length for password salt.", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
