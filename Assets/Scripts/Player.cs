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
    public float speed = 2.0f;
    public int FlyingEffectCounter = 0;
    public bool SnakeEffectActive = false;

    public bool IsPlayerMoving = false;

    public void MoveOnPath(List<Chunk> path)
    {
        if(!IsPlayerMoving && path.Count !=0)
        {
            IsPlayerMoving = true;
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

        transform.rotation = Quaternion.LookRotation(worldDirection);
    }
}
