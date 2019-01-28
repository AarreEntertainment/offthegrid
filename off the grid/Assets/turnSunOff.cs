using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnSunOff : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (transform.rotation.x) > 0.7)
			transform.GetChild (0).gameObject.SetActive (false);
		else
			transform.GetChild (0).gameObject.SetActive (true);

	}
}
