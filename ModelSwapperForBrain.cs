using UnityEngine;
using System.Collections;
using Vuforia;

public class ModelSwapperForBrain : MonoBehaviour, ITrackableEventHandler 
{
	private TrackableBehaviour mTrackableBehaviour;
	public GameObject WithoutPartModel;
	private bool mSwapModel = false;
	private bool mSwapModel1 = false;
	static int count=0;
	public Transform WithPartPrefab;
	private bool mShowGUIButton = false;
	public GUISkin skin;
	
	// Use this for initialization
	void Start ()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
		if (mTrackableBehaviour == null)
		{
			Debug.Log ("Warning: Trackable not set !!");
		}
	}
	//whenever a button pressed it calls the required function
	void Update () {
		if (mSwapModel && mTrackableBehaviour != null) {
			SwapToWithParts();
			mSwapModel = false;
		}
		if(mSwapModel1 && mTrackableBehaviour !=null){
			SwapToWithoutParts();
			mSwapModel1 = false;
		}
	}
	//creating buttons
	void OnGUI() {
		GUI.skin=skin;
		//Getting screen width and size of the the device used  
		int sw = Screen.width;
		int sh = Screen.height;
		if (mShowGUIButton)//To display the buttons only if the model is tracked 
		{
			if (GUI.Button (new Rect (0, sh - 80, 100, 60), "Withparts")) {
				mSwapModel = true;
			}
			if (GUI.Button (new Rect (sw - 100, sh - 80, 100, 60), "Without Parts")) {
				mSwapModel1 = true;
			}
		}
		if (GUI.Button (new Rect ((sw / 2) - 50, sh - 80, 100, 60), "Quit", "button")) {
			Application.Quit ();
		}
	}
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			mShowGUIButton = true;
		} 
		else {
			mShowGUIButton=false;
		}
	}
	
	//Function for swapping model from without part to with part
	private void SwapToWithParts() {
		GameObject trackableGameObject = mTrackableBehaviour.gameObject;
		//disable any pre-existing augmentation
		for (int i = 0; i < trackableGameObject.transform.GetChildCount(); i++) {
			Transform child = trackableGameObject.transform.GetChild (i);
			child.gameObject.active = false;
		}
		if (count == 0) {
			WithPartPrefab = GameObject.Instantiate (WithPartPrefab)as Transform;
			//setting imagetarget as a parent to prefab
			WithPartPrefab.parent = mTrackableBehaviour.transform;
			//setting Position For new object
			WithPartPrefab.transform.localPosition=new Vector3(-0.01f,0.208f,-0.021f);
			WithPartPrefab.transform.localRotation=Quaternion.Euler(348.0202f,90.35582f,90.73158f);
			WithPartPrefab.transform.localScale=new Vector3(35.49875f,35.49876f,35.49876f);
			WithPartPrefab.gameObject.active = true;
			count = count + 1;
		} else {
			WithPartPrefab.gameObject.active = true;
		}
	}
	
	//Function for swapping model from with part to withoutpart
	private void SwapToWithoutParts()
	{
		GameObject trackableGameObject = mTrackableBehaviour.gameObject;
		//disable any pre-existing augmentation
		for (int i = 0; i < trackableGameObject.transform.GetChildCount(); i++)
		{
			Transform child = trackableGameObject.transform.GetChild(i);
			child.gameObject.active = false;
		}
		WithoutPartModel.gameObject.active = true;
	}
}

