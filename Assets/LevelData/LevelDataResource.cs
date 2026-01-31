using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataResource", menuName = "Level Data/Chunk Level")]
public class LevelDataResource : ScriptableObject
{
    public List<ChunkLevelData> chunks;
}
