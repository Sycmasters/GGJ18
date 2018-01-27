using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class CreaturesBehaviour : MonoBehaviour 
{
	public int[] codePatter;
	public SpriteRenderer spriteRender;
	public float timeToWait;

	public void ShowCode ()
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
}
