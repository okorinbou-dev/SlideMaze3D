using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Position {
	public int x;
	public int y;

	public Position( int _x, int _y ) {
		x = _x;
		y = _y;
	}
}

[Serializable]
public class Data {

	public int Width;
	public int Height;

	public string AnswerRouteJson;
	public string NeedBlockJson;
	public string NeedlessBlockJson;
	public string SwitchBlockJson;

	public List<Position>	AnswerRoute = new List<Position>();
	public List<Position>	NeedBlock = new List<Position>();
	public List<Position>	NeedlessBlock = new List<Position>();
	public List<Position>	SwitchBlock = new List<Position>();

	public int CoefficientA;
	public int CoefficientB;
	public int CoefficientC;

    public const int GRIDTYPE_NOBLOCK = 0;
    public const int GRIDTYPE_NEEDBLOCK = 1;
    public const int GRIDTYPE_NEEDLESSBLOCK = 2;
    public const int GRIDTYPE_SWITCHBLOCK = 3;

    public int[,] Grid = null;

	// List<T>
	[Serializable]
	public class Serialization<T>
	{
		[SerializeField]
		List<T> target;

		public List<T> ToList() { return target; }

		public Serialization(List<T> target)
		{
			this.target = target;
		}
	}

	// Dictionary<TKey, TValue>
	[Serializable]
	public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField]
		List<TKey> keys;

		[SerializeField]
		List<TValue> values;

		Dictionary<TKey, TValue> target;

		public Dictionary<TKey, TValue> ToDictionary() { return target; }

		public Serialization(Dictionary<TKey, TValue> target)
		{
			this.target = target;
		}

		public void OnBeforeSerialize()
		{
			keys = new List<TKey>(target.Keys);
			values = new List<TValue>(target.Values);
		}

		public void OnAfterDeserialize()
		{
			var count = Math.Min(keys.Count, values.Count);
			target = new Dictionary<TKey, TValue>(count);
			for (var i = 0; i < count; ++i)
			{
				target.Add(keys[i], values[i]);
			}
		}
	}

	public Data() {
	}

	public void AddAnswerRoute( int x, int y ) { 
		AnswerRoute.Add (new Position (x, y));
	}

	public void RemoveAnswerRoute() { 
		AnswerRoute.RemoveAt (AnswerRoute.Count - 1);
	}

	public Position GetAnswerRoute( int index ) {
		return AnswerRoute[index];
	}

	public void Load( string path ) {
        AnswerRoute = new List<Position>();

        var json = Resources.Load(path) as TextAsset;

        Data _data = JsonUtility.FromJson<Data>(json.text);
		AnswerRoute = JsonUtility.FromJson<Serialization<Position>>(_data.AnswerRouteJson).ToList();
		NeedBlock = JsonUtility.FromJson<Serialization<Position>>(_data.NeedBlockJson).ToList();
		NeedlessBlock = JsonUtility.FromJson<Serialization<Position>>(_data.NeedlessBlockJson).ToList();
		SwitchBlock = JsonUtility.FromJson<Serialization<Position>>(_data.SwitchBlockJson).ToList();

		Width = _data.Width;
		Height = _data.Height;

        Grid = new int[Height, Width];

        for (int y = 0; y < Height; y++ )
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[y, x] = GRIDTYPE_NOBLOCK;
            }
        }

        for (int i = 0; i < NeedBlock.Count; i++){
            Grid[NeedBlock[i].y, NeedBlock[i].x] = GRIDTYPE_NEEDBLOCK;
        }

        for (int i = 0; i < NeedlessBlock.Count; i++)
        {
            Grid[NeedlessBlock[i].y, NeedlessBlock[i].x] = GRIDTYPE_NEEDLESSBLOCK;
        }

        for (int i = 0; i < SwitchBlock.Count; i++)
        {
            Grid[SwitchBlock[i].y, SwitchBlock[i].x] = GRIDTYPE_SWITCHBLOCK;
        }

	}

}

public class StageData {

    private static StageData mInstance;

	public Data data;

	const int MAX_WIDTH = 12;
	const int MAX_HEIGHT = 20;

	public static StageData Instance {
		get {
			if( mInstance == null ) mInstance = new StageData();
			return mInstance;
		}
	}

	public StageData() {
	}

	public void Initialize() {
		data = new Data ();
	}

	public void Load( string packname, int stageno ) {
		string path = "StageData/" + packname + "/" + stageno.ToString().PadLeft(6,'0');// + ".txt";

		data.Load ( path );
	}

}
