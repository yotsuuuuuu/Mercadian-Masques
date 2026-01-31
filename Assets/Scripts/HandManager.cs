using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] public GameObject cardPrefab;
    public Transform handTransform;
    public int handsize;
    public float spread = 5.0f;
    public List<Cards> cardsInHand = new List<Cards>();
    int cardAmount;
    bool processing;




  
    private void AddCardsToHand(int handsize_,List<Cards> cardslist_)
    {
        handsize = handsize_;
        cardAmount = handsize_;
        cardsInHand= cardslist_;
        
    }
    private void UpdateHandVisual()
    {

    }
    
}
