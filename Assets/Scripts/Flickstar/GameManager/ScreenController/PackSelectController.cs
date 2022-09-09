using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackSelectController : ScreenController
{

    private int CHANGESCREEN_TOP = 0;
    private int CHANGESCREEN_STAGESELECT = 1;

    [SerializeField]
    ScreenController screenTop = null;

    [SerializeField]
    ScreenController screenStageSelect = null;

	[SerializeField]
	Text txtClearedStages;

	[SerializeField]
	Text txtTotalStages;

    GameObject[] objPackSelectButton;
    GameObject objBackButton;

    ButtonControl[] btnPackSelectButton;
    ButtonControl btnBackButton;

    bool Initialized = false;

    public PackSelectController() {
    }

	public override void Initialize() {

        if ( !Initialized ) {
            addChangeScreen(screenTop);            // CHANGESCREEN_TOP
            addChangeScreen(screenStageSelect);    // CHANGESCREEN_STAGESELECT

            objPackSelectButton = new GameObject[Global.Instance.PACK_NUM];
            btnPackSelectButton = new ButtonControl[Global.Instance.PACK_NUM];

            GameObject parent = GameObject.Find("GameManager/Canvas/ScreenPackSelect");

            GameObject prefabBPackButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabPackageSelectButton");
			GameObject prefabButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabButton");

            for (int i = 0; i < Global.Instance.PACK_NUM; i++) {
                objPackSelectButton[i] = Instantiate(prefabBPackButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
                objPackSelectButton[i].transform.SetParent(parent.transform);
                objPackSelectButton[i].name = "PackButton_" + Global.Instance.GetPackName(i);
                objPackSelectButton[i].transform.localScale = new Vector3(1, 1, 1);
                objPackSelectButton[i].GetComponent<RectTransform>().localPosition = new Vector2(0.0f, 500.0f - (260.0f * i));
                objPackSelectButton[i].transform.Find("ButtonBack/ButtonFace").GetComponent<Image>().color = Global.Instance.PackCol[i];
				objPackSelectButton[i].transform.Find("ButtonBack/txtPackname").GetComponent<TextMeshProUGUI>().text = Global.Instance.GetPackName(i);
                btnPackSelectButton[i] = objPackSelectButton[i].transform.Find("ButtonBack").GetComponent<ButtonControl>();
            }

			objBackButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
            objBackButton.transform.SetParent(parent.transform);
            objBackButton.name = "BackButton";
            objBackButton.transform.localScale = new Vector3(1, 1, 1);
			objBackButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.BACKBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
			objBackButton.transform.Find ("imgBack").gameObject.SetActive (true);
            btnBackButton = objBackButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

            Initialized = true;
        }

		int ClearedNum = 0;
		int curnum = 0;

		for (int i = 0; i < Global.Instance.PACK_NUM; i++) {
			curnum = Global.Instance.GetStageClearNum(i);
			ClearedNum += curnum;
			objPackSelectButton [i].transform.Find ("ButtonBack/txtClearedStages").GetComponent<Text> ().text = curnum.ToString ();
			objPackSelectButton [i].transform.Find ("ButtonBack/imgDiamond").GetComponent<Image> ().enabled = ( curnum == Global.Instance.GetPackStageNum (i) );
		}

		txtClearedStages.text = ClearedNum.ToString();
    }

    public override void Update() {
        for (int i = 0; i < Global.Instance.PACK_NUM; i++) {
            if ( btnPackSelectButton[i].isClick() ) {
                Global.Instance.SelectPackNo = i;
                setChangeScreen(CHANGESCREEN_STAGESELECT);
            }
        }
        if (btnBackButton.isClick()) {
            setChangeScreen(CHANGESCREEN_TOP);
        }
    }
}
