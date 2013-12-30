using UnityEngine;
using System.Collections;

using RetaClient;

public class MainSceneController : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		Reta.Instance.SetApplicationVersion("0.1");
		Reta.Instance.SetDebugMode(true);

		Reta.Instance.StartSession();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
