using UnityEngine;

public enum ChunkType
{
    AIR = 0,
    GROUND = 1,
    HEDGE = 2,
    AIR_HEDGE = 3,
    ROCK = 4,
    PIT = 5,
    START = 6,
    GOAL = 7
}

public class Chunk : MonoBehaviour{


    [SerializeField] int chunksize = 5; // size of the chunnk (square so 5x5)
    Vector3 worldPosition; //  maybe not necessary
    Vector3Int gridIndex;
    [SerializeField] ChunkType chunkType;
    [SerializeField] GameObject meshObject;

    public Chunk(Vector3 centrePos, ChunkType chunkType)
    {
        //this.centrePos = centrePos;
        this.chunkType = chunkType;
    }

    public void SetCentrePos(int x, int y, int z)
    {
        transform.position = new Vector3(x*chunksize, y*chunksize, z*chunksize);
        Debug.Log("X:" + x + " Y:" + y + " Z:" + z);
        Debug.Log("Chunksize:" + chunksize);
        Debug.Log("Chunk position set to " + transform.position);
        gridIndex = new Vector3Int(x, y, z);

        //if (x == 0 && z == 0)
        //{
        //    var meshRenderer = meshObject.GetComponent<MeshRenderer>();
        //    meshRenderer.enabled = false;
        //}
    }

    public void SetChunkType(ChunkType type_)
    {
        this.chunkType = type_;
        // set mesh or material based on type
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public ChunkType GetChunkType()
    {
        return chunkType;
    }

    public Vector3Int GetGridIndex()
    {
        return gridIndex;
    }
}
