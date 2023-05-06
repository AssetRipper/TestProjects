using UnityEngine;
using System.Collections;

public class BlinkIt : MonoBehaviour
{
	public Color source_color;
	public ushort blink_count=2;
	public Color target_color;
	private Animator anim_ator;

	private Color current;
	private float time_accumulator = 0.0f;
	private float blink_time = 0.5f;
	// Use this for initialization
	void Start () {

		anim_ator = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			anim_ator.SetTrigger("GoToColor");
		}
		// if current state is CBaseState then do something
		if (anim_ator.GetCurrentAnimatorStateInfo(0).IsName("CBaseState"))
		{
			// we blink the object color
			if (blink_count > 0)
			{
				time_accumulator += Time.deltaTime;
				float lerp = Mathf.PingPong(time_accumulator, blink_time); // 4 seconds
				current = GetComponent<Renderer>().material.color;
				current.r = Mathf.Lerp(source_color.r, target_color.r, lerp/blink_time);
				if (time_accumulator >= blink_time)
				{
					time_accumulator = 0.0f;
					blink_count--;
				}
			}
		}
		// if in CColorize state then do something else
		if (anim_ator.GetCurrentAnimatorStateInfo(0).IsName("CColorize"))
		{
			blink_count = 2;
			time_accumulator = 0.0f;
		}
	}
	void LateUpdate()
	{
		if(!anim_ator.enabled) return;
		MaterialPropertyBlock block = new MaterialPropertyBlock();
		GetComponent<Renderer>().GetPropertyBlock(block);
		Vector4 color = block.GetVector("_Color");
		color.x = current.r;
		block.SetColor("_Color", color);
		GetComponent<Renderer>().SetPropertyBlock(block);
	}
}
