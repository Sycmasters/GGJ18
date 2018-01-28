using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;
using matnesis.Liteprint;

public class CreaturesBehaviour : MonoBehaviour 
{
	public List<int> codePattern;
    public Sprite[] codePatternSprites;
    public Color capsuleColor;
    public GameObject[] patternCount;
    public GameObject enemyModel;
    public Renderer capsuleInside;
    public int patternLevel = 0;

	public SpriteRenderer spriteRender;
	public float timeToWait;
    public int lifes = 1;
    public bool extratable;
    public Animator anim;
    public GameObject particles;

    private int originalLifes;
    private Rigidbody rbody;
    private Collider cols;

    private void Start()
    {
        originalLifes = lifes;
        cols = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        capsuleInside.materials[1].color = capsuleColor;
        capsuleInside.materials[1].SetColor("_EmissionColor", capsuleColor);
    }

    private void ShowCode ()
	{
		this.tt("@ShowCode").Add(() =>
		{
			spriteRender.transform.localPosition = new Vector3(0.2f, 1, 1.5f);
			spriteRender.transform.SetParent(null);
			spriteRender.transform.eulerAngles = new Vector3(-90, 0, 0);
			spriteRender.gameObject.SetActive(true);

            Transform currPattern = patternCount[patternLevel].transform;
            currPattern.gameObject.SetActive(true);

            for (int i = 0; i < currPattern.childCount; i++)
            {
                int rmd = Random.Range(0, codePatternSprites.Length);
                codePattern.Add(rmd);
                currPattern.GetChild(i).GetComponent<SpriteRenderer>().sprite = codePatternSprites[rmd];
            }

        }).Add(timeToWait, () => 
		{
			spriteRender.transform.SetParent(transform);
			spriteRender.gameObject.SetActive(false);
            gameObject.SetActive(false);
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
            anim.SetBool("Dead", true);
            this.tt("ChangeModel").Add(.5f, () => 
            {
                enemyModel.SetActive(false);
                capsuleInside.gameObject.SetActive(true);
                anim.SetBool("Dead", false);
            });
        }

        this.tt("@ShowTripas").Add(() => { particles.SetActive(true); }).Add(1, () => { particles.SetActive(false);  }).Immutable();
    }

    public CreaturesBehaviour ExtractGenetic (PlayerActor actor)
    {
        if(extratable && !spriteRender.gameObject.activeInHierarchy)
        {
            ShowCode();
            actor.capsule.gameObject.SetActive(true);
            actor.capsule.materials[1].color = capsuleColor;
            actor.capsule.materials[1].SetColor("_EmissionColor", capsuleColor);
            return this;
        }

        return null;
    }
}
