using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // 
    int boardSizeX, boardSizeZ; 
    int boardSizeY = 2;

    Chunk[,,] board;

    [SerializeField] Chunk chunkPrefab;
    //private void Start()
    //{
    //    Initialize(new int[boardSizeX, boardSizeY, boardSizeZ]);
    //}

    public List<Chunk> CheckMovement(Queue<KeyValuePair<GlobalDirection, int>> movementInstructions, Vector3Int playerPos)
    {
        // x: east-west +/-
        // y: height level
        // z: north-south +/-
        List<Chunk> potentialChunks = new List<Chunk>();
        while (movementInstructions.Count > 0)
        {
            KeyValuePair<GlobalDirection, int> instruction = movementInstructions.Dequeue();
            for (int i = 0; i < instruction.Value; i++) // go one step at a time
            {
                switch (instruction.Key)
                {
                    case GlobalDirection.North:
                        playerPos.z++;
                        //z++;
                        break;
                    case GlobalDirection.East:
                        playerPos.x++; ;
                        //x++;
                        break;
                    case GlobalDirection.South:
                        playerPos.z--;
                        //z--;
                        break;
                    case GlobalDirection.West:
                        playerPos.x--;
                        //x--;
                        break;
                    //case dir.up:
                    //    playerPos.y++;
                    //    break;
                    //case dir.down:
                    //    playerPos.y--;
                    //    break;
                    default:
                        break;
                }

                Chunk temp = GetChunkAtPosition(playerPos);
                if( temp == null)
                {
                    Debug.Log("out of bounds movement");
                    return potentialChunks;
                }
                potentialChunks.Add(temp);

            }
        }
            return potentialChunks;
    }

    public void Initialize(int[,,] chunkInfoArray, int boardX, int boardY, int boardZ)
    {

        //board = new Chunk[boardSizeX, boardSizeY, boardSizeZ];
        boardSizeX = boardX;
        boardSizeY = boardY;
        boardSizeZ = boardZ;

        board = new Chunk[boardSizeX, boardSizeY, boardSizeZ];

        //int[,,] chunkInfosArray = new int[boardSizeX, boardSizeY, boardSizeZ];

        for (int y = 0; y < boardSizeY; y++)
        {
            for (int x = 0; x < boardSizeX; x++)
            {
                for (int z = 0; z < boardSizeZ; z++)
                {
                    // edit here to set chunk type based on chunkInfosArray
                    // chunkInfoArray will be empty except for the given chunk types.


                    var chunkType = chunkInfoArray[x, y, z];
                    //if (chunkType == -1) do something; // empty chunks == air
                    Debug.Log("chunkType:" + chunkType);
                    var chunk = Instantiate(chunkPrefab);

                    if ((x == 0 || z == 0) || (x == boardSizeX-1 || z == boardSizeZ-1)) // if on edge, make wall
                    {
                        chunk.SetChunkType(ChunkType.WALL);
                        continue;
                    }

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
                    // plus one to center the chunk
                    chunk.SetCentrePos(x+1,y,z+1);
                    // chunk.SetMesh (maybe do this on set type)
                    board[x+1, y, z+1] = chunk;

                    //Debug.Log("x:" + x + " y:" + y + " z:" + z);

                    //Chunk chunk = Instantiate(chunkPrefab);
                    //var chunkInfo = chunk.GetComponents<Chunk>();
                    //board[x, y, z] = new; // Initialize all to GROUND (0
                }

            }

        }
    }

    public Chunk GetChunkAtPosition(Vector3Int position)
    {
        if (position.x < 0 || position.x >= boardSizeX ||
            position.y < 0 || position.y >= boardSizeY ||
            position.z < 0 || position.z >= boardSizeZ)
        {
            Debug.Log("out of bounds");
            return null;
        }
        return board[position.x, position.y, position.z];
    }

}

