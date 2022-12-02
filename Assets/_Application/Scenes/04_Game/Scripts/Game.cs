using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using YCLib.Utility;

using UniRx;
using UniRx.Triggers;

using TMPro;

public class Game : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI txtClear;
	[SerializeField] TextMeshProUGUI txtFailed;

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
					case Data.GRIDTYPE_NOBLOCK:
						// Floor配置
						if (!(StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].x == x && StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].y == y))
						{
							GameObject objFloor = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.FLOOR);

							objFloor.transform.SetParent(GameObject.Find("Game/Stage").transform);
							objFloor.transform.localPosition = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0.5f);
							objFloor.transform.localRotation = Quaternion.Euler(0, 0, 0);
							objFloor.SetActive(true);

							listFloor.Add(objFloor);
						}
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

		objGoal = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.GOAL);
		objGoal.transform.SetParent(GameObject.Find("Game/Stage").transform);
		objGoal.transform.localPosition = new Vector3(StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].x - (StageData.Instance.data.Width / 2.0f), StageData.Instance.data.AnswerRoute[StageData.Instance.data.AnswerRoute.Count - 1].y - (StageData.Instance.data.Height / 2.0f), 0.5f);
		objGoal.transform.localRotation = Quaternion.Euler(0, 0, 0);
		objGoal.SetActive(true);

		txtClear.enabled = false;
		txtFailed.enabled = false;

		gamePhase = GamePhase.TOUCHWAIT;
    }

    Position CurrentPlayerPos;

    GameObject objPlayer;
    Block playerBlock;

	GameObject objGoal;

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
								return i;
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							return i + 1;
						default:
							return i + 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
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
								return i;
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							return i - 1;
						default:
							return i - 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
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
								return i;
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							return i - 1;
						default:
							return i - 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
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
								return i;
							}
							break;
						case Data.GRIDTYPE_SWITCHBLOCK:
							return i + 1;
						default:
							return i + 1;
					}
				}
				nextGamePhase = GamePhase.GAMEFAILED;
				return -5;
		}

		return 0;
	}

	void UpdateGameClear()
	{
		txtClear.enabled = true;

		if (Input.GetMouseButtonUp(0))
        {
			SaveData.Instance.SetStageStatus(gameInfo.SelectLevel, gameInfo.SelectStage, Global.STAGEICON_CLEAR);
			Global.Instance.SetStageStatus(gameInfo.SelectLevel, gameInfo.SelectStage, Global.STAGEICON_CLEAR);

			gameInfo.SelectStage++;

			foreach (GameObject obj in listBlock)
			{
				YCLib.Utility.ObjectPool.Return(0, obj);
			}

			YCLib.Utility.ObjectPool.Return(1, objPlayer);
			YCLib.Utility.ObjectPool.Return(2, objGoal);

			if (gameInfo.SelectStage >= 60)
			{
				gameInfo.SelectStage = 0;
				SceneManager.LoadSceneAsync("LevelSelect");
			}
			else
			{
				if (SaveData.Instance.GetStageStatus(gameInfo.SelectLevel, gameInfo.SelectStage) == Global.STAGEICON_LOCK)
				{
					SaveData.Instance.SetStageStatus(gameInfo.SelectLevel, gameInfo.SelectStage, Global.STAGEICON_OPEN);
				}
				Initialize();
			}

			SaveData.Instance.Save();

			txtClear.enabled = false;
		}
	}

	void UpdateGameFailed()
    {
		txtFailed.enabled = true;

		if (Input.GetMouseButtonUp(0))
		{
			txtFailed.enabled = false;

			// 操作ブロック配置
			CurrentPlayerPos = new Position(StageData.Instance.data.AnswerRoute[0].x, StageData.Instance.data.AnswerRoute[0].y);

			objPlayer.transform.localPosition = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);

			playerBlock = new Block(objPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
			playerBlock.SetPlayer();

			/*
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
			*/
			gamePhase = GamePhase.TOUCHWAIT;
		}
	}

	float Speed = 0.025f;

	void SetTouchPhase()
    {
		this.UpdateAsObservable()
			.Where(_ => isActive(GamePhase.TOUCHWAIT) && TouchInfo.Instance.GetTouch() == TouchInfo.TouchPhase.Began)
			.Subscribe(_ =>
			{
				StartPos = Global.Instance.GetTouchPosition();
				nextGamePhase = GamePhase.TOUCHWAIT;
			}).AddTo(this);

		this.UpdateAsObservable()
			.Where(_ => isActive(GamePhase.TOUCHWAIT) && TouchInfo.Instance.GetTouch() == TouchInfo.TouchPhase.Moved)
			.Subscribe(_ =>
			{
				Direction dir;

				EndPos = Global.Instance.GetTouchPosition();

				float distance = (StartPos - EndPos).sqrMagnitude;

				if (distance > 300.0f)
				{
					float movex = StartPos.x - EndPos.x;
					float movey = StartPos.y - EndPos.y;

					if (Mathf.Abs(movex) > Mathf.Abs(movey))
					{
						// 横移動
						dir = (movex > 0.0f) ? Direction.LEFT : Direction.RIGHT;
						int TargetNum = SearchMoveTarget(dir);
						playerBlock.SetMove(dir, (float)CurrentPlayerPos.x, (float)TargetNum, Mathf.Abs(Speed * (CurrentPlayerPos.x - TargetNum)));
						CurrentPlayerPos.x = TargetNum;
					}
					else
					{
						// 縦移動
						dir = (movey > 0.0f) ? Direction.UP : Direction.DOWN;
						int TargetNum = SearchMoveTarget(dir);
						playerBlock.SetMove(dir, (float)CurrentPlayerPos.y, (float)TargetNum, Mathf.Abs(Speed * (CurrentPlayerPos.y - TargetNum)));
						CurrentPlayerPos.y = TargetNum;
					}
					gamePhase = GamePhase.MOVE;
				}
			}).AddTo(this);
	}

	// Start is called before the first frame update
	void Start()
    {
        this.UpdateAsObservable().Where(_ => isActive(GamePhase.INITIALIZE)).Subscribe(_ => Initialize()).AddTo(this);

		SetTouchPhase();

		this.UpdateAsObservable()
			.Where(_ => isActive(GamePhase.MOVE))
			.Subscribe(_ =>
			{
				if (!playerBlock.Update())
				{
					gamePhase = nextGamePhase;
				}
			})
			.AddTo(this);

		this.UpdateAsObservable().Where(_ => isActive(GamePhase.GAMECLEAR)).Subscribe(_ => UpdateGameClear()).AddTo(this);
		this.UpdateAsObservable().Where(_ => isActive(GamePhase.GAMEFAILED)).Subscribe(_ => UpdateGameFailed()).AddTo(this);
	}

}
