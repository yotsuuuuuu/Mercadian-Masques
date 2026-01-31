using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;

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
    }
    public override void Exit()
    {
        Debug.Log("Exit Idle State");
    }

    public override void Update()
    {
        Debug.Log("Update Idle State");
        // idel should check if player has won if not  wait for next card.
    }
}

public class ProcessingState : State
{
    public ProcessingState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        Debug.Log("Enter Processing State");
    }
    public override void Exit()
    {
        Debug.Log("Exit Processing State");
    }
    public override void Update()
    {
        Debug.Log("Update Processing State");
    }
}

public class CheckPLayerState : State
{
    public CheckPLayerState(GameManager gameManager) : base(gameManager) { }

    public override void Enter()
    {
        Debug.Log("Enter CheckPLayer State");
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