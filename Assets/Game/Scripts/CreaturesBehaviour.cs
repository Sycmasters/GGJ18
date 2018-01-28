using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;
using matnesis.Liteprint;
using UnityEngine.AI;

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
    public NavMeshAgent agent;
    public GameObject particles;

    private int originalLifes;
    private Rigidbody rbody;
    private Collider cols;
    private bool moving = true;

    private void Start()
    {
        originalLifes = lifes;
        cols = GetComponent<Collider>();
        rbody = GetComponent<Rigidbody>();
        if (capsuleInside != null)
        {
            capsuleInside.materials[1].color = capsuleColor;
            capsuleInside.materials[1].SetColor("_EmissionColor", capsuleColor);
        }
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        Vector2 rmdDir = Random.insideUnitCircle;
        agent.SetDestination(transform.position + (new Vector3(rmdDir.x, 0, rmdDir.y) * 5));
    }

    private void Update()
    {
        if (moving)
        {
            if ((agent.hasPath && agent.remainingDistance < 2) || agent.isStopped)
            {
                Vector2 rmdDir = Random.insideUnitCircle;
                agent.SetDestination(transform.position + (new Vector3(rmdDir.x, 0, rmdDir.y) * 5));
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void ShowCode ()
	{
		this.tt("@ShowCode").Add(() =>
		{
            if (capsuleInside != null) { capsuleInside.gameObject.SetActive(false); }
			spriteRender.transform.localPosition = new Vector3(0.2f, 1, 1.5f);
			spriteRender.transform.SetParent(null);
			spriteRender.transform.eulerAngles = new Vector3(90, 0, 180);
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
            Transform currPattern = patternCount[patternLevel].transform;
            currPattern.gameObject.SetActive(false);

            for (int i = 0; i < currPattern.childCount; i++)
            {
                currPattern.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }

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
            moving = false;
            this.tt("ChangeModel").Add(1f, () => 
            {
                enemyModel.SetActive(false);
                if (capsuleInside != null) { capsuleInside.gameObject.SetActive(true); }
                anim.SetBool("Dead", false);
            });
        }

        this.tt("@ShowTripas").Add(() => { particles.SetActive(true); }).Add(1, () => { particles.SetActive(false);  }).Immutable();
    }

    public void SetDestination()
    {
        moving = true;
        Vector2 rmdDir = Random.insideUnitCircle;
        agent.SetDestination(transform.position + (new Vector3(rmdDir.x, 0, rmdDir.y) * 5));
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
