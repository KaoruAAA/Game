using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
	private string weaponFileDataBase = "abc";

	public  readonly JSONObject weaponDB;

	public DataBase()
	{
		TextAsset text = (TextAsset)Resources.Load("abc");
		weaponDB = new JSONObject(text.text);

	}
    
}
