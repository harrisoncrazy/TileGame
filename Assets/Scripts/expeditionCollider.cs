﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expeditionCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.gameObject.tag == "BaseTile") {
			col.GetComponent<Renderer> ().material.color = Color.red;
		}
	}
}
