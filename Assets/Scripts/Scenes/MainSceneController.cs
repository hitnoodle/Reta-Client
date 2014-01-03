using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RetaClient;

public class MainSceneController : MonoBehaviour 
{
	protected const int LOG_LINES = 4;

	protected User _User;
	protected Queue<string> _Logs;

	// Use this for initialization
	void Start () 
	{
		_User = User.LoadRandomUser();
		if (_User == null)
		{
			User.GenerateFirstTimeUsers();
			_User = User.LoadRandomUser();
		}

		_Logs = new Queue<string>();

		Reta.Instance.SetApplicationVersion("0.1");
		Reta.Instance.SetUserID(_User.Name);

		Reta.Instance.SetDebugMode(true);
		Reta.Instance.onDebugLog += DebugLog;
	}

	void DebugLog(string log)
	{
		if (_Logs.Count > LOG_LINES)
			_Logs.Dequeue();

		_Logs.Enqueue(log);
	}

	void OnGUI () {
		GUI.BeginGroup(new Rect(10, 10, 780, 105));

		// User Info
		GUI.Box(new Rect(0,0,120,105), "User Info");
		GUI.Label(new Rect(10, 20, 100, 25), "Name: " + _User.Name);
		GUI.Label(new Rect(10, 40, 100, 25), "Level: " + _User.Level);
		GUI.Label(new Rect(10, 60, 100, 25), "Progress: " + _User.Progress);
		GUI.Label(new Rect(10, 80, 100, 25), "Tutorial: " + _User.LastFinishedTutorial);

		//Log
		GUI.Box(new Rect(130,0,650,105), "Application Log");

		string[] logs = _Logs.ToArray();
		int loglen = logs.Length;
		for(int i=0;i<loglen;i++)
		{
			int len = logs[i].Length;
			string log;
			if (len > 100)
			{
				log = logs[i].Substring(0, 100);
				log += " ...";
			} else log = logs[i];

			GUI.Label(new Rect(140, (i + 1) * 20, 630, 25), log);
		}

		GUI.EndGroup();
	}
}
