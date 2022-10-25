using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogGetHint : MonoBehaviour {

	[SerializeField]
	ButtonControl btnClose;

	[SerializeField]
	ButtonControl btnGet10Hints;

	[SerializeField]
	GameObject	objDialogNoVideo;

	const int DLGGETHINT_IDLE = 0;
	const int DLGGETHINT_NOVIDEO = 1;

	int phase = DLGGETHINT_IDLE;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		switch (phase) {
		case DLGGETHINT_IDLE:
			if (btnGet10Hints.isClick ()) {
					/*
				// 動画リワード表示
				if (Magicant.AdManager.Instance.IsRewardBasedVideoAdLoaded (Magicant.AdCategory.REWARDMOVIE)) {
					AnalyticsManager.Instance.SendRewardMovieShow ();
					Magicant.AdManager.Instance.ShowRewardBasedVideoAd (Magicant.AdCategory.REWARDMOVIE, (Magicant.AdCategory category, bool isShown, bool isRewarded) => {
						// 広告を閉じた際の処理
						if (isRewarded) {
							AnalyticsManager.Instance.SendRewardMovieWatch ();
							SaveData.Instance.AddHintNum(10);
							SaveData.Instance.Save();
							this.gameObject.SetActive (false);
						}
					});
				} else
					*/
					{
					objDialogNoVideo.SetActive (true);
					phase = DLGGETHINT_NOVIDEO;
				}
			} else if (btnClose.isClick ()) {
				this.gameObject.SetActive (false);
			}
			break;
		case DLGGETHINT_NOVIDEO:
			if (!objDialogNoVideo.activeSelf) {
				phase = DLGGETHINT_IDLE;
			}
			break;
		}
	}
}
