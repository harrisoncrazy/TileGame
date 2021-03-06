﻿using UnityEngine;
using System.Collections;

public class tileHandler : MonoBehaviour {

	public GameObject tileOutlineSprite; //gameobject of highlight Sprite
	public GameObject tileBlacked;
	public GameObject tileGreyed;
	public GameObject tileHighlighter;

	public CircleCollider2D colliderMain;
	public bool shutdown = false;

	//Tile selection values
	private Transform trSelect = null;
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

	//Tile seen values
	public bool discovered = false;
	public bool inSight = false;

	private float distToPlayer;

	public bool isNearWater = false; //if the tile is close by to water

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		colliderMain = gameObject.GetComponent<CircleCollider2D> ();
		tileGreyed.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .75f);
		tileHighlighter.GetComponent<SpriteRenderer> ().color = new Color (200f, 0f, 0f, .65f);
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.selectedTile != null) {
			trSelect = GameManager.Instance.selectedTile;
		}
		//Swapping selected tile
		if (selected == true && this.transform != trSelect) { //If the currently selected tile and the transform do not equal the new selection, deselect this tile
			selected = false;
			tileOutlineSprite.SetActive (false);
		}

		if (shutdown == true) {
			if (generationManager.Instance.homePlaced == true) {
				if (baseHandler.Instance.toggleCityUI == true) {
					selected = false;
					tileOutlineSprite.SetActive (false);
				}
			}
		}

		if (shutdown == false) { //shutting down all tile colliders at game start
			if (generationManager.Instance.genStepTwoDone) {
				StartCoroutine ("shutOffColliders");
				shutdown = true;
			}
		}

		if (discovered == false) { //If not discovered yet
			tileBlacked.SetActive (true);
		} else if (discovered == true) {
			tileBlacked.SetActive (false);
		}
		if (inSight == false && discovered == true) {//If not seen currently
			tileGreyed.SetActive (true);
		} else if (inSight == true) {
			tileGreyed.SetActive (false);

			if (UIManager.Instance.expeditionEnabled == true) {
				if (tileType != "Ocean") {
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
		}
	}

	IEnumerator shutOffColliders() {//turning off colliders after a delay
		yield return new WaitForSeconds (2.0f);
		colliderMain.enabled = false;
	}

	public void OnMouseDown() {
		if (discovered) {//if the tile has been seen and discovered
			if (UIManager.Instance.expeditionEnabled == false) {//if an expedtion is currently out
				if (UIManager.Instance.expeditionEnabled == false) {
					if (selected && transform == trSelect) {
						selected = false;
						trSelect = null;
						tileOutlineSprite.SetActive (false);
					} else {
						selected = true;
						GameManager.Instance.selectedTile = this.transform;
						tileOutlineSprite.SetActive (true);
					}
				} 
			} else if (UIManager.Instance.expeditionEnabled == true) {//seeing if the expedition is in moving mode or not
				if (expeditionHandler.Instance.isMovingMode == false) {
					if (selected && transform == trSelect) {
						selected = false;
						trSelect = null;
						tileOutlineSprite.SetActive (false);
					} else {
						selected = true;
						GameManager.Instance.selectedTile = this.transform;
						tileOutlineSprite.SetActive (true);
					}
				} else if (expeditionHandler.Instance.isMovingMode == true) {
					if (tileType != "Ocean") {
						if (Vector3.Distance (this.transform.position, expeditionHandler.Instance.expLocationTile.transform.position) <= 7.4f) {
							expeditionHandler.Instance.expLocationTile = this.gameObject;
							expeditionHandler.Instance.targetPos = transform.position;
							expeditionHandler.Instance.isMoving = true;
						}
					}
				}
			}
		}
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.gameObject.tag == "viewCollider") {
			discovered = true;
		}
		if (col.gameObject.tag == "sightCollider") {
			inSight = true;
		}

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
						col.gameObject.GetComponent<tileHandler> ().sr.sprite = generationManager.Instance.mountainTile;
						col.gameObject.GetComponent<tileHandler> ().mountainSeed = true;
						col.gameObject.GetComponent<tileHandler> ().tileType = "Mountain";
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

				if (adjacentOcean == true) {//setting any adjacent tiles to ocean tiles
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
				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Sand") {//setting adjacent tile bounderies
					if (tileType != "Sand") {
						sr.sprite = generationManager.Instance.defaultDirt;
						tileType = "Dirt";
					}
				}

				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Snow") {//setting adjacent tile bounderies
					if (tileType != "Snow") {
						sr.sprite = generationManager.Instance.defaultStone;
						tileType = "Stone";
					}
				}

				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Mountain") {//setting adjacent tile bounderies
					if (tileType != "Mountain") {
						sr.sprite = generationManager.Instance.defaultStone;
						tileType = "Stone";
					}
				}

				if (col.gameObject.GetComponent<tileHandler> ().tileType == "Ocean") {//setting tiles adjacent to water to be harvestable
					isNearWater = true;
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D col) {//leaving sight, enabling fog of war
		if (col.gameObject.tag == "sightCollider") {
			inSight = false;
		}
	}

	void OnBecameVisible() {//enabling collider when in frame of the camera
		colliderMain.enabled = true;
	}

	void OnBecameInvisible() { //disabling collider when out of frame of the camera
		colliderMain.enabled = false;
	}
}
