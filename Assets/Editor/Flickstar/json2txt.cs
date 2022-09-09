using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

public class json2txt {

	[MenuItem("FlickstarTools/json2txt")]
	public static void JsonToTxt() {
		string path = "Assets/Resources/StageData";


//		IEnumerable<string> files = System.IO.Directory.GetFileSystemEntries(path, "*.*");
		string[] files = System.IO.Directory.GetFiles( path, "*.json", SearchOption.AllDirectories);

		for (int i=0; i<files.Length; i++) {
			Debug.Log (files[i]);
			if ( files[i].Contains(".json") ){
				string rename = files[i].Substring(0,files[i].Length-5);
				File.Move(files[i], rename + ".txt");
				Debug.Log ( files[i] + " -> " + rename + ".txt" );
			}
		}
	}
}
