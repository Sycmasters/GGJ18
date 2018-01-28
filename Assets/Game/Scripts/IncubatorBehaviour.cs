using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.Liteprint;
using matnesis.TeaTime;

public class IncubatorBehaviour : MonoBehaviour
{
    public PlayerActor actor;
    public CreaturesBehaviour eggOne, eggTwo;
    public float hatchTime = 0;
    public Transform eggObj, eggSpotOne, eggSpotTwo, eggSpotThree;
    public Transform codeDisplay;

    private CreaturesBehaviour hatchedEgg;

    public void SetEgg(CreaturesBehaviour egg)
    {
        if (eggOne == null)
        {
            eggOne = egg;
            Transform newEgg = egg.transform.liteInstantiate(eggSpotOne.position, eggObj.rotation);
            eggOne = newEgg.GetComponent<CreaturesBehaviour>();
        }
        else if (eggTwo == null)
        {
            eggTwo = egg;
            Transform newEgg = egg.transform.liteInstantiate(eggSpotTwo.position, eggObj.rotation);
            eggTwo = newEgg.GetComponent<CreaturesBehaviour>();
        }

        if (eggOne != null && eggTwo != null)
        {
            this.tt("@HatchEgg").Add(hatchTime, () =>
            {
                Transform newEgg = eggObj.liteInstantiate(eggSpotThree.position, eggObj.rotation);
                newEgg.GetComponent<Renderer>().material.color = eggOne.GetComponent<Renderer>().material.color + eggTwo.GetComponent<Renderer>().material.color;
                hatchedEgg = newEgg.GetComponent<CreaturesBehaviour>();
                hatchedEgg.patternLevel = 3;
                this.tt("@ShowCode").Add(() =>
                {
                    codeDisplay.gameObject.SetActive(true);
                    hatchedEgg.codePattern = new List<int>();

                    for(int i = 0; i < codeDisplay.childCount; i++)
                    {
                        int rmd = Random.Range(0, hatchedEgg.codePatternSprites.Length);
                        hatchedEgg.codePattern.Add(rmd);
                        codeDisplay.GetChild(i).GetComponent<SpriteRenderer>().sprite = hatchedEgg.codePatternSprites[rmd];
                    }
                }).Add(hatchedEgg.timeToWait, () => {
                    if(hatchedEgg != null)
                    {
                        actor.currentCode = hatchedEgg;
                        hatchedEgg.gameObject.SetActive(false);
                        eggOne.gameObject.SetActive(false);
                        eggTwo.gameObject.SetActive(false);
                        hatchedEgg = null;
                        eggOne = null;
                        eggTwo = null;

                        codeDisplay.gameObject.SetActive(false);
                    }
                }).Immutable();
            });
            // Init new egg
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.Equals(actor.gameObject))
        {
            if(actor.currentCode != null)
            {
                SetEgg(actor.currentCode);
                actor.currentCode = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(actor.gameObject))
        {
            if (hatchedEgg != null)
            {
                actor.currentCode = hatchedEgg;
                hatchedEgg.gameObject.SetActive(false);
                eggOne.gameObject.SetActive(false);
                eggTwo.gameObject.SetActive(false);
                hatchedEgg = null;
                eggOne = null;
                eggTwo = null;
            }
        }
    }
}
