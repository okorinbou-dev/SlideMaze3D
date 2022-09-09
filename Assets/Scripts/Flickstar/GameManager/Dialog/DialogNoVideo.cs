using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNoVideo : MonoBehaviour {

	[SerializeField]
	ButtonControl btnClose;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (btnClose.isClick ()) {
			this.gameObject.SetActive (false);
		}
	}
}
