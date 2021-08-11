using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroManager : MonoSingleton<HeroManager>
{
    [SerializeField]
    private BaseObject objCamp;
    [SerializeField]
    private Deck[] playDeck;
    [SerializeField]
    private TileManager managerTile;
    private List<BaseHero> mActiveList = new List<BaseHero>(0);

    private bool isGameEnd;
    public bool IsGameEnd
    {
        get { return isGameEnd; }
        set { isGameEnd = value; }
    }

    public void StartGame(Deck _Camp, List<Deck> _playDeck, int _Mode)
    {
        objCamp.InitObject();
        playDeck = _playDeck.ToArray();
        objCamp.SetObject(_Camp);
        isGameEnd = false;
        StartCoroutine(CheckCamp());
    }
    private IEnumerator CheckCamp()
    {
        while (!isGameEnd)
        {
            if (objCamp.GetObjStat().HP <= 0)
                isGameEnd = true;
            yield return null;
        }
        StopAllCoroutines();
    }

    public void BuildTower(int i)
    {
        if (!(managerTile.EmptyTileCount > 0) || playDeck == null)
            return;

        mActiveList.Add(DrawHero(i));
    }

    private BaseHero DrawHero(int i=-1)
    {
        string _towerType;
        if (i<=-1)
            _towerType = playDeck[UnityEngine.Random.Range(0, playDeck.Length)].key;
        else
            _towerType = playDeck[i].key;

        BaseHero temp = ObjectPoolerManager.Instance.SpawnFromPool<BaseHero>(_towerType, transform.position, transform);
        temp.SetObject(playDeck[i]);
        temp.SetTile(managerTile.GetEmptyTile());
        temp.strObjName = _towerType;
        return temp;
    }

    public void SeleteTower(string _name)
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            if (mActiveList[i].strObjName != _name)
                mActiveList[i].ChangColor();
        }
    }

    public void ResetSelect()
    {
        for (int i = 0; i < mActiveList.Count; i++)
        {
            mActiveList[i].ResetColor();
        }
    }

    public void MergeTower(BaseHero _tower)
    {
        managerTile.ReturnTile(_tower.myTile);
        _tower.Merge();
    }
    public BaseObject GetBaseCamp()
    {
        return objCamp;
    }
}