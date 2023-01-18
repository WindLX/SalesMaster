using System;
using System.Collections.Generic;

namespace SalesMaster.Utils
{
    public class Broadcaster : Singleton<Broadcaster>
    {
        private Dictionary<string, Action<object>> notifier;

        public Broadcaster()
        {
            notifier = new Dictionary<string, Action<object>>();
        }

        public void Publish(object parameter, string eventID)
        {
            if (!notifier.ContainsKey(eventID))
                notifier.Add(eventID, new Action<object>((para) => para = 1));
            notifier[eventID]?.Invoke(parameter);
        }

        public void Subscribe(Action<object> action, string eventID)
        {
            if (!notifier.ContainsKey(eventID))
                notifier.Add(eventID, new Action<object>(action));
            else
                notifier[eventID] += action;
        }
    }
}
