using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldown : MonoBehaviour {
	float cooldownTime;
	public float coolDown;
	// Use this for initialization
	void Start () {
		
	}
	public void setCoolDown()
	{
		cooldownTime = coolDown;
	}
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag ("controller").GetComponent<weatherControl> ().process > 0) {
			GetComponent<Button> ().interactable = false;
		} else if (GameObject.FindGameObjectWithTag ("controller").GetComponent<weatherControl> ().process > 0 && cooldownTime <= 0) {
			GetComponent<Button> ().interactable = false;
		}

		if (cooldownTime > 0 && GetComponent<Button> ().interactable)
			GetComponent<Button> ().interactable = false;
		if (cooldownTime <=0 && !GetComponent<Button> ().interactable && GameObject.FindGameObjectWithTag("controller").GetComponent<weatherControl>().process<=0)
			GetComponent<Button> ().interactable = true;
		if (cooldownTime > 0)
			cooldownTime -= Time.deltaTime;
		
	}
}
