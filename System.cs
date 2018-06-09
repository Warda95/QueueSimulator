using System;
using System.Collections.Generic;

namespace QueueSimulator
{
	public class System
	{
		public Random Random = new Random();
		public double Lambda = 1;
		public double Mi = 10;
		public int SimulationTime = 3;
		public double Time = 0;
		public double TimeOfComingOut = 0;
		public double TimeOfComingOutOfPreviousPacket = 0;
		public bool IsSystemOccupied = false;
		public double NumberOfPacket = 0;
		public double NumberOfPackets = 10000;
		public double NumberOfPacketsIncome = 0;
		public double NumberOfPacketsServed = 0;
		public double TimeInSystem = 0;
		public double AllTimesInSystem = 0;
		public int IterationsForSystem = 0;
		public double TimeSpentInQueue = 0;
		public double AllTimesSpentInQueue = 0;
		public int IterationsForQueue = 0;

		public void Simulation()
		{
			Queue queue = new Queue();
			Event Event = null;
			Time = GenerateEventTime(Random.NextDouble(), Lambda);
			queue.PutEvent(GenerateEvent(EventType.Incoming, Time)); //PUT pierwszego zdarzenia
			
			while (NumberOfPacket < NumberOfPackets)
			{
				if (NumberOfPacket == 0.01*NumberOfPackets)
				{
					NumberOfPacketsIncome = 0;
					NumberOfPacketsServed = 0;

					TimeInSystem = 0;
					AllTimesInSystem = 0;
					IterationsForSystem = 0;

					TimeSpentInQueue = 0;
					AllTimesSpentInQueue = 0;
					IterationsForQueue = 0;
				}

				Event = GetEventFromQueue(queue);

				if (Event.EventType == EventType.Incoming && IsSystemOccupied)
				{
					IsSystemOccupied = true;
					NumberOfPacketsIncome++;
					TimeOfComingOutOfPreviousPacket = TimeOfComingOut; //czas wyjscia poprzedniego pakietu = czas wejscia tego
					TimeOfComingOut = TimeOfComingOut + GenerateEventTime(Random.NextDouble(), Mi); //czas wyjscia z systemu

					TimeInSystem = TimeOfComingOut - TimeOfComingOutOfPreviousPacket; //czas mielenia przez system
					AllTimesInSystem += TimeInSystem;
					IterationsForSystem++;

					TimeSpentInQueue = TimeOfComingOutOfPreviousPacket - Time; //czas w kolejce
					AllTimesSpentInQueue += TimeSpentInQueue;
					IterationsForQueue++;

					queue.PutEvent(GenerateEvent(EventType.Outgoing, TimeOfComingOut)); //obsluga wyjscia pakietu w systemie zajetym
				}
				else if (Event.EventType == EventType.Incoming && !IsSystemOccupied)
				{
					IsSystemOccupied = true;
					NumberOfPacketsIncome++;
					TimeOfComingOut = Time + GenerateEventTime(Random.NextDouble(), Mi); //czas wyjscia z systemu

					TimeInSystem = TimeOfComingOut - Time; //czas mielenia przez system
					AllTimesInSystem += TimeInSystem;
					IterationsForSystem++;

					queue.PutEvent(GenerateEvent(EventType.Outgoing, TimeOfComingOut)); //obsluga wyjscia pakietu w systemie wolnym
				}
				else if (Event.EventType == EventType.Outgoing && TimeOfComingOut == Time)
				{
					IsSystemOccupied = false;
					NumberOfPacketsServed++;
				}
				else if (Event.EventType == EventType.Outgoing)
				{
					NumberOfPacketsServed++;
				}

				Console.WriteLine(Time);
				//Console.WriteLine("Income: " + NumberOfPacketsIncome);
				//Console.WriteLine("Served: " + NumberOfPacketsServed);
			}
			Console.WriteLine("Served/Income: " + NumberOfPacketsServed / NumberOfPacketsIncome);
			Console.WriteLine("Time in system: " + AllTimesInSystem / IterationsForSystem);
			Console.WriteLine("Time in queue: " + AllTimesSpentInQueue / IterationsForQueue);
			Console.ReadLine();
		}

		public Event GetEventFromQueue(Queue queue)
		{
			Event Event = queue.GetEvent();
			Time = Event.ComingTime;
			queue.PutEvent(GenerateEvent(EventType.Incoming, Time + GenerateEventTime(Random.NextDouble(), Lambda)));
			NumberOfPacket++;
			return Event;
		}

		public Event GenerateEvent(EventType eventType, double time)
		{
			return new Event(eventType, time);
		}

		public double GenerateEventTime(double seed, double parametr)
		{
			return ((-1) * Math.Log(1 - seed) / parametr);
		}
	}
}
