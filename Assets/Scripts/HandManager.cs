using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] public GameObject cardPrefab;
    public Transform handTransform;
    public int handsize;
    public float spread = 50.0f;
    float spacing = 100;
    float vspacing = 100;
    public List<Cards> cardsInHand = new List<Cards>();
    public List<Cards> cardstest = new List<Cards>();
   
    int cardAmount;
    bool processing;

    private void Start()
    {
        cardstest.Add(cardPrefab.GetComponent<Cards>());
        cardstest.Add(cardPrefab.GetComponent<Cards>());
        AddCardsToHand(cardstest);

        
    }




    private void AddCardsToHand(List<Cards> cardslist_)
    {
        cardAmount = cardslist_.Count;
       
       for(int i = 0;cardslist_.Count > i;i++)
        {
            GameObject newCardGO = Instantiate(cardPrefab, handTransform.position,Quaternion.identity,handTransform);
            Cards cardUI = newCardGO.GetComponent<Cards>();

            cardUI.mask = cardslist_[i].mask;
            cardUI.cardMovement = cardslist_[i].cardMovement;

            cardsInHand.Add(cardUI);
        }
        UpdateHandVisual();
        
    }
    private void UpdateHandVisual()
    {
        cardAmount = cardsInHand.Count;
        if(cardAmount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardAmount; i++)
        {
            float rotationAngle = (spread * (i - (cardAmount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            //float horizontalOffset = (spacing * (i - (cardAmount - 1) / 2f));

            //float normalizedpos = (2f * i / (cardAmount - 1) - 1f);
            //float verticlOffset = vspacing * (i - normalizedpos*normalizedpos);
            //cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticlOffset, 0f);
        }
    }
    
}
