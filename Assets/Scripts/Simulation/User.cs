using UnityEngine;
using System.Collections;

public class User
{
	public const int TUTORIAL_STEPS = 3;

	public string Name;
	public int Level;
	public float Progress;
	public bool[] FinishedTutorial;

	public int LastFinishedTutorial
	{
		get 
		{
			for(int i=TUTORIAL_STEPS-1;i>=0;i--)
				if (FinishedTutorial[i]) return i;
			return 0;
		}
	}

	public User()
	{
		Name = "";
		Level = 0;
		Progress = 0f;
		FinishedTutorial = new bool[TUTORIAL_STEPS];
	}

	public User(string name)
	{
		Name = name;
		Level = 0;
		Progress = 0f;
		FinishedTutorial = new bool[TUTORIAL_STEPS];
	}

	public void Save()
	{
		XmlManager.SaveInstanceAsXml(Name + ".xml", typeof(User), this);
	}

	public void LevelUp()
	{
		Level++;
	}

	public void ProgressUp(float progression, bool random)
	{
		float progress = (random) ? Random.Range(0, progression) : progression;
		Progress += progress;
	}

	public void FinishTutorial(int tutorial)
	{
		if (tutorial > -1 && tutorial < TUTORIAL_STEPS)
			FinishedTutorial[tutorial] = true;
	}

	#region Static for simulations 

	public static void GenerateFirstTimeUsers()
	{
		string[] names = new string[] {"Lucy", "Karu", "Andy", "Suarez", "Nia"
			, "Hasebe", "UserA", "UserB", "UserC", "Sai"};

		foreach(string name in names)
		{
			User u = new User(name);
			u.Save();
		}
	}

	public static User LoadRandomUser()
	{
		string[] names = new string[] {"Lucy", "Karu", "Andy", "Suarez", "Nia"
			, "Hasebe", "UserA", "UserB", "UserC", "Sai"};

		string name = names[Random.Range(0, names.Length - 1)];
		User u = (User)XmlManager.LoadInstanceAsXml(name + ".xml", typeof(User));

		return u;
	}

	#endregion
}
