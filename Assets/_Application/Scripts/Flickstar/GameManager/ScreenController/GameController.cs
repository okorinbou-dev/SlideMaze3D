using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : ScreenController
{

	[SerializeField]
	GameObject txtTitle;

	[SerializeField]
	GameObject txtStage;

	[SerializeField]
	Image imgFade;

	[SerializeField]
	ScreenController screenStageSelect = null;

	[SerializeField]
	ScreenController screenPackSelect = null;

	[SerializeField]
	GameObject objBoard;

	[SerializeField]
	Image imgLine;

	[SerializeField]
	Image imgFinger;

	[SerializeField]
	GameObject objShopDialog;

	[SerializeField]
	GameObject	objDialogGetHint;

	[SerializeField]
	GameObject	objDialogNoVideo;

	const int MOVE_UP = 0;
	const int MOVE_DOWN = 1;
	const int MOVE_LEFT = 2;
	const int MOVE_RIGHT = 3;

	private int CHANGESCREEN_STAGESELECT = 0;
	private int CHANGESCREEN_PACKSELECT = 1;

	GameObject objBackButton;
	GameObject objReloadButton;
	GameObject objVideoAdsButton;
	GameObject objHintButton;

	ButtonControl btnBackButton;
	ButtonControl btnReloadButton;
	ButtonControl btnVideoAdsButton;
	ButtonControl btnHintButton;

	bool Initialized = false;

	string[] packname = {
		"01_OPAL",
		"02_AMETHYST",
		"03_GARNET",
		"04_AQUAMARINE",
		"05_QUARTZ"
	};

	// * 横12 × 縦20
	// * 横10 × 縦16
	// * 横 8 × 縦13

	private const int GRID_MAX_WIDTH = 12;
	private const int GRID_MAX_HEIGHT = 20;

	GameObject objPlayer;
	GameObject objPlayerShadow;
	GameObject objGoalBlock;
	GameObject objGoalBlockShadow;
//	GameObject[,] objBlock;
//	GameObject[,] objBlockShadow;
	GameObject objHintBlock;
	GameObject objHintBlockShadow;


	GameObject obj3DPlayer;
	GameObject obj3DGoal;
	List<GameObject> list3DBlock;

	float BlockSize = 0.0f;
	Position CurrentPlayerPos;

	Block hintBlock;
	Block playerBlock;
	Block goalBlock;

	List<Block> normalBlock = new List<Block> ();
	List<Block> switchBlock = new List<Block> ();

	List<GameObject> normal3DBlock = new List<GameObject>();
	List<GameObject> switch3DBlock = new List<GameObject>();

	int UseHintNum = 0;

	System.DateTime StartTime;

	private void SetButton()
    {
		GameObject prefabButton = (GameObject)Resources.Load("Prefabs/Flickstar/prefabButton");

		objBackButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
		objBackButton.transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenGame/Buttons").transform);
		objBackButton.name = "BackButton";
		objBackButton.transform.localScale = new Vector3(1, 1, 1);
		objBackButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.BACKBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
		objBackButton.transform.Find("imgBack").gameObject.SetActive(true);
		btnBackButton = objBackButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

		objReloadButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
		objReloadButton.transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenGame/Buttons").transform);
		objReloadButton.name = "ReloadButton";
		objReloadButton.transform.localScale = new Vector3(1, 1, 1);
		objReloadButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.RELOADBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
		objReloadButton.transform.Find("imgReload").gameObject.SetActive(true);
		btnReloadButton = objReloadButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

		objVideoAdsButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
		objVideoAdsButton.transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenGame/Buttons").transform);
		objVideoAdsButton.name = "VideoAdsButton";
		objVideoAdsButton.transform.localScale = new Vector3(1, 1, 1);
		objVideoAdsButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.VIDEOADSBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
		objVideoAdsButton.transform.Find("imgVideoAds").gameObject.SetActive(true);
		btnVideoAdsButton = objVideoAdsButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();

		objHintButton = Instantiate(prefabButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
		objHintButton.transform.SetParent(GameObject.Find("GameManager/Canvas/ScreenGame/Buttons").transform);
		objHintButton.name = "HintButton";
		objHintButton.transform.localScale = new Vector3(1, 1, 1);
		objHintButton.GetComponent<RectTransform>().localPosition = new Vector2(Global.Instance.HINTBUTTON_POSX, Global.Instance.BACKBUTTON_POSY);
		objHintButton.transform.Find("imgHint").gameObject.SetActive(true);
		btnHintButton = objHintButton.transform.Find("ButtonBack").GetComponent<ButtonControl>();
	}

	private void FirstInitialize ()
	{
		if (!Initialized)
		{
			addChangeScreen (screenStageSelect);    // CHANGESCREEN_STAGESELECT
			addChangeScreen (screenPackSelect);    // CHANGESCREEN_PACKSELECT

			SetButton();

			/*
			GameObject prefabBlock135 = (GameObject)Resources.Load ("Prefabs/Flickstar/prefabBlock135");
			GameObject prefabBlock135Shadow = (GameObject)Resources.Load ("Prefabs/Flickstar/prefabBlock135DropShadow");

			objPlayerShadow = Instantiate (prefabBlock135Shadow, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objPlayerShadow.transform.SetParent (objBoard.transform);
			objPlayerShadow.name = "PlayerShadow";
			objPlayerShadow.transform.localScale = new Vector3 (1, 1, 1);
			objPlayerShadow.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objPlayerShadow.SetActive (false);

			objGoalBlockShadow = Instantiate (prefabBlock135Shadow, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objGoalBlockShadow.transform.SetParent (objBoard.transform);
			objGoalBlockShadow.name = "GoalBlockShadow";
			objGoalBlockShadow.transform.localScale = new Vector3 (1, 1, 1);
			objGoalBlockShadow.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objGoalBlockShadow.SetActive (false);
			*/

			/*
			objBlockShadow = new GameObject[GRID_MAX_HEIGHT, GRID_MAX_WIDTH];

			for (int y = GRID_MAX_HEIGHT - 1; y >= 0; y--) {
				for (int x = GRID_MAX_WIDTH - 1; x >= 0; x--) {
					objBlockShadow [y, x] = Instantiate (prefabBlock135Shadow, new Vector2 (0.0f, 0.0f), Quaternion.identity);
					objBlockShadow [y, x].transform.SetParent (objBoard.transform);
					objBlockShadow [y, x].name = "BLOCKSHADOW_" + x.ToString () + "_" + y.ToString ();
					objBlockShadow [y, x].transform.localScale = new Vector3 (1, 1, 1);
					objBlockShadow [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
					objBlockShadow [y, x].SetActive (false);
				}
			}

			objGoalBlock = Instantiate (prefabBlock135, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objGoalBlock.transform.SetParent (objBoard.transform);
			objGoalBlock.name = "GOALBLOCK";
			objGoalBlock.transform.localScale = new Vector3 (1, 1, 1);
			objGoalBlock.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ().enabled = true;
			objGoalBlock.SetActive (false);

			objBlock = new GameObject[GRID_MAX_HEIGHT, GRID_MAX_WIDTH];

			for (int y = GRID_MAX_HEIGHT - 1; y >= 0; y--) {
				for (int x = GRID_MAX_WIDTH - 1; x >= 0; x--) {
					objBlock [y, x] = Instantiate (prefabBlock135, new Vector2 (0.0f, 0.0f), Quaternion.identity);
					objBlock [y, x].transform.SetParent (objBoard.transform);
					objBlock [y, x].name = "BLOCK_" + x.ToString () + "_" + y.ToString ();
					objBlock [y, x].transform.localScale = new Vector3 (1, 1, 1);
					objBlock [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
					objBlock [y, x].SetActive (false);
				}
			}

			for (int y = GRID_MAX_HEIGHT - 1; y >= 0; y--)
			{
				for (int x = GRID_MAX_WIDTH - 1; x >= 0; x--)
				{
					objBlock[y, x] = Instantiate(prefabBlock135, new Vector2(0.0f, 0.0f), Quaternion.identity);
					objBlock[y, x].transform.SetParent(objBoard.transform);
					objBlock[y, x].name = "BLOCK_" + x.ToString() + "_" + y.ToString();
					objBlock[y, x].transform.localScale = new Vector3(1, 1, 1);
					objBlock[y, x].GetComponent<RectTransform>().localPosition = new Vector2(0.0f, 0.0f);
					objBlock[y, x].SetActive(false);
				}
			}
			*/

			/*
			objPlayer = Instantiate (prefabBlock135, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objPlayer.transform.SetParent (objBoard.transform);
			objPlayer.name = "PLAYER";
			objPlayer.transform.localScale = new Vector3 (1, 1, 1);
			objPlayer.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objPlayer.transform.Find ("BlockStar").GetComponent<Image> ().enabled = false;
			objPlayer.SetActive (false);

			objHintBlock = Instantiate (prefabBlock135, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objHintBlock.transform.SetParent (objBoard.transform);
			objHintBlock.name = "HintBlock";
			objHintBlock.transform.localScale = new Vector3 (1, 1, 1);
			objHintBlock.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objHintBlock.transform.Find ("BlockStar").GetComponent<Image> ().enabled = false;
			objHintBlock.SetActive (false);

			objHintBlockShadow = Instantiate (prefabBlock135Shadow, new Vector2 (0.0f, 0.0f), Quaternion.identity);
			objHintBlockShadow.transform.SetParent (objBoard.transform);
			objHintBlockShadow.name = "HintBlockShadow";
			objHintBlockShadow.transform.localScale = new Vector3 (1, 1, 1);
			objHintBlockShadow.GetComponent<RectTransform> ().localPosition = new Vector2 (0.0f, 0.0f);
			objHintBlockShadow.SetActive (false);
			*/

			// ota add 3D
			/*
			YCLib.Utility.ObjectPool.Initialize(3);
			YCLib.Utility.ObjectPool.SetPrefab(0, (GameObject)Resources.Load("Prefabs/3D/Block"));
			YCLib.Utility.ObjectPool.SetPrefab(1, (GameObject)Resources.Load("Prefabs/3D/Player"));
			YCLib.Utility.ObjectPool.SetPrefab(2, (GameObject)Resources.Load("Prefabs/3D/Goal"));
			*/
			Initialized = true;
		}
	}

	private float fadeval = 1.0f;

	public void UpdateHintNum ()
	{
		if (SaveData.Instance.GetHintNum () <= 99) {
			if (SaveData.Instance.GetHintNum () > 0) {
				objHintButton.transform.Find ("imgHint/txtHintNum").GetComponent<Text> ().text = SaveData.Instance.GetHintNum ().ToString ();
			} else {
				objHintButton.transform.Find ("imgHint/txtHintNum").GetComponent<Text> ().text = "+";
			}
		} else {
			objHintButton.transform.Find ("imgHint/txtHintNum").GetComponent<Text> ().text = "99+";
		}
	}

	public override void Initialize ()
	{
		// 初回のみ
		FirstInitialize ();

		UseHintNum = 0;
		StartTime = System.DateTime.Now;

		imgLine.enabled = false;
		imgFinger.enabled = false;

		// ブロックオブジェクト初期化
		/*
		for (int y = 0; y < GRID_MAX_HEIGHT; y++) {
			for (int x = 0; x < GRID_MAX_WIDTH; x++) {
				objBlock [y, x].SetActive (false);
				objBlock [y, x].transform.Find ("BlockStar").GetComponent<Image> ().enabled = false;
				objBlockShadow [y, x].SetActive (false);
			}
		}
		*/

		// ステージデータ読み込み
		StageData.Instance.Initialize ();
		StageData.Instance.Load (packname [Global.Instance.SelectPackNo], Global.Instance.SelectStageNo + 1);

		// タイトルテキスト設定
		txtTitle.GetComponent<TextMeshProUGUI> ().text = Global.Instance.GetCurrentPackName ();
		txtTitle.GetComponent<TextMeshProUGUI> ().color = Global.Instance.GetCurrentPackColor ();

		// ステージテキスト設定
		int stageno = Global.Instance.SelectStageNo + 1;
		txtStage.GetComponent<TextMeshProUGUI> ().text = "STAGE " + stageno.ToString ();
		txtStage.GetComponent<TextMeshProUGUI> ().color = Global.Instance.GetCurrentPackColor ();

		objBackButton.transform.Find ("imgBack").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objReloadButton.transform.Find ("imgReload").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objVideoAdsButton.transform.Find ("imgVideoAds").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objHintButton.transform.Find ("imgHint").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();

		// ブロック配置
		float defaultblocksize = 135.0f;
		float scale = 1480.0f / (StageData.Instance.data.Height * defaultblocksize);
		BlockSize = defaultblocksize * scale;
		objBoard.transform.localPosition = new Vector2 (-(BlockSize * ((float)StageData.Instance.data.Width) / 2.0f) + (BlockSize / 2.0f), -(BlockSize * ((float)StageData.Instance.data.Height) / 2.0f) + (BlockSize / 2.0f));

		// ヒントブロック配置
		CurrentPlayerPos = new Position (StageData.Instance.data.AnswerRoute [0].x, StageData.Instance.data.AnswerRoute [0].y);

		/*
		objHintBlock.transform.localScale = new Vector2 (scale, scale);
		objHintBlock.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (CurrentPlayerPos.x), ((BlockSize) * (CurrentPlayerPos.y)));
		objHintBlock.transform.Find ("BlockSurface").GetComponent<Image> ().color = new Color32 (Global.Instance.GetCurrentPackColor ().r, Global.Instance.GetCurrentPackColor ().g, Global.Instance.GetCurrentPackColor ().b, 128);
		objHintBlock.transform.Find ("BlockClear").GetComponent<Image> ().enabled = false;
		HintBlockPhase = HITBLOCKPHASE_IDLE;
		objHintBlock.SetActive (false);

		hintBlock = new Block (objHintBlock, objHintBlockShadow, CurrentPlayerPos.x, CurrentPlayerPos.y); 
		hintBlock.SetPlayer ();
		*/

		// 操作ブロック配置
		CurrentPlayerPos = new Position (StageData.Instance.data.AnswerRoute [0].x, StageData.Instance.data.AnswerRoute [0].y);

		/*
		objPlayer.transform.localScale = new Vector2 (scale, scale);
		objPlayer.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (CurrentPlayerPos.x), ((BlockSize) * (CurrentPlayerPos.y)));
		objPlayer.transform.Find ("BlockSurface").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objPlayer.transform.Find ("BlockClear").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objPlayer.SetActive (true);

		objPlayerShadow.transform.localScale = new Vector2 (scale, scale);
		objPlayerShadow.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (CurrentPlayerPos.x), ((BlockSize) * (CurrentPlayerPos.y)));
		objPlayerShadow.SetActive (true);
		*/

		//		playerBlock = new Block (objPlayer, objPlayerShadow, CurrentPlayerPos.x, CurrentPlayerPos.y); 
		//		playerBlock.SetPlayer ();

		// 3D
		obj3DPlayer = YCLib.Utility.ObjectPool.Rent(1);
//		obj3DPlayer.GetComponent<MeshRenderer>().material.color = Color.red;
		obj3DPlayer.transform.position = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);
		obj3DPlayer.SetActive(true);

		playerBlock = new Block(obj3DPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
		playerBlock.SetPlayer();

		// ゴールブロック配置
		/*
		objGoalBlock.transform.localScale = new Vector2 (scale, scale);
		objGoalBlock.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x), ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y)));
		objGoalBlock.transform.Find ("BlockBackground").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
		objGoalBlock.transform.Find ("BlockSurface").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_WHITE);

		objGoalBlock.SetActive (true);

		objGoalBlockShadow.transform.localScale = new Vector2 (scale, scale);
		objGoalBlockShadow.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x), ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y)));
		objGoalBlockShadow.SetActive (true);
		*/

//		goalBlock = new Block (objGoalBlock, objGoalBlockShadow, StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x, StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y); 
//		goalBlock.SetGoal (objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ());

		// 3D
		obj3DGoal = YCLib.Utility.ObjectPool.Rent(2);
		obj3DGoal.transform.position = new Vector3(StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].x - (StageData.Instance.data.Width / 2.0f), StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].y - (StageData.Instance.data.Height / 2.0f), 0);
		obj3DGoal.SetActive(true);

		normalBlock = new List<Block> ();
		switchBlock = new List<Block> ();

//		normal3DBlock = new List<GameObject>();
//		switch3DBlock = new List<GameObject>();

		for (int y = 0; y < StageData.Instance.data.Height; y++)
		{
			for (int x = 0; x < StageData.Instance.data.Width; x++)
			{
				// 固定ブロック配置
				switch (StageData.Instance.data.Grid [y, x])
				{
					case Data.GRIDTYPE_NEEDBLOCK:
					case Data.GRIDTYPE_NEEDLESSBLOCK:
					case Data.GRIDTYPE_SWITCHBLOCK:

						/*
						List<Block> curList = (StageData.Instance.data.Grid [y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? switchBlock : normalBlock;
						Color32 col = (StageData.Instance.data.Grid [y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? Global.Instance.GetColor (Global.Instance.COL_MINT) : Global.Instance.GetColor (Global.Instance.COL_GRAY);

						objBlock [y, x].transform.localScale = new Vector2 (scale, scale);
						objBlock [y, x].transform.Find ("BlockSurface").GetComponent<Image> ().color = col;
						objBlock [y, x].SetActive (true);

						objBlockShadow [y, x].transform.localScale = new Vector2 (scale, scale);
						objBlockShadow [y, x].SetActive (true);
						*/

						// 3D
						list3DBlock = (StageData.Instance.data.Grid[y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? switch3DBlock : normal3DBlock;

						list3DBlock.Add(YCLib.Utility.ObjectPool.Rent(0));
						list3DBlock[list3DBlock.Count - 1].GetComponent<MeshRenderer>().material.color = Color.white;
						list3DBlock[list3DBlock.Count - 1].transform.position = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0);
						list3DBlock[list3DBlock.Count - 1].SetActive(true);


						/*
						int xmin = x;
						int xmax = StageData.Instance.data.Width - x;
						int ymin = y;
						int ymax = StageData.Instance.data.Height - y;
						int dir;

						if (xmin < xmax) {
							if (xmin < ymin) {
								if (xmin < ymax) {
									// 左から
									dir = MOVE_RIGHT;
								} else {
									// 下から
									dir = MOVE_UP;
								}
							} else {
								if (ymin < ymax) {
									// 上から
									dir = MOVE_DOWN;
								} else {
									// 下から
									dir = MOVE_UP;
								}
							}
						} else {
							if (xmax < ymin) {
								if (xmax < ymax) {
									// 右から
									dir = MOVE_LEFT;
								} else {
									// 下から
									dir = MOVE_UP;
								}
							} else {
								if (ymin < ymax) {
									// 上から
									dir = MOVE_DOWN;
								} else {
									// 下から
									dir = MOVE_UP;
								}
							}
						}

						float add_x = 0.0f;
						float add_y = 0.0f;

						curList.Add (new Block (objBlock [y, x], objBlockShadow [y, x], x, y));
						curList [curList.Count - 1].SetSwitchFlag (StageData.Instance.data.Grid [y, x] == Data.GRIDTYPE_SWITCHBLOCK);

						if (dir == MOVE_LEFT) {
							add_x = 2000.0f;
							objBlock [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							objBlockShadow [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							curList [curList.Count - 1].SetMove (dir, objBlock [y, x].GetComponent<RectTransform> ().localPosition.x, (BlockSize) * (x), (StageData.Instance.data.Height) * 0.05f); 
						}
						if (dir == MOVE_RIGHT) {
							add_x = -2000.0f;
							objBlock [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							objBlockShadow [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							curList [curList.Count - 1].SetMove (dir, objBlock [y, x].GetComponent<RectTransform> ().localPosition.x, (BlockSize) * (x), (StageData.Instance.data.Height) * 0.05f); 
						}
						if (dir == MOVE_UP) {
							add_y = -2000.0f;
							objBlock [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							objBlockShadow [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							curList [curList.Count - 1].SetMove (dir, objBlock [y, x].GetComponent<RectTransform> ().localPosition.y, ((BlockSize) * (y)), (StageData.Instance.data.Height) * 0.05f); 
						}
						if (dir == MOVE_DOWN) {
							add_y = 2000.0f;
							objBlock [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							objBlockShadow [y, x].GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (x) + add_x, ((BlockSize) * (y)) + add_y);
							curList [curList.Count - 1].SetMove (dir, objBlock [y, x].GetComponent<RectTransform> ().localPosition.y, ((BlockSize) * (y)), (StageData.Instance.data.Height) * 0.05f); 
						}
						*/
						break;
				}
			}
		}

		/*
		objGoalBlock.transform.Find ("BlockBackground").GetComponent<Image> ().color = (switchBlock.Count == 0) ? Global.Instance.GetCurrentPackColor () : Global.Instance.GetColor (Global.Instance.COL_GRAY);
		objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ().color = (switchBlock.Count == 0) ? Global.Instance.GetCurrentPackColor () : Global.Instance.GetColor (Global.Instance.COL_GRAY);

		objHintButton.transform.Find ("imgHint/imgHintBatch").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		*/

		UpdateHintNum ();

//		objPlayer.transform.SetAsLastSibling ();
		GameCleared = false;
		GameFailed = false;

		if (Global.Instance.SelectPackNo == 0 && Global.Instance.SelectStageNo == 0) {
			imgLine.enabled = true;
			imgFinger.enabled = true;
		} else {
			imgLine.enabled = false;
			imgFinger.enabled = false;
		}
		imgLine.gameObject.transform.SetParent (objBoard.transform);
		imgFinger.gameObject.transform.SetParent (objBoard.transform);
		TutorialWait = 0.0f;
		TutorialPhase = TUTORIAL_FIRST;

		RestartTouchCheck = false;

		GamePhase = GAMEPHASE_TOUCHSCREEN;
//		GamePhase = GAMEPHASE_STARTANIME;
		StartAnimePhase = (Global.Instance.FirstPlay) ? STARTANIMEPHASE_BLOCK : STARTANIMEPHASE_FADE;

		// Fade
//		fadeval = (Global.Instance.FirstPlay) ? 0.0f : 1.0f;
//		Color fadecol = Global.Instance.GetCurrentPackColor ();
//		imgFade.color = new Color (fadecol.r, fadecol.g, fadecol.b, fadeval);

		Global.Instance.FirstPlay = false;
	}

	public void Restart ()
	{
		// ブロック配置
		/*
		float defaultblocksize = 135.0f;
		float scale = 1480.0f / (StageData.Instance.data.Height * defaultblocksize);
		BlockSize = defaultblocksize * scale;
		objBoard.transform.localPosition = new Vector2 (-(BlockSize * ((float)StageData.Instance.data.Width) / 2.0f) + (BlockSize / 2.0f), -(BlockSize * ((float)StageData.Instance.data.Height) / 2.0f) + (BlockSize / 2.0f));
		*/

		// 操作ブロック配置
		CurrentPlayerPos = new Position (StageData.Instance.data.AnswerRoute [0].x, StageData.Instance.data.AnswerRoute [0].y);

		obj3DPlayer.transform.position = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);

		playerBlock = new Block(obj3DPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
		playerBlock.SetPlayer();
		/*
		objPlayer.transform.localScale = new Vector2 (scale, scale);
		objPlayer.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (CurrentPlayerPos.x), ((BlockSize) * (CurrentPlayerPos.y)));
		objPlayer.transform.Find ("BlockSurface").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objPlayer.transform.Find ("BlockClear").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
		objPlayer.SetActive (true);

		objPlayerShadow.transform.localScale = new Vector2 (scale, scale);
		objPlayerShadow.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (CurrentPlayerPos.x), ((BlockSize) * (CurrentPlayerPos.y)));
		objPlayerShadow.SetActive (true);

		playerBlock = new Block (objPlayer, objPlayerShadow, CurrentPlayerPos.x, CurrentPlayerPos.y); 
		playerBlock.SetPlayer ();
		*/

		// ゴールブロック配置
		/*
		objGoalBlock.transform.localScale = new Vector2 (scale, scale);
		objGoalBlock.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x), ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y)));
		objGoalBlock.transform.Find ("BlockBackground").GetComponent<Image> ().color = (switchBlock.Count == 0) ? Global.Instance.GetCurrentPackColor () : Global.Instance.GetColor (Global.Instance.COL_GRAY);
		objGoalBlock.transform.Find ("BlockSurface").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_WHITE);
		objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ().color = (switchBlock.Count == 0) ? Global.Instance.GetCurrentPackColor () : Global.Instance.GetColor (Global.Instance.COL_GRAY);
		objGoalBlock.SetActive (true);

		objGoalBlockShadow.transform.localScale = new Vector2 (scale, scale);
		objGoalBlockShadow.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x), ((BlockSize) * (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y)));
		objGoalBlockShadow.SetActive (true);
		*/

		//		goalBlock = new Block (objGoalBlock, objGoalBlockShadow, StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x, StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y); 
		//		goalBlock.SetGoalStar (objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ());

		//		goalBlock.GoalAnimeInit ();

		for (int i = 0; i < switchBlock.Count; i++) {
			switchBlock [i].SetSwitchFlag (true);
		}

		for (int y = 0; y < StageData.Instance.data.Height; y++) {
			for (int x = 0; x < StageData.Instance.data.Width; x++) {
				// 固定ブロック配置
				switch (StageData.Instance.data.Grid [y, x]) {
				case Data.GRIDTYPE_NEEDBLOCK:
				case Data.GRIDTYPE_NEEDLESSBLOCK:
				case Data.GRIDTYPE_SWITCHBLOCK:
					List<Block> curList = (StageData.Instance.data.Grid [y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? switchBlock : normalBlock;
					Color32 col = (StageData.Instance.data.Grid [y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? Global.Instance.GetColor (Global.Instance.COL_MINT) : Global.Instance.GetColor (Global.Instance.COL_GRAY);
//					objBlock [y, x].transform.Find ("BlockSurface").GetComponent<Image> ().color = col;
					break;
				}
			}
		}

//		objPlayer.transform.SetAsLastSibling ();

		GameCleared = false;
		GameFailed = false;

		RestartTouchCheck = false;
		StartAnimeTime = 0.0f;

		GamePhase = GAMEPHASE_STARTANIME;
		StartAnimePhase = STARTANIMEPHASE_RESTART_PLAYER;

		StartTime = System.DateTime.Now;
	}

	Vector3 StartPos;
	Vector3 EndPos;

	const int GAMEPHASE_STARTANIME = 0;
	const int GAMEPHASE_TOUCHSCREEN = 1;
	const int GAMEPHASE_MOVEPLAYER = 2;
	const int GAMEPHASE_GAMEFAILED = 3;
	const int GAMEPHASE_GAMECLEARED = 4;
	const int GAMEPHASE_CLEAREDINTERSTITIAL = 5;
	const int GAMEPHASE_HINTGETDIALOG = 6;
	const int GAMEPHASE_REWORDMOVIE = 7;
	const int GAMEPHASE_NEXTGAME = 8;

	int GamePhase = GAMEPHASE_STARTANIME;

	bool GameCleared = false;
	bool GameFailed = false;

	bool RestartTouchCheck = false;

	float StartAnimeTime = 0.0f;

	const int STARTANIMEPHASE_FADE = 0;
	const int STARTANIMEPHASE_BLOCK = 1;
	const int STARTANIMEPHASE_PLAYER_WAIT = 2;
	const int STARTANIMEPHASE_PLAYER = 3;
	const int STARTANIMEPHASE_GOAL_WAIT = 4;
	const int STARTANIMEPHASE_GOAL = 5;
	const int STARTANIMEPHASE_RESTART_PLAYER = 6;

	int StartAnimePhase = STARTANIMEPHASE_FADE;

	private void PhaseStartAnime ()
	{
		/*
		bool check = true;

		switch (StartAnimePhase) {
		case STARTANIMEPHASE_FADE:
			StartAnimeTime += Time.deltaTime;
			Color fadecol = Global.Instance.GetCurrentPackColor ();
			if (StartAnimeTime > 1.0f) {
				StartAnimeTime = 1.0f;
				StartAnimePhase = STARTANIMEPHASE_BLOCK;
			}
			imgFade.color = new Color (fadecol.r, fadecol.g, fadecol.b, 1.0f - StartAnimeTime / 1.0f);
			break;
		case STARTANIMEPHASE_BLOCK:
			for (int i = 0; i < switchBlock.Count; i++) {
				if (switchBlock [i].Update ()) {
					switch3DBlock[i].transform.position = new Vector3(switchBlock[i].GetPos().x, switchBlock[i].GetPos().z, 0) * 0.01f + new Vector3(-4.0f,-8.0f,0.0f);
					check = false;
				}
			}
			for (int i = 0; i < normalBlock.Count; i++) {
				if (normalBlock [i].Update ()) {
					normal3DBlock[i].transform.position = new Vector3(normalBlock[i].GetPos().x, normalBlock[i].GetPos().y, 0) * 0.01f + new Vector3(-4.0f, -8.0f, 0.0f);
					check = false;
				}
			}
			if (check) {
				StartAnimeTime = 0.0f;
				StartAnimePhase = STARTANIMEPHASE_PLAYER_WAIT;
			}
			break;
		case STARTANIMEPHASE_PLAYER_WAIT:
			StartAnimeTime += Time.deltaTime;
			if (StartAnimeTime > 0.5f) {
				StartAnimePhase = STARTANIMEPHASE_PLAYER;
			}
			break;
		case STARTANIMEPHASE_PLAYER:
			if (!playerBlock.PlayerAppear ()) {
				StartAnimeTime = 0.0f;
				StartAnimePhase = STARTANIMEPHASE_GOAL_WAIT;
			}
			break;
		case STARTANIMEPHASE_GOAL_WAIT:
			StartAnimeTime += Time.deltaTime;
			if (StartAnimeTime > 0.1f) {
				StartAnimePhase = STARTANIMEPHASE_GOAL;
			}
			break;
		case STARTANIMEPHASE_RESTART_PLAYER:
			if (!playerBlock.PlayerAppear ()) {
				GamePhase = GAMEPHASE_TOUCHSCREEN;
				StartAnimePhase = STARTANIMEPHASE_FADE;
			}
			break;
		case STARTANIMEPHASE_GOAL:
			if (!goalBlock.GoalAppear ()) {
				if (switchBlock.Count == 0) {
					goalBlock.SetGoalEnable ();
				}
				GamePhase = GAMEPHASE_TOUCHSCREEN;
				AnalyticsManager.Instance.SendPlayStage ();
				StartAnimePhase = STARTANIMEPHASE_FADE;
			}
			break;
		}
		*/
	}

	private bool isGoal (int x, int y)
	{
		if (StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].x == x &&
		    StageData.Instance.data.AnswerRoute [StageData.Instance.data.AnswerRoute.Count - 1].y == y) {
			return true;
		}
		return false;
	}

	private bool isSwitchBlockOn ()
	{
		for (int i = 0; i < switchBlock.Count; i++) {
			if (switchBlock [i].isSwitchBlock ()) {
				return false;
			}
		}
		return true;
	}

	private int SearchMoveTarget (int direction)
	{
		switch (direction) {
		case MOVE_UP:
			for (int i = CurrentPlayerPos.y; i >= 0; i--) {
				switch (StageData.Instance.data.Grid [i, CurrentPlayerPos.x]) { 
				case Data.GRIDTYPE_NOBLOCK:
					if (isGoal (CurrentPlayerPos.x, i)) {
						if (isSwitchBlockOn ()) {
							GameCleared = true;
							System.TimeSpan diffTime = System.DateTime.Now - StartTime;
							Global.Instance.SetClearTime (diffTime.Seconds);
							AnalyticsManager.Instance.SendClearStage ();
							return i;
						} else {
							goalBlock.SetTouchAnime (direction);
							return i + 1;
						}
					}
					break;
				case Data.GRIDTYPE_SWITCHBLOCK:
					for (int j = 0; j < switchBlock.Count; j++) {
						if (switchBlock [j].x == CurrentPlayerPos.x && switchBlock [j].y == i && switchBlock [j].isSwitchBlock ()) {
							switchBlock [j].SetTouchAnime (direction);
							switchBlock [j].SetSwitchFlag (false);
						}
					}
					return i + 1;
				default:
					for (int j = 0; j < normalBlock.Count; j++) {
						if (normalBlock [j].x == CurrentPlayerPos.x && normalBlock [j].y == i) {
							normalBlock [j].SetTouchAnime (direction);
						}
					}
					return i + 1;
				}
			}
			GameFailed = true;
			return -5;
		case MOVE_DOWN:
			for (int i = CurrentPlayerPos.y; i < StageData.Instance.data.Height; i++) {
				switch (StageData.Instance.data.Grid [i, CurrentPlayerPos.x]) { 
				case Data.GRIDTYPE_NOBLOCK:
					if (isGoal (CurrentPlayerPos.x, i)) {
						if (isSwitchBlockOn ()) {
							GameCleared = true;
							return i;
						} else {
							goalBlock.SetTouchAnime (direction);
							return i - 1;
						}
					}
					break;
				case Data.GRIDTYPE_SWITCHBLOCK:
					for (int j = 0; j < switchBlock.Count; j++) {
						if (switchBlock [j].x == CurrentPlayerPos.x && switchBlock [j].y == i && switchBlock [j].isSwitchBlock ()) {
							switchBlock [j].SetTouchAnime (direction);
							switchBlock [j].SetSwitchFlag (false);
						}
					}
					return i - 1;
				default:
					for (int j = 0; j < normalBlock.Count; j++) {
						if (normalBlock [j].x == CurrentPlayerPos.x && normalBlock [j].y == i) {
							normalBlock [j].SetTouchAnime (direction);
						}
					}
					return i - 1;
				}
			}
			GameFailed = true;
			return StageData.Instance.data.Height + 5;
		case MOVE_RIGHT:
			for (int i = CurrentPlayerPos.x; i < StageData.Instance.data.Width; i++) {
				switch (StageData.Instance.data.Grid [CurrentPlayerPos.y, i]) { 
				case Data.GRIDTYPE_NOBLOCK:
					if (isGoal (i, CurrentPlayerPos.y)) {
						if (isSwitchBlockOn ()) {
							GameCleared = true;
							return i;
						} else {
							goalBlock.SetTouchAnime (direction);
							return i - 1;
						}
					}
					break;
				case Data.GRIDTYPE_SWITCHBLOCK:
					for (int j = 0; j < switchBlock.Count; j++) {
						if (switchBlock [j].x == i && switchBlock [j].y == CurrentPlayerPos.y && switchBlock [j].isSwitchBlock ()) {
							switchBlock [j].SetTouchAnime (direction);
							switchBlock [j].SetSwitchFlag (false);
						}
					}
					return i - 1;
				default:
					for (int j = 0; j < normalBlock.Count; j++) {
						if (normalBlock [j].x == i && normalBlock [j].y == CurrentPlayerPos.y) {
							normalBlock [j].SetTouchAnime (direction);
						}
					}
					return i - 1;
				}
			}
			GameFailed = true;
			return StageData.Instance.data.Width + 5;
		case MOVE_LEFT:
			for (int i = CurrentPlayerPos.x; i >= 0; i--) {
				switch (StageData.Instance.data.Grid [CurrentPlayerPos.y, i]) { 
				case Data.GRIDTYPE_NOBLOCK:
					if (isGoal (i, CurrentPlayerPos.y)) {
						if (isSwitchBlockOn ()) {
							GameCleared = true;
							return i;
						} else {
							goalBlock.SetTouchAnime (direction);
							return i + 1;
						}
					}
					break;
				case Data.GRIDTYPE_SWITCHBLOCK:
					for (int j = 0; j < switchBlock.Count; j++) {
						if (switchBlock [j].x == i && switchBlock [j].y == CurrentPlayerPos.y && switchBlock [j].isSwitchBlock ()) {
							switchBlock [j].SetTouchAnime (direction);
							switchBlock [j].SetSwitchFlag (false);
						}
					}
					return i + 1;
				default:
					for (int j = 0; j < normalBlock.Count; j++) {
						if (normalBlock [j].x == i && normalBlock [j].y == CurrentPlayerPos.y) {
							normalBlock [j].SetTouchAnime (direction);
						}
					}
					return i + 1;
				}
			}
			GameFailed = true;
			return -5;
		}

		return 0;
	}

	private void PhaseTouchScreen ()
	{

		Global.TouchInfo info = Global.Instance.GetTouch ();

		switch (info) {
		case Global.TouchInfo.Began:
			RestartTouchCheck = true;
			StartPos = Global.Instance.GetTouchPosition ();
			break;
		case Global.TouchInfo.Moved:
			if (!RestartTouchCheck) {
				return;
			}
			EndPos = Global.Instance.GetTouchPosition ();

			float distance = (StartPos - EndPos).sqrMagnitude;

			if (distance > 300.0f) {
				float movex = StartPos.x - EndPos.x;
				float movey = StartPos.y - EndPos.y;
				int dir;

				if (Mathf.Abs (movex) > Mathf.Abs (movey)) {
					// 横移動
					dir = (movex > 0.0f) ? MOVE_LEFT : MOVE_RIGHT;
					int TargetNum = SearchMoveTarget (dir);
//					playerBlock.SetMove (dir, (float)CurrentPlayerPos.x, (float)TargetNum, Mathf.Abs (0.05f * (CurrentPlayerPos.x - TargetNum)));
//					playerBlock.SetMove(dir, objPlayer.GetComponent<RectTransform>().localPosition.x, (BlockSize * TargetNum), Mathf.Abs(0.05f * (CurrentPlayerPos.x - TargetNum)));
					CurrentPlayerPos.x = TargetNum;
				} else {
					// 縦移動
					dir = (movey > 0.0f) ? MOVE_UP : MOVE_DOWN;
					int TargetNum = SearchMoveTarget (dir);
//					playerBlock.SetMove(dir, (float)CurrentPlayerPos.y, (float)TargetNum, Mathf.Abs(0.05f * (CurrentPlayerPos.y - TargetNum)));
//						playerBlock.SetMove (dir, objPlayer.GetComponent<RectTransform> ().localPosition.y, (BlockSize * TargetNum), Mathf.Abs (0.05f * (CurrentPlayerPos.y - TargetNum)));
					CurrentPlayerPos.y = TargetNum;
				}
				GamePhase = GAMEPHASE_MOVEPLAYER;
			}
			break;
		case Global.TouchInfo.Ended:
			break;
		}
			
		if (btnBackButton.isClick ()) {
			setChangeScreen (CHANGESCREEN_STAGESELECT);
		}

		if (btnReloadButton.isClick ()) {
			Restart ();
		}

		/*
		if (btnVideoAdsButton.isClick ()) {
			if (UseHintNum * 3 < StageData.Instance.data.AnswerRoute.Count - 1) {
				// 動画リワード表示
				if (Magicant.AdManager.Instance.IsRewardBasedVideoAdLoaded (Magicant.AdCategory.REWARDMOVIE)) {
					AnalyticsManager.Instance.SendRewardMovieShow ();
					Global.Instance.isRewardMovieOpen = true;
					Magicant.AdManager.Instance.ShowRewardBasedVideoAd (Magicant.AdCategory.REWARDMOVIE, (Magicant.AdCategory category, bool isShown, bool isRewarded) => {
						// 広告を閉じた際の処理
						if (isRewarded) {
							AnalyticsManager.Instance.SendRewardMovieWatch ();
							AnalyticsManager.Instance.SendUseHint ();
							SetHintBlock ();
							if (UseHintNum * 3 >= StageData.Instance.data.AnswerRoute.Count - 1) {
								objHintButton.transform.Find ("imgHint").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
								objHintButton.transform.Find ("imgHint/imgHintBatch").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
								objVideoAdsButton.transform.Find ("imgVideoAds").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
							}
						}
					});
				} else {
					objDialogNoVideo.SetActive (true);
				}
			}
		}
		*/

		if (btnHintButton.isClick ()) {
			if (SaveData.Instance.GetHintNum () > 0) {
				if (UseHintNum * 3 < StageData.Instance.data.AnswerRoute.Count - 1) {
					AnalyticsManager.Instance.SendUseHint ();
					SetHintBlock ();
					SaveData.Instance.SetHintNum (SaveData.Instance.GetHintNum () - 1);
					UpdateHintNum ();
					if (UseHintNum * 3 >= StageData.Instance.data.AnswerRoute.Count - 1) {
						objHintButton.transform.Find ("imgHint").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
						objHintButton.transform.Find ("imgHint/imgHintBatch").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
						objVideoAdsButton.transform.Find ("imgVideoAds").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
					}
				} else {
					objHintButton.transform.Find ("imgHint").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
					objHintButton.transform.Find ("imgHint/imgHintBatch").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
					objVideoAdsButton.transform.Find ("imgVideoAds").GetComponent<Image> ().color = Global.Instance.GetColor (Global.Instance.COL_GRAY);
				}
			} else {
				Global.Instance.SetHintReflesh (true);
				objShopDialog.SetActive (true);
			}
		}
	}

	private void PhaseMovePlayer ()
	{

		if (!playerBlock.Update ()) {
			/*
			for (int j = 0; j < switchBlock.Count; j++) {
				switchBlock [j].PlayTouchAnime ();
			}
			for (int j = 0; j < normalBlock.Count; j++) {
				normalBlock [j].PlayTouchAnime ();
			}
			goalBlock.PlayTouchAnime ();
			*/
			if (GameFailed) {
				GamePhase = GAMEPHASE_GAMEFAILED;
			} else if (GameCleared) {
//				playerBlock.SetClear ();
				GamePhase = GAMEPHASE_GAMECLEARED;
			} else {
				RestartTouchCheck = false;
				GamePhase = GAMEPHASE_TOUCHSCREEN;
			}
		}
	}

	private void PhaseGameFailed ()
	{
		Restart ();
	}

	private void PhaseGameCleared ()
	{
		playerBlock.ClearAnimeUpdate ();

		if (!playerBlock.isClearAnime ())
		{
			/*
			if (Global.Instance.GetPassedTime () >= Magicant.FirebaseRemoteConfig.Instance.GetLong ("AD_INTERVAL_SEC_GAMECLEAR") && SaveData.Instance.GetRemoveAds () == 0) {
				AnalyticsManager.Instance.SendStageClearInterstitialShow ();
				Global.Instance.isClearInterstitialOpen = true;
				Magicant.AdManager.Instance.GetInterstitialAdListener(Magicant.AdCategory.APP_INTERSTITIAL).OnAdLeavingApplication += (Magicant.AdCategory category) => {
					AnalyticsManager.Instance.SendStageClearInterstitialClick();
				};
				Magicant.AdManager.Instance.ShowInterstitial (Magicant.AdCategory.APP_INTERSTITIAL, (Magicant.AdCategory category, bool ret) => {
					// 広告を閉じた際の処理

					if (SaveData.Instance.GetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo) == Global.STAGEICON_OPEN &&
						(Global.Instance.SelectStageNo !=0) && ((Global.Instance.SelectStageNo + 1) % 6 == 0)) {
						SaveData.Instance.AddHintNum(1);
						SaveData.Instance.SetStageHintStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo, Global.Instance.STAGEHINTICON_OFF);
						SaveData.Instance.Save();
						objDialogGetHint.SetActive (true);
						GamePhase = GAMEPHASE_HINTGETDIALOG;
					} else {
						GamePhase = GAMEPHASE_NEXTGAME;
					}
//					GamePhase = GAMEPHASE_NEXTGAME;

					Global.Instance.ResetPassedTime ();
				});
				GamePhase = GAMEPHASE_CLEAREDINTERSTITIAL;
			} else {
			*/
				if (SaveData.Instance.GetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo) == Global.STAGEICON_OPEN &&
					(Global.Instance.SelectStageNo !=0) && ((Global.Instance.SelectStageNo + 1) % 6 == 0)) {
					SaveData.Instance.AddHintNum(1);
					SaveData.Instance.SetStageHintStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo, Global.Instance.STAGEHINTICON_OFF);
					SaveData.Instance.Save();
					objDialogGetHint.SetActive (true);
					GamePhase = GAMEPHASE_HINTGETDIALOG;
				} else {
					GamePhase = GAMEPHASE_NEXTGAME;
				}
//			}
		}
	}

	private void PhaseGameClearedInterstitial () {
	}

	private void PhaseHintGetDialog() {
		if (!objDialogGetHint.activeSelf) {
			GamePhase = GAMEPHASE_NEXTGAME;
		}
	}

	private void PhaseGameNextGame() {

		SaveData.Instance.SetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo, Global.STAGEICON_CLEAR);
		Global.Instance.SetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo, Global.STAGEICON_CLEAR);

		Global.Instance.SelectStageNo++;

		foreach (GameObject obj in list3DBlock)
        {
			YCLib.Utility.ObjectPool.Return(0, obj);
		}

		YCLib.Utility.ObjectPool.Return(1, obj3DPlayer);
		YCLib.Utility.ObjectPool.Return(2, obj3DGoal);

		if (Global.Instance.SelectStageNo >= 60)
		{
			Global.Instance.SelectStageNo = 0;
			setChangeScreen (CHANGESCREEN_PACKSELECT);
		} else {
			if (SaveData.Instance.GetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo) == Global.STAGEICON_LOCK) {
				SaveData.Instance.SetStageStatus (Global.Instance.SelectPackNo, Global.Instance.SelectStageNo, Global.STAGEICON_OPEN);
			}
			Initialize ();
		}

		SaveData.Instance.Save ();
	}

	const int HITBLOCKPHASE_IDLE = 0;
	const int HITBLOCKPHASE_MOVE_INIT = 1;
	const int HITBLOCKPHASE_MOVE_WAIT = 2;
	const int HITBLOCKPHASE_WAIT = 3;

	float HintMoveTime = 0.12f;

	int HintBlockPhase = HITBLOCKPHASE_IDLE;
	float HintBlockTime = 0.0f;
	int HintRouteNo = 0;

	public void SetHintBlock ()
	{
		HintBlockTime = 0.0f;
		UseHintNum++;
		HintBlockInit ();
		HintBlockPhase = HITBLOCKPHASE_MOVE_INIT;
	}

	public void HintBlockInit ()
	{
		float defaultblocksize = 135.0f;
		float scale = 1480.0f / (StageData.Instance.data.Height * defaultblocksize);
		BlockSize = defaultblocksize * scale;

		hintBlock.x = StageData.Instance.data.AnswerRoute [0].x;
		hintBlock.y = StageData.Instance.data.AnswerRoute [0].y;

		objHintBlock.transform.localScale = new Vector2 (scale, scale);
		objHintBlock.GetComponent<RectTransform> ().localPosition = new Vector2 ((BlockSize) * (hintBlock.x), ((BlockSize) * (hintBlock.y)));

		objHintBlock.SetActive (true);

		HintBlockTime = 0.0f;

		HintRouteNo = 1;
	}

	public void HintBlockUpdate ()
	{

		switch (HintBlockPhase) {
		case HITBLOCKPHASE_MOVE_INIT:
			int targetpos = 0;
			if (hintBlock.x == StageData.Instance.data.AnswerRoute [HintRouteNo].x) {
				// 縦移動
				targetpos = StageData.Instance.data.AnswerRoute [HintRouteNo].y;
//				hintBlock.SetMove ((hintBlock.y - targetpos < 0) ? MOVE_DOWN : MOVE_UP, objHintBlock.GetComponent<RectTransform> ().localPosition.y, (BlockSize * targetpos), Mathf.Abs (HintMoveTime * (hintBlock.y - targetpos)));
			} else {
				// 横移動
				targetpos = StageData.Instance.data.AnswerRoute [HintRouteNo].x;
//				hintBlock.SetMove ((hintBlock.x - targetpos < 0) ? MOVE_RIGHT : MOVE_LEFT, objHintBlock.GetComponent<RectTransform> ().localPosition.x, (BlockSize * targetpos), Mathf.Abs (HintMoveTime * (hintBlock.x - targetpos)));
			}
			hintBlock.x = StageData.Instance.data.AnswerRoute [HintRouteNo].x;
			hintBlock.y = StageData.Instance.data.AnswerRoute [HintRouteNo].y;
			HintBlockPhase = HITBLOCKPHASE_MOVE_WAIT;
			break;
		case HITBLOCKPHASE_MOVE_WAIT:
			if (!hintBlock.Update ()) {
				if (HintRouteNo == StageData.Instance.data.AnswerRoute.Count - 1 || HintRouteNo >= UseHintNum * 3) {
					HintBlockPhase = HITBLOCKPHASE_WAIT;
					objHintBlock.SetActive (false);
				} else {
					HintRouteNo++;
					HintBlockPhase = HITBLOCKPHASE_MOVE_INIT;
				}
			}
			break;
		case HITBLOCKPHASE_WAIT:
			HintBlockTime += Time.deltaTime;
			if (HintBlockTime > 1.0f) {
				HintBlockInit ();
				HintBlockPhase = HITBLOCKPHASE_MOVE_INIT;
			}
			break;
		}

	}

	const int TUTORIAL_FIRST = 0;
	const int TUTORIAL_INIT = 1;
	const int TUTORIAL_INIT_WAIT = 2;
	const int TUTORIAL_MOVE = 3;
	const int TUTORIAL_WAIT = 4;

	int TutorialPhase = TUTORIAL_FIRST;
	float TutorialWait = 0.0f;

	public void TutorialUpdate ()
	{
		if (Global.Instance.SelectPackNo == 0 && Global.Instance.SelectStageNo == 0) {
			switch (TutorialPhase) {
			case TUTORIAL_FIRST:
				TutorialWait += Time.deltaTime;
				imgFinger.rectTransform.localPosition = new Vector2 (150.0f, imgFinger.rectTransform.localPosition.y);
				if (TutorialWait >= 3.0f) { 
					TutorialWait = 0.0f;
					TutorialPhase = TUTORIAL_INIT;
				}
				break;
			case TUTORIAL_INIT:
				TutorialWait = 0.0f;
				imgFinger.rectTransform.localPosition = new Vector2 (150.0f, imgFinger.rectTransform.localPosition.y);
				TutorialPhase = TUTORIAL_INIT_WAIT;
				break;
			case TUTORIAL_INIT_WAIT:
				TutorialWait += Time.deltaTime;
				if (TutorialWait > 2.0f) {
					TutorialWait = 0.0f;
					TutorialPhase = TUTORIAL_MOVE;
				}
				break;
			case TUTORIAL_MOVE:
				TutorialWait += Time.deltaTime;
				float move = Global.Instance.GetBreakPos (150.0f, 150.0f + 530.0f, TutorialWait, 1.0f);
				if (TutorialWait >= 1.0f) { 
					TutorialWait = 0.0f;
					move = 150.0f + 530.0f;
					TutorialPhase = TUTORIAL_WAIT;
				}
				imgFinger.rectTransform.localPosition = new Vector2 (move, imgFinger.rectTransform.localPosition.y);
				break;
			case TUTORIAL_WAIT:
				TutorialWait += Time.deltaTime;
				if (TutorialWait > 2.0f) {
					TutorialPhase = TUTORIAL_INIT;
				}
				break;
			}
			imgLine.enabled = true;
			imgFinger.enabled = true;
		}
	}

	public override void Update ()
	{
		if (objShopDialog.activeSelf) {
			return;
		}

//		obj3DPlayer.transform.position = new Vector3(playerBlock.GetPos().x - (StageData.Instance.data.Width / 2.0f), playerBlock.GetPos().y - (StageData.Instance.data.Height / 2.0f), 0);

		switch (GamePhase) {
		case GAMEPHASE_STARTANIME:
			GamePhase = GAMEPHASE_TOUCHSCREEN;
//			PhaseStartAnime ();
			break;
		case GAMEPHASE_TOUCHSCREEN:
			if (!objDialogNoVideo.activeSelf) {
				PhaseTouchScreen ();
			}
			break;
		case GAMEPHASE_MOVEPLAYER:
			if (!objDialogNoVideo.activeSelf) {
				PhaseMovePlayer ();
			}
			break;
		case GAMEPHASE_GAMEFAILED:
			if (!objDialogNoVideo.activeSelf) {
				PhaseGameFailed ();
			}
			break;
		case GAMEPHASE_GAMECLEARED:
			if (!objDialogNoVideo.activeSelf) {
				PhaseGameCleared ();
			}
			break;
		case GAMEPHASE_CLEAREDINTERSTITIAL:
			if (!objDialogNoVideo.activeSelf) {
				PhaseGameClearedInterstitial ();
			}
			break;
		case GAMEPHASE_HINTGETDIALOG:
			if (!objDialogNoVideo.activeSelf) {
				PhaseHintGetDialog ();
			}
			break;
		case GAMEPHASE_REWORDMOVIE:
			break;
		case GAMEPHASE_NEXTGAME: 
			PhaseGameNextGame ();
			break;
		}

		if (objDialogNoVideo.activeSelf) {
			return;
		}

		for (int i = 0; i < switchBlock.Count; i++) {
			if (switchBlock [i].isTouchAnime ()) {
				switchBlock [i].TouchAnimeUpdate ();
				if (isSwitchBlockOn ()) {
					objGoalBlock.transform.Find ("BlockBackground").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
					objGoalBlock.transform.Find ("BlockStar").GetComponent<Image> ().color = Global.Instance.GetCurrentPackColor ();
					goalBlock.SetGoalEnable ();
				}
			}
		}

		for (int i = 0; i < normalBlock.Count; i++) {
			if (normalBlock [i].isTouchAnime ()) {
				normalBlock [i].TouchAnimeUpdate ();
			}
		}

		/*
		if (goalBlock.isTouchAnime ()) {
			goalBlock.TouchAnimeUpdate ();
		}

		goalBlock.GoalEnableAnime ();
		*/
		HintBlockUpdate ();
		TutorialUpdate ();

	}

}
