using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class PlayerBase : MonoBehaviour 
{
	public bool isWorking = true, transmitting;

	public List<GameObject> batteries = new List<GameObject>();
	public bool[] batteriesOn;
	public PlayerActor actor;
    public GameObject codeDisplay;
    public GameObject[] codeDisplayLevels;

	private Vector3[] batteriesSnapPos;
	private Quaternion[] batteriesSnapRot;
	private int batteryLoopCount;

	// Use this for initialization
	public void Start () 
	{
		batteriesOn = new bool[batteries.Count];
		batteriesSnapPos = new Vector3[batteries.Count];
		batteriesSnapRot = new Quaternion[batteries.Count];

		for(int i = 0; i < batteries.Count; i++)
		{
			batteriesSnapPos[i] = batteries[i].transform.position;
			batteriesSnapRot[i] = batteries[i].transform.rotation;
		}



		this.tt ("@CheckIfIsWorking").Add (() => 
		{
			bool working = true;
			for (int i = 0; i < batteries.Count; i++) 
			{
				if (batteriesOn [i] == false) 
				{
					working = false;
				}
			}
			isWorking = working;
            GetComponent<Renderer>().materials[1].color = working ? Color.green : Color.red;
		}).Add (0.2f).Repeat ();
	}

	private void OnTriggerEnter (Collider other)
	{
		for(int i = 0; i < batteries.Count; i++)
		{
			if(batteries.Contains(other.gameObject))
			{
				int batteryIndex = batteries.IndexOf (other.gameObject);

				batteriesOn [batteryIndex] = true;
				batteries [batteryIndex].GetComponent<Renderer> ().materials [1].color = Color.green;
				batteries[batteryIndex].transform.position = batteriesSnapPos[batteryIndex];
				batteries[batteryIndex].transform.rotation = batteriesSnapRot[batteryIndex];
				actor.ReleaseObject();
			}
		}
	}

    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < batteries.Count; i++)
        {
            if (batteries.Contains(other.gameObject))
            {
                if (!batteriesOn[batteries.IndexOf(other.gameObject)])
                {
                    int batteryIndex = batteries.IndexOf(other.gameObject);
                    batteriesOn[batteryIndex] = true;
                    batteries[batteryIndex].GetComponent<Renderer>().materials[1].color = Color.green;
                }
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

    public void ActivatingCodeInput ()
    {
        codeDisplay.SetActive(true);

        Transform currPattern = actor.currentCode.patternCount[actor.currentCode.patternLevel].transform;
        currPattern.gameObject.SetActive(true);
        transmitting = true;
    }

    public void DeactivateCodeInput ()
    {
        codeDisplay.SetActive(false);
        actor.currentCode = null;
        transmitting = false;
    }
}
