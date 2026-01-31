using System.Collections.Generic;
using UnityEngine;
using static Cards;



public enum maskType { Bull, Frog, Deer, Bird, Snake };

public enum dir { north, east, sout, west };


[System.Serializable] public struct CardMove
{
    public dir direction;
    public int amount;
}
public class Cards:MonoBehaviour
{
   
    public Queue<CardMove>cardMovement;
    [SerializeField]private List<CardMove> cardMovementList;
    [SerializeField] public maskType mask;
    [SerializeField] public dir direction;
    public Sprite cardSprite;
    private void Awake()
    {
        cardMovement = new Queue<CardMove>(cardMovementList);
    }

}
