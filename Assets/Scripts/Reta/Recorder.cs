using System;
using System.Collections;
using System.Collections.Generic;

namespace Reta 
{
	/* Module for managing recorded data */
	public class Recorder 
	{
		protected Queue<EventDatum> _EventData;
		protected List<TimedEventDatum> _TimedEventData;

		public Recorder() 
		{
			_EventData = new Queue<EventDatum>();
			_TimedEventData = new List<TimedEventDatum>();
		}

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

		public bool EndTimedEvent(string eventName)
		{
			//Search for the record indicating event beginning
			bool found = false;
			foreach(TimedEventDatum datum in _TimedEventData)
			{
				if (datum.Name == eventName)
				{
					//Update duration
					datum.EndEvent();
					found = true;

					break;
				}
			}

			return found;
		}
	}
}
