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

		StartCoroutine(Test());
	}

	IEnumerator Test() {
		yield return new WaitForSeconds(1);

		Reta.Instance.Record("Game began.");

		yield return new WaitForSeconds(1);

		Reta.Instance.Record("Game ended.");
	}
}
