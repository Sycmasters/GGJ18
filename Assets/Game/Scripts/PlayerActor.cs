using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class PlayerActor : MonoBehaviour 
{
	public int inputIndex = 0;
	public float moveSpeed = 3, runSpeed = 6;
    public int playerLifes = 3;
    public bool invertControls = true;

	public PlayerBase ownBase;
    public Renderer capsule;
	private float movementSpeed, lifeSecondCounter;
	private Vector3 movement;
	private int originalLifeCount;
	private bool isDead = true, noAxis;

	private Rigidbody rbody;
	public Transform grabbedObj;
	private Transform formerParent;

	public LayerMask mask;
    public CreaturesBehaviour currentCode;
    private int currCodeIndex;
    private bool currCodeCorrection;

    public GameObject explosionParticles;
    private List<string> controllers;

	// Use this for initialization
	void Start () 
	{
		rbody = GetComponent<Rigidbody> ();
		originalLifeCount = playerLifes;
		ownBase.actor = this;

        controllers = new List<string>(Input.GetJoystickNames());

        foreach(string con in controllers)
        {
            Debug.Log(inputIndex + " - " + con);
        }

        RespawnCharacter();
	}

	private void Update ()
	{
        if(ownBase.transmitting)
        {
            GetCodeInput();
            return;
        }

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

    private void GetCodeInput()
    {
        {
            int pressedKey = -1;

            if (Input.GetKeyDown("joystick " + (inputIndex + 1) + " button " + (!controllers[inputIndex].ToLower().Contains("xbox") ? 1 : 0)))
            {
                pressedKey = 3;
            }
            else if (Input.GetKeyDown("joystick " + (inputIndex + 1) + " button " + (!controllers[inputIndex].ToLower().Contains("xbox") ? 2 : 1)))
            {
                pressedKey = 1;
            }
            else if (Input.GetKeyDown("joystick " + (inputIndex + 1) + " button " + (!controllers[inputIndex].ToLower().Contains("xbox") ? 0 : 2)))
            {
                pressedKey = 0;
            }
            else if (Input.GetKeyDown("joystick " + (inputIndex + 1) + " button 3"))
            {
                pressedKey = 2;
            }

            if (pressedKey < 0)
            {
                return;
            }
            else
            {
                Debug.Log("Pressed " + pressedKey + " but in real was " + currentCode.codePattern[currCodeIndex]);
            }
            ownBase.codeDisplayLevels[currentCode.patternLevel].transform.GetChild(currCodeIndex).GetComponent<SpriteRenderer>().sprite = currentCode.codePatternSprites[pressedKey];


            if (currentCode.codePattern[currCodeIndex] == pressedKey)
            {
                currCodeIndex++;
            }
            else
            {
                currCodeIndex++;
                currCodeCorrection = false;
            }

            if (currentCode != null && currCodeIndex >= currentCode.codePattern.Count)
            {
                if (!currCodeCorrection)
                {
                    for (int i = 0; i < ownBase.batteries.Count; i++)
                    {
                        ownBase.batteries[i].GetComponent<Rigidbody>().AddExplosionForce(250, transform.position, 50);
                    }
                }
                this.tt("@WaitCoupleOfSeconds").Add(1, () => 
                {
                    for (int i = 0; i < ownBase.codeDisplayLevels[currentCode.patternLevel].transform.childCount; i++)
                    {
                        ownBase.codeDisplayLevels[currentCode.patternLevel].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
                    }
                    ownBase.DeactivateCodeInput();
                }).Immutable();
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
	{
        if(ownBase.transmitting)
        {
            return;
        }

		float horizontal = inputIndex >= 0 ? Input.GetAxis ("Horizontal" + inputIndex) : Input.GetAxis ("Horizontal");
		float vertical = inputIndex >= 0 ? Input.GetAxis ("Vertical" + inputIndex) : Input.GetAxis ("Vertical");

        if (!invertControls)
        {
            movement = new Vector3(horizontal * movementSpeed,
                                   rbody.velocity.y,
                                   vertical * movementSpeed);
        }
        else
        {
            movement = new Vector3(-horizontal * movementSpeed,
                                   rbody.velocity.y,
                                   -vertical * movementSpeed);
        }
		rbody.velocity = movement;

		if(horizontal != 0 || vertical != 0)
			transform.rotation = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
	}

	private void OnTriggerStay (Collider other)
	{
		if(other.name.ToLower().Contains("heal") && other.transform.IsChildOf(ownBase.transform) && ownBase.isWorking)
		{
            if(currentCode != null && !ownBase.transmitting)
            {
                noAxis = true;
                currCodeIndex = 0;
                currCodeCorrection = true;
                ownBase.codeDisplayLevels[currentCode.patternLevel].SetActive(true);
                ownBase.ActivatingCodeInput();
            }
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
        Debug.Log(other.collider.gameObject.name);
		if(other.collider.gameObject.name.Contains(ownBase.name) && isDead)
		{
            explosionParticles.SetActive(true);
			for(int i = 0; i < ownBase.batteries.Count; i++)
			{
				ownBase.batteries[i].GetComponent<Rigidbody>().AddExplosionForce(350, transform.position, 50);
			}
			isDead = false;

            this.tt("@ExplotionRecycle").Add(2, () => { explosionParticles.SetActive(false); });
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
		if(grabbedObj == null || currentCode == null)
		{
			Debug.Log("Try");
			Collider[] cols = Physics.OverlapSphere(transform.position, 1, mask);
            if (cols.Length > 0)
            {

                CreaturesBehaviour creature = cols[0].GetComponent<CreaturesBehaviour>();
				if(creature)
				{
					currentCode = creature.ExtractGenetic(this);
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
