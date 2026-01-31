using UnityEngine;

public class Player : MonoBehaviour
{
    public ChunkLevelData CurrentChunkData; // contains grid position and current chunk type
    public GlobalDirection currentDir; // current facing direction

    public int FlyingEffectCounter = 0;
    public bool SnakeEffectActive = false;

}
