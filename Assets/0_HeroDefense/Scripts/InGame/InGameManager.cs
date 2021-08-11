using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum InGameState
{ 
    None,
    Play,
    Win_Hero,
    Win_Enemy,
}

public class InGameManager : MonoBehaviour
{
    [SerializeField]
    private Text textState;
    [SerializeField]
    private GameObject[] objUI;
    private int GameMode;
    private EnemyManager enemyManager;
    private HeroManager heroManager;
    private InGameState stateGame;
    private Deck decks;
    private bool isStart;

	private void Start()
	{
        Initialized();
        objUI[1].SetActive(false);
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && !isStart)
        {
            OnClick_Start();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            objUI[0].SetActive(!objUI[0].activeSelf);
            objUI[1].SetActive(!objUI[1].activeSelf);
        }

        CheckInGameState();
        textState.text = stateGame.ToString();
    }

	public void Initialized()
    {
        decks = new Deck();
        decks.key = "Camp";
        decks.statChar.HP = 500f;
        decks.tierChar = Tier.Normal;

        GameMode = 0;
        enemyManager = EnemyManager.Instance;
        heroManager = HeroManager.Instance;
    }

    public void OnClick_Start()
    {
        // TODO_DEK : 나중에 인게임 데이터 확립시 바뀜
        Deck[] monTemp = new Deck[5];
        for (int i = 0; i < monTemp.Length; i++)
        {
            monTemp[i].key = $"Monster_{i + 1}";
            monTemp[i].tierChar = (Tier)Random.Range(0, (int)(Tier.Rare + 1));
            monTemp[i].statChar.RandStat();
        }

        Deck[] heroTemp = new Deck[5];
        for (int i = 0; i < heroTemp.Length; i++)
        {
            heroTemp[i].key = $"Hero_{i + 1}";
            heroTemp[i].tierChar = (Tier)Random.Range(0, (int)(Tier.Rare + 1));
            heroTemp[i].statChar.RandStat();
        }
        enemyManager.StartGame(decks, monTemp.ToList(), GameMode);
        heroManager.StartGame(decks, heroTemp.ToList(), GameMode);
        isStart = false;
    }

    public bool CheckInGameState()
    {
        bool _isGameState;
        if (enemyManager == null || heroManager == null)
        {
            stateGame = InGameState.None;
            _isGameState = false;
        }

        if (enemyManager.IsGameEnd)
        {
            stateGame = InGameState.Win_Hero;
            _isGameState = true;
        }
        else if(heroManager.IsGameEnd)
        {
            stateGame = InGameState.Win_Enemy;
            _isGameState = true;
        }
        else
        {
            stateGame = InGameState.Play;
            _isGameState = false;
        }

        return _isGameState;
    }
}
