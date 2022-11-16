using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YCLib.Utility;

using UniRx;
using UniRx.Triggers;

using TMPro;

public class Game : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI txtClear;

    enum GamePhase
    {
        INITIALIZE,

        TOUCHWAIT,
        MOVE,

		GAMEFAILED,
		GAMECLEAR,
	};

    GamePhase gamePhase = GamePhase.INITIALIZE;

	GamePhase nextGamePhase = GamePhase.INITIALIZE;

	List<GameObject> listFloor = new List<GameObject>();
    List<GameObject> listBlock = new List<GameObject>();

    GameInfo gameInfo;

    private void Initialize()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        string[] packname = {
                    "01_OPAL",
                    "02_AMETHYST",
                    "03_GARNET",
                    "04_AQUAMARINE",
                    "05_QUARTZ"
                };

        StageData.Instance.Initialize();
        StageData.Instance.Load(packname[gameInfo.SelectLevel], gameInfo.SelectStage + 1);

        for (int y = 0; y < StageData.Instance.data.Height; y++)
        {
            for (int x = 0; x < StageData.Instance.data.Width; x++)
            {
                // Floor配置
                GameObject objFloor = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.FLOOR);

                objFloor.transform.SetParent(GameObject.Find("Game/Stage").transform);
                objFloor.transform.localPosition = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0.5f);
                objFloor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                objFloor.SetActive(true);

                listFloor.Add(objFloor);

                // 固定ブロック配置
                switch (StageData.Instance.data.Grid[y, x])
                {
                    case Data.GRIDTYPE_NEEDBLOCK:
                    case Data.GRIDTYPE_NEEDLESSBLOCK:
                    case Data.GRIDTYPE_SWITCHBLOCK:

                        GameObject obj = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.BLOCK);

                        obj.transform.SetParent(GameObject.Find("Game/Stage").transform);
                        obj.transform.localPosition = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0);
                        obj.transform.localRotation = Quaternion.Euler(0,0,0);
                        obj.SetActive(true);

                        listBlock.Add(obj);
                        break;
                }
            }
        }

        // プレイヤー
        CurrentPlayerPos = new Position(StageData.Instance.data.AnswerRoute[0].x, StageData.Instance.data.AnswerRoute[0].y);

        objPlayer = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.PLAYER);
        objPlayer.transform.SetParent(GameObject.Find("Game/Stage").transform);
        objPlayer.transform.localPosition = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);
        objPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        objPlayer.SetActive(true);

        playerBlock = new Block(objPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
        playerBlock.SetPlayer();

		txtClear.enabled = false;

		gamePhase = GamePhase.TOUCHWAIT;
    }

    Position CurrentPlayerPos;

    GameObject objPlayer;
    Block playerBlock;

    bool isActive(GamePhase phase)
    {
        return gamePhase == phase;
    }

	public enum Direction
    {
		UP,
		DOWN,
		LEFT,
		RIGHT
    }

	Vector3 StartPos;
    Vector3 EndPos;

//	bool GameCleared = false;
//	bool GameFailed = false;

	private bool isGoal(int x, int y)
	{
		if (StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].x == x &&
			StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].y == y)
		{
			return true;
		}
		return false;
	}

	private int SearchMoveTarget(Direction direction)
	{
		switch (direction)
		{
			case Direction.UP:
				for (int i = CurrentPlayerPos.y; i >= 0; i--)
				{
					switch (StageData.Instance.data.Grid[i, CurrentPlayerPos.x])
					{
						case Data.GRIDTYPE_NOBLOCK:
							if (isGoal(CurrentPlayerPos.x, i))
							{
								nextGamePhase = GamePhase.GAMECLEAR;
//								GameCleared = true;
								return i;
								/*
								if (isSwitchBlockOn())
								{
									GameCleared = true;
									System.TimeSpan diffTime = System.DateTime.Now - StartTime;
									Global.Instance.SetClearTime(diffTime.Seconds);
									AnalyticsManager.Instance.SendClearStage();
									return i;
								}
								else
								{
									goalBlock.SetTouchAnime(direction);
									return i + 1;
								}
								*/
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							/*
							for (int j = 0; j < switchBlock.Count; j++)
							{
								if (switchBlock[j].x == CurrentPlayerPos.x && switchBlock[j].y == i && switchBlock[j].isSwitchBlock())
								{
									switchBlock[j].SetTouchAnime(direction);
									switchBlock[j].SetSwitchFlag(false);
								}
							}
							*/
							return i + 1;
						default:
							/*
							for (int j = 0; j < normalBlock.Count; j++)
							{
								if (normalBlock[j].x == CurrentPlayerPos.x && normalBlock[j].y == i)
								{
									normalBlock[j].SetTouchAnime(direction);
								}
							}
							*/
							return i + 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
//				GameFailed = true;
				return -5;
			case Direction.DOWN:
				for (int i = CurrentPlayerPos.y; i < StageData.Instance.data.Height; i++)
				{
					switch (StageData.Instance.data.Grid[i, CurrentPlayerPos.x])
					{
						case Data.GRIDTYPE_NOBLOCK:
							if (isGoal(CurrentPlayerPos.x, i))
							{
								nextGamePhase = GamePhase.GAMECLEAR;
//								GameCleared = true;
								return i;
								/*
								if (isSwitchBlockOn())
								{
									GameCleared = true;
									return i;
								}
								else
								{
									goalBlock.SetTouchAnime(direction);
									return i - 1;
								}
								*/
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							/*
							for (int j = 0; j < switchBlock.Count; j++)
							{
								if (switchBlock[j].x == CurrentPlayerPos.x && switchBlock[j].y == i && switchBlock[j].isSwitchBlock())
								{
									switchBlock[j].SetTouchAnime(direction);
									switchBlock[j].SetSwitchFlag(false);
								}
							}
							*/
							return i - 1;
						default:
							/*
							for (int j = 0; j < normalBlock.Count; j++)
							{
								if (normalBlock[j].x == CurrentPlayerPos.x && normalBlock[j].y == i)
								{
									normalBlock[j].SetTouchAnime(direction);
								}
							}
							*/
							return i - 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
//				GameFailed = true;
				return StageData.Instance.data.Height + 5;
			case Direction.RIGHT:
				for (int i = CurrentPlayerPos.x; i < StageData.Instance.data.Width; i++)
				{
					switch (StageData.Instance.data.Grid[CurrentPlayerPos.y, i])
					{
						case Data.GRIDTYPE_NOBLOCK:

							if (isGoal(i, CurrentPlayerPos.y))
							{
								nextGamePhase = GamePhase.GAMECLEAR;
//								GameCleared = true;
								return i;
								/*
								if (isSwitchBlockOn())
								{
									GameCleared = true;
									return i;
								}
								else
								{
									goalBlock.SetTouchAnime(direction);
									return i - 1;
								}
								*/
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							/*
							for (int j = 0; j < switchBlock.Count; j++)
							{
								if (switchBlock[j].x == i && switchBlock[j].y == CurrentPlayerPos.y && switchBlock[j].isSwitchBlock())
								{
									switchBlock[j].SetTouchAnime(direction);
									switchBlock[j].SetSwitchFlag(false);
								}
							}
							*/
							return i - 1;
						default:
							/*
							for (int j = 0; j < normalBlock.Count; j++)
							{
								if (normalBlock[j].x == i && normalBlock[j].y == CurrentPlayerPos.y)
								{
									normalBlock[j].SetTouchAnime(direction);
								}
							}
							*/
							return i - 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
//				GameFailed = true;
				return StageData.Instance.data.Width + 5;
			case Direction.LEFT:
				for (int i = CurrentPlayerPos.x; i >= 0; i--)
				{
					switch (StageData.Instance.data.Grid[CurrentPlayerPos.y, i])
					{
						case Data.GRIDTYPE_NOBLOCK:
							if (isGoal(i, CurrentPlayerPos.y))
							{
								nextGamePhase = GamePhase.GAMECLEAR;
//								GameCleared = true;
								return i;
								/*
								if (isSwitchBlockOn())
								{
									GameCleared = true;
									return i;
								}
								else
								{
									goalBlock.SetTouchAnime(direction);
									return i + 1;
								}
								*/
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							/*
							for (int j = 0; j < switchBlock.Count; j++)
							{
								if (switchBlock[j].x == i && switchBlock[j].y == CurrentPlayerPos.y && switchBlock[j].isSwitchBlock())
								{
									switchBlock[j].SetTouchAnime(direction);
									switchBlock[j].SetSwitchFlag(false);
								}
							}
							*/
							return i + 1;
						default:
							/*
							for (int j = 0; j < normalBlock.Count; j++)
							{
								if (normalBlock[j].x == i && normalBlock[j].y == CurrentPlayerPos.y)
								{
									normalBlock[j].SetTouchAnime(direction);
								}
							}
							*/
							return i + 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
//				GameFailed = true;
				return -5;
		}

		return 0;
	}

	// タッチ待ち
	void UpdateTouchWait()
    {
		switch (TouchInfo.Instance.GetTouch())
        {
		    case TouchInfo.TouchPhase.Began:
			    StartPos = Global.Instance.GetTouchPosition ();
				nextGamePhase = GamePhase.TOUCHWAIT;
				break;

		    case TouchInfo.TouchPhase.Moved:
			    EndPos = Global.Instance.GetTouchPosition ();

			    float distance = (StartPos - EndPos).sqrMagnitude;

			    if (distance > 300.0f) {
				    float movex = StartPos.x - EndPos.x;
				    float movey = StartPos.y - EndPos.y;
					Direction dir;

				    if (Mathf.Abs (movex) > Mathf.Abs (movey)) {
					    // 横移動
					    dir = (movex > 0.0f) ? Direction.LEFT : Direction.RIGHT;
					    int TargetNum = SearchMoveTarget (dir);
					    playerBlock.SetMove (dir, (float)CurrentPlayerPos.x, (float)TargetNum, Mathf.Abs (0.05f * (CurrentPlayerPos.x - TargetNum)));
					    CurrentPlayerPos.x = TargetNum;
				    } else {
					    // 縦移動
					    dir = (movey > 0.0f) ? Direction.UP : Direction.DOWN;
					    int TargetNum = SearchMoveTarget (dir);
						Debug.Log("CurrentPlayerPos.y " + CurrentPlayerPos.y + " TargetNum " + TargetNum);
					    playerBlock.SetMove(dir, (float)CurrentPlayerPos.y, (float)TargetNum, Mathf.Abs(0.05f * (CurrentPlayerPos.y - TargetNum)));
					    CurrentPlayerPos.y = TargetNum;
				    }
				    gamePhase = GamePhase.MOVE;
			    }
			    break;

		    case TouchInfo.TouchPhase.Ended:
			    break;
		}
    }

	void UpdateMove()
    {
		if (!playerBlock.Update())
		{
			gamePhase = nextGamePhase;
			/*
			if (GameFailed)
			{
				GamePhase = GAMEPHASE_GAMEFAILED;
			}
			else if (GameCleared)
			{
				gamePhase = GamePhase.TOUCHWAIT;
			}
			else
			{
				//				RestartTouchCheck = false;
				gamePhase = GamePhase.TOUCHWAIT;
			}
			*/
		}
	}

	void UpdateGameClear()
	{
		txtClear.enabled = true;

		if (Input.GetMouseButton(0))
        {
			txtClear.enabled = false;
		}
	}

	void UpdateGameFailed()
    {
		// 操作ブロック配置
		CurrentPlayerPos = new Position(StageData.Instance.data.AnswerRoute[0].x, StageData.Instance.data.AnswerRoute[0].y);

		objPlayer.transform.position = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);

		playerBlock = new Block(objPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
		playerBlock.SetPlayer();

		for (int y = 0; y < StageData.Instance.data.Height; y++)
		{
			for (int x = 0; x < StageData.Instance.data.Width; x++)
			{
				// 固定ブロック配置
				switch (StageData.Instance.data.Grid[y, x])
				{
					case Data.GRIDTYPE_NEEDBLOCK:
					case Data.GRIDTYPE_NEEDLESSBLOCK:
					case Data.GRIDTYPE_SWITCHBLOCK:
///						List<Block> curList = (StageData.Instance.data.Grid[y, x] == Data.GRIDTYPE_SWITCHBLOCK) ? switchBlock : normalBlock;
						break;
				}
			}
		}

		//		objPlayer.transform.SetAsLastSibling ();

		//		StartAnimeTime = 0.0f;

		//		StartTime = System.DateTime.Now;

		gamePhase = GamePhase.TOUCHWAIT;
	}

	// Start is called before the first frame update
	void Start()
    {
        this.UpdateAsObservable().Where(_ => isActive(GamePhase.INITIALIZE)).Subscribe(_ => Initialize()).AddTo(this);
		this.UpdateAsObservable().Where(_ => isActive(GamePhase.TOUCHWAIT)).Subscribe(_ => UpdateTouchWait()).AddTo(this);
		this.UpdateAsObservable().Where(_ => isActive(GamePhase.MOVE)).Subscribe(_ => UpdateMove()).AddTo(this);
		this.UpdateAsObservable().Where(_ => isActive(GamePhase.GAMECLEAR)).Subscribe(_ => UpdateGameClear()).AddTo(this);
		this.UpdateAsObservable().Where(_ => isActive(GamePhase.GAMEFAILED)).Subscribe(_ => UpdateGameFailed()).AddTo(this);
	}

}
