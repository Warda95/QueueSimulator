using System;
using System.Collections.Generic;
using System.IO;

namespace QueueSimulator
{
	public class System
	{
		public double Lambda = 5;
		public double Mi = 20;
		public double Time = 0;
		public double TimeOfComingOut = 0;
		public double TimeOfComingOutOfPreviousPacket = 0;
		public bool IsSystemOccupied = false;
		public double NumberOfPacket = 0;
		public double NumberOfPackets = 100000;
		public double TimeInSystem = 0;
		public double AllTimesInSystem = 0;
		public int IterationsForSystem = 0;
		public double TimeSpentInQueue = 0;
		public double AllTimesSpentInQueue = 0;
		public int IterationsForQueue = 0;
		public bool IsFirst = true;
		public double StartTime = 0;
		public double CountingTime = 0;
		public Random random = new Random();

		public void Simulation()
		{
			Queue queue = new Queue();
			Event Event = null;
			Time = GenerateEventTime(random.NextDouble(), Lambda);
			queue.PutEvent(GenerateEvent(EventType.Incoming, Time)); //PUT pierwszego zdarzenia

			while (NumberOfPacket < NumberOfPackets)
			{
				Event = GetEventFromQueue(queue);

				if (Event.EventType == EventType.Incoming && IsSystemOccupied)
				{
					IsSystemOccupied = true;
					TimeOfComingOutOfPreviousPacket = queue.GetTimeOfSystemFree(); //czas wyjscia poprzedniego pakietu = czas wejscia tego
					TimeOfComingOut = TimeOfComingOut + GenerateEventTime(random.NextDouble(), Mi); //czas wyjscia z systemu

					if (NumberOfPacket < 0.9 * NumberOfPackets && NumberOfPacket > 0.1 * NumberOfPackets)
					{
						if (IsFirst)
						{
							StartTime = Time;
							IsFirst = false;
						}

						TimeInSystem = TimeOfComingOut - TimeOfComingOutOfPreviousPacket; //czas mielenia przez system
						AllTimesInSystem += TimeInSystem;
						IterationsForSystem++;

						TimeSpentInQueue = TimeOfComingOutOfPreviousPacket - Time; //czas w kolejce
						AllTimesSpentInQueue += TimeSpentInQueue;
						IterationsForQueue++;

						CountingTime = Time - StartTime;
					}
					
					queue.PutEvent(GenerateEvent(EventType.Outgoing, TimeOfComingOut)); //obsluga wyjscia pakietu w systemie zajetym
				}
				else if (Event.EventType == EventType.Incoming && !IsSystemOccupied)
				{
					IsSystemOccupied = true;
					TimeOfComingOut = Time + GenerateEventTime(random.NextDouble(), Mi); //czas wyjscia z systemu

					if (NumberOfPacket < 0.9 * NumberOfPackets && NumberOfPacket > 0.1 * NumberOfPackets)
					{
						if (IsFirst)
						{
							StartTime = Time;
							IsFirst = false;
						}

						TimeInSystem = TimeOfComingOut - Time; //czas mielenia przez system
						AllTimesInSystem += TimeInSystem;
						IterationsForSystem++;

						TimeSpentInQueue = 0; //czas w kolejce
						AllTimesSpentInQueue += TimeSpentInQueue;
						IterationsForQueue++;

						CountingTime = Time - StartTime;
					}
					
					queue.PutEvent(GenerateEvent(EventType.Outgoing, TimeOfComingOut)); //obsluga wyjscia pakietu w systemie wolnym
				}
				else if (Event.EventType == EventType.Outgoing && queue.GetTimeOfSystemFree() == 0)
				{
					IsSystemOccupied = false;
				}
				//Console.WriteLine("Packet: " + NumberOfPacket);
			}

			string text =
				"N = " + (AllTimesInSystem / CountingTime) + Environment.NewLine +
				"Nfi = " + (AllTimesSpentInQueue / CountingTime) + Environment.NewLine +
				"T = " + (AllTimesInSystem / IterationsForSystem) + Environment.NewLine +
				"W = " + (AllTimesSpentInQueue / IterationsForQueue) + Environment.NewLine;
			
			File.WriteAllText(@"C:\Users\Warda\Desktop\Projekt\result.txt", text);

			Console.WriteLine("Average value of packets in system: " + AllTimesInSystem / CountingTime);
			Console.WriteLine("Average value of packets in queue: " + AllTimesSpentInQueue / CountingTime);
			Console.WriteLine("Time in system: " + AllTimesInSystem / IterationsForSystem);
			Console.WriteLine("Time in queue: " + AllTimesSpentInQueue / IterationsForQueue);
			Console.ReadLine();
		}

		public Event GetEventFromQueue(Queue queue)
		{
			Event Event = queue.GetEvent();
			Time = Event.ComingTime;
			if (Event.EventType == EventType.Incoming)
				queue.PutEvent(GenerateEvent(EventType.Incoming, Time + GenerateEventTime(random.NextDouble(), Lambda)));
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

		public double GenerateEventTimeConstantIncome(double seed, double parametr)
		{
			return parametr;
		}
	}
}
