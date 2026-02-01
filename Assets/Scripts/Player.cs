using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.XR;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ChunkLevelData CurrentChunkData; // contains grid position and current chunk type
    public GlobalDirection currentDir; // current facing direction
    [SerializeField] public float speed = 2.0f;
    public int FlyingEffectCounter = 0;
    public bool SnakeEffectActive = false;

    public bool IsPlayerMoving = false;

    public void MoveOnPath(List<Chunk> path)
    {
        if(!IsPlayerMoving && path.Count !=0)
        {
            
            IsPlayerMoving = true;
            //DebugPrintPath(path);
            StartCoroutine(MoveAlongThePath(path));
        }
    }
    IEnumerator MoveAlongThePath(List<Chunk> path)
    {
        yield return null;
        Chunk targetChunk = path[0];
        UpdateRotation(targetChunk, CurrentChunkData.position);
        int index = 0;
        while (index < path.Count) {
            Vector3 targetPos = targetChunk.GetWorldPosition();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
           
            if ((transform.position - targetPos).sqrMagnitude < 0.0001f)
            {
                transform.position = targetPos;

                CurrentChunkData.position = targetChunk.GetGridIndex();
                CurrentChunkData.type = targetChunk.GetChunkType();

                index++;

                if (index >= path.Count)
                    break;

                targetChunk = path[index];                
                UpdateRotation(targetChunk, CurrentChunkData.position);
            }

            yield return null;
        }
        IsPlayerMoving = false;
    }

    private void DebugPrintPath(List<Chunk> path)
    {
        foreach (Chunk chunk in path)
        {
            Vector3Int pos = chunk.GetGridIndex();
            Debug.Log("Path Chunk Position: " + pos.ToString());
            Debug.Log("Path Chunk Type: " + chunk.GetChunkType().ToString());
        }
    }
    private void UpdateRotation(Chunk targetChunk, Vector3Int position)
    {
        Vector3Int targetIndexPos = targetChunk.GetGridIndex();
        Vector3Int currentIndexPos = position;
        Vector3Int directionIndexVector = targetIndexPos - currentIndexPos;
        if (directionIndexVector.y !=  0)
            return;

        Vector3 worldDirection = new Vector3(directionIndexVector.x,0,directionIndexVector.z);

        if (worldDirection == Vector3.zero)
            return;
        currentDir = VectorToWorldDirection(directionIndexVector);
        transform.rotation = Quaternion.LookRotation(worldDirection);
    }


    private GlobalDirection VectorToWorldDirection(Vector3Int dir)
    {
        // Ignore vertical movement if any
        dir.y = 0;

        if (dir == Vector3Int.forward)   // (0, 0, 1)
            return GlobalDirection.North;

        if (dir == Vector3Int.back)      // (0, 0, -1)
            return GlobalDirection.South;

        if (dir == Vector3Int.right)     // (1, 0, 0)
            return GlobalDirection.East;

        if (dir == Vector3Int.left)      // (-1, 0, 0)
            return GlobalDirection.West;

        throw new System.ArgumentException("Invalid direction vector: " + dir);
    }
}
