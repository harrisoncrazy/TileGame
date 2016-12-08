using UnityEngine;
using System.Collections;

public class tileHandler : MonoBehaviour {

	public GameObject tileOutlineSprite; //gameobject of highlight Sprite

	public PolygonCollider2D colliderMain;
	public bool shutdown = false;

	//Tile selection values
	static private Transform trSelect = null;
	private bool selected = false;

	//Generation type values
	public bool sandSeed = false;
	public bool snowSeed = false;
	public bool oceanSeed = false;
	public bool mountainSeed = false;
	public bool adjacentSand = false;
	public bool adjacentSnow = false;
	public bool adjacentOcean = false;

	//Tile info values
	public string tileType;
	public Vector2 gridPosition;

	public SpriteRenderer sr;
	public bool stopSpread = false;

	public bool newSpriteSet = false;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		colliderMain = gameObject.GetComponent<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Swapping selected tile
		if (selected && transform != trSelect) { //If the currently selected tile and the transform do not equal the new selection, deselect this tile
			selected = false;
			tileOutlineSprite.SetActive (false);
			enabled = false;
		}

		if (shutdown == false) { //shutting down all tile colliders at game start
			if (generationManager.Instance.genStepTwoDone) {
				colliderMain.enabled = false;
				shutdown = true;
			}
		}
	}

	public void OnMouseDown() {
		if (selected && transform == trSelect) {
			selected = false;
			trSelect = null;
			tileOutlineSprite.SetActive (false);
		} else {
			selected = true;
			trSelect = transform;
			tileOutlineSprite.SetActive (true);
		}
	}

	void OnTriggerStay2D (Collider2D col) {
		if (generationManager.Instance.genStepOneDone != true) {
			if (sandSeed == true) { //if the current tile is the sand seed
				col.gameObject.GetComponent<tileHandler> ().adjacentSand = true;
				col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultSand;
				col.gameObject.GetComponent<tileHandler> ().tileType = "Sand";
			}

			if (snowSeed == true) { //if the current tile is the sand seed
				col.gameObject.GetComponent<tileHandler> ().adjacentSnow = true;
				col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
				col.gameObject.GetComponent<tileHandler> ().tileType = "Snow";
			}

			if (oceanSeed == true) { //if the current tile is the sand seed
				col.gameObject.GetComponent<tileHandler> ().adjacentOcean = true;
				col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.oceanTile;
				col.gameObject.GetComponent<tileHandler> ().tileType = "Ocean";
			}

			if (mountainSeed == true) {
				if (stopSpread == false) {
					if (col.gameObject.GetComponent<tileHandler> ().mountainSeed != true) {
						col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.oceanTile;
						col.gameObject.GetComponent<tileHandler> ().mountainSeed = true;
						stopSpread = true;
					}
				}
			}

			if (stopSpread == false) {//if the current tile has not stopped spreading
				if (adjacentSand == true) {//setting any adjacent tiles to sand tiles
					col.gameObject.GetComponent<tileHandler> ().adjacentSand = true;
					col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.defaultSand;
					col.gameObject.GetComponent<tileHandler> ().tileType = "Sand";
					int stopper = Random.Range (0, 5); //stopping spread if a certain value is selected
					if (stopper >= 3) {
						stopSpread = true;
					}
				}

				if (adjacentSnow == true) {//setting any adjacent tiles to sand tiles
					col.gameObject.GetComponent<tileHandler> ().adjacentSnow = true;
					col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.GrassSnowTile;
					col.gameObject.GetComponent<tileHandler> ().tileType = "Snow";
					int stopper = Random.Range (0, 5); //stopping spread if a certain value is selected
					if (stopper >= 3) {
						stopSpread = true;
					}
				}

				if (adjacentOcean == true) {//setting any adjacent tiles to sand tiles
					col.gameObject.GetComponent<tileHandler> ().adjacentOcean = true;
					col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.oceanTile;
					col.gameObject.GetComponent<tileHandler> ().tileType = "Ocean";
					int stopper = Random.Range (0, 5); //stopping spread if a certain value is selected
					if (stopper >= 3) {
						stopSpread = true;
					}
				}
			}
			if (col.gameObject.tag != "Player") {
				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Sand") {
					if (tileType != "Sand") {
						sr.sprite = generationManager.Instance.defaultDirt;
						tileType = "Dirt";
					}
				}

				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Snow") {
					if (tileType != "Snow") {
						sr.sprite = generationManager.Instance.defaultStone;
						tileType = "Stone";
					}
				}
			}
		}
	}

	void OnBecameVisible() {
		colliderMain.enabled = true;
	}

	void OnBecameInvisible() {
		colliderMain.enabled = false;
	}
}
