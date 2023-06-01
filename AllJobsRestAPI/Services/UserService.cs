using AllJobsRestAPI.Entities;
using AllJobsRestAPI.Helpers;
using AllJobsRestAPI.Models.Users;
using BCryptNet = BCrypt.Net.BCrypt;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        
        User GetByFields(CreateRequest model);
        UserLogin CreateNewUserBySP(CreateRequest model);
        UserLogin CheckUserEmailPassword(LoginRequest model);
        
        void Create(CreateRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);                
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        public User GetByFields(CreateRequest model)
        {
            return getUser(model);
        }

        public UserLogin CreateNewUserBySP(CreateRequest model)
        {
            return _context.CreateNewUserByProcedure(model);
             
        }

        public UserLogin CheckUserEmailPassword(LoginRequest model)
        {
            var user = _context.CheckUserEmailPassword(model);
            //a.create a hash from the user password
            if (user!= null)
            {
                var trmpUser = getUser(user.ID);
                var userPasswordHash = BCryptNet.HashPassword(trmpUser.Password);

                //b.create a base64 string from the password hash
                Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(userPasswordHash));

                string userPasswordHashBase64 = Convert.ToBase64String(PhraseAsByte);
                //TODD SAVE userPasswordHashBase64
            }
            
            return user;
        }

        

        public void Create(CreateRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            var userPasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = model.Password;
            //user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // helper methods        
        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        private User getUser(CreateRequest model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PhoneNumber == model.PhoneNumber && u.FullName == model.FullName && u.Password == model.Password);
            //if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        
    }
}
