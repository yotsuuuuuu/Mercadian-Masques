using System.Collections.Generic;
using UnityEngine;


public abstract class State
{
    protected GameManager gameManager;

    public State(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void Enter() {  }
    public virtual void Exit() { }
    public virtual void Update() { }

}

public class TransitonState
{
    public System.Func<bool> condition;
    public State TargetState;
    
    public TransitonState(System.Func<bool> condition, State state)
    {
        this.condition = condition;
        this.TargetState = state;
    }
}

public class StateMachine
{
    public State currentState { get; private set; }
    private Dictionary<State, List<TransitonState>> transitions = new Dictionary<State, List<TransitonState>>();

    public void AddTransitions(State form, TransitonState transition)
    {
        if (!transitions.ContainsKey(form))
            transitions[form] = new List<TransitonState>();

        transitions[form].Add(transition);
    }
    public void SetState(State s)
    {
        currentState = s;
        currentState.Enter();
    }
    public void Update() {
        if (currentState == null)
            return;

        if(transitions.TryGetValue(currentState,out var edges))
        {
            foreach (var edge in edges)
            {
                if (edge.condition()) 
                {
                    ChangeState(edge.TargetState);
                    break;
                }
            }

        }
        currentState?.Update();
    }


    private void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

}

public class IdleState : State 
{
    public IdleState(GameManager gameManager) : base(gameManager) { }
    public override void Enter()
    {
        Debug.Log("Enter Idle State");
        gameManager.isProcessingCard = false;
        gameManager.state = GameManagerState.IDLE;
    }
    public override void Exit()
    {
        Debug.Log("Exit Idle State");
    }

    public override void Update()
    {
        Debug.Log("Update Idle State");
        // idel should check if player has won if not  wait for next card.

        // if player has won
        // call to next level or end game
        // if player has has lost
        // call to restart level
        if (gameManager.HasPlayerWon())
            gameManager.LoadNextLevel();
        
        if(gameManager.HasPlayerLost())
            gameManager.ResetLevel();
    }
}

public class ProcessingState : State
{
    public ProcessingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        Debug.Log("Enter Processing State");
        gameManager.state = GameManagerState.PROCESSING;

        gameManager.isProcessingCard = true;
        gameManager.playerpath.Clear();
        if (gameManager.currentPlayMask == maskType.Deer)
        {
            List<Chunk> possiblePath = new List<Chunk>();
            var currentPos = gameManager.player.CurrentChunkData.position;
            bool isValidMove = true;
            while (isValidMove)
            {
                possiblePath.AddRange(gameManager.board.CheckMovement(gameManager.movementInstructions, currentPos));
                Chunk possibleChunk = possiblePath[possiblePath.Count - 1];
                if (possibleChunk == null || possibleChunk.GetChunkType() == ChunkType.ROCK)
                {
                    possiblePath.Clear();
                    isValidMove = false;
                    continue;
                }
                ChunkType type = possibleChunk.GetChunkType();
                if (type == ChunkType.HEDGE || type == ChunkType.AIR_HEDGE)
                {
                    currentPos = possibleChunk.GetGridIndex();
                    continue;
                }
                else
                {
                    isValidMove = false;
                }

            }

           gameManager.player.MoveOnPath(possiblePath);
        }
        else
        {
            List<Chunk> possiblePath = gameManager.board.CheckMovement(gameManager.movementInstructions,gameManager.player.CurrentChunkData.position ) ;
            possiblePath.RemoveAll(c => c == null);


            gameManager.player.MoveOnPath(possiblePath);

            //send instsructoins to player to move
        }
    }
    public override void Exit()
    {
        Debug.Log("Exit Processing State");
    }
    public override void Update()
    {
        Debug.Log("Update Processing State");
        // check if player has finished moving
    }
}

public class CheckPLayerState : State
{
    public CheckPLayerState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        Debug.Log("Enter CheckPLayer State");
        gameManager.state = GameManagerState.CHECKPLAYER;
    }
    public override void Exit()
    {
        Debug.Log("Exit CheckPLayer State");
    }
    public override void Update()
    {
        Debug.Log("Update CheckPLayer State");
    }
} 