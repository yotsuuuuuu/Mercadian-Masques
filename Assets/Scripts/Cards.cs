using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cards;



public enum maskType { Bull, Frog, Deer, Bird, Snake };

public enum dir { forward, right, back, left,up,down };


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
    public Image[] typeImages;

    public void SetCard(Queue<CardMove> card_, maskType type_)
    {
        cardMovement = card_;
        mask = type_;
    }

}
