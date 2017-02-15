using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseHandler : MonoBehaviour {

	public static baseHandler Instance;
	public GameObject cityUIScreen;

	public Vector3 baseLocation;

	public bool toggleCityUI = false;

	// Use this for initialization
	void Start () {
		Instance = this;
		cityUIScreen = GameObject.Find ("cityPanel");
		cityUIScreen.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
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
