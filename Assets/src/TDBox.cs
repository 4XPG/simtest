﻿using UnityEditor;
using UnityEngine;
public class TDBox : MonoBehaviour {

	public RectTransform targetCanvas;
	public Texture targetbox;
	public GameObject TargetObject;
	public Vector3 screenPos;
    public Vector3 offset;
	public Camera MainCam;

	void Start(){
        TargetObject = GameObject.FindGameObjectWithTag("SelectedTarget");
    }
	void OnGUI(){
        if(TargetObject != null)
			applyBox();
	}
		
	public void applyBox(){
        Vector3 screenPoint = MainCam.WorldToViewportPoint(TargetObject.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		if (TargetObject != null) {
            if (onScreen){
                screenPos = MainCam.WorldToScreenPoint (TargetObject.transform.position);
                GUI.DrawTexture(new Rect(screenPos.x - 25, Screen.height - screenPos.y - 25, 50, 50), targetbox, ScaleMode.ScaleToFit, true, 0);

            }
		}
	}
}