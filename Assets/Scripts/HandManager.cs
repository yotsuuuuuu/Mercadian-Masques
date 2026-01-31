using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] public GameObject cardPrefab;
    public int handsize;
    public int spread;
    int cardsInHand;
    bool processing;




    public void set(int handsize_,GameObject[] array_)
    {
        handsize = handsize_;
        cardsInHand = handsize_;
    }
    private void UpdateHandVisual()
    {

    }
}
