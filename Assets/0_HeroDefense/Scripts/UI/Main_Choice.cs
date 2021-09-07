using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Choice : MonoBehaviour
{
    public List<GameObject> obj_PresetBox;//5개의 프리셋이 담겨있는 5개의 프리셋박스
    public List<GameObject> obj_PresetList;//5개의 프리셋
    public List<GameObject> obj_HeroList;//5개의 프리셋

    //선택된 프리셋
    public void OnClick_Preset(int _index) 
    {
        switch (_index) 
        {
            case 0:
                obj_PresetList[0].SetActive(true);
                Debug.Log("0번 프리셋 활성화");
                break;
            case 1:
                obj_PresetList[1].SetActive(true);
                Debug.Log("1번 프리셋 활성화");
                break;            
            case 2:
                obj_PresetList[2].SetActive(true);
                Debug.Log("2번 프리셋 활성화");
                break;            
            case 3:
                obj_PresetList[3].SetActive(true);
                Debug.Log("3번 프리셋 활성화");
                break;            
            case 4:
                obj_PresetList[4].SetActive(true);
                Debug.Log("4번 프리셋 활성화");
                break;
            default:
                break;
        }
    }

    //선택된 초인 상세설명
    public void OnClick_Hero(int _index) 
    {
        switch (_index)
        {
            case 0:
                Debug.Log("0번 초인 설명");
                break;
            case 1:
                Debug.Log("1번 초인 설명");
                break;
            case 2:
                Debug.Log("2번 초인 설명");
                break;
            case 3:
                Debug.Log("3번 초인 설명");
                break;
            case 4:
                Debug.Log("4번 초인 설명");
                break;
            default:
                break;
        }
    }
}
