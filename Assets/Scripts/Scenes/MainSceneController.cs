using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RetaClient;

public class MainSceneController : MonoBehaviour 
{
	protected const int LOG_LINES = 4;

	protected User _User;
	protected Queue<string> _Logs;

	protected bool _IsQuestStarted = false;
	protected int _CurrentTutorialStep = 0;

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
		Debug.Log(log);

		if (_Logs.Count > LOG_LINES)
			_Logs.Dequeue();

		_Logs.Enqueue(log);
	}

	#region UI

	protected Vector2 _ScrollPosition;

	void OnGUI () {
		//Dumb handling
		if (_User == null) 
			return;

		GUI.BeginGroup(new Rect(10, 10, 780, 105));

		//User Info
		GUI.Box(new Rect(0,0,120,105), "User Info");
		GUI.Label(new Rect(10, 20, 100, 25), "Name: " + _User.Name);
		GUI.Label(new Rect(10, 40, 100, 25), "Level: " + _User.Level);
		GUI.Label(new Rect(10, 60, 100, 25), "Progress: " + _User.Progress.ToString("0.00"));
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

		GUI.BeginGroup(new Rect(10, 125, 780, 355));

		_ScrollPosition = GUILayout.BeginScrollView(_ScrollPosition, 
		                                            GUILayout.Width(780), 
		                                            GUILayout.Height(355));

		//Game features
		GUILayout.Box("Game Features", GUILayout.Height(25));

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();

		GUI.enabled = !_IsQuestStarted;

		if (GUILayout.Button("Start Quest", GUILayout.Height(70)))
		{
			_IsQuestStarted = true;

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Progression", _User.Progress.ToString("0.00")));

			Reta.Instance.Record("Quest Started", parameters, true);
		}

		GUI.enabled = _IsQuestStarted;

		if (GUILayout.Button("End Quest", GUILayout.Height(70)))
		{
			_IsQuestStarted = false;

			_User.ProgressUp(5, true);
			_User.Save();

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Progression", _User.Progress.ToString("0.00")));

			Reta.Instance.EndTimedRecord("Quest Started", parameters);
		}

		GUI.enabled = true;

		if (GUILayout.Button("Upgrade Stuffs", GUILayout.Height(70)))
		{
			Reta.Instance.Record("Stuffs Upgraded");
		}

		if (GUILayout.Button("Level Up", GUILayout.Height(70)))
		{
			if (_User.Level > 0)
			{
				_User.ProgressUp(3, true);

				List<Parameter> p = new List<Parameter>();
				p.Add(new Parameter("Progression", _User.Progress.ToString("0.00")));

				Reta.Instance.EndTimedRecord("Level Duration", p);
			}

			_User.LevelUp();
			_User.Save();

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("User Level", _User.Level.ToString()));
			parameters.Add(new Parameter("Progression", _User.Progress.ToString("0.00")));

			Reta.Instance.Record("Level Up", parameters);
			Reta.Instance.Record("Level Duration", parameters, true);
		}

		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		//Social features
		GUILayout.Box("Social Features", GUILayout.Height(25));

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Invite Friends", GUILayout.Height(70)))
		{
			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Feature", "Invite Friends"));
			
			Reta.Instance.Record("Social Feature Consumed", parameters);
		}

	    if (GUILayout.Button("Share Progress", GUILayout.Height(70)))
		{
			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Feature", "Sharing Progress"));
			
			Reta.Instance.Record("Social Feature Consumed", parameters);
		}

	    if (GUILayout.Button("Ask Help from Friends", GUILayout.Height(70)))
		{
			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Feature", "Ask Help"));
			
			Reta.Instance.Record("Social Feature Consumed", parameters);
		}

	    if (GUILayout.Button("Lend Help to Friends", GUILayout.Height(70)))
		{
			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Feature", "Lend Help"));
			
			Reta.Instance.Record("Social Feature Consumed", parameters);
		}

		GUILayout.EndHorizontal();
		
		GUILayout.Space(5);

		//Tutorials
		int tutorial = _User.LastFinishedTutorial + 1;
		if (tutorial == User.TUTORIAL_STEPS)
			GUI.enabled = false;

		GUILayout.Box("Tutorial " + tutorial, GUILayout.Height(25));

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();

		GUI.enabled = (_CurrentTutorialStep == 0);

		if (GUILayout.Button("Start Tutorial | Step 1", GUILayout.Height(80)))
		{
			_CurrentTutorialStep = 1;

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Tutorial", _User.LastFinishedTutorial.ToString()));

			Reta.Instance.Record("Tutorial Duration", parameters, true);

			parameters.Add(new Parameter("Step", _CurrentTutorialStep.ToString()));
			Reta.Instance.Record("Tutorial Step Duration", parameters, true);
		}

		GUI.enabled = (_CurrentTutorialStep == 1);

		if (GUILayout.Button("Step 2", GUILayout.Height(80)))
		{
			_CurrentTutorialStep = 2;

			Reta.Instance.EndTimedRecord("Tutorial Step Duration");

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Step", _CurrentTutorialStep.ToString()));
			Reta.Instance.Record("Tutorial Step Duration", parameters, true);
		}

		GUI.enabled = (_CurrentTutorialStep == 2);
		
		if (GUILayout.Button("Step 3", GUILayout.Height(80)))
		{
			_CurrentTutorialStep = 3;

			Reta.Instance.EndTimedRecord("Tutorial Step Duration");
			
			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Step", _CurrentTutorialStep.ToString()));
			Reta.Instance.Record("Tutorial Step Duration", parameters, true);
		}

		GUI.enabled = (_CurrentTutorialStep == 3);
		
		if (GUILayout.Button("End Tutorial", GUILayout.Height(80)))
		{
			_CurrentTutorialStep = 0;

			Reta.Instance.EndTimedRecord("Tutorial Step Duration");

			_User.FinishTutorial(_User.LastFinishedTutorial + 1);
			_User.ProgressUp(3, false);
			_User.Save();

			List<Parameter> parameters = new List<Parameter>();
			parameters.Add(new Parameter("Tutorial", _User.LastFinishedTutorial.ToString()));
			parameters.Add(new Parameter("Progression", _User.Progress.ToString("0.00")));

			Reta.Instance.EndTimedRecord("Tutorial Duration", parameters); 
		}
		
		GUILayout.EndHorizontal();

		if (!GUI.enabled) GUI.enabled = true;

		GUILayout.EndScrollView();

		GUI.EndGroup();
	}

	#endregion
}
