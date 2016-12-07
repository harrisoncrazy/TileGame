using UnityEngine;
using System.Collections;

public class colDetector : MonoBehaviour {

	public Renderer rend;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.tag == "BaseTile") {
			col.transform.GetComponent<Renderer> ().material = new Material (Shader.Find ("Diffuse"));
			col.transform.GetComponent<Renderer> ().material.color = Color.cyan;
			StartCoroutine ("DestroySelf");
		}
	}

	IEnumerator DestroySelf(){
		yield return new WaitForSeconds (.00001f);
		Destroy (gameObject);
	}
}
