using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCardDataResource", menuName = "Level Data/Card Level Data")]
public class LevelCardDataResource : ScriptableObject
{
    public List<CardLevelData> ListOfCards;
}
