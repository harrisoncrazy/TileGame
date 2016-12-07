using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {

	public string sceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void LoadLevel() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (sceneToLoad);
	}
}
