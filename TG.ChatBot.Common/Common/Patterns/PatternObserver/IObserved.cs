using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TG.ChatBot.Common.Common.Pattern;

namespace TG.ChatBot.Common.Common.Patterns.PatternObserver
{
    public interface IObserved
    {
        void Watch(IObserver observer);
        void StopWatching(IObserver observer);
        void Notify(ChatRoomMediator chatRoom, long initiatorId, NotifyTypeEnum type);
    }
}
