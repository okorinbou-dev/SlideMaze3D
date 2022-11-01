using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global {

    private static Global mInstance;

    public int COL_MARINEBLUE = 0;
    public int COL_PURPLE = 1;
    public int COL_ROSE = 2;
    public int COL_TANGERINE = 3;
    public int COL_SAFRAN = 4;
    public int COL_MINT = 5;
    public int COL_GRAY = 6;
	public int COL_WHITE = 7;

    public Color32[] PackCol = {
        new Color32( 0x26, 0x8c, 0xff, 0xff ),
        new Color32( 0x67, 0x65, 0xDF, 0xff ),
        new Color32( 0xf0, 0x51, 0x87, 0xff ),
        new Color32( 0xf1, 0x5a, 0x24, 0xff ),
        new Color32( 0xff, 0xaf, 0x05, 0xff ),
        new Color32( 0x00, 0xc2, 0xb2, 0xff ),
        new Color32( 0xb3, 0xb3, 0xb3, 0xff ),
		new Color32( 0xff, 0xff, 0xff, 0xff )
    };

    public int PACK_OPAL = 0;
    public int PACK_AMETHYST = 1;
    public int PACK_GARNET = 2;
    public int PACK_AQUAMARINE = 3;
    public int PACK_QUARTZ = 4;
    public int PACK_NUM = 5;

    string[] PackName = {
        "OPAL",
        "AMETHYST",
        "GARNET",
        "AQUAMARINE",
        "QUARTZ",
    };

    public int SelectPackNo = 0;
    public int SelectStageNo = 0;

	public const int STAGEICON_LOCK = 0;
	public const int STAGEICON_OPEN = 1;
	public const int STAGEICON_CLEAR = 2;

	public int STAGEHINTICON_OFF = 0;
	public int STAGEHINTICON_ON = 1;

	public float BACKBUTTON_POSY =  830.0f;

	public float BACKBUTTON_POSX = -450.0f;
	public float RELOADBUTTON_POSX =  -330.0f;
	public float VIDEOADSBUTTON_POSX   =  330.0f;
	public float HINTBUTTON_POSX   =  450.0f;

	public float SHOPBUTTON_POSX = 400.0f;
	public float SHOPBUTTON_POSY = 780.0f;

	private int[,] StageStatus;

	private bool HintReflesh = false;

//	public bool IS_DEBUG = true;
	public bool IS_DEBUG = false;

	private float PassedTime = 0.0f;

	public float PassedResumeAdTime = 0.0f;

	public bool FirstPlay = false;

	private int ClearTime = 0;

	public bool isShopDialogOpen = false;

	public bool isRewardMovieOpen = false;
	public bool isClearInterstitialOpen = false;

	private Global() {
		StageStatus = new int[PACK_NUM,60];
    }

    public static Global Instance {
        get {
            if (mInstance == null) mInstance = new Global();
            return mInstance;
        }
    }

	public void UpdatePassedTime() {
		PassedTime += Time.deltaTime;
	}

	public void ResetPassedTime() {
		PassedTime = 0.0f;
	}

	public float GetPassedTime() {
		PassedTime += Time.deltaTime;
		return PassedTime;
	}

	public void ResetPassedResumeAdTime() {
		PassedResumeAdTime = Time.realtimeSinceStartup;
	}

	public float GetPassedResumeAdTime() {
		return Time.realtimeSinceStartup - PassedResumeAdTime;
	}

	public void SetHintReflesh( bool flag ) {
		HintReflesh = flag;
	}

	public bool isHintReflesh() {
		bool ret = HintReflesh;
		HintReflesh = false;
		return ret;
	}

    public string GetPackName(int no) {
        return PackName[no];
    }

    public string GetCurrentPackName() {
        return PackName[SelectPackNo];
    }

	public Color32 GetColor(int color)
	{
		return PackCol[color];
	}

    public Color32 GetCurrentPackColor()
    {
        return PackCol[SelectPackNo];
    }

	public int GetPackStageNum(int packno)
	{
		return 60;
	}

    public int GetCurrentPackStageNum()
    {
        return 60;
    }
		
	public void SetClearTime( int time ) {
		ClearTime = time;
	}

	public int GetClearTime() {
		return ClearTime;
	}
		
	private Vector3 TouchPosition = Vector3.zero;

	/// <summary>
	/// タッチ情報を取得(エディタと実機を考慮)
	/// </summary>
	/// <returns>タッチ情報。タッチされていない場合は null</returns>
	public TouchInfo GetTouch()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
		if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
		if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
#else
		if (Input.touchCount > 0)
		{
			return (TouchInfo)((int)Input.GetTouch(0).phase);
		}
#endif

		return TouchInfo.None;
	}

	/// <summary>
	/// タッチポジションを取得(エディタと実機を考慮)
	/// </summary>
	/// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
	public Vector3 GetTouchPosition()
	{
#if UNITY_EDITOR
		TouchInfo touch = GetTouch();
		if (touch != TouchInfo.None) {
			return Input.mousePosition; 
		}
#else
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			TouchPosition.x = touch.position.x;
			TouchPosition.y = touch.position.y;
			return TouchPosition;
		}
#endif

		return Vector3.zero;
	}

	/// <summary>
	/// タッチワールドポジションを取得(エディタと実機を考慮)
	/// </summary>
	/// <param name='camera'>カメラ</param>
	/// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
	public Vector3 GetTouchWorldPosition(Camera camera)
	{
		return camera.ScreenToWorldPoint(GetTouchPosition());
	}

	/// <summary>
	/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
	/// </summary>
	public enum TouchInfo
	{
		/// <summary>
		/// タッチなし
		/// </summary>
		None = 99,

		// 以下は UnityEngine.TouchPhase の値に対応
		/// <summary>
		/// タッチ開始
		/// </summary>
		Began = 0,
		/// <summary>
		/// タッチ移動
		/// </summary>
		Moved = 1,
		/// <summary>
		/// タッチ静止
		/// </summary>
		Stationary = 2,
		/// <summary>
		/// タッチ終了
		/// </summary>
		Ended = 3,
		/// <summary>
		/// タッチキャンセル
		/// </summary>
		Canceled = 4,
	}

	public float GetBreakPos( float startpos, float endpos, float nowtime, float maxtime ) {
		return (startpos + (endpos - startpos) * ((maxtime * 2 - nowtime + 1) * nowtime / 2.0f) / ((maxtime + 1) * maxtime / 2.0f));
	}

	public void SetStageStatus() {
		for (int i = 0; i < PACK_NUM; i++) {
			for (int j = 0; j < 60; j++) {
				StageStatus [i, j] = SaveData.Instance.GetStageStatus (i, j);
			}
		}
	}

	public void SetStageStatus(int packid, int stageno, int status) {
		StageStatus [packid, stageno] = status;
	}

	public int GetStageClearNum( int packid ) {
		int ret = 0;
		for (int j = 0; j < 60; j++) {
			if ( StageStatus [packid, j] == Global.STAGEICON_CLEAR ) {
				ret++;
			}
		}
		return ret;
	}
}
