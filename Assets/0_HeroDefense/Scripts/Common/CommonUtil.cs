using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LanguageCode
{
	us, // 영어
	kr, // 한국어
	cn, // 중국어
		//JA, // 일본어
		//FR, // 불어
		//DE, // 독일어
		//RU, // 러시아어
		//ES, // 스페인어
};

public class CommonUtil : MonoBehaviour
{
	public delegate void OnEvent();
	public delegate void OnEventBool(bool value);
	public delegate void OnEventInt(int value);
	public delegate void OnEventFloat(float value);
	public delegate void OnEventString(string value);
	public delegate void OnEventVector2(Vector2 value);
	public delegate void OnEventVector3(Vector3 value);
	public delegate void OnEventTexture(Texture2D value);

	private static string tablePath = null;

	/// <summary>
	/// Scene에 존재하는 Ojbects들의 컴포넌트 리스트를 반환.
	/// </summary>
	public static List<T> FindObjectsOfTypeAll<T>()
	{
		List<T> results = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var s = SceneManager.GetSceneAt(i);
			if (s.isLoaded)
			{
				var allGameObjects = s.GetRootGameObjects();
				for (int j = 0; j < allGameObjects.Length; j++)
				{
					var go = allGameObjects[j];
					results.AddRange(go.GetComponentsInChildren<T>(true));
				}
			}
		}
		return results;
	}

	/// <summary>
	/// Paramater Obj의 자식 Ojbects들의 컴포넌트 리스트를 반환.
	/// </summary>
	public static List<T> FindObjectsOfTypeAll<T>(GameObject obj)
	{
		List<T> results = new List<T>();
		results.AddRange(obj.GetComponentsInChildren<T>(true));
		return results;
	}

	/// <summary>
	/// DeviceLangauge의 언어코드 반환 ( 설정되어있는 DeviceLangauge 반환 )
	/// </summary>
	public static string GetDeviceLanguage()
	{
		return Application.systemLanguage.ToString();
	}

	/// <summary>
	/// App에서 설정된 Language 코드 반환.
	/// </summary>
	public static string GetLanguage()
	{
		return UITextManager.Instance.AppLanguageCode.ToString();
	}

	/// <summary>
	/// 데이터 테이블 접근용 기본 패스설정 및 설정된 패스 반환.
	/// </summary>
	public static string GetTablePath()
	{
		if (tablePath == null)
		{
			if (RuntimePlatform.Android == Application.platform)
			{
				// TODO : HY - prototype 으로 인해 수정
				//tablePath = Application.persistentDataPath;
				tablePath = Path.Combine(Application.streamingAssetsPath, "LocalTable");
			}
			else if (RuntimePlatform.IPhonePlayer == Application.platform)
			{
				// TODO : HY - prototype 으로 인해 수정
				//tablePath = Application.persistentDataPath;
				tablePath = Path.Combine(Application.streamingAssetsPath, "LocalTable");
			}
			else
			{
				tablePath = Path.GetDirectoryName(Application.dataPath) + "/localTable/";
			}
		}
		return tablePath;
	}

	/// <summary>
	/// SafeArea 영역 설정 및 초기화 후 설정된 Rect값 반환.
	/// </summary>
	public static Rect GetSafeArea()
	{
		Rect safeArea = Screen.safeArea;

#if UNITY_EDITOR
		var gameViewSizeTitle = PageUtil.GetCurrentGameViewSizeTitle();
		if (string.IsNullOrEmpty(gameViewSizeTitle) == false)
		{
			var scale = PageUtil.GetScreenScale();
			scale = (int)scale;
			var lower = gameViewSizeTitle.ToLower();
			if (lower.Contains("iphone") == true && lower.Contains("x") == true)
			{
				if (Screen.width < Screen.height)
				{
					safeArea.y = 34 * scale;
					safeArea.height = safeArea.height - (34 * scale) - (44 * scale);
				}
				else
				{
					safeArea.x = 34 * scale;
					safeArea.width = safeArea.width - (34 * scale) - (44 * scale);
				}
			}
			else if (lower.Contains("\"") == true)
			{
				if (Screen.width < Screen.height)
				{
					safeArea.y = 34 * scale;
					safeArea.height = safeArea.height - (34 * scale);
				}
				else
				{
					safeArea.x = 34 * scale;
					safeArea.width = safeArea.width - (34 * scale);
				}
			}
		}
#endif
		return safeArea;
	}

	private LanguageCode mAppLanguageCode;

	public LanguageCode AppLanguageCode
	{
		get { return mAppLanguageCode; }
		set { mAppLanguageCode = value; }
	}

	/// <summary>
	/// Application LangageCode 설정 ( 로컬 환경 셋팅 )
	/// </summary>
	public void SettingApplicationLanguage()
	{
		if (PlayerPrefs.HasKey("LanguageCode"))
		{
			AppLanguageCode = (LanguageCode)PlayerPrefs.GetInt("LanguageCode");
		}
		else
		{
			switch (Application.systemLanguage)
			{
				case SystemLanguage.Chinese:
				case SystemLanguage.ChineseSimplified:
				case SystemLanguage.ChineseTraditional:
					AppLanguageCode = LanguageCode.cn;
					break;
				case SystemLanguage.Korean:
					AppLanguageCode = LanguageCode.kr;
					break;
				case SystemLanguage.English:
				default:
					AppLanguageCode = LanguageCode.us;
					break;
			}
			PlayerPrefs.SetInt("LanguageCode", (int)AppLanguageCode);
		}
	}

	#region GameObject
	public static T GetComponent<T>(GameObject _object) where T : Component
	{
		T component = _object.GetComponent<T>();
		if (component != null)
			return component;

		return _object.AddComponent<T>();
	}
	#endregion

	public static class EnumUtil<T>
	{
		public static T Parse(string s)
		{
			return (T)Enum.Parse(typeof(T), s);
		}
	}


	#region RectTransform To Screen Point
	public static bool IsScreenPointRectTransformContains(Camera _camera, RectTransform _rectTransform, Vector2 _screenPoint)
	{
		return RectTransformToScreenPoint(_camera, _rectTransform).Contains(_screenPoint);
	}
	public static Rect RectTransformToScreenPoint(Camera _camera, RectTransform _rectTransform)
	{
		float xMin = float.MaxValue;
		float xMax = float.MinValue;
		float yMin = float.MaxValue;
		float yMax = float.MinValue;

		Vector3[] corners = new Vector3[4];
		_rectTransform.GetWorldCorners(corners);
		foreach (Vector3 corner in corners)
		{
			Vector2 point = RectTransformUtility.WorldToScreenPoint(_camera, corner);
			xMin = Mathf.Min(xMin, point.x);
			xMax = Mathf.Max(xMax, point.x);
			yMin = Mathf.Min(yMin, point.y);
			yMax = Mathf.Max(yMax, point.y);
		}
		return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
	}
	#endregion


	#region Vector
	public static float Angle(Vector2 _pos1, Vector2 _pos2)
	{
		Vector2 from = _pos2 - _pos1;
		Vector2 to = new Vector2(1, 0);

		float result = Vector2.Angle( from, to );
		Vector3 cross = Vector3.Cross( from, to );

		if (cross.z > 0)
		{
			result = 360f - result;
		}

		return result;
	}
	#endregion


	#region File Size Convert
	public static string GetFileSizeConvert(long _size, int _sizeRange = 0)
	{
		switch (_sizeRange)
		{
			case 1:
				return GetSizeKB(_size).ToString("0.##");

			case 2:
				return GetSizeMB(_size).ToString("0.##");

			default:
				return GetSizeByte(_size).ToString("0.##");
		}
	}
	private static double GetSizeByte(long _size)
	{
		return _size;
	}
	private static double GetSizeKB(long _size)
	{
		return _size / 1024f;
	}
	private static double GetSizeMB(long _size)
	{
		return _size / 1024f / 1024f;
	}
	#endregion


	#region Change Layers
	public static void ChangeLayers(GameObject _go, string _layer)
	{
		ChangeLayers(_go, LayerMask.NameToLayer(_layer));
	}
	public static void ChangeLayers(GameObject _go, int _layer)
	{
		_go.layer = _layer;
		foreach (Transform child in _go.transform)
		{
			ChangeLayers(child.gameObject, _layer);
		}
	}
	public static void ChangeLayersTag(GameObject _go, string _layer, string _tag)
	{
		ChangeLayersTag(_go, LayerMask.NameToLayer(_layer), _tag);
	}
	public static void ChangeLayersTag(GameObject _go, int _layer, string _tag)
	{
		_go.tag = _tag;
		_go.layer = _layer;
		foreach (Transform child in _go.transform)
		{
			ChangeLayersTag(child.gameObject, _layer, _tag);
		}
	}
	#endregion


	#region Clear Console
	public static void ConsoleClear()
	{
#if UNITY_EDITOR
		// From 2017.1 it is no longer namespaced in `UnityEditorInternal`.
		var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		var type = assembly.GetType("UnityEditor.LogEntries");
		var method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
#endif
	}
	#endregion
}
