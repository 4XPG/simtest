﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FCR : MonoBehaviour {
	//public Camera FCRCamera;
	public GameObject playerObject;
	public GameObject[] airObjects;
	public GameObject[] groundObjects;
	public RadarIcon[] airIcons;
	public Transform[] groundpositions;
    public List<GameObject> airObjectList;
    public List<RadarIcon> mapenemies;
    public List<GameObject> groundObjectList;
	public Image FCRAirCursor;
	public Image FCRGroundCursor;
	private Vector3 CameraPos;

    public Text radarZoomText;
    private int RadarzoomLevelSelected = 1;
    private float[] RadarZoomLevels = new float[] { 60, 4, 20 };
    private int zoomchange = 0;  //<<<<<<<<<<<<<
	private Vector3 offset;
	private int cullmask;

    public GameObject radar;

    public RectTransform map;
    public List<Bogey> mapEnemies;
    public List<GameObject> enemies;

	// Use this for initialization
	void Start () {
        StartCoroutine (UpdateMapPos());
		CameraPos.x = transform.position.x - playerObject.transform.position.x;
		CameraPos.z = transform.position.z - playerObject.transform.position.z;
        airObjects = GameObject.FindGameObjectsWithTag ("Air");
		groundObjects = GameObject.FindGameObjectsWithTag ("Ground");
		foreach (GameObject aiplanes in airObjects){
            airObjectList.Add(aiplanes);
        }
        airIcons = FindObjectsOfType(typeof(RadarIcon)) as RadarIcon[];
		foreach (RadarIcon radar in airIcons) {
            mapenemies.Add(radar);
		}
	}
	
	// Update is called once per frame
	void Update () {
        if(playerObject.transform.position!=radar.transform.position){
//Debug.Log("NOT SAME1");
            radar.transform.position = new Vector3 (playerObject.transform.position.x,radar.transform.position.y,playerObject.transform.position.z);
        }
		//CameraPos.x = playerObject.transform.position.x;
		//CameraPos.z = playerObject.transform.position.z;
		transform.position = playerObject.transform.position + CameraPos;

		//set so that radar icons only moves in z axis 
	}

    private void radarZoomIn(){

    }
/*    void radarZoomControl(){
        if (Input.GetKeyDown(name:"o") ){
            zoomchange += 1;
        }
        else if (Input.GetKeyDown(name:"p") ){
            zoomchange -= 1;
        }
    }*/

	void changeRadarMode(){

	}

    void designateTarget(){
    //raycast this
        //enemyobject.tag = "SelectedTarget";
    }

    void AirRadarInit(){
        AeroplaneAI tempAir = Instantiate(airObjectList[0].gameObject).GetComponent<AeroplaneAI>();

        //airObjectList.Add(tempEnemy);
        //tempEnemy.rTransform.localRotation = Quaternion.Euler(Vector3.zero);
        //tempEnemy.rTransform.localScale = new Vector3(1,1,1);
        //tempEnemy.rTransform.anchoredPosition = Vector3.zero;
        //tempEnemy.gameObject.SetActive(true);
    }


    void OnTriggerEnter(Collider collider){
        Debug.Log("Collide");
        if(collider.tag == "Air"){
            BoxCollider colliders = collider.GetComponent<BoxCollider>();
            if(enemies.Contains(collider.gameObject)){
            }else{
                collider.tag = "LockAir";
                Debug.Log(collider.tag);
                Bogey tempEnemy = Instantiate(mapEnemies[0].gameObject).GetComponent<Bogey>();
                mapEnemies.Add(tempEnemy);
                enemies.Add(collider.gameObject);
                tempEnemy.rTransform.SetParent(map);
                tempEnemy.rTransform.localRotation = Quaternion.Euler(Vector3.zero);
                tempEnemy.rTransform.localScale = new Vector3(1,1,1);
                tempEnemy.rTransform.anchoredPosition = Vector3.zero;
                tempEnemy.gameObject.SetActive(true);
//colliders.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.tag == "LockAir"){
//BoxCollider colliders = collider.GetComponent<BoxCollider>();
            if(enemies.Contains(collider.gameObject)){
                Debug.Log("ReleaseEnemy");
                Bogey tempRemoval = mapEnemies[enemies.IndexOf(collider.gameObject)+1];
                mapEnemies.Remove(tempRemoval);
                Destroy(tempRemoval.gameObject);
                collider.tag = "Air";
                enemies.Remove(collider.gameObject);
            }else{

            }
        }
    }


    IEnumerator UpdateMapPos(){
        while (GameObject.Find("AircraftJet") != null) {
            if(enemies.Count>0){
                for (int i=0; i<enemies.Count; i++) {
                    mapEnemies [i + 1].UpdatePos (enemies [i].transform.position.x - playerObject.transform.position.x , enemies [i].transform.position.z- playerObject.transform.position.z);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield return 0;
    }

}
