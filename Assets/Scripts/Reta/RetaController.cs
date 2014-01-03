using UnityEngine;
using System.Collections;

namespace RetaClient 
{
	/* Manage lifecycle of Reta */
	public class RetaController : MonoBehaviour 
	{
		void Start()
		{
			Reta.Instance.Record("Reta | Session Started");
		}

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
				Reta.Instance.Record("Reta | Session Suspended");
			else 
				Reta.Instance.Record("Reta | Session Resumed");
		}
	}
}