using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private float speed = 4f;

	private bool movedStart = false;

	// Update is called once per frame
	void Update () {
		if (movedStart == false) {
			if (generationManager.Instance.genStepTwoDone == true) {
				StartCoroutine ("startCam");
			}
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			speed = 8f;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			speed = 4f;
		}

		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (Vector2.right * speed * Time.deltaTime); 
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (-Vector2.right * speed * Time.deltaTime);   
		}
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector2.up * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (-Vector2.up * speed * Time.deltaTime);  
		}
	}

	IEnumerator startCam(){
		yield return new WaitForSeconds (1.0f);
		GameObject.Find ("CameraMovement").transform.position = new Vector2 (generationManager.Instance.mapSizeX/2, generationManager.Instance.mapSizeY/2);
		movedStart = true;
	}
}
