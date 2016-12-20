﻿using System.Collections;
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
		if (UIManager.Instance.expeditionEnabled == true) {
			if (expeditionHandler.Instance.isMovingMode != true) {
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
