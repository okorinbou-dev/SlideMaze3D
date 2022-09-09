using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelectController : ScreenController {

    [SerializeField]
    GameObject txtTitle;

    [SerializeField]
    ScreenController screenPackSelect = null;

    [SerializeField]
    ScreenController screenGame = null;

    private int CHANGESCREEN_PACKSELECT = 0;
    private int CHANGESCREEN_GAME = 1;

    GameObject[] objStageSelectButton;
    GameObject objBackButton;

    Button[] btnStageSelectButton;
    ButtonControl btnBackButton;

    bool Initialized = false;

    private void SetStageButtonOpen(int no) 
    {
        objStageSelectButton[no].transform.Find("ButtonBack/StageText").gameObject.SetActive(true);
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonLock").gameObject.SetActive(false);
        objStageSelectButton[no].transform.Find("ButtonBack/Star").gameObject.SetActive(false);

        objStageSelectButton[no].transform.Find("ButtonBack").GetComponent<Image>().color = Color.white;
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonFace").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();

		objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStage").GetComponent<TextMeshProUGUI>().color = Color.white;
        objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStageNo").GetComponent<Text>().color = Color.white;    
    }

    private void SetStageButtonCleared(int no)
    {
        objStageSelectButton[no].transform.Find("ButtonBack/StageText").gameObject.SetActive(true);
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonLock").gameObject.SetActive(false);
        objStageSelectButton[no].transform.Find("ButtonBack/Star").gameObject.SetActive(true);

        objStageSelectButton[no].transform.Find("ButtonBack").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonFace").GetComponent<Image>().color = Color.white;

        objStageSelectButton[no].transform.Find("ButtonBack/Star/BatchStarBack").GetComponent<Image>().color = Color.white;
        objStageSelectButton[no].transform.Find("ButtonBack/Star/BatchStarFace").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();

		objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStage").GetComponent<TextMeshProUGUI>().color = Global.Instance.GetCurrentPackColor();
        objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStageNo").GetComponent<Text>().color = Global.Instance.GetCurrentPackColor();    
    }

    private void SetStageButtonLock(int no)
    {
        objStageSelectButton[no].transform.Find("ButtonBack/StageText").gameObject.SetActive(false);
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonLock").gameObject.SetActive(true);
        objStageSelectButton[no].transform.Find("ButtonBack/Star").gameObject.SetActive(false);

        objStageSelectButton[no].transform.Find("ButtonBack").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();
        objStageSelectButton[no].transform.Find("ButtonBack/ButtonFace").GetComponent<Image>().color = Color.white;

        objStageSelectButton[no].transform.Find("ButtonBack/ButtonLock").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();
 
		objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStage").GetComponent<TextMeshProUGUI>().color = Global.Instance.GetCurrentPackColor();
        objStageSelectButton[no].transform.Find("ButtonBack/StageText/txtStageNo").GetComponent<Text>().color = Global.Instance.GetCurrentPackColor();    
    }

    private void SetHintBatch(int no, bool on)
    {
        objStageSelectButton[no].transform.Find("ButtonBack/HintMark").gameObject.SetActive(on);

        objStageSelectButton[no].transform.Find("ButtonBack/HintMark/BatchHintBack").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();
        objStageSelectButton[no].transform.Find("ButtonBack/HintMark/BatchHintFace").GetComponent<Image>().color = Color.white;
        objStageSelectButton[no].transform.Find("ButtonBack/HintMark/BatchHintIcon").GetComponent<Image>().color = Global.Instance.GetCurrentPackColor();
    }

    public override void Initialize() {

        if (!Initialized)
        {
            GameObject parent = GameObject.Find("GameManager/Canvas/ScreenStageSelect");

            GameObject prefabStageSelectButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabStageButton");
			GameObject prefabButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabButton");

			GameObject prefabStageButtonNode = (GameObject)Resources.Load("Prefabs/Flickstar/prefabStageButtonNode");

            objStageSelectButton = new GameObject[Global.Instance.GetCurrentPackStageNum()];
            btnStageSelectButton = new Button[Global.Instance.GetCurrentPackStageNum()];

            for (int i = 0; i < Global.Instance.GetCurrentPackStageNum(); i++)
            {
                objStageSelectButton[i] = Instantiate(prefabStageSelectButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
                int no = i + 1;
                objStageSelectButton[i].name = "StageButton_" + no.ToString().PadLeft(2, '0');

                objStageSelectButton[i].transform.localScale = new Vector3(1, 1, 1);

                objStageSelectButton[i].transform.Find("ButtonBack/StageText/txtStageNo").GetComponent<Text>().text = no.ToString();

                btnStageSelectButton[i] = objStageSelectButton[i].transform.Find("ButtonBack").GetComponent<Button>();
            }

			GameObject.Find ("GameManager/Canvas/ScreenStageSelect/svStageIcon").GetComponent<Image> ().rectTransform.localScale = new Vector3 (1,1,1);
			GameObject[] objStageButtonNode = new GameObject[Global.Instance.GetCurrentPackStageNum () / 3];

			for ( int i=0; i<Global.Instance.GetCurrentPackStageNum() / 3; i++ ) {
				objStageButtonNode[i] = Instantiate(prefabStageButtonNode, new Vector2(0.0f, 0.0f), Quaternion.identity);

				for ( int j=0; j<3; j++ ) {
					objStageSelectButton[i*3+j].GetComponent<RectTransform>().localPosition = new Vector3(300.0f * ( j-1 ), 0.0f, 1.0f);
					objStageSelectButton[i*3+j].transform.SetParent(objStageButtonNode[i].transform);
				}
				objStageButtonNode[i].transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenStageSelect/svStageIcon/Viewport/Content").GetComponent<RectTransform>().transform);
				objStageButtonNode[i].GetComponent<RectTransform>().transform.localPosition = new Vector3(0, -280.0f*i-150.0f, 1);
				objStageButtonNode[i].GetComponent<RectTransform>().transform.localScale = new Vector3(1, 1, 1);
			}

			objBackButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
            objBackButton.transform.SetParent(parent.transform);
            objBackButton.name = "BackButton";
            objBackButton.transform.localScale = new Vector3(1, 1, 1);
			objBackButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.BACKBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
			objBackButton.transform.Find ("imgBack").gameObject.SetActive (true);
            btnBackButton = objBackButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

            addChangeScreen(screenPackSelect);      // CHANGESCREEN_PACKSELECT
            addChangeScreen(screenGame);            // CHANGESCREEN_GAME

            Initialized = true;
        }

		txtTitle.GetComponent<TextMeshProUGUI>().text = Global.Instance.GetCurrentPackName();
		txtTitle.GetComponent<TextMeshProUGUI>().color = Global.Instance.GetCurrentPackColor();

		GameObject.Find ("GameManager/Canvas/ScreenStageSelect/svStageIcon").GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;

		for (int i = 0; i < Global.Instance.GetCurrentPackStageNum(); i++)
        {
			switch (SaveData.Instance.GetStageStatus(Global.Instance.SelectPackNo, i)) {
			case Global.STAGEICON_LOCK:
				SetStageButtonLock(i);
				break;
			case Global.STAGEICON_OPEN:
				SetStageButtonOpen(i);
				break;
			case Global.STAGEICON_CLEAR:
				SetStageButtonCleared(i);
				break;
			}

			SetHintBatch(i, (SaveData.Instance.GetStageStatus(Global.Instance.SelectPackNo, i) != Global.STAGEICON_CLEAR) && (i !=0) && ((i + 1) % 6 == 0) ? true : false);
        }
	}

    public override void Update()
    {
        if (btnBackButton.isClick())
        {
            setChangeScreen(CHANGESCREEN_PACKSELECT);
        }
        for (int i = 0; i < Global.Instance.GetCurrentPackStageNum(); i++)
        {
            if (btnStageSelectButton[i].isClick())
            {
				if (SaveData.Instance.GetStageStatus (Global.Instance.SelectPackNo,i) != Global.STAGEICON_LOCK) 
				{
					Global.Instance.SelectStageNo = i; 
					setChangeScreen(CHANGESCREEN_GAME);
				}
            }
        }
    }
}
