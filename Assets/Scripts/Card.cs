using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Card;



public enum maskType { Bull, Frog, Deer, Bird, Snake, Null };

public enum GlobalDirection { North, East, South, West, Up, Down };
public enum CardDir { Foward, Right, Back,  Left, Up,Down };


[System.Serializable] public struct CardMove
{
    public CardDir direction;
    public int amount;
}

public class Card:MonoBehaviour
{
   
    public Queue<CardMove> cardMovement;
    //[SerializeField] public CardDir direction;
    [SerializeField]public List<CardMove> cardMovementList;
    [SerializeField] public maskType mask;
    [SerializeField] private GameObject maskImages;
    //public Sprite cardSprite; // not used?


    public void SetCard(Queue<CardMove> moves_, maskType mask_)
    {
        cardMovement = moves_;
        mask = mask_;

        if(mask == maskType.Frog)
        {
            maskImages.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

}
