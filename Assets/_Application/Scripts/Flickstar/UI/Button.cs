using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	private bool click = false;

	public void OnClick() {
		click = true;
	}

	public bool isClick() {
		bool ret = click;
		click = false;
		return ret;
	}

}
