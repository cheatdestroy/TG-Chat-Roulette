using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Domain.Repository.Interfaces
{
    public interface IUsersIgnored
    {
        Task<IEnumerable<IgnoreUsers>> Get(
            long? ownerId = null,
            long? targetId = null,
            int? offset = null,
            int? limit = null);
    }
}
