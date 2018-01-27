using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour 
{
	public bool isWorking = true;

	public List<GameObject> batteries = new List<GameObject>();
	public bool[] batteriesOn;

	private int batteryLoopCount;

	// Use this for initialization
	public void Start () 
	{
		batteriesOn = new bool[batteries.Count];
	}
	
	// Update is called once per frame
	void Update () 
	{
//		batteryLoopCount = 0;
//		for(int i = 0; i < batteries.Count; i++)
//		{
//			if(batte)
//		}
	}

	private void OnTriggerStay (Collider other)
	{
		for(int i = 0; i < batteries.Count; i++)
		{
			if(batteries.Contains(other.gameObject))
			{
				int batteryIndex = batteries.IndexOf (other.gameObject);
				batteriesOn [batteryIndex] = true;
				batteries [batteryIndex].GetComponent<Renderer> ().materials [1].color = Color.green;
			}
		}
	}

	private void OnTriggerExit (Collider other)
	{
		for(int i = 0; i < batteries.Count; i++)
		{
			if(batteries.Contains(other.gameObject))
			{
				int batteryIndex = batteries.IndexOf (other.gameObject);
				batteriesOn [batteryIndex] = false;
				batteries [batteryIndex].GetComponent<Renderer> ().materials [1].color = Color.red;
			}
		}
	}
}
