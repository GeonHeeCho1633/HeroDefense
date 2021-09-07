using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NEXT.ADA
{
	//?????? ?????? ???? ?????? ??????
	public class ScreenSize
	{
		public int width  = 0;
		public int height = 0;

		public ScreenSize(int _width, int _height)
		{
			width = _width;
			height = _height;
		}
	}

	public class GameStarter : MonoSingleton<GameStarter>
	{
		public static ScreenSize screenOrigin = new ScreenSize(Screen.width, Screen.height);	//?????? ???? ??????
		public static ScreenSize screenSetting = new ScreenSize(720, 1280);                     //?????? ???? ??????720, 1280?? ??????

		private bool isPlayingGameStart;
		//private ADA_Manager.SceneManager sceneManager;
		//private ADA_Manager.PageManager pagemManager;
		private string strSetupSceneName = "0__INTRO";

		/// <summary>
		/// enable???? ?????? ???? ?????? ?? ????
		/// ?????? ?? ???? ?????? ???? ????
		/// ?????????? ?????? ?????? ????
		/// </summary>

		private void OnEnable()
		{
			Application.targetFrameRate = 60;
			screenOrigin = new ScreenSize(Screen.width, Screen.height);
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneLoadEvent;
			//sceneManager = ADA_Manager.SceneManager.Instance;
			//pagemManager = ADA_Manager.PageManager.Instance;
			GameStart();
		}

		/// <summary>
		/// ?????? ???? ???? ???? ???? ???????? 1080 ???????? ????
		/// ?????? X?? ???? ?????? ???????? ????
		/// </summary>
		public void GameStart()
		{
			SetGraphic();
			//UIUtil.ChangeOrientation_Portrait();
			isPlayingGameStart = true;

#if UNITY_EDITOR
			Caching.ClearCache();
			//PageUtil.SetMainGameViewSize();
			Application.runInBackground = true;
#endif

#if UNITY_IOS
            if (UnityEngine.iOS.Device.generation.ToString().Contains("iPhoneX"))
                //StatusBarManager.Show(true);
#endif
			//sceneManager.Init();
			//pagemManager.Init();
			StartCoroutine(nameof(R_LoadNextScene));
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		/// <summary>
		/// sprlash???? ????
		/// </summary>
		IEnumerator R_LoadNextScene()
		{
			yield return new WaitForSeconds(1f);
			//sceneManager.Load(ADA_Manager.SceneEnum.Splash);
		}

		/// <summary>
		/// ???? ?????? ????
		/// </summary>
		private void SceneLoadEvent(UnityEngine.SceneManagement.Scene _scene,
			UnityEngine.SceneManagement.LoadSceneMode _mode)
		{
			if (_scene.name == strSetupSceneName)
			{
				GameStart();
			}
		}

		/// <summary>
		/// ?????? ???? ???? ????(???????? 1080 ???????? ????)
		/// </summary>
		public void SetGraphic()
		{
			float fRatio =  Screen.height/((float)Screen.width);
			int index;// = LocalDataController.GetLocalDataValue_Int("SETTING_GRAPHIC", 1);
#if !KR_DEV
			index = 1;
#endif

			switch (index)
			{
				case 0:
					if (Screen.width > 720)
						screenSetting.width = 720;
					else
						screenSetting.width = screenOrigin.width;
					break;
				case 1:
					if (Screen.width > 1080)
						screenSetting.width = 1080;
					else
						screenSetting.width = screenOrigin.width;
					break;
				case 2:
					screenSetting.width = screenOrigin.width;
					break;
				default:
					screenSetting.width = 1080;
					break;
			}
			screenSetting.height = Mathf.RoundToInt(fRatio * screenSetting.width);
			Screen.SetResolution(screenSetting.width, screenSetting.height, true);

			QualitySettings.SetQualityLevel(index);
		}
	}
}
