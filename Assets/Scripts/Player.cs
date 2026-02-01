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
    [SerializeField] public float rotationSpeed = 360.0f;
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
        // UpdateRotation(targetChunk, CurrentChunkData.position);
        Quaternion targetRotation = GetTargetRotation(targetChunk, CurrentChunkData.position);
        Vector3Int lastpostion = CurrentChunkData.position;
        int index = 0;
        while (index < path.Count)
        {
            Vector3 targetPos = targetChunk.GetWorldPosition();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime );
            if ((transform.position - targetPos).sqrMagnitude < 0.0001f)
            {
                currentDir = VectorToWorldDirection(targetChunk.GetGridIndex() - lastpostion);
                lastpostion = targetChunk.GetGridIndex();
                transform.position = targetPos;

                CurrentChunkData.position = targetChunk.GetGridIndex();
                CurrentChunkData.type = targetChunk.GetChunkType();
                //UpdateRotation(targetChunk, CurrentChunkData.position);

                index++;

                if (index >= path.Count)
                    break;

                targetChunk = path[index];

                targetRotation = GetTargetRotation(targetChunk, CurrentChunkData.position);
            }

            yield return null;
        }
        IsPlayerMoving = false;
    }
    Quaternion GetTargetRotation(Chunk targetChunk, Vector3Int currentPos)
    {
        Vector3Int dir = targetChunk.GetGridIndex() - currentPos;
        dir.y = 0;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            Debug.Log("x direction");
            return dir.x > 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, 270, 0);
        }
        else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.z))
        {
            Debug.Log("z direction");
            return dir.z > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        }
        else
        {
            return transform.rotation; // No change in rotation
        }
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

        return currentDir;

        // throw new System.ArgumentException("Invalid direction vector: " + dir);
    }
}
