using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public GameObject obj_Panel;

    public void OnClick_Panel() 
    {
        if (!obj_Panel.activeSelf)
            obj_Panel.SetActive(true);
        else
            obj_Panel.SetActive(false);
    }

    public void OnClick_Close() 
    {
        gameObject.SetActive(false);
    }
}
