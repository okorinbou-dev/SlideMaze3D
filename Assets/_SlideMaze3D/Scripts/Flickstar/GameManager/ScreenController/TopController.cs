using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopController : ScreenController {

	[SerializeField]
    ScreenController screenPackSelect = null;

	[SerializeField]
	ScreenController screenGame = null;

	[SerializeField]
	TextMeshProUGUI[] text;

	[SerializeField]
	GameObject objShopDialog;

	[SerializeField]
	ButtonControl btnStartButton;

	[SerializeField]
	GameObject objDebugButton;

	GameObject objShopButton;

	ButtonControl btnShopButton;

	float[] textPos;
	float[] textWait;
	float[] textAlpha;

	private int CHANGESCREEN_PACKSELECT = 0;

    bool Initialized = false;

    public override void Initialize() {
        if (!Initialized) {
            addChangeScreen(screenPackSelect);		// CHANGESCREEN_PACKSELECT
			addChangeScreen(screenGame);

			text [0].color = Global.Instance.GetColor (Global.Instance.COL_MINT);
			text [1].color = Global.Instance.GetColor (Global.Instance.COL_MARINEBLUE);
			text [2].color = Global.Instance.GetColor (Global.Instance.COL_PURPLE);
			text [3].color = Global.Instance.GetColor (Global.Instance.COL_ROSE);
			text [4].color = Global.Instance.GetColor (Global.Instance.COL_TANGERINE);
			text [5].color = Global.Instance.GetColor (Global.Instance.COL_SAFRAN);
			text [6].color = Global.Instance.GetColor (Global.Instance.COL_MINT);
			text [7].color = Global.Instance.GetColor (Global.Instance.COL_MARINEBLUE);
			text [8].color = Global.Instance.GetColor (Global.Instance.COL_PURPLE);

			textPos = new float[text.Length];
			textWait = new float[text.Length];
			textAlpha = new float[text.Length];

			GameObject prefabButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabButton");

			objShopButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
			objShopButton.transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenTop/Buttons").transform);
			objShopButton.name = "BackButton";
			objShopButton.transform.localScale = new Vector3(1, 1, 1);
			objShopButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.SHOPBUTTON_POSX, Global.Instance.SHOPBUTTON_POSY);
			objShopButton.transform.Find ("imgShop").gameObject.SetActive (true);
			btnShopButton = objShopButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

			Initialized = true;
        }

		float stringoffset = 90.0f;

		for ( int i=0; i<text.Length; i++ ) {
			textPos [i] = 500.0f;
			textAlpha [i] = 0.0f;
			textWait [i] = (i+1) * 0.15f;
			text [i].transform.localPosition = new Vector3 ( stringoffset*i-(stringoffset*8.0f/2.0f), 250.0f+textPos[i],text [i].transform.localPosition.z  );
			text [i].color = new Color (text [i].color.r, text [i].color.g, text [i].color.b, textAlpha [i]);
		}

		if (objShopDialog.activeSelf) {
			objShopDialog.SetActive (false);
		}
	}

    public override void Update() {
		bool anime = false;
		float alpha = 1.0f;

		for ( int i=0; i<text.Length; i++ ) {
			if (textWait [i] > 0.0f) {
				anime = true;
				textWait [i] -= Time.deltaTime;
			} else {
				textAlpha [i] += Time.deltaTime;
				if (textAlpha [i] > 0.5f) {
					textAlpha [i] = 0.5f;
				}
				alpha = 1.0f * (textAlpha [i] / 0.5f);
				text [i].transform.localPosition = new Vector3 ( text [i].transform.localPosition.x, 250.0f+textPos[i]-(textPos[i]*alpha),text [i].transform.localPosition.z  );
				text [i].color = new Color (text [i].color.r, text [i].color.g, text [i].color.b, alpha);
			}
		}

		if (anime) {
			btnShopButton.Clear ();
			btnStartButton.Clear ();
		} else {
			if (objShopDialog.activeSelf) {
			} else {
				if (objDebugButton.activeSelf) {
					if (objDebugButton.GetComponent<ButtonControl> ().isClick ()) {
						SaveData.Instance.SetDebugAds (true);
					}
				}
				if (btnShopButton.isClick ()) {
					Global.Instance.SetHintReflesh (true);
					objShopDialog.SetActive (true);
				} else {
					if (btnStartButton.isClick()) {
						setChangeScreen (CHANGESCREEN_PACKSELECT);
						for ( int i=0; i<text.Length; i++ ) {
							textPos [i] = 100.0f;
							textAlpha [i] = 0.0f;
							textWait [i] = (i+1) * 0.25f;
							text [i].color = new Color (text [i].color.r, text [i].color.g, text [i].color.b, textAlpha [i]);
						}
					}
				}
			}
		}
	}

}
