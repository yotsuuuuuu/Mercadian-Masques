using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // 
    int boardSizeX = 9;
    int boardSizeZ = 9;
    int boardSizeY = 2; // height levels

    [SerializeField] Chunk chunkPrefab;
    private void Start()
    {
        Initialize(new int[boardSizeX, boardSizeY, boardSizeZ]);
    }

    void Initialize(int[,,] chunkInfoArray)
    {
        Chunk[,,] board = new Chunk[boardSizeX, boardSizeY, boardSizeZ];
        int[,,] chunkInfosArray = new int[boardSizeX, boardSizeY, boardSizeZ];

        for (int y = 0; y < boardSizeY; y++)
        {
            for (int x = 0; x < boardSizeX; x++)
            {
                for (int z = 0; z < boardSizeZ; z++)
                {
                    var chunkType = chunkInfosArray[x, y, z];
                    //if (chunkType == -1) do something; // empty chunks == air
                    Debug.Log("chunkType:" + chunkType);
                    var chunk = Instantiate(chunkPrefab);
                    switch (chunkType)
                    {
                        case 0: // AIR
                            chunk.SetChunkType(ChunkType.AIR);
                            break;
                        case 1: // GROUND
                            chunk.SetChunkType(ChunkType.GROUND);
                            break;
                        case 2: // HEDGE
                            chunk.SetChunkType(ChunkType.HEDGE);
                            break;
                        case 3: // AIR_HEDGE
                            chunk.SetChunkType(ChunkType.AIR_HEDGE);
                            break;
                        case 4: // ROCK
                            chunk.SetChunkType(ChunkType.ROCK);
                            break;
                        case 5: // PIT
                            chunk.SetChunkType(ChunkType.PIT);
                            break;
                        case 6: // START
                            chunk.SetChunkType(ChunkType.START);
                            break;
                        case 7: // GOAL
                            chunk.SetChunkType(ChunkType.GOAL);
                            break;
                        default:
                            break;
                    }
                    chunk.SetCentrePos(x,y,z);
                    board[x, y, z] = chunk;

                    //Debug.Log("x:" + x + " y:" + y + " z:" + z);

                    //Chunk chunk = Instantiate(chunkPrefab);
                    //var chunkInfo = chunk.GetComponents<Chunk>();
                    //board[x, y, z] = new; // Initialize all to GROUND (0
                }

            }

        }
    }
}

