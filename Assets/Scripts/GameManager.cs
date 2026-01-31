using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private StateMachine stateMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        return true;
    }
}
