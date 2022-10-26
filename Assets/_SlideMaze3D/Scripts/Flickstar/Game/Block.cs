using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block
{
	const int MOVE_UP = 0;
	const int MOVE_DOWN = 1;
	const int MOVE_LEFT = 2;
	const int MOVE_RIGHT = 3;

	GameObject objBlock;
//	GameObject objBlockShadow;
//	Image imgSurface;

	int MoveDirection = MOVE_UP;

	float StartMovePos = 0.0f;
	float TargetMovePos = 0.0f;

	float MoveTime = 0.0f;
	float MaxTime = 0.0f;

	float localScale = 1.0f;
	float Scale = 1.0f;

	float AppearTime = 0.0f;

	bool SwitchFlag = false;
	bool MoveFlag = false;

	public int x;
	public int y;

	public Block(GameObject obj, int gridx, int gridy)
//	public Block(GameObject obj, GameObject objshd, int gridx, int gridy)
	{
		objBlock = obj;
//		objBlockShadow = objshd;
		x = gridx;
		y = gridy;
		SwitchFlag = false;
		goalFlag = false;
//		imgSurface = objBlock.transform.Find("BlockSurface").GetComponent<Image>();
	}

	public void SetSwitchFlag(bool flag)
	{
		SwitchFlag = flag;
	}

	public bool isSwitchBlock()
	{
		return SwitchFlag;
	}

	public void SetPlayer()
	{
		localScale = objBlock.transform.localScale.x;
		Scale = 0.0f;
		AppearTime = 0.0f;
		InitClear();
//		objBlock.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
//		objBlockShadow.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
	}

	public bool PlayerAppear()
	{
		bool ret = true;

		AppearTime += Time.deltaTime;

		if (AppearTime > 0.5f)
		{
			AppearTime = 0.5f;
			ret = false;
		}

		Scale = 1.0f * (AppearTime / 0.5f);

//		objBlock.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
//		objBlockShadow.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
		return ret;
	}

	Image GoalStar;
	bool goalFlag = false;

	public void SetGoalStar(Image img)
	{
		GoalStar = img;
		goalFlag = true;
	}

	public void SetGoal(Image img)
	{
		localScale = objBlock.transform.localScale.x;
		RotStar = 0.0f;
		GoalStar = img;
		Scale = 0.0f;
		AppearTime = 0.0f;
		goalFlag = true;
//		objBlock.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
//		objBlockShadow.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
	}

	float RotStar = 0.0f;
	float AppearTimeMax = 1.0f;

	public bool GoalAppear()
	{
		bool ret = true;

		AppearTime += Time.deltaTime;
		RotStar -= 5.0f;

		if (AppearTime > AppearTimeMax)
		{
			AppearTime = AppearTimeMax;
			RotStar = 0.0f;
			ret = false;
		}

		GoalStar.transform.rotation = Quaternion.Euler(0, 0, RotStar);

		Scale = (AppearTime / AppearTimeMax);

//		objBlock.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);
//		objBlockShadow.transform.localScale = new Vector3(localScale * Scale, localScale * Scale, 1.0f);

		return ret;
	}

	public void GoalAnimeInit()
	{
		GoalStar.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	bool GoalEnableAnimeFlag = false;
	float GoalEnableAnimeTime = 0.0f;
	float GoalEnableAnimeTimeMax = 3.0f;

	public void SetGoalEnable()
	{
		GoalEnableAnimeFlag = true;
		GoalEnableAnimeTime = 0.0f;
		RotStar = 0.0f;
	}

	public void GoalEnableAnime()
	{
		if (GoalEnableAnimeFlag)
		{
			GoalEnableAnimeTime += Time.deltaTime;
			if (GoalEnableAnimeTime < GoalEnableAnimeTimeMax)
			{
				RotStar = -GetBreakPos(0.0f, 360.0f * 4.0f, GoalEnableAnimeTime, GoalEnableAnimeTimeMax);
				RotStar %= 360.0f;
			}
			else
			{
				RotStar = 0.0f;
				GoalEnableAnimeFlag = false;
			}
			GoalStar.transform.rotation = Quaternion.Euler(0, 0, RotStar);
		}
	}

	public void SetMove(int dir, float startpos, float targetpos, float time)
	{
		MoveFlag = true;
		MoveDirection = dir;
		StartMovePos = startpos;
		TargetMovePos = targetpos;
		MoveTime = 0.0f;
		MaxTime = time;

		Debug.Log("StartMovePos " + StartMovePos + " : TargetMovePos " + TargetMovePos);
	}

	public bool isMove()
	{
		return MoveFlag;
	}

	public float GetBreakPos(float startpos, float endpos, float nowtime, float maxtime)
	{
		return (startpos + (endpos - startpos) * ((maxtime * 2 - nowtime + 1) * nowtime / 2.0f) / ((maxtime + 1) * maxtime / 2.0f));
	}

	bool ClearAnime = false;
	float ClearScale = 0.0f;

	public void InitClear()
	{
		ClearScale = 1.0f;
//		imgSurface.transform.localScale = new Vector3(1, 1, 1);
	}

	public void SetClear()
	{
		ClearScale = 1.0f;
//		imgSurface.transform.localScale = new Vector3(ClearScale, ClearScale, 1);
		ClearAnime = true;
	}

	public bool isClearAnime()
	{
		return ClearAnime;
	}

	public void ClearAnimeUpdate()
	{

		if (ClearScale > 50.0f)
		{
			ClearAnime = false;
		}

		ClearScale += 0.5f * (60 * Time.deltaTime);
//		imgSurface.transform.localScale = new Vector3(ClearScale, ClearScale, 1);
	}

	bool TouchAnimeRequest = false;
	bool TouchAnime = false;
	int TouchDirection = 0;
	float TouchMoveMax = 5.0f;
	float TouchMoveVal = 0;
	float TouchMoveAddVal = 1.0f;
	float TouchMove = 0.0f;
	float TouchStartPos = 0.0f;

	public void SetTouchAnime(int dir)
	{
		TouchMove = 0.0f;
		TouchMoveVal = TouchMoveMax;
		TouchDirection = dir;
		TouchStartPos = (dir == MOVE_UP || dir == MOVE_DOWN) ? objBlock.GetComponent<RectTransform>().localPosition.y : objBlock.GetComponent<RectTransform>().localPosition.x;
		TouchAnimeRequest = true;
		TouchAnime = false;
	}

	public void PlayTouchAnime()
	{
		if (TouchAnimeRequest)
		{
			TouchAnime = true;
			TouchAnimeRequest = false;
		}
	}

	public bool isTouchAnime()
	{
		return TouchAnime;
	}

	public void TouchAnimeUpdate()
	{
		/*
		if (TouchAnime)
		{

			TouchMove += TouchMoveVal;
			TouchMoveVal -= TouchMoveAddVal;
			if (TouchMoveVal < -TouchMoveMax)
			{
				TouchMoveVal = -TouchMoveMax;
				if (!goalFlag)
				{
					objBlock.transform.Find("BlockSurface").GetComponent<Image>().color = Global.Instance.GetColor(Global.Instance.COL_GRAY);
				}
				TouchAnime = false;
			}

			switch (TouchDirection)
			{
				case MOVE_DOWN:
					objBlock.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, TouchStartPos + TouchMove, 1);
//					objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, TouchStartPos + TouchMove, 1);
					break;
				case MOVE_UP:
					objBlock.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, TouchStartPos - TouchMove, 1);
//					objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, TouchStartPos - TouchMove, 1);
					break;
				case MOVE_LEFT:
					objBlock.GetComponent<RectTransform>().localPosition = new Vector3(TouchStartPos - TouchMove, objBlock.GetComponent<RectTransform>().localPosition.y, 1);
//					objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(TouchStartPos - TouchMove, objBlock.GetComponent<RectTransform>().localPosition.y, 1);
					break;
				case MOVE_RIGHT:
					objBlock.GetComponent<RectTransform>().localPosition = new Vector3(TouchStartPos + TouchMove, objBlock.GetComponent<RectTransform>().localPosition.y, 1);
//					objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(TouchStartPos + TouchMove, objBlock.GetComponent<RectTransform>().localPosition.y, 1);
					break;
			}
		}
		*/
	}

	public Vector3 GetPos()
    {
		return objBlock.GetComponent<RectTransform>().localPosition;
    }

	public bool Update()
	{

		bool ret = true;

		MoveTime += Time.deltaTime;

		float pos = GetBreakPos(StartMovePos, TargetMovePos, MoveTime, MaxTime);

		Debug.Log("pos : " + pos);

		if (MoveTime >= MaxTime)
		{
			pos = TargetMovePos;
			MoveFlag = false;
			ret = false;
		}

		switch (MoveDirection)
		{
			case MOVE_UP:
			case MOVE_DOWN:
				objBlock.transform.position = new Vector3(objBlock.transform.position.x, pos - (StageData.Instance.data.Height / 2.0f), objBlock.transform.position.z);
//				objBlock.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, pos, 0);
//				objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(objBlock.GetComponent<RectTransform>().localPosition.x, pos, 0);
				break;
			case MOVE_LEFT:
			case MOVE_RIGHT:
				objBlock.transform.position = new Vector3(pos - (StageData.Instance.data.Width / 2.0f), objBlock.transform.position.y, objBlock.transform.position.z);
//				objBlock.GetComponent<RectTransform>().localPosition = new Vector3(pos, objBlock.GetComponent<RectTransform>().localPosition.y, 0);
//				objBlockShadow.GetComponent<RectTransform>().localPosition = new Vector3(pos, objBlock.GetComponent<RectTransform>().localPosition.y, 0);
				break;
		}

		if (ClearAnime)
		{
			ClearAnimeUpdate();
		}

		return ret;
	}
}
