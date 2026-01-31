using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Card;



public enum maskType { Bull, Frog, Deer, Bird, Snake };

public enum GlobalDirection { North, East, South, West, Up, Down };
public enum CardDir { Foward, Right, Back,  Left, Up,Down };


[System.Serializable] public struct CardMove
{
    public CardDir direction;
    public int amount;
}
public class Card:MonoBehaviour
{
   
    public Queue<CardMove>cardMovement;
    [SerializeField]public List<CardMove> cardMovementList;
    [SerializeField] public maskType mask;
    [SerializeField] public CardDir direction;
    public Sprite cardSprite;
    public Image[] typeImages;

    public void SetCard(Queue<CardMove> card_, maskType type_)
    {
        cardMovement = card_;
        mask = type_;
    }

}
