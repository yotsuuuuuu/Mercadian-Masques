using System.Collections.Generic;
using TMPro;
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
            maskImages.transform.GetChild(0).gameObject.SetActive(true);
            maskImages.transform.GetChild(1).gameObject.SetActive(false);
            maskImages.transform.GetChild(2).gameObject.SetActive(false);
            maskImages.transform.GetChild(3).gameObject.SetActive(false);
            maskImages.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (mask == maskType.Snake)
        {
            maskImages.transform.GetChild(1).gameObject.SetActive(true);
            maskImages.transform.GetChild(0).gameObject.SetActive(false);
            maskImages.transform.GetChild(2).gameObject.SetActive(false);
            maskImages.transform.GetChild(3).gameObject.SetActive(false);
            maskImages.transform.GetChild(4).gameObject.SetActive(false);
        }
        if (mask == maskType.Bull)
        {
            maskImages.transform.GetChild(2).gameObject.SetActive(true);
            maskImages.transform.GetChild(1).gameObject.SetActive(false);
            maskImages.transform.GetChild(3).gameObject.SetActive(false);
            maskImages.transform.GetChild(4).gameObject.SetActive(false);
            maskImages.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (mask == maskType.Bird)
        {
            maskImages.transform.GetChild(3).gameObject.SetActive(true);
            maskImages.transform.GetChild(4).gameObject.SetActive(false);
            maskImages.transform.GetChild(2).gameObject.SetActive(false);
            maskImages.transform.GetChild(0).gameObject.SetActive(false);
            maskImages.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (mask == maskType.Deer)
        {
            maskImages.transform.GetChild(4).gameObject.SetActive(true);
            maskImages.transform.GetChild(0).gameObject.SetActive(false);
            maskImages.transform.GetChild(1).gameObject.SetActive(false);
            maskImages.transform.GetChild(2).gameObject.SetActive(false);
            maskImages.transform.GetChild(3).gameObject.SetActive(false);
        }
        TMP_Text text;
        text = GetComponentInChildren<TMP_Text>(true);

        foreach (CardMove move in moves_)
        {
            text.text += move.direction + " " + move.amount + "\n"; 
         }
    }

}
