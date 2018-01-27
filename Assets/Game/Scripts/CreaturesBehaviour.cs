using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class CreaturesBehaviour : MonoBehaviour 
{
	public int[] codePatter;
	public SpriteRenderer spriteRender;
	public float timeToWait;
    public int lifes = 1;
    public bool extratable;

    private int originalLifes;
    private Rigidbody rbody;
    private Collider cols;

    private void Start()
    {
        originalLifes = lifes;
        cols = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
    }

    private void ShowCode ()
	{
		this.tt("@ShowCode").Add(() =>
		{
			spriteRender.transform.localPosition = new Vector3(0.2f, 1, 1.5f);
			spriteRender.transform.SetParent(null);
			spriteRender.transform.eulerAngles = new Vector3(-90, 0, 0);
			spriteRender.gameObject.SetActive(true);
		}).Add(timeToWait, () => 
		{
			spriteRender.transform.SetParent(transform);
			spriteRender.gameObject.SetActive(false);
		}).Immutable();
	}

    public void HitCreature ()
    {
        if(lifes > 0)
        {
            lifes--;
        }

        if(lifes <= 0)
        {
            extratable = true;
            cols.isTrigger = true;
            rbody.isKinematic = true;
        }
    }

    public void ExtractGenetic ()
    {
        if(extratable)
        {
            ShowCode();
        }
    }
}
