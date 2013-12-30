using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Retention Analytics - Clients for Unity3D */
namespace RetaClient 
{
	/* Analytics main class 
		TODO: Pause-resume support
	 */
	public class Reta 
	{
		public static bool DEBUG_ENABLED = false;

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

		//Object which represents the service
		protected GameObject _GameObject;

		//Components
		protected Recorder _Recorder; 
		protected Connector _Connector;

		//Temp
		TimedEventDatum _TempTimedEventDatum;
		
		//Hidden constructor
		protected Reta() 
		{
			_GameObject = new GameObject();
			_GameObject.transform.position = Vector3.zero;
			_GameObject.name = "_Reta";

			//TODO: Check this shit, leak-prone
			GameObject.DontDestroyOnLoad(_GameObject); 

			_Recorder = new Recorder();
			_Connector = _GameObject.AddComponent<Connector>();
		}

		#region Delegates

		protected void EventSendingSucceed(string result)
		{
			_Connector.onSendingSucceed -= EventSendingSucceed;
			_Connector.onSendingFailed -= EventSendingFailed;

			//Check whether result is OK
			_Recorder.DequeueEvent();

			//Go again
			ProcessEventData();
		}

		protected void EventSendingFailed(string error)
		{
			_Connector.onSendingSucceed -= EventSendingSucceed;
			_Connector.onSendingFailed -= EventSendingFailed;
		}

		protected void TimedEventSendingSucceed(string result)
		{
			_Connector.onSendingSucceed -= TimedEventSendingSucceed;
			_Connector.onSendingFailed -= TimedEventSendingFailed;

			//Check whether result is OK
			_Recorder.DeleteTimedEvent(_TempTimedEventDatum);
			
			//Go again
			ProcessTimedEventData();
		}

		protected void TimedEventSendingFailed(string error)
		{
			_Connector.onSendingSucceed -= TimedEventSendingSucceed;
			_Connector.onSendingFailed -= TimedEventSendingFailed;
		}

		#endregion

		#region Protected Event Processing

		protected void ProcessEventData()
		{
			EventDatum datum = _Recorder.CurrentEventDatum;

			if (datum != null)
			{
				_Connector.onSendingSucceed += EventSendingSucceed;
				_Connector.onSendingFailed += EventSendingFailed;
				
				_Connector.SendData(datum.ToString());
			}
		}

		protected void ProcessTimedEventData()
		{
			TimedEventDatum datum = _Recorder.FinishedTimedEvent;

			if (datum != null)
			{
				_TempTimedEventDatum = datum;

				_Connector.onSendingSucceed += TimedEventSendingSucceed;
				_Connector.onSendingFailed += TimedEventSendingFailed;

				_Connector.SendData(datum.ToString());
			}
		}

		#endregion

		#region Exposed API

		public void SetDebugMode(bool debug)
		{
			DEBUG_ENABLED = debug;
		}

		public void SetApplicationVersion(string version)
		{
			_Connector.AppVersion = version;
		}

		public void EnableSecureConnection()
		{
			_Connector.UsingSecureChannel = true;
		}

		public void StartSession()
		{
			ProcessEvents();
		}

		public void ProcessEvents()
		{
			ProcessEventData();
			ProcessTimedEventData();
		}

		public void Record(string eventName)
		{
			_Recorder.AddEvent(eventName);
			ProcessEventData();
		}

		public void Record(string eventName, List<Parameter> parameters)
		{
			_Recorder.AddEvent(eventName, parameters);
			ProcessEventData();
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
			ProcessTimedEventData();
		}

		#endregion
	}
}
