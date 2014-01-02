using System;
using System.Collections;
using System.Collections.Generic;

using MiniJSON;

namespace RetaClient
{
	/* Simple key value pair for holding parameter data */
	public class Parameter 
	{
		protected string _Key;
		public string Key 
		{
			get { return _Key; } 
		}
		
		protected string _Value;
		public string Value 
		{
			get { return _Value; }
		}
		
		public Parameter()
		{
			_Key = "";
			_Value = "";
		}
		
		public Parameter(string key, string value)
		{
			_Key = key;
			_Value = value;
		}

		//JSON formatted string
		public override string ToString ()
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add(_Key, _Value);

			return Json.Serialize(dict);
		}
	}
	
	/* Class for holding event datum */
	public class EventDatum
	{
		protected string _Name;
		public string Name
		{
			get { return _Name; } 
		}

		protected List<Parameter> _Parameters;
		protected DateTime _Time;
		
		public EventDatum()
		{
			_Name = "";
			_Parameters = null;
			_Time = DateTime.Now;
		}

		public EventDatum(string name)
		{
			_Name = name;
			_Parameters = null;
			_Time = DateTime.Now;
		}

		public EventDatum(string name, List<Parameter> parameters)
		{
			_Name = name;
			_Parameters = parameters;
			_Time = DateTime.Now;
		}

		//JSON formatted string
		public override string ToString()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict.Add("Name",_Name);

			if (_Parameters != null) {
				List<string> paramStrings = new List<string>();
				foreach(Parameter param in _Parameters) {
					if (param != null) 
						paramStrings.Add(param.ToString());
				}

				dict.Add("Parameter", paramStrings);
			}

			dict.Add("Time", _Time.ToString()); //TODO: Check what time should be converted at

			return Json.Serialize(dict);
		}
	}

	/* Class for holding timed event datum */
	public class TimedEventDatum : EventDatum
	{
		protected TimeSpan _Duration;
		public bool IsFinished 
		{
			get { return _Duration != TimeSpan.Zero; }
		}

		public TimedEventDatum() : base()
		{
			_Duration = TimeSpan.Zero;
		}

		public TimedEventDatum(string name) : base(name)
		{
			_Duration = TimeSpan.Zero;
		}

		public TimedEventDatum(string name, List<Parameter> parameters) : base(name, parameters)
		{
			_Duration = TimeSpan.Zero;
		}

		public void EndEvent()
		{
			_Duration = DateTime.Now - _Time;
		}

		public void EndEvent(DateTime endTime)
		{
			_Duration = endTime - _Time;
		}

		//JSON formatted string
		public override string ToString()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict.Add("Name",_Name);

			if (_Parameters != null) {
				List<string> paramStrings = new List<string>();
				foreach(Parameter param in _Parameters) {
					if (param != null) 
						paramStrings.Add(param.ToString());
				}

				dict.Add("Parameter", paramStrings);
			}

			dict.Add("Time", _Time.ToString()); //TODO: Check what time should be converted at
			dict.Add("Duration", _Duration);
			
			return Json.Serialize(dict);
		}
	}
}
