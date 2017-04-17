using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseHandler : MonoBehaviour {

	public static baseHandler Instance;
	public GameObject cityUIScreen;

	public GameObject tileHighlighter;

	public Vector3 baseLocation;

	public bool toggleCityUI = false;

	private float distToPlayer;

	// Use this for initialization
	void Start () {
		Instance = this;
		cityUIScreen = GameObject.Find ("cityPanel");
		cityUIScreen.SetActive (false);

		tileHighlighter.GetComponent<SpriteRenderer> ().color = new Color (200f, 0f, 0f, .65f);
	}
	
	// Update is called once per frame
	void Update () {
		if (UIManager.Instance.expeditionEnabled == true) {
			distToPlayer = Vector3.Distance (expeditionHandler.Instance.transform.position, transform.position);

			if (expeditionHandler.Instance.isMovingMode == true && expeditionHandler.Instance.isMoving == false) {
				if (distToPlayer <= 7.4f) {
					tileHighlighter.SetActive (true);
				} else {
					tileHighlighter.SetActive (false);
				}
			}

			if (expeditionHandler.Instance.isMovingMode == false) {
				if (tileHighlighter.activeInHierarchy == true) {
					tileHighlighter.SetActive (false);
				}
			}

			if (expeditionHandler.Instance.hasMoved == true) {
				if (tileHighlighter.activeInHierarchy == true) {
					tileHighlighter.SetActive (false);
				}
			}
		}
	}

	void OnMouseDown() {
		if (UIManager.Instance.expeditionEnabled == false) {
			if (toggleCityUI == false) {//toggling the base ui on
				cityUIScreen.SetActive (true);
				toggleCityUI = true;
				UIManager.Instance.expeditionPanel.SetActive (false);
				if (UIManager.Instance.expeditionEnabled == true) {
					expeditionHandler.Instance.expOutline.SetActive (false);
					expeditionHandler.Instance.isSelectedMode = false;
				}
			} else if (toggleCityUI == true) {//toggling base off
				cityUIScreen.SetActive (false);
				toggleCityUI = false;
				UIManager.Instance.expeditionPanel.SetActive (false);
				if (UIManager.Instance.expeditionEnabled == true) {//disabling the outline on the expedition if it is enabled
					expeditionHandler.Instance.expOutline.SetActive (false);
				}
			}
		} else if (expeditionHandler.Instance.isMovingMode == true) {//if the expedition is in moving mode, move to the home base tile
			expeditionHandler.Instance.targetPos = transform.position;
			expeditionHandler.Instance.isMoving = true;
			expeditionHandler.Instance.expLocationTile = this.gameObject;
		}
		if (UIManager.Instance.expeditionEnabled == true) {
			if (expeditionHandler.Instance.isMovingMode != true) {//setting the interaction if the expedition is enabled
				if (toggleCityUI == false) {
					cityUIScreen.SetActive (true);
					toggleCityUI = true;
					UIManager.Instance.expeditionPanel.SetActive (false);
					if (UIManager.Instance.expeditionEnabled == true) {
						expeditionHandler.Instance.expOutline.SetActive (false);
						expeditionHandler.Instance.isSelectedMode = false;
					}
				} else if (toggleCityUI == true) {
					cityUIScreen.SetActive (false);
					toggleCityUI = false;
					UIManager.Instance.expeditionPanel.SetActive (false);
					if (UIManager.Instance.expeditionEnabled == true) {
						expeditionHandler.Instance.expOutline.SetActive (false);
					}
				}
			}
		}
	}
}
