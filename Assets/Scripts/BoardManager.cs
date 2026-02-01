using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // 
    int boardActualX, boardActualZ; 
    int boardActualY = 2;

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
                    case GlobalDirection.Up:
                        playerPos.y++;
                        playerPos.y = Mathf.Clamp(playerPos.y, 0, boardActualY - 1);
                        //y++;
                        break;
                    case GlobalDirection.Down:
                        playerPos.y--;
                        //y--;
                        break;
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

    public void Initialize(int[,,] chunkInfoArray, int boardGivenX, int boardGivenY, int boardGivenZ)
    {

        //board = new Chunk[boardSizeX, boardSizeY, boardSizeZ];
        boardActualX = boardGivenX + 2;
        boardActualY = boardGivenY;
        boardActualZ = boardGivenZ + 2;

        board = new Chunk[boardActualX, boardActualY, boardActualZ];

        //int[,,] chunkInfosArray = new int[boardSizeX, boardSizeY, boardSizeZ];

        for (int y = 0; y < boardActualY; y++)
        {
            for (int x = 0; x < boardActualX; x++)
            {
                for (int z = 0; z < boardActualZ; z++)
                {
                    var chunk = Instantiate(chunkPrefab);

                    if ((x == 0 || z == 0) || (x == boardActualX - 1 || z == boardActualZ - 1)) // if on edge, make wall
                    {
                        chunk.SetChunkType(ChunkType.WALL);
                    }
                    else
                    {
                        if (y == 0)
                        {
                            chunk.SetChunkType(ChunkType.GROUND);
                        }
                        else
                        {
                            chunk.SetChunkType(ChunkType.AIR);
                        }
                    }

                    // plus one to center the chunk
                    chunk.SetCentrePos(x, y, z);
                    board[x, y, z] = chunk;

                }

            }

        }

        for (int x = 0; x < boardGivenX; x++)
        {
            for (int z = 0; z < boardGivenZ; z++)
            {
                if (chunkInfoArray[x, 0, z] != 0)
                {
                    ChunkType type = (ChunkType)chunkInfoArray[x, 0, z];
                    //Debug.Log(type);
                    board[x+1, 0, z+1].SetChunkType(type);
                    if (type == ChunkType.HEDGE)
                    {
                        board[x + 1, 1, z + 1].SetChunkType(ChunkType.AIR_HEDGE);
                    }
                }
            }
        }
    }

    public Chunk GetChunkAtPosition(Vector3Int position)
    {
        if (position.x < 0 || position.x >= boardActualX ||
            position.y < 0 || position.y >= boardActualY ||
            position.z < 0 || position.z >= boardActualZ)
        {
            //Debug.Log("out of bounds");
            return null;
        }
        return board[position.x, position.y, position.z];
    }

}

