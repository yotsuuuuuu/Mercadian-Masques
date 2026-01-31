using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CardLevelData
{
    public maskType maskType;
    public List<CardMove> moveList;
}
[System.Serializable]
public class ChunkLevelData
{
    public ChunkType type;
    public Vector3Int position;
}

public class GameManager : MonoBehaviour
{
    private StateMachine stateMachine;
    public bool isProcessingCard;
    [SerializeField] private GameObject boardManager;
    [SerializeField] private GameObject handManager;
    [SerializeField] public LevelCardDataResource CardsData;
    [SerializeField] public LevelDataResource ChunkData;
    [SerializeField] private Player player;
    private List<CardData> ListofCardData;
    
    int boardSizeX = 9; // default values to for the board
    int boardSizeZ = 9;
    int boardSizeY = 2; 

    public struct CardData
    {
        public maskType maskType;
        public Queue<CardMove> moveList;
    }

    private BoardManager board;
    private HandManager hand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        board = boardManager.GetComponent<BoardManager>();
        //hand = handManager.GetComponent<HandManager>();
        ListofCardData = PopulateCardData(CardsData.ListOfCards);
        //board.Initilize(ArrayofChunks(ChunkData.chunks), boardSizeX, boardSizeY, boardSizeZ);

        isProcessingCard = false;

        stateMachine = new StateMachine();
        IdleState idel = new IdleState(this);
        ProcessingState processing = new ProcessingState(this);
        CheckPLayerState playerState = new CheckPLayerState(this);

        stateMachine.SetState(idel);


        stateMachine.AddTransitions(idel, new TransitonState(() => CardPlayed(), processing));
        //stateMachine.AddTransitions(idel, new TransitonState(() => !CardPlayed(), idel));
        stateMachine.AddTransitions(processing, new TransitonState(() => ProcessingCard(), playerState));
        //stateMachine.AddTransitions(processing, new TransitonState(() => !ProcessingCard(), processing));
        stateMachine.AddTransitions(playerState, new TransitonState(() => PlayerCheck(), idel));
        stateMachine.AddTransitions(playerState, new TransitonState(() => !PlayerCheck(), processing));
    }

    private int[,,] ArrayofChunks(List<ChunkLevelData> chunks)
    {
        int[,,] array = new int[boardSizeX, boardSizeY, boardSizeZ];
        for(int i = 0; i < chunks.Count; i++)
        {
            Vector3Int pos = chunks[i].position;
            array[pos.x, pos.y, pos.z] = (int)chunks[i].type;
        }

        return array;
    }

    private List<CardData> PopulateCardData(List<CardLevelData> listOfCards)
    {
        List<CardData> data = new List<CardData>();
        for(int i  = 0; i < listOfCards.Count; i++)
        {
            CardData cardData = new CardData();
            cardData.maskType = listOfCards[i].maskType;
            cardData.moveList = new Queue<CardMove>(listOfCards[i].moveList);
            data.Add(cardData);
        }
        return data;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
    // TODO : IMPLEMENT PLAYER CHECK LOGIC
    // IF  PLAYER 
    bool PlayerCheck() 
    {
        return true;
    }
    bool ProcessingCard()
    {
        return true;
    }
    bool CardPlayed()
    {
        return isProcessingCard;
    }
    public void AddCard(maskType mask)
    {
        // TODO : waiting on Board 
        // NEED TO INFER MOVE LIST TO FEED TO BOARD
        // all are actions but snake
       isProcessingCard = true;
    }
    public void AddCard(Queue<CardMove> movelist)
    {
        // TODO : waiting on Board 
        // check  current player orientation
        // ACTION ALL

        isProcessingCard = true;
    }

    // TODO:: WAITING ON BOARD FOR gET CHUNK AT POSITION
    // NEED PLAYER OBJECT WITH GIRD POSITION
    // 
    public bool HasPlayerWon() { return true; }
    // TODO : WAITING ON BOARD FOR gET CHUNK AT POSITION
    // NEED ALSO TO CHECK HAND SIZE
    public bool HasPlayerLost() { return false; }

    public void ResetLevel() {
        Debug.Log("Resetting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel() 
    {
        Debug.Log("Try to Load Next Level");
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading Next Level");
            SceneManager.LoadScene(nextIndex);
        }
    }

}
