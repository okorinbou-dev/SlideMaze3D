using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {

	[SerializeField]
	GameObject obj;

	[SerializeField]
	bool ClickAnime = false;

	[SerializeField]
	bool Enable = true;

	bool PointerIn = false;
	bool PointerDown = false;

	bool PointerUp = false;

	Color DefaultColor = new Color(1.0f,1.0f,1.0f,1.0f);
	Color ClickedColor = new Color(0.5f,0.5f,0.5f,1.0f);

	// Use this for initialization
	void Start () {
		
	}

	public void Clear() {
		PointerUp = false;
	}

	public void SetEnable( bool flg ) {
		obj.GetComponent<Image> ().color = (flg) ? DefaultColor : ClickedColor;
		Enable = flg;
	}

	public bool isEnable() {
		return Enable;
	}

	// Update is called once per frame
//	void Update () {
//		if (Input.GetMouseButtonUp (0)) {
//			PointerIn = PointerDown = false;
//		}
//	}

	public void SetDefaultColor( Color col ) {
		DefaultColor = col;
	}

	public void SetClickedColor( Color col ) {
		ClickedColor = col;
	}

	void SetPress( bool flg ) {
		if (ClickAnime && Enable) {
			obj.GetComponent<Image> ().color = (flg) ? ClickedColor : DefaultColor;
		}
	}

	public void OnPointerEnter() {
		PointerIn = true;
		SetPress (PointerDown);
	}

	public void OnPointerExit() {
		PointerIn = false;
		SetPress (false);
	}

	public void OnPointerUp() {
		if (PointerIn) {
			PointerUp = true;
		}
		PointerDown = false;
		SetPress (false);
	}

	public void OnPointerDown() {
		PointerUp = false;
		PointerDown = true;
		SetPress (true);
	}

	public bool isClick() {
		if (Enable) {
			bool ret = PointerUp;
			PointerUp = false;
			return ret;
		}
		return false;
	}

	public Color GetCurrentColor() {
		return obj.GetComponent<Image> ().color;
	}
}
