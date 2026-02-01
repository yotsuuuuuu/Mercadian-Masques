using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] public GameObject cardPrefab;
    public Transform handTransform;
    public int handsize;
    public float spread = 5.0f;
    public float spacing = -100;
    public float vspacing = 100;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public List<GameObject> cardstest = new List<GameObject>();
    private GameManager gameManager;
    [SerializeField] float hOFFSET;
    [SerializeField] float vOFFSET;

    int cardAmount;
    bool processing;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);
        //cardstest.Add(cardPrefab);


        //AddCardsToHand(cardstest);

    }

    private void Update()
    {
        //UpdateHandVisual();   
    }


    public void AddCardsToHand(List<CardData> cardslist_)
    {
        cardAmount = cardslist_.Count;

        for (int i = 0; cardslist_.Count > i; i++)
        {
            // card data includes the mask (enum) and the move instructions (queue)

            GameObject cardObject = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(cardObject);
            Card cardData = cardObject.GetComponent<Card>();
            cardData.SetCard(cardslist_[i].moveList, cardslist_[i].maskType);
        }

        UpdateHandVisual();
        
    }

    public void AddCardsToHand(List<GameObject> cardslist_)
    {
        cardAmount = cardslist_.Count;
        for (int i = 0; cardslist_.Count > i; i++)
        {
            // card data includes the mask (enum) and the move instructions (queue)
            GameObject cardObject = Instantiate(cardslist_[i], handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(cardObject);
            //Card cardData = cardObject.GetComponent<Card>();
            //cardData.SetCard(cardslist_[i].movelist, cardslist_[i].maskType);
        }
        UpdateHandVisual();
    }

    private void UpdateHandVisual()
    {
        int cardCount = cardsInHand.Count;
        if (cardCount == 1)
        {
            Debug.Log("only one card in hand");
            // set card position
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            cardsInHand[0].transform.localPosition = new Vector3(hOFFSET, vspacing+ vOFFSET , 0f);
            //cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);

            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (spread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);

            float horizontalOffset = spacing * (i - (cardCount - 1) / 2f);
            float normalizedPosition = (2f * i) / (cardCount - 1) - 1f; // Normalize between -1 and 1
            float verticalOffset = vspacing * (1 - normalizedPosition * normalizedPosition); // More offset towards the center

            // set card position
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset + hOFFSET, verticalOffset + vOFFSET , 0f);
        }
    }
    
    public int GetNumCards()
    {
        return cardsInHand.Count;
    }
    public void getCardInfo(GameObject card_, int state_)
    {
        GameObject card = card_;
        if(gameManager.state != GameManagerState.IDLE)
        {
            return;
        }

        if (state_ == 3)
        {
            gameManager.AddCard(card.GetComponent<Card>().mask);
            RemoveCardFormHand(card_);
           // Debug.Log("mask");
            if(card.GetComponent<Card>().mask == maskType.Frog)
            {
                SoundManager.PlaySound(SoundEffect.Frog);
            }
            else if (card.GetComponent<Card>().mask == maskType.Bull)
            {
                SoundManager.PlaySound(SoundEffect.Bull,0.2f);
            }
            else if (card.GetComponent<Card>().mask == maskType.Snake)
            {
                SoundManager.PlaySound(SoundEffect.Snake);
            }
            else if (card.GetComponent<Card>().mask == maskType.Bird)
            {
                SoundManager.PlaySound(SoundEffect.Owl);
            }
            else if (card.GetComponent<Card>().mask == maskType.Deer)
            {
                SoundManager.PlaySound(SoundEffect.Deer);
            }



        }
        if (state_== 4)
        {
            gameManager.AddCard(card.GetComponent<Card>().cardMovement);
            RemoveCardFormHand(card_);
            //Debug.Log("movement");
              SoundManager.PlaySound(SoundEffect.Walk);
        }

    }


   void RemoveCardFormHand(GameObject card_)
    {
        GameObject tobeDestroyed = null;
        foreach (GameObject c in cardsInHand)
        {
            if (c == card_)
            {
                tobeDestroyed = c;
                cardsInHand.Remove(c);
                break;
            }
        }

        if (tobeDestroyed != null)
        {
            Destroy(tobeDestroyed);
            UpdateHandVisual();
        }
    }
}
