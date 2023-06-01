using AllJobsRestAPI.Entities;
using AllJobsRestAPI.Models.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllJobsRestAPI.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            options.UseSqlServer("server=34.77.187.87;database=test-alexgof;User Id=testalexgof;Password=testalexgof;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        public UserLogin CreateNewUserByProcedure(CreateRequest model)
        {            
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@Email",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 200,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Email
                        },
                        new SqlParameter() {
                            ParameterName = "@Password",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 200,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Password
                        },
                        new SqlParameter() {
                            ParameterName = "@PhoneNumber",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.PhoneNumber
                        },
                        new SqlParameter() {
                            ParameterName = "@FullName",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.FullName
                        },
                        new SqlParameter() {
                            ParameterName = "@UserIP",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.UserIP
                        }};
            var user = UserLogins.FromSqlRaw("[dbo].[sp_CreateNewUser] @Email, @Password, @PhoneNumber, @FullName, @UserIP", param).ToList()[0];
           
            return user;
        }

        public UserLogin CheckUserEmailPassword(LoginRequest model)
        {            
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@Email",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 150,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Email
                        },
                        new SqlParameter() {
                            ParameterName = "@Password",
                            SqlDbType =  System.Data.SqlDbType.NVarChar,Size = 100,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Password
                        }};

            UserLogin userLogin = null;
            try
            {
                userLogin = UserLogins.FromSqlRaw("[dbo].[sp_CheckUserEmailPassword] @Email, @Password", param).ToList()[0];
            }
            catch { }

            return userLogin;
        }    
    }
}
