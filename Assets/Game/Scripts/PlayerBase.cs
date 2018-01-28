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
    private Renderer myRender;

	// Use this for initialization
	public void Start () 
	{
		batteriesOn = new bool[batteries.Count];
		batteriesSnapPos = new Vector3[batteries.Count];
		batteriesSnapRot = new Quaternion[batteries.Count];

        myRender = GetComponent<Renderer>();


        for (int i = 0; i < batteries.Count; i++)
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
            myRender.materials[1].color = working ? Color.green : Color.red;
            myRender.materials[1].SetColor("_EmissionColor", working ? Color.green : Color.red);
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
                Renderer render = batteries[batteryIndex].GetComponent<Renderer>();
                render.materials[1].color = Color.green;
                render.materials[1].SetColor("_EmissionColor", Color.green);
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
                    Renderer render = batteries[batteryIndex].GetComponent<Renderer>();
                    render.materials[1].color = Color.green; 
                    render.materials[1].SetColor("_EmissionColor", Color.green); 
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
                Renderer render = batteries[batteryIndex].GetComponent<Renderer>();
                render.materials[1].color = Color.red;
                render.materials[1].SetColor("_EmissionColor", Color.red);
            }
		}
	}

    public void ActivatingCodeInput ()
    {
        codeDisplay.SetActive(true);

        Transform currPattern = codeDisplayLevels[actor.currentCode.patternLevel].transform;
        currPattern.gameObject.SetActive(true);
        transmitting = true;
    }

    public void DeactivateCodeInput ()
    {
        actor.capsule.gameObject.SetActive(false);
        codeDisplay.SetActive(false);
        actor.currentCode = null;
        transmitting = false;
    }
}
