using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroManager : MonoSingleton<HeroManager>
{
    [SerializeField]
    private TileManager managerTile;
    [SerializeField]
    private GameObject[] objTempTower;
    private RaycastHit mHit;
    private List<BaseTower> mActiveList = new List<BaseTower>(0);
    public bool isDrag;
    public void BuildTower()
    {
        if (!(managerTile.EmptyTileCount > 0))
            return;

        //GameObject temp = GameObject.Instantiate(objTempTower);
        BaseTower temp = ObjectPoolerManager.Instance.SpawnFromPool<BaseTower>("Tower", transform.position, transform);
        temp.SetTile(managerTile.GetEmptyTile());
        mActiveList.Add(temp);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out mHit))
            {
                BaseTower objectHit = mHit.transform.GetComponent<BaseTower>();
                SeleteTower(objectHit.mTowerName);
                isDrag = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isDrag)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out mHit))
            {
                BaseTower objectHit = mHit.transform.GetComponent<BaseTower>();
            }
            ResetSelect();
            isDrag = false;
        }
    }

    public void SeleteTower(string _name)
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            if(mActiveList[i].mTowerName == _name)
                mActiveList[i].mRenderer.materials[0].color = new Color(0, 0, 0, 0.3f);
        }
    }

    public void ResetSelect()
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            mActiveList[i].ResetColor();
        }
    }
}