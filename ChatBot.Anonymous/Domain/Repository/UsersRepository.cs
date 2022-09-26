using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Domain.Entities;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using ChatBot.Anonymous.Models;

namespace ChatBot.Anonymous.Domain.Repository
{
    public class UsersRepository : IUser
    {
        private readonly BotDbContext _context;
        private readonly BotConfiguration _configuration;

        public UsersRepository(BotDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration.GetMainConfigurationToObject();
        }

        public IQueryable<User> Get(int? limit = null, int? offset = null)
        {
            var users = _context.Users.Include(x => x.Action)
                .Skip(offset ?? _configuration.DefaultOffset)
                .Take(limit ?? _configuration.DefaultLimit);

            return users;
        }

        public async Task<User?> GetById(long userId)
        {
            var user = await Get().FirstOrDefaultAsync(x => x.UserId == userId);

            return user;
        }

        public async Task<User> SaveUser(
            long userId,
            int? gender = null,
            int? age = null)
        {
            var user = await GetById(userId);

            if (user == null)
            {
                user = new User()
                {
                    UserId = userId,
                    Gender = (int?)gender,
                    Age = age,
                    UserSetting = new UserSetting()
                    {
                        UserId = userId,
                    },
                    Action = new ActionData()
                    {
                        UserId = userId,
                        ChatId = userId,
                    },
                };

                _context.Users.Add(user);
            }
            else
            {
                if (gender.HasValue)
                {
                    user.Gender = gender;
                }

                if (age.HasValue)
                {
                    user.Age = age;
                }

                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
