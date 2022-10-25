using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogShop : MonoBehaviour {

	[SerializeField]
	ButtonControl btnClose;

	[SerializeField]
	ButtonControl[] button;

	[SerializeField]
	GameObject objButtonRemoveAds;

	[SerializeField]
	GameObject objButtonRestore;

	[SerializeField]
	TextMeshProUGUI txtHintNum;

	[SerializeField]
	Text[] txtPrice;
//	TextMeshProUGUI[] txtPrice;

	[SerializeField]
	GameObject	objDialogNoVideo;

	const int BUTTON_REMOVEADS = 0;
	const int BUTTON_VIDEOREWORD = 1;
	const int BUTTON_HINT10 = 2;
	const int BUTTON_HINT55 = 3;
	const int BUTTON_HINT110 = 4;

	bool GetMetaData = false;

	// Use this for initialization
	void Start () {
		// リストア処理 (iOSのみ)
		#if UNITY_IOS
		objButtonRestore.SetActive(!SaveData.Instance.isRestored());
		#else
		objButtonRestore.SetActive(false);
		#endif

		if (SaveData.Instance.GetRemoveAds () == 1) {
			objButtonRemoveAds.SetActive (false);
		}

		GetMetaData = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Global.Instance.isShopDialogOpen = true;

		if (objButtonRemoveAds.activeSelf) {
			if (SaveData.Instance.GetRemoveAds () == 1) {
				objButtonRemoveAds.SetActive (false);
			}
		}

		if (!GetMetaData) {
			/*
			Magicant.IAPMetadata meta = Magicant.IAPManager.Instance.GetMetadata (Magicant.IAPProduct.REMOVEADS);

			if (meta != null) {
				txtPrice [BUTTON_REMOVEADS].text = meta.LocalizedPriceString;
				txtPrice [BUTTON_HINT10].text = Magicant.IAPManager.Instance.GetMetadata (Magicant.IAPProduct.HINT10).LocalizedPriceString;
				txtPrice [BUTTON_HINT55].text = Magicant.IAPManager.Instance.GetMetadata (Magicant.IAPProduct.HINT55).LocalizedPriceString;
				txtPrice [BUTTON_HINT110].text = Magicant.IAPManager.Instance.GetMetadata (Magicant.IAPProduct.HINT110).LocalizedPriceString;
			}
			*/
			GetMetaData = true;
		}

		if (objDialogNoVideo.activeSelf) {
			return;
		}

		if (Global.Instance.isHintReflesh ()) {
			txtHintNum.text = SaveData.Instance.GetHintNum().ToString();
		}

		if (button [BUTTON_REMOVEADS].isClick ()) {	
			// 課金処理
//			Magicant.IAPManager.Instance.Purchase(Magicant.IAPProduct.REMOVEADS);
		}

		if (button [BUTTON_VIDEOREWORD].isClick ()) {
			/*
			// 動画リワード表示
			if (Magicant.AdManager.Instance.IsRewardBasedVideoAdLoaded (Magicant.AdCategory.REWARDMOVIE)) {
				AnalyticsManager.Instance.SendRewardMovieShow ();
				Global.Instance.isRewardMovieOpen = true;
				Magicant.AdManager.Instance.ShowRewardBasedVideoAd (Magicant.AdCategory.REWARDMOVIE, (Magicant.AdCategory category, bool isShown, bool isRewarded) => {
					// 広告を閉じた際の処理
					if (isRewarded) {
						AnalyticsManager.Instance.SendRewardMovieWatch ();
						SaveData.Instance.AddHintNum(1);
						SaveData.Instance.Save();
						AnalyticsManager.Instance.SendRewardMovieWatch ();
						txtHintNum.text = SaveData.Instance.GetHintNum().ToString();
					}
				});
			} else {
				objDialogNoVideo.SetActive (true);
			}
			*/
		}

		if (button [BUTTON_HINT10].isClick ()) {	
			// 課金処理
//			Magicant.IAPManager.Instance.Purchase(Magicant.IAPProduct.HINT10);
		}
		if (button [BUTTON_HINT55].isClick ()) {	
			// 課金処理
//			Magicant.IAPManager.Instance.Purchase(Magicant.IAPProduct.HINT55);
		}
		if (button [BUTTON_HINT110].isClick ()) {	
			// 課金処理
//			Magicant.IAPManager.Instance.Purchase(Magicant.IAPProduct.HINT110);
		}

		// リストア処理 (iOSのみ)
		#if UNITY_IOS
		/*
		if ( !SaveData.Instance.isRestored() ){
			if (objButtonRestore.GetComponent<ButtonControl> ().isClick ()) {
				Magicant.IAPManager.Instance.Restore (RestoreCallback);
			}
		}
		*/
		#endif

		if (btnClose.isClick ()) {
			Global.Instance.isShopDialogOpen = false;
			this.gameObject.SetActive (false);
		}
	}

	private void RestoreCallback(bool ret) {
		if (ret) {
			SaveData.Instance.SetRestored ();
			objButtonRemoveAds.SetActive (false);
			objButtonRestore.SetActive(false);
		}
	}
}
