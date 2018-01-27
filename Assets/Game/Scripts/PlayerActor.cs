using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour 
{
	public int inputIndex = 0;
	public float moveSpeed = 3, runSpeed = 6;
	public int playerLifes = 3;

	public PlayerBase ownBase;

	private float movementSpeed, lifeSecondCounter;
	private Vector3 movement;
	private int originalLifeCount;
	private bool isDead = true;

	private Rigidbody rbody;
	public Transform grabbedObj;
	private Transform formerParent;

	public LayerMask mask;

	// Use this for initialization
	void Start () 
	{
		rbody = GetComponent<Rigidbody> ();
		originalLifeCount = playerLifes;
		ownBase.actor = this;

		RespawnCharacter();
	}

	private void Update ()
	{
		if (inputIndex >= 0) 
		{
			movementSpeed = Input.GetKey ("joystick " + (inputIndex + 1) + " button 0") ? runSpeed : moveSpeed;

			// Grab things
			if(Input.GetKeyDown("joystick " + (inputIndex + 1) + " button 4"))
			{
				GrabObject();
			}
			else if(Input.GetKeyUp("joystick " + (inputIndex + 1) + " button 4"))
			{
				ReleaseObject();
			}

            if (Input.GetKeyDown("joystick " + (inputIndex + 1) + " button 5"))
            {
                HitSomething();
            }
        } 
		else 
		{
			movementSpeed = Input.GetKey (KeyCode.LeftShift) ? runSpeed : moveSpeed;

            // Grab things
            if (Input.GetKeyDown(KeyCode.X))
            {
                GrabObject();
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                ReleaseObject();
            }

            //  Hit
            if (Input.GetKeyDown(KeyCode.C))
            {
                HitSomething();
            }

        }
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float horizontal = inputIndex >= 0 ? Input.GetAxis ("Horizontal" + inputIndex) : Input.GetAxis ("Horizontal");
		float vertical = inputIndex >= 0 ? Input.GetAxis ("Vertical" + inputIndex) : Input.GetAxis ("Vertical");

		movement = new Vector3(horizontal * movementSpeed,
							   rbody.velocity.y,
							   vertical * movementSpeed);

		rbody.velocity = movement;

		if(horizontal != 0 || vertical != 0)
			transform.rotation = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
	}

	private void OnTriggerStay (Collider other)
	{
		if(other.name.ToLower().Contains("heal") && other.transform.IsChildOf(ownBase.transform) && ownBase.isWorking)
		{
			if (Time.time > lifeSecondCounter) 
			{
				if(playerLifes < originalLifeCount)
				{
					playerLifes++;
				}
				lifeSecondCounter = Time.time + 1;
			}
		}
	}

	private void OnCollisionEnter (Collision other)
	{
		if(other.collider.gameObject.name.Contains(ownBase.name) && isDead)
		{
			for(int i = 0; i < ownBase.batteries.Count; i++)
			{
				ownBase.batteries[i].GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 50);
			}
			isDead = false;
		}
	}

	public void RespawnCharacter ()
	{
		isDead = true;


		Vector3 localBase = transform.InverseTransformDirection(ownBase.transform.position);
		localBase.y += 4;
		transform.rotation = Quaternion.LookRotation(-ownBase.transform.up);
		localBase += transform.forward.normalized;

		transform.localPosition = localBase;
	}

    public void HitSomething ()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 2);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                CreaturesBehaviour creature = cols[i].GetComponent<CreaturesBehaviour>();
                if (creature)
                {
                    creature.HitCreature();
                }
            }
        }
    }

	private void GrabObject ()
	{
		if(grabbedObj == null)
		{
			Debug.Log("Try");
			Collider[] cols = Physics.OverlapSphere(transform.position, 1, mask);
            if (cols.Length > 0)
            {

                CreaturesBehaviour creature = cols[0].GetComponent<CreaturesBehaviour>();
				if(creature)
				{
					creature.ExtractGenetic();
				}
                else
                {

                    grabbedObj = cols[0].transform;
                    cols[0].isTrigger = true;
                    grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
                    formerParent = grabbedObj.parent;
                    grabbedObj.SetParent(transform);
                }
			}
		}
	}

	public void ReleaseObject ()
	{
		if(grabbedObj != null)
		{
			grabbedObj.GetComponent<Collider>().isTrigger = false;
			grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
			grabbedObj.SetParent(formerParent);
			formerParent = null;
			grabbedObj = null;
		}
	}

}
