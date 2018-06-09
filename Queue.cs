using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulator
{
    public class Queue
    {
        private Queue<Event> _EventQueue = new Queue<Event>();

        public Queue()
        {

        }

		public void PutEvent(Event Event)
        {
            _EventQueue.Enqueue(Event);
            _EventQueue = new Queue<Event>(_EventQueue.OrderBy(item => item.ComingTime));
        }

		public Event GetEvent()
		{
			return _EventQueue.Dequeue();
		}

		public void Write()
		{
			Console.WriteLine(_EventQueue.Count);
		}
	}
}
