using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
	[SerializeField]
	string[] scenesToInitiate;

	void Awake()
	{
		foreach (string scene in this.scenesToInitiate)
		{
			if (!SceneManager.GetSceneByName(scene).IsValid())
				SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		}
	}
}