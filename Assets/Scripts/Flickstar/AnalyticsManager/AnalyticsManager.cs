using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager {

	private static AnalyticsManager mInstance;

	private AnalyticsManager() {
	}

	public static AnalyticsManager Instance {
		get {
			if (mInstance == null) mInstance = new AnalyticsManager();
			return mInstance;
		}
	}

	public void SendPlayStage() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("play_stage", param );
		*/
	}

	public void SendClearStage() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[3];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());
		param [2] = new Magicant.AnalyticsParameter ("time", Global.Instance.GetClearTime().ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("clear_stage", param );
		*/
	}

	public void SendUseHint() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("use_hint", param );
		*/
	}

	public void SendLaunch() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[1];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());

		Magicant.AnalyticsManager.Instance.LogEvent("launch", param );
		*/
	}

	public void SendStageClearInterstitialShow() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_show_interstitial_stage_clear", param );
		*/
	}

	public void SendStageClearInterstitialClick() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_click_interstitial_stage_clear", param );
		*/
	}

	public void SendReturnInterstitialShow() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[1];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_show_interstitial_return", param );
		*/
	}

	public void SendReturnInterstitialClick() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[1];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_click_interstitial_return", param );
		*/
	}

	public void SendRewardMovieShow() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_begin_reward_movie_hint", param );
		*/
	}

	public void SendRewardMovieWatch() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[2];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());
		param [1] = new Magicant.AnalyticsParameter ("stage", Global.Instance.GetCurrentPackName()+"_"+Global.Instance.SelectStageNo.ToString());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_watched_reward_movie_hint", param );
		*/
	}

	public void SendFutterBannerClick() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[1];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());

		Magicant.AnalyticsManager.Instance.LogEvent("ad_click_banner_footer", param );
		*/
	}

	public void SendRemoveAds() {
		/*
		Magicant.AnalyticsParameter[] param = new Magicant.AnalyticsParameter[1];

		param [0] = new Magicant.AnalyticsParameter ("day", SaveData.Instance.GetPlayDays());

		Magicant.AnalyticsManager.Instance.LogEvent("no_ads_purchased", param );
		*/
	}
}
