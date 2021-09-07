using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainPage : MonoBehaviour
{
    public GameObject obj_Option;//옵션
    public GameObject obj_PvpModeStart;//pvp모드
    public GameObject obj_CollaborationModeStart;//협동모드
    public GameObject obj_SpecialModeStart;//스페셜모드
    public GameObject obj_BattlePass;//배틀패스
    public GameObject obj_UserProfile;//유저프로필
    public GameObject ojb_DailyMission;//일일미션
    public GameObject obj_CardBox;//카드박스
    public GameObject obj_Fame;//명성
    public GameObject obj_Store;//상점
    public GameObject obj_Configuration;//구성
    public GameObject obj_Main;//메인
    public GameObject obj_Spetical;//스페셜


	#region 컨텐츠 버튼이벤트
	//유저프로필
	public void OnClick_UserProfile()
    {
        if (!obj_UserProfile.activeSelf)
            obj_UserProfile.SetActive(true);
        else
            obj_UserProfile.SetActive(false);
    }


    //옵션
    public void OnClick_Option() 
    {
        if (!obj_Option.activeSelf)
            obj_Option.SetActive(true);
        else
            obj_Option.SetActive(false);
    }

    //카드상자
    public void OnClick_obj_CardBox()
    {
        if (!obj_CardBox.activeSelf)
            obj_CardBox.SetActive(true);
        else
            obj_CardBox.SetActive(false);
    }

    //일일 미션
    public void OnClick_DailyMission() 
    {
        if (!ojb_DailyMission.activeSelf)
            ojb_DailyMission.SetActive(true);
        else
            ojb_DailyMission.SetActive(false);
    }


    //명성
    public void OnClick_Fame() 
    {
        if (!obj_Fame.activeSelf)
            obj_Fame.SetActive(true);
        else
            obj_Fame.SetActive(false);
    }

    //배틀패스
    public void OnClick_BattlePass()
    {
        if (!obj_BattlePass.activeSelf)
            obj_BattlePass.SetActive(true);
        else
            obj_BattlePass.SetActive(false);
    }
	#endregion

	#region 바텀 버튼이벤트
	public void OnClick_Store()
    {
        if (!obj_Store.activeSelf)
            obj_Store.SetActive(true);
        else
            obj_Store.SetActive(false);
    }

    public void OnClick_Configuration()
    {
        if (!obj_Configuration.activeSelf)
            obj_Configuration.SetActive(true);
        else
            obj_Configuration.SetActive(false);
    }

    public void OnClick_Main()
    {
        if (!obj_Main.activeSelf)
            obj_Main.SetActive(true);
        else
            obj_Main.SetActive(false);
    }

    public void OnClick_Spetical()
    {
        if (!obj_Spetical.activeSelf)
            obj_Spetical.SetActive(true);
        else
            obj_Spetical.SetActive(false);
    }

	#endregion

    #region 게임모드 버튼이벤트
    public void OnClick_PvpModeStart() 
    {
        if (!obj_PvpModeStart.activeSelf)
            obj_PvpModeStart.SetActive(true);
        else
            obj_PvpModeStart.SetActive(false);
    }

    public void OnClick_CollaborationModeStart() 
    {
        if (!obj_CollaborationModeStart.activeSelf)
            obj_CollaborationModeStart.SetActive(true);
        else
            obj_CollaborationModeStart.SetActive(false);
    }

    public void OnClick_SpecialModeStart() 
    {
        if (!obj_SpecialModeStart.activeSelf)
            obj_SpecialModeStart.SetActive(true);
        else
            obj_SpecialModeStart.SetActive(false);
    }
    #endregion


    public void OnClick_Close()
    {
        gameObject.SetActive(false);
    }
}
