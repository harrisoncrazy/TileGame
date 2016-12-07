using UnityEngine;
using System.Collections;

public class cityTile : MonoBehaviour {

	public static cityTile Instance;

	public GameObject tileOutlineSprite;

	public bool enabled = false;
	static private Transform trSelect = null;
	public bool selected = false;

	public float cityFoodResource = 0;
	public float cityProductionResource = 0;

	private bool updated = false;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (selected && transform != trSelect) { //If the currently selected tile and the transform do not equal the new selection, deselect this tile
			selected = false;
			tileOutlineSprite.SetActive (false);
			enabled = false;
			GameManager.Instance.tileSelected = false;
		}
	}

	public void OnMouseDown() {
		if (selected && transform == trSelect) {
			selected = false;
			trSelect = null;
			tileOutlineSprite.SetActive (false);
			enabled = false;
			GameManager.Instance.tileSelected = false;
			GameManager.Instance.tileInfo.text = " ";
		} else {
			selected = true;
			trSelect = transform;
			tileOutlineSprite.SetActive (true);
			enabled = true;
			GameManager.Instance.tileSelected = true;
			GameManager.Instance.tileInfo.text = "City Food Resource: " + cityFoodResource + "\nCity Production Resource: " + cityProductionResource;
		}
	}	
}
