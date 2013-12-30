using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Retention Analytics - Clients for Unity3D */
namespace Reta 
{
	/* Analytics main class */
	public class Reta 
	{
		// Singleton for ease of access and managing resource
		protected static Reta _Instance;
		public static Reta Instance 
		{
			get  
			{ 
				if (_Instance == null) 
				{
					//Create for the first time
					_Instance = new Reta();
				}
				
				return _Instance; 
			}
		}

		//Configurations
		protected string _Key;
		protected string _ID;
		protected string _AppVersion = "UNDEFINED";
		protected bool _UsingSecureChannel = false;

		//Components
		protected Recorder _Recorder; 
		
		//Hidden constructor
		protected Reta() 
		{
			_ID = SystemInfo.deviceUniqueIdentifier;

			_Recorder = new Recorder();
		}

		#region Exposed API

		public void SetApplicationVersion(string version)
		{
			_AppVersion = version;
		}

		public void EnableSecureConnection()
		{
			_UsingSecureChannel = true;
		}

		public void StartSession(string key)
		{
			_Key = key;
		}

		public void Record(string eventName)
		{
			_Recorder.AddEvent(eventName);
		}

		public void Record(string eventName, List<Parameter> parameters)
		{
			_Recorder.AddEvent(eventName, parameters);
		}
		
		public void Record(string eventName, bool isTimed)
		{
			if (isTimed)
			{
				_Recorder.AddTimedEvent(eventName);
			}
			else Record(eventName);
		}
		
		public void Record(string eventName, List<Parameter> parameters, bool isTimed)
		{
			if (isTimed)
			{
				_Recorder.AddTimedEvent(eventName, parameters);
			}
			else Record(eventName, parameters);
		}

		public void EndTimedRecord(string eventName)
		{
			_Recorder.EndTimedEvent(eventName);
		}

		#endregion
	}
}
