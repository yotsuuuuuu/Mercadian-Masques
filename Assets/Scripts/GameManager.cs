using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private StateMachine stateMachine;
    public bool isProcessingCard;
    [SerializeField] private GameObject boardManager;
    [SerializeField] private GameObject handManager;
    private BoardManager board;
    private HandManager hand;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = boardManager.GetComponent<BoardManager>();
        hand = handManager.GetComponent<HandManager>();
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

    //TODO :  IMPLEMENT NOT WAITING ON ANYTHING
    public void ResetLevel() { }
    public void LoadNextLevel() { }

}
