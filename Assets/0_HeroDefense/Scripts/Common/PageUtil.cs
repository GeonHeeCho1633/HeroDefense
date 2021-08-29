//using ADA_Camera; 99번째줄 체크
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PageUtil
{
	public enum PageType
	{
		Default = 0,
		Effect,
		MainCam,
	}
	/// <summary>
	/// 해당 오브젝트(자식포함)에 존재하는 CanvasObject에 있는 Canvas 컴포넌트를 반환한다.
	/// </summary>
	public static Canvas GetCanvas(GameObject _target, PageType _pageType = PageType.Default)
	{
		Canvas canvas = null;
		if (_pageType == PageType.Default)
		{
			Canvas[] canvasArray = _target.GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < canvasArray.Length; ++i)
			{
				if (canvasArray[i].name == "Canvas")
					canvas = canvasArray[i];
			}
			if (canvas == null)
				canvas = canvasArray.Length > 0 ? canvasArray[0] : null;

			SetCanvas(canvas);
		}
		else if (_pageType == PageType.Effect)
		{
			Canvas[] canvasArray = _target.GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < canvasArray.Length; ++i)
			{
				if (canvasArray[i].name == "CanvasEffect")
					canvas = canvasArray[i];
			}

			SetCanvas(canvas);
		}
		else if(_pageType == PageType.MainCam)
		{
			Canvas[] canvasArray = _target.GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < canvasArray.Length; ++i)
			{
				if (canvasArray[i].name == "Canvas_MainCam")
					canvas = canvasArray[i];
			}

			SetCanvas(canvas, false);
		}
		return canvas;
	}

	/// <summary>
	/// 컨트롤하는 Canvas를 target Object에 있는 Canvas로 설정해준다.
	/// </summary>
	public static void SetCanvas(GameObject _target)
	{
		Canvas canvas = _target.GetComponent<Canvas>();
		SetCanvas(canvas);
	}


#if UNITY_EDITOR
	/// <summary>
	/// 에디터 함수
	/// </summary>
	/// <returns></returns>
	public static UnityEditor.EditorWindow GetMainGameView()
	{
#if UNITY_2019_4_OR_NEWER
		System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
		System.Type type = assembly.GetType("UnityEditor.GameView");
		UnityEditor.EditorWindow gameview = UnityEditor.EditorWindow.GetWindow(type);
		return gameview;
#else
			System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
			System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			System.Object Res = GetMainGameView.Invoke(null, null);
			return (UnityEditor.EditorWindow)Res;
#endif
	}
#endif

	/// <summary>
	/// 파라미터 Canvas를 셋팅해준다.
	/// </summary>
	public static void SetCanvas(Canvas _canvas, bool _isUICam = true)
	{
		if (_canvas != null)
		{
			_canvas.renderMode = RenderMode.ScreenSpaceCamera;
			//_canvas.worldCamera = _isUICam == true ? CameraManager.Instance.uiCameraDefault : CameraManager.Instance.mainCamera;
			_canvas.planeDistance = 1;
		}
	}

	/// <summary>
	/// 파라미터 Object의 CanvasScaler 컴포넌트를 가져오고 셋팅한다.
	/// </summary>
	public static CanvasScaler GetCanvasScaler(GameObject _target)
	{
		Canvas canvas = GetCanvas(_target);

		return GetCanvasScaler(canvas);
	}

	public static CanvasScaler GetCanvasScaler(Canvas _canvas)
	{
		CanvasScaler canvasScale = null;
		if (_canvas != null && canvasScale == null)
			canvasScale = _canvas.GetComponent<CanvasScaler>();

		if (canvasScale != null)
		{
			canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScale.referenceResolution = new Vector2(720f, 1280f);
			canvasScale.matchWidthOrHeight = 0f;
		}
		SetCanasMatchWidthOrHeight(_canvas);

		return canvasScale;
	}

	/// <summary>
	/// 해당 Canvas의  MatchWidthOrHeight 값 조정
	/// </summary>
	public static void SetCanasMatchWidthOrHeight(Canvas _canvas)
	{
		if (_canvas == null)
			return;

		CanvasScaler cs = _canvas.GetComponent<CanvasScaler>();
		if (cs == null)
			return;

		float ratio = GetScreenRatio();
		float matchWidthOrHeight = ratio >= 0.74f ? 1 : 0;
		cs.matchWidthOrHeight = matchWidthOrHeight;
	}

	/// <summary>
	/// Screen Size Ratio 반환.
	/// </summary>
	public static float GetScreenRatio()
	{
		float width = (float)Screen.width;
		float height = (float)Screen.height;
		if (Screen.width > Screen.height)
		{
			width = Screen.height;
			height = Screen.width;
		}

		return width / height;
	}

	public static bool ChangeGameViewSize(int _width, int _height)
	{
#if UNITY_ANDROID
#if UNITY_EDITOR
		return SetSize(FindSize(UnityEditor.GameViewSizeGroupType.Android, _width, _height));
#endif
#elif UNITY_IOS
			return SetSize(FindSize(UnityEditor.GameViewSizeGroupType.iOS, _width, _height));
#endif
		return true;
	}

	public static bool SetSize(int _index)
	{
		if (_index < 0) return false;
		try
		{
#if UNITY_EDITOR
			System.Type gvWndType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
			System.Reflection.PropertyInfo selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
				  System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
			UnityEditor.EditorWindow gvWnd = UnityEditor.EditorWindow.GetWindow(gvWndType);
			selectedSizeIndexProp.SetValue(gvWnd, _index, null);
#endif
		}
		catch (System.Exception e)
		{
			Debug.Log($"GameView SetSize - {e.Message}");
		}
		return true;
	}
#if UNITY_EDITOR
	public static int FindSize(UnityEditor.GameViewSizeGroupType _sizeGroupType, int _width, int _height)
	{
		try
		{
			object group = GetGroup(_sizeGroupType);
			System.Type groupType = group.GetType();
			System.Reflection.MethodInfo getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
			System.Reflection.MethodInfo getCustomCount = groupType.GetMethod("GetCustomCount");
			int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
			System.Reflection.MethodInfo getGameViewSize = groupType.GetMethod("GetGameViewSize");
			System.Type gvsType = getGameViewSize.ReturnType;
			System.Reflection.PropertyInfo widthProp = gvsType.GetProperty("width");
			System.Reflection.PropertyInfo heightProp = gvsType.GetProperty("height");
			object[] indexValue = new object[1];
			for (int i = 0; i < sizesCount; i++)
			{
				indexValue[0] = i;
				var size = getGameViewSize.Invoke(group, indexValue);
				int sizeWidth = (int)widthProp.GetValue(size, null);
				int sizeHeight = (int)heightProp.GetValue(size, null);
				if (sizeWidth == _width && sizeHeight == _height)
					return i;
			}
		}
		catch (System.Exception e)
		{
			Debug.Log($"GameView FindSize - {e.Message}");
		}
		return -1;
	}
#endif

#if UNITY_EDITOR
	static object GetGroup(UnityEditor.GameViewSizeGroupType _type)
	{
		System.Type sizesType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSizes");
		System.Type singleType = typeof(UnityEditor.ScriptableSingleton<>).MakeGenericType(sizesType);
		System.Reflection.PropertyInfo instanceProp = singleType.GetProperty("instance");
		object gameViewSizesInstance = instanceProp.GetValue(null, null);
		System.Reflection.MethodInfo getGroup = sizesType.GetMethod("GetGroup");
		return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)_type });
	}
#endif

	public static void SetMainGameViewSize()
	{
#if UNITY_EDITOR
		UnityEditor.EditorWindow gameView = GetMainGameView();
		Rect pos = gameView.position;
		float height = Screen.width > Screen.height ? Screen.width : Screen.height;
		float targetScale = (pos.height - 17) / height;

		System.Type gvWndType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
		UnityEditor.EditorWindow gvWnd = UnityEditor.EditorWindow.GetWindow(gvWndType);
		System.Reflection.FieldInfo areaField = gvWndType.GetField("m_ZoomArea",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		object areaObj = areaField.GetValue(gvWnd);
		System.Reflection.FieldInfo scaleField = areaObj.GetType().GetField("m_Scale",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		scaleField.SetValue(areaObj, new Vector2(targetScale, targetScale));
#endif
	}

	public static string GetCurrentGameViewSizeTitle()
	{
		string title = "";
#if UNITY_EDITOR
		UnityEditor.EditorWindow gameView = GetMainGameView();
		System.Reflection.Assembly asm = typeof(UnityEditor.Editor).Assembly;
		System.Type gvWndType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameView");
		System.Type sizesType = asm.GetType("UnityEditor.GameViewSizes");
		System.Type singleType = typeof(UnityEditor.ScriptableSingleton<>).MakeGenericType(sizesType);
		System.Reflection.PropertyInfo instanceProp = singleType.GetProperty("instance");
		System.Reflection.MethodInfo getGroup = sizesType.GetMethod("GetGroup");
		object instance = instanceProp.GetValue(null, null);

		object group = getGroup.Invoke(instance, new object[] {(int) UnityEditor.GameViewSizeGroupType.Android});
		System.Type groupType = group.GetType();

		object[] indexValue = new object[1];
		System.Reflection.MethodInfo selectedSizeIndex = gvWndType.GetMethod("get_selectedSizeIndex",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
		if (selectedSizeIndex != null)
			indexValue[0] = (int)selectedSizeIndex.Invoke(gameView, null);

		System.Reflection.MethodInfo GetGameViewSize = groupType.GetMethod("GetGameViewSize");
		if (GetGameViewSize != null)
		{
			object viewSize = GetGameViewSize.Invoke(group, indexValue);
			System.Reflection.MethodInfo get_baseText = viewSize.GetType().GetMethod("get_baseText");
			if (get_baseText != null)
				title = (string)get_baseText.Invoke(viewSize, null);
		}
#endif
		return title;
	}

	/// <summary>
	/// 화면의 UI ScreenScale 값 반환.
	/// </summary>
	public static float GetScreenScale()
	{
		Vector2 defaultSize = new Vector2(720f, 1280f);
		Vector2 screenSize = new Vector2(NEXT.ADA.GameStarter.screenSetting.width, NEXT.ADA.GameStarter.screenSetting.height);
		if (screenSize.x > screenSize.y)
		{
			defaultSize.x = 720f;
			defaultSize.y = 1280f;
		}
		float scale = Mathf.Max(screenSize.x / defaultSize.x, screenSize.y / defaultSize.y);
		return Mathf.Clamp(scale, 1, 6);
	}
}
