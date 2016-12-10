using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private float speed = 6f;

	private bool movedStart = false;

	// Update is called once per frame
	void Update () {
		if (movedStart == false) {
			if (generationManager.Instance.genStepTwoDone == true) {
				StartCoroutine ("startCam");
			}
		}

		if (generationManager.Instance.genStepTwoDone == true) {
			if (baseHandler.Instance.toggleCityUI == true) {//moving camera to city if city selected
				Vector2 pos = GameObject.Find ("homeBase").transform.position;
				transform.position = pos;
			}
		

			if (baseHandler.Instance.toggleCityUI == false) { //stopping movement if city selected
				if (Input.GetKeyDown (KeyCode.LeftShift)) {
					speed = 12f;
				}

				if (Input.GetKeyUp (KeyCode.LeftShift)) {
					speed = 6f;
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
		}
	}

	IEnumerator startCam(){
		yield return new WaitForSeconds (1.0f);
		Vector2 outPos = transform.position;
		Vector2 pos = GameObject.Find ("homeBase").transform.position;
		GameObject.Find ("CameraMovement").transform.position = pos;
		GameObject.Find ("CameraMovement").transform.position = outPos;
		yield return new WaitForSeconds (2.0f);
		GameObject.Find ("CameraMovement").transform.position = pos;
		movedStart = true;
	}
}
