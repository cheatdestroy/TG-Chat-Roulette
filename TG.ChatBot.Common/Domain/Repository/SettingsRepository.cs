using TG.ChatBot.Common.Domain.Context;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Domain.Repository.Interfaces;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TG.ChatBot.Common.Domain.Repository
{
    public class SettingsRepository : ISettings
    {
        private readonly BotDbContext _context;
        private readonly BotConfiguration _configuration;

        public SettingsRepository(BotDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration.GetMainConfigurationToObject();
        }

        public IQueryable<UserSetting> Get(
            int? limit = null,
            int? offset = null)
        {
            var settings = _context.UserSettings
                .Skip(offset ?? _configuration.DefaultOffset)
                .Take(limit ?? _configuration.DefaultLimit);

            return settings;
        }

        public async Task<UserSetting?> GetByUserId(long userId)
        {
            var userSettings = await Get().FirstOrDefaultAsync(x => x.UserId == userId);

            return userSettings;
        }

        public async Task<UserSetting> SaveSetting(
            long userId,
            int? preferredGender = null,
            int? preferredAge = null,
            int? preferredChatType = null)
        {
            var userSettings = await GetByUserId(userId: userId);

            if (userSettings == null)
            {
                userSettings = new UserSetting()
                {
                    UserId = userId,
                    PreferredGender = preferredGender,
                    PreferredAge = preferredAge,
                    PreferredChatType = preferredChatType
                };

                _context.UserSettings.Add(userSettings);
            }
            else
            {
                if (preferredGender.HasValue)
                {
                    userSettings.PreferredGender = preferredGender;
                }

                if (preferredAge.HasValue)
                {
                    userSettings.PreferredAge = preferredAge;
                }

                if (preferredChatType.HasValue)
                {
                    userSettings.PreferredChatType= preferredChatType;
                }

                _context.UserSettings.Update(userSettings);
            }

            await _context.SaveChangesAsync();

            return userSettings;
        }
    }
}
