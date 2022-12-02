using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInfo
{
    private static TouchInfo mInstance;

    private Vector3 TouchPosition = Vector3.zero;

    public static TouchInfo Instance
    {
        get
        {
            if (mInstance == null) mInstance = new TouchInfo();
            return mInstance;
        }
    }

    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public TouchPhase GetTouch()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) { return TouchPhase.Began; }
        if (Input.GetMouseButton(0)) { return TouchPhase.Moved; }
        if (Input.GetMouseButtonUp(0)) { return TouchPhase.Ended; }
#else
		if (Input.touchCount > 0)
		{
			return (TouchPhase)((int)Input.GetTouch(0).phase);
		}
#endif

        return TouchPhase.None;
    }

    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public Vector3 GetTouchPosition()
    {
#if UNITY_EDITOR
        TouchPhase touch = GetTouch();
        if (touch != TouchPhase.None)
        {
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

    /// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
    public Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }

    /// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
    public enum TouchPhase
    {        
        Began = 0,          // タッチ開始
        Moved = 1,          // タッチ移動
        Stationary = 2,     // タッチ静止
        Ended = 3,          // タッチ終了
        Canceled = 4,       // タッチキャンセル

        None = 99,          // タッチなし
    }
}
