using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public Transform selectedTile;

	public string[] playerPeople;

	// Use this for initialization
	void Start () {
		Instance = this;
		playerPeople = new string[32];
		playerPeople [0] = "Tim";
		for (int i = 1; i <= playerPeople.Length-1; i++) {
			playerPeople [i] = "null";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
