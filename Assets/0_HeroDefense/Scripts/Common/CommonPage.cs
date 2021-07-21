
public enum Tier
{
    Normal=0,
    Rare,
    Hero,
    Legend,
}

public class Deck
{
    public Tier tierChar;
    public Stat statChar;
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
}