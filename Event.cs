using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulator
{
	/// <summary>
	/// Event of packet coming to queue.
	/// </summary>
    public class Event
    {
        public EventType EventType;
        public double ComingTime;

		public Event() { }

        public Event(EventType eventType, double comingTime)
        {
            EventType = eventType;
			ComingTime = comingTime;
		}
    }
}
