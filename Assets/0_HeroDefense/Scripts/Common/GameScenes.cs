using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ADA_Manager;

[CreateAssetMenu(menuName = "Main/GameScenes", fileName = "GameScenes", order = 1)]
public class GameScenes : ScriptableObject
{
	public List<SceneInfo> listSceneInfo;

	[System.Serializable]
	public class SceneInfo
	{
		//public SceneEnum SceneType;
		public string SceneName;
	}
}
