using NUnit.Framework.Interfaces;
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

public struct CardData
{
    public maskType maskType;
    public Queue<CardMove> moveList;
}

public enum  GameManagerState
{
    IDLE,
    PROCESSING,
    CHECKPLAYER
}


public class GameManager : MonoBehaviour
{
    private StateMachine stateMachine;
    public bool isProcessingCard;
    [SerializeField] private GameObject boardManager;
    [SerializeField] private GameObject handManager;
    [SerializeField] public LevelCardDataResource CardsData;
    [SerializeField] public LevelDataResource ChunkData;
    [SerializeField] public Player player;
   
    int boardSizeX = 9; // default values to for the board
    int boardSizeZ = 9;
    int boardSizeY = 2;

    public GameManagerState state;
 
    public BoardManager board { get; private set; }
    private HandManager hand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        board = boardManager.GetComponent<BoardManager>();
        hand = handManager.GetComponent<HandManager>();
        //hand.AddCardsToHand( PopulateCardData(CardsData.ListOfCards));
        board.Initialize(ArrayofChunks(ChunkData.chunks), boardSizeX, boardSizeY, boardSizeZ);


        isProcessingCard = false;

        stateMachine = new StateMachine();
        IdleState idel = new IdleState(this);
        ProcessingState processing = new ProcessingState(this);
        CheckPLayerState playerState = new CheckPLayerState(this);

        stateMachine.SetState(idel);
        

        stateMachine.AddTransitions(idel, new TransitonState(() => CardPlayed(), processing));
        //stateMachine.AddTransitions(idel, new TransitonState(() => !CardPlayed(), idel));
        stateMachine.AddTransitions(processing, new TransitonState(() => !ProcessingCard(), playerState));
        //stateMachine.AddTransitions(processing, new TransitonState(() => !ProcessingCard(), processing));
        stateMachine.AddTransitions(playerState, new TransitonState(() => PlayerCheck(), idel));
        stateMachine.AddTransitions(playerState, new TransitonState(() => !PlayerCheck(), processing));
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
        return isProcessingCard;
    }
    bool CardPlayed()
    {
        return movementInstructions.Count != 0 ;
    }

    public Queue<KeyValuePair<GlobalDirection, int>> movementInstructions;
    public List<Chunk> playerpath;
    public maskType currentPlayMask = maskType.Null;
    public void AddCard(maskType mask)
    {
   
        movementInstructions.Clear();

        switch (mask)
        {
            case maskType.Bull:
                currentPlayMask = maskType.Bull;
                Queue<CardMove> bullMoves = new Queue<CardMove>();
                bullMoves.Enqueue(new CardMove { direction = CardDir.Foward, amount = 1  });
                movementInstructions = ConvertCardMoveToBoardMoves(bullMoves);
                break;
            case maskType.Frog:
                currentPlayMask = maskType.Frog;
                Queue<CardMove> frogMoves = new Queue<CardMove>();
                frogMoves.Enqueue(new CardMove { direction = CardDir.Up, amount = 1  });
                frogMoves.Enqueue(new CardMove { direction = CardDir.Foward, amount = 2  });
                frogMoves.Enqueue(new CardMove { direction = CardDir.Down, amount = 1 });
                movementInstructions = ConvertCardMoveToBoardMoves(frogMoves);
                break;
            case maskType.Deer:
                currentPlayMask = maskType.Deer;
                Queue<CardMove> deerMoves = new Queue<CardMove>();
                deerMoves.Enqueue(new CardMove { direction = CardDir.Foward, amount = 1  });
                movementInstructions = ConvertCardMoveToBoardMoves(deerMoves);
                break;
            case maskType.Bird:
                currentPlayMask = maskType.Bird;
                player.FlyingEffectCounter += 2;
                Queue<CardMove> birdMoves = new Queue<CardMove>();
                birdMoves.Enqueue(new CardMove { direction = CardDir.Up, amount = 1  });
                movementInstructions = ConvertCardMoveToBoardMoves(birdMoves);
                break;
            case maskType.Snake:
                currentPlayMask = maskType.Snake;
                player.SnakeEffectActive = true;
                movementInstructions.Clear();
                break;
        }

        
    }

    public void AddCard(Queue<CardMove> movelist)
    {
      
        // idealy 2 foward , 2 left ,2 left with player facing north  should map to 2 north, 2 west, 2 south.
        movementInstructions.Clear();
        currentPlayMask = maskType.Null;
        movementInstructions = ConvertCardMoveToBoardMoves(movelist);


    }

    public Queue<KeyValuePair<GlobalDirection, int>> ConvertCardMoveToBoardMoves(Queue<CardMove> moveList) {
        Queue<KeyValuePair<GlobalDirection, int>> list = new Queue<KeyValuePair<GlobalDirection, int>>();
        GlobalDirection CurrentPlayerDir = player.currentDir;
        int moveMultiplier = (player.SnakeEffectActive) ? 2 : 1;
        player.SnakeEffectActive = false;
        if (player.FlyingEffectCounter > 0)
        {
            player.FlyingEffectCounter--;
        }
        while (moveList.Count > 0)
        {
            CardMove move = moveList.Dequeue();
            GlobalDirection globalDirection = MaptoRelativetoWorld(CurrentPlayerDir, move.direction);
            list.Enqueue(new KeyValuePair<GlobalDirection, int>(globalDirection, move.amount * moveMultiplier));
            CurrentPlayerDir = globalDirection;
        }

        return list;

    }


    GlobalDirection MaptoRelativetoWorld(GlobalDirection currentDir, CardDir moveDir)
    {

        if (moveDir == CardDir.Up)
            return GlobalDirection.Up;
        if (moveDir == CardDir.Down)
            return GlobalDirection.Down;

        int facing = (int)currentDir;
        int offset = moveDir switch
        {
            CardDir.Foward => 0,
            CardDir.Right => 1,
            CardDir.Back => 2,
            CardDir.Left => 3,
            _ => 0
        };

        int WorldDir = (facing + offset) % 4;
        return (GlobalDirection)WorldDir;
    }

    
    public bool HasPlayerWon() { return (player.CurrentChunkData.type == ChunkType.GOAL); }
 
    public bool HasPlayerLost() { 
        if(hand.cardsInHand.Count == 0 && player.CurrentChunkData.type != ChunkType.GOAL)
        {
            return true;
        }
        return false; 
    }

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

}
