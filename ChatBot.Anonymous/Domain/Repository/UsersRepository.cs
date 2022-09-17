using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Domain.Context;
using ChatBot.Anonymous.Domain.Entities;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Anonymous.Domain.Repository
{
    public class UsersRepository : IUser
    {
        private readonly BotDbContext _context;

        public UsersRepository(BotDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> Get(int? limit = null, int? offset = null)
        {
            var users = _context.Users.Skip(offset ?? 0)
                .Take(limit ?? 25);

            return users;
        }

        public async Task<User?> GetById(long userId)
        {
            var user = await Get().FirstOrDefaultAsync(x => x.UserId == userId);

            return user;
        }

        public async Task<User> SaveUser(long userId)
        {
            var user = new User()
            {
                UserId = userId,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
