using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData {

	const int DEFAULT_HINT_NUM = 20;

	bool DebugADS = false;

	private static SaveData mInstance;

	private SaveData() {
	}

	public static SaveData Instance {
		get {
			if (mInstance == null) mInstance = new SaveData();
			return mInstance;
		}
	}

	private void FirstInitialize() {
		Debug.Log ("FirstInitialize");
		for (int i = 0; i < Global.Instance.PACK_NUM; i++) {
			SetStageStatus (i,0,Global.STAGEICON_OPEN);
			SetStageHintStatus (i,0,Global.Instance.STAGEHINTICON_OFF);
			for (int j = 1; j < Global.Instance.GetPackStageNum(i); j++) {
				SetStageStatus (i,j,Global.STAGEICON_LOCK);
				SetStageHintStatus (i,j,Global.Instance.STAGEHINTICON_OFF);
			}
		}
		Save ();
	}

	public void DebugDeleteAll() {
		PlayerPrefs.DeleteAll();
	}

	public bool isFirstPlay() {
		return (!PlayerPrefs.HasKey ("0_0"));
	}

	public void Initialize1_0_0() {
		if (!PlayerPrefs.HasKey ("0_0")) {
			// 初回起動
			FirstInitialize ();
		}
	}

	public void Initialize1_1_0() {
		if (GetHintNum()<0) {
			SetHintNum (DEFAULT_HINT_NUM);
		}
		if (!PlayerPrefs.HasKey ("major_version")) {
			SetAppVersion(1,1,0);
		}
		if (!PlayerPrefs.HasKey ("removeads")) {
			SetRemoveAds(0);
		}
		if (!PlayerPrefs.HasKey ("startdate_year")) {
			System.DateTime now = System.DateTime.Now;
			PlayerPrefs.SetInt ("startdate_year", now.Year);
			PlayerPrefs.SetInt ("startdate_month", now.Month);
			PlayerPrefs.SetInt ("startdate_day", now.Day);
			PlayerPrefs.SetInt ("startdate_hour", now.Hour);
			PlayerPrefs.SetInt ("startdate_minute", now.Minute);
			PlayerPrefs.SetInt ("startdate_second", now.Second);
		}
		if (!PlayerPrefs.HasKey ("lastlaunchdate_year")) {
			SetLastLaunchDate ();
		}
	}

	System.DateTime StartDate;

	public string GetPlayDays() {
		System.TimeSpan diffTime = System.DateTime.Now - StartDate;
		return diffTime.Days.ToString ();
	}

	public void Initialize() {

//		DebugDeleteAll ();

		Initialize1_0_0 ();
		Initialize1_1_0 ();

		StartDate = new System.DateTime ( PlayerPrefs.GetInt ("startdate_year"),
										  PlayerPrefs.GetInt ("startdate_month"),
										  PlayerPrefs.GetInt ("startdate_day"),
										  PlayerPrefs.GetInt ("startdate_hour"),
										  PlayerPrefs.GetInt ("startdate_minute"),
										  PlayerPrefs.GetInt ("startdate_second"));

//		SetHintNum (1);

	}

	public void SetAppVersion( int major, int minor, int buildno ) {
		PlayerPrefs.SetInt ("major_version", major);
		PlayerPrefs.SetInt ("minor_version", minor);
		PlayerPrefs.SetInt ("build_number", buildno);
	}

	public int GetAppMajorVersion() {
		return PlayerPrefs.GetInt ("major_version", -1);
	}

	public int GetAppMinorVersion() {
		return PlayerPrefs.GetInt ("minor_version", -1);
	}

	public int GetAppBuildNumber() {
		return PlayerPrefs.GetInt ("build_number", -1);
	}

	public void SetHintNum( int num ) {
		PlayerPrefs.SetInt ("hint_num", num);
	}

	public void AddHintNum( int num ) {
		int hintnum = PlayerPrefs.GetInt ("hint_num", 0) + num;
		PlayerPrefs.SetInt ("hint_num", hintnum);
	}

	public void DegHintNum( int num ) {
		int hintnum = PlayerPrefs.GetInt ("hint_num", 0) - num;
		PlayerPrefs.SetInt ("hint_num", (hintnum<0) ? 0 : hintnum);
	}

	public int GetHintNum() {
		return PlayerPrefs.GetInt ("hint_num", -1);
	}

	public void SetStageStatus( int packid, int stageno, int status ) {
		PlayerPrefs.SetInt (packid.ToString () + "_" + stageno.ToString(), status);
	}

	public int GetStageStatus( int packid, int stageno ) {
		return PlayerPrefs.GetInt (packid.ToString () + "_" + stageno.ToString (), -1);
	}

	public void SetStageHintStatus( int packid, int stageno, int status ) {
		PlayerPrefs.SetInt ("hint_" + packid.ToString () + "_" + stageno.ToString(), status);
	}

	public int GetStageHintStatus( int packid, int stageno ) {
		Debug.Log ("GetStageHintStatus "+PlayerPrefs.GetInt ("hint_" + packid.ToString () + "_" + stageno.ToString (), -1).ToString());
		return PlayerPrefs.GetInt ("hint_" + packid.ToString () + "_" + stageno.ToString (), -1);
	}

	public void Save() {
		PlayerPrefs.Save ();
	}

	public void SetDebugAds(bool flag) {
		DebugADS = flag;
	}

	public void SetRemoveAds(int ads) {
		PlayerPrefs.SetInt ("removeads", ads);
	}

	public int GetRemoveAds() {
		if (DebugADS) {
			return 0;
		}
		return PlayerPrefs.GetInt ("removeads", 0);
	}

	public void SetLastLaunchDate() {
		System.DateTime now = System.DateTime.Now;
		PlayerPrefs.SetInt ("lastlaunchdate_year", now.Year);
		PlayerPrefs.SetInt ("lastlaunchdate_month", now.Month);
		PlayerPrefs.SetInt ("lastlaunchdate_day", now.Day);
	}

	public bool isChangeLastLaunchDate() {
		System.DateTime NowDate = new System.DateTime ( System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0);
		System.DateTime LastLaunchDate = new System.DateTime (PlayerPrefs.GetInt ("lastlaunchdate_year", 0), PlayerPrefs.GetInt ("lastlaunchdate_month", 0), PlayerPrefs.GetInt ("lastlaunchdate_day", 0), 0, 0, 0);

		System.TimeSpan diffTime = NowDate - LastLaunchDate;
		return (diffTime.Days > 0);
	}

	public bool isRestored() {
		return (PlayerPrefs.GetInt ("restored", 0) == 1);
	}

	public void SetRestored() {
		PlayerPrefs.SetInt ("restored", 1);
	}
}
