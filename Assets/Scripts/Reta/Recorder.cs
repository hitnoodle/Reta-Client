using System;
using System.Collections;
using System.Collections.Generic;

namespace RetaClient 
{
	/* Module for managing recorded data 
		- Nice to have: Save and load to local data
	 */
	public class Recorder 
	{
		protected Queue<EventDatum> _EventData;
		protected List<TimedEventDatum> _TimedEventData;

		public Recorder() 
		{
			_EventData = new Queue<EventDatum>();
			_TimedEventData = new List<TimedEventDatum>();
		}

		#region Getter

		//Will return the first event datum in queue
		public EventDatum CurrentEventDatum 
		{
			get 
			{ 
				if (_EventData.Count > 1)
					return _EventData.Peek(); 
				else 
					return null;
			}
		}

		//Will return the first finished found timed event
		public TimedEventDatum FinishedTimedEvent
		{
			get
			{
				foreach(TimedEventDatum datum in _TimedEventData)
				{
					if (datum.IsFinished)
						return datum;
				}
				
				return null;
			}
		}

		#endregion

		#region Insertion

		public void AddEvent(string eventName)
		{
			EventDatum datum = new EventDatum(eventName);
			_EventData.Enqueue(datum);
		}

		public void AddEvent(string eventName, List<Parameter> parameters)
		{
			EventDatum datum = new EventDatum(eventName, parameters);
			_EventData.Enqueue(datum);
		}

		public void AddTimedEvent(string eventName)
		{
			TimedEventDatum datum = new TimedEventDatum(eventName);
			_TimedEventData.Add(datum);
		}

		public void AddTimedEvent(string eventName, List<Parameter> parameters)
		{
			TimedEventDatum datum = new TimedEventDatum(eventName, parameters);
			_TimedEventData.Add(datum);
		}

		#endregion

		#region Edit

		public bool EndTimedEvent(string eventName)
		{
			//Search for the record indicating event beginning
			foreach(TimedEventDatum datum in _TimedEventData)
			{
				if (datum.Name == eventName)
				{
					//Update duration
					datum.EndEvent();

					return true;
				}
			}

			return false;
		}

		#endregion

		#region Delete

		public void DequeueEvent()
		{
			_EventData.Dequeue();
		}

		public void DeleteTimedEvent(TimedEventDatum datum)
		{
			_TimedEventData.Remove(datum);
		}

		#endregion
	}
}
