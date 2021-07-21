
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
    public float AttackPoint;       //���ݷ�
    public float DefensePoint;      //����
    public float CreticalChance;    //ġ��Ÿ Ȯ��
    public float CreticalPoint;     //ġ��Ÿ ������
    public float EvasionChance;     // ȸ����
    public float MoveSpeed;         //�̵��ӵ�
    public float AttackSpeed;        //���ݼӵ�
    public float HP;                //ü��
    public float CreatCost;         //���� �ڽ�Ʈ
    public float SkillCost;         //��ų �ڽ�Ʈ
}