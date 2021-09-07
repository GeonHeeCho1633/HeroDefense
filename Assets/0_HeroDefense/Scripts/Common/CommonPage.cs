
public enum ObjectType
{
    Hero,
    Monster,
}

public enum Tier
{
    Normal=0,
    Rare,
    Hero,
    Legend,
}

[System.Serializable]
public struct Deck
{
    public string key;
    public Tier tierChar;
    public Stat statChar;

    public void Set(Deck _Deck)
    {
        tierChar = _Deck.tierChar;
        statChar.SetStat(_Deck.statChar);
    }
    public void Set(Tier _tier,Stat _param)
    {
        tierChar = _tier;
        statChar.SetStat(_param);
    }
}

[System.Serializable]
public struct Stat
{
    public float AttackPoint;       //공격력
    public float DefensePoint;      //방어력
    public float CreticalChance;    //치명타 확률
    public float CreticalPoint;     //치명타 데미지
    public float EvasionChance;     // 회피율
    public float MoveSpeed;         //이동속도
    public float AttackSpeed;        //공격속도
    public float HP;                //체력
    public float CreatCost;         //생산 코스트
    public float SkillCost;         //스킬 코스트

    public void SetStat(Stat _param)
    {
        AttackPoint = _param.AttackPoint;
        DefensePoint = _param.DefensePoint;
        CreticalChance = _param.CreticalChance;
        CreticalPoint = _param.CreticalPoint;
        EvasionChance = _param.EvasionChance;
        MoveSpeed = _param.MoveSpeed;
        AttackSpeed = _param.AttackSpeed;
        HP = _param.HP;
        CreatCost = _param.CreatCost;
        SkillCost = _param.SkillCost;
    }

    public void RandStat()
    {
        AttackPoint = UnityEngine.Random.Range(1, 10);
        DefensePoint = UnityEngine.Random.Range(1, 10);
        CreticalChance = UnityEngine.Random.Range(1, 10);
        CreticalPoint = UnityEngine.Random.Range(1, 10);
        EvasionChance = UnityEngine.Random.Range(1, 10);
        MoveSpeed = UnityEngine.Random.Range(1, 10);
        AttackSpeed = UnityEngine.Random.Range(1, 10);
        HP = UnityEngine.Random.Range(1, 10);
        CreatCost = UnityEngine.Random.Range(1, 10);
        SkillCost = UnityEngine.Random.Range(1, 10);
    }
}