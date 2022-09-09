using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static int SCREEN_TOP = 0;
	private static int SCREEN_PACKSELECT = 1;
	private static int SCREEN_STAGESELECT = 2;
	private static int SCREEN_GAME = 3;

	private static int SCREEN_NUM = 4;

	[SerializeField]
	Text	txtVer;

	[SerializeField]
	GameObject	screenTop;

	[SerializeField]
	GameObject	screenPackSelect;

	[SerializeField]
	GameObject	screenStageSelet;

	[SerializeField]
	GameObject	screenGame;

	[SerializeField]
	GameObject	DialogGetHint;

	[SerializeField]
	GameObject	DialogNoVideo;

	[SerializeField]
	GameObject	DialogShop;

	GameObject[] screenObject = new GameObject[SCREEN_NUM];
	ScreenController[] screenController = new ScreenController[SCREEN_NUM];

	ScreenController currentScreenController;

	bool isInitialize = false;

	// Use this for initialization
	void Start () {

		/*
		Magicant.TrackingManager.Initialize<Magicant.AppsFlyerModule>();
		Magicant.IAPManager.Initialize<Magicant.UnityIAPModule>();
		Magicant.AdManager.Initialize<Magicant.AdMobModule>();
		Magicant.AnalyticsManager.Initialize<Magicant.FirebaseAnalyticsModule>();
		Magicant.NotificationManager.Initialize<Magicant.OneSignalModule>();

		txtVer.text = Magicant.NativeUtils.GetVersion ();
		*/

		txtVer.enabled = Debug.isDebugBuild;

		screenObject [SCREEN_TOP] = screenTop;
		screenObject [SCREEN_PACKSELECT] = screenPackSelect;
		screenObject [SCREEN_STAGESELECT] = screenStageSelet;
		screenObject [SCREEN_GAME] = screenGame;

		Global.Instance.SelectPackNo = 0;
		Global.Instance.SelectStageNo = 0;

		for ( int i=0; i<screenObject.Length; i++ ) {
			screenController [i] = screenObject[i].GetComponent<ScreenController>();
            screenController[i].Initialize();
			screenObject [i].SetActive (false);
		}
			
		Application.targetFrameRate = 60; //60FPSに設定

//		SaveData.Instance.DebugDeleteAll ();

		Global.Instance.FirstPlay = SaveData.Instance.isFirstPlay ();

		if (Global.Instance.FirstPlay) { 
			screenObject [SCREEN_GAME].SetActive (true);
			currentScreenController = screenController[SCREEN_GAME];
		} else {
			screenObject [SCREEN_TOP].SetActive (true);
			currentScreenController = screenController[SCREEN_TOP];
			StageData.Instance.Initialize();
		}

		SaveData.Instance.Initialize ();
		Global.Instance.SetStageStatus ();

		Debug.Log ("PassedDay " + SaveData.Instance.GetPlayDays());

		if (SaveData.Instance.isChangeLastLaunchDate ()) {
			AnalyticsManager.Instance.SendLaunch ();
			SaveData.Instance.SetLastLaunchDate ();
		}

		currentScreenController.Initialize ();

		GameObject.Find ("GameManager/DebugLog").SetActive(Global.Instance.IS_DEBUG);
		GameObject.Find ("GameManager/Canvas/DebugLogBase").SetActive(Global.Instance.IS_DEBUG);

		// FireBase
		/*
		Dictionary<string, object> defaultValues = new Dictionary<string, object>() {
			{"INIT_AD_FREE_TIME_NEXT", 3L},
			{"AD_INTERVAL_SEC_GAMECLEAR", 90L},
			{"AD_INTERVAL_SEC_RETURN", 180L},
		};

		System.Action<Magicant.FirebaseRemoteConfig.FetchStatus> onFetchCompleted = (Magicant.FirebaseRemoteConfig.FetchStatus status) => {
			Debug.Log(status);
		};

		Magicant.FirebaseRemoteConfig.Initialize(defaultValues, onFetchCompleted);
		*/

		// バナー表示
		if (SaveData.Instance.GetRemoveAds () == 0) { 
			// バナークリック時
			/*
			Magicant.AdManager.Instance.GetBannerAdListener(Magicant.AdCategory.FUTTER_BANNER).OnAdOpening += (Magicant.AdCategory category) => {
				AnalyticsManager.Instance.SendFutterBannerClick();
			};
			Magicant.AdManager.Instance.ShowBannerAd(Magicant.AdCategory.FUTTER_BANNER);
			Magicant.AdManager.Instance.SetBannerAdPosition(Magicant.AdCategory.FUTTER_BANNER, Magicant.BannerAdPosition.Bottom);
			*/
		}

		Global.Instance.ResetPassedTime ();
		Global.Instance.ResetPassedResumeAdTime ();

		DialogGetHint.SetActive (false);
		DialogNoVideo.SetActive (false);
		objLoading.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		isInitialize = true;
		Global.Instance.UpdatePassedTime ();

		/*
		Magicant.AppConfig.Instance.AdManager.InterstitialAdIntervalTimeSec = (int)Magicant.FirebaseRemoteConfig.Instance.GetLong ("AD_INTERVAL_SEC_RETURN");

		txtIntervalTime.text = Magicant.FirebaseRemoteConfig.Instance.GetLong ("AD_INTERVAL_SEC_RETURN").ToString();
		txtStartTime.text = Global.Instance.PassedResumeAdTime.ToString();
		txtResumeTime.text = Global.Instance.GetPassedResumeAdTime ().ToString();
		*/

		currentScreenController.Update ();
		if (currentScreenController.isChangeScreen ()) {
            ScreenController prevScreenController = currentScreenController;
            prevScreenController.gameObject.SetActive(false);
            currentScreenController = prevScreenController.getChangeScreen ();
            prevScreenController.Initialize();
			currentScreenController.Initialize ();
            currentScreenController.gameObject.SetActive(true);
		}			
	}

	[SerializeField]
	Text txtIntervalTime;

	[SerializeField]
	Text txtStartTime;

	[SerializeField]
	Text txtResumeTime;

	[SerializeField]
	GameObject objLoading;

	#if UNITY_ANDROID
	bool firstPauseSkip = false;
//	bool firstPauseSkip = true;
	#else
	bool firstPauseSkip = false;
	#endif

/*		
	void ResumeInterstitialCallback(Magicant.AdCategory category, bool ret) {
		objLoading.SetActive(false);
		Global.Instance.ResetPassedResumeAdTime ();
	}
*/
	void OnApplicationPause (bool status)
	{
		bool skip = false;

//		Magicant.AppConfig.Instance.AdManager.InterstitialAdOnResume.LoadingTimeSec = 1.0f;

		if (!status) {
			if (Global.Instance.isRewardMovieOpen) {
				skip = true;
				Global.Instance.isRewardMovieOpen = false;
			}
			if (Global.Instance.isClearInterstitialOpen) {
				skip = true;
				Global.Instance.isClearInterstitialOpen = false;
			}
		}

		if (SaveData.Instance.GetRemoveAds () == 1) {
			skip = true;
		}

		/*
		if (isInitialize && !skip) {
			if (!status) {
				if (SaveData.Instance.isChangeLastLaunchDate () && !firstPauseSkip) {
					AnalyticsManager.Instance.SendLaunch ();
					SaveData.Instance.SetLastLaunchDate ();
				}
				if (!Global.Instance.isShopDialogOpen && !firstPauseSkip) {
					if (Global.Instance.GetPassedResumeAdTime () > Magicant.FirebaseRemoteConfig.Instance.GetLong ("AD_INTERVAL_SEC_RETURN")) {
						if (Magicant.AdManager.Instance.IsInterstitialAdLoaded (Magicant.AdCategory.RESUME_INTERSTITIAL)) {
							objLoading.SetActive (true);
							AnalyticsManager.Instance.SendReturnInterstitialShow ();
							Magicant.AdManager.Instance.GetInterstitialAdListener(Magicant.AdCategory.RESUME_INTERSTITIAL).OnAdLeavingApplication += (Magicant.AdCategory category) => {
								AnalyticsManager.Instance.SendReturnInterstitialClick();
							};
							Magicant.AdManager.Instance.ShowInterstitialWithLoading (Magicant.AdCategory.RESUME_INTERSTITIAL, ResumeInterstitialCallback, 1.0f);
						} else {
							Magicant.AdManager.Instance.LoadInterstitialAd (Magicant.AdCategory.RESUME_INTERSTITIAL);
						}
					}
				}
				firstPauseSkip = false;
			}
		}
		*/
	}
}
