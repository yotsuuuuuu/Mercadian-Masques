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


    [SerializeField] int chunksize = 5; // size of the chunnk (square so 16x16)
    //[SerializeField] Vector3 centrePos; //  maybe not necessary
    [SerializeField] ChunkType chunkType;
    [SerializeField] GameObject meshObject;

    public Chunk(Vector3 centrePos, ChunkType chunkType)
    {
        //this.centrePos = centrePos;
        this.chunkType = chunkType;
        OnCreate();
    }

    public void SetCentrePos(int x, int y, int z)
    {
        transform.position = new Vector3(x*chunksize, y*chunksize, z*chunksize);
        Debug.Log("X:" + x + " Y:" + y + " Z:" + z);
        Debug.Log("Chunksize:" + chunksize);
        Debug.Log("Chunk position set to " + transform.position);
    }

    public void SetChunkType(ChunkType type_)
    {
        this.chunkType = type_;
    }

    public Vector3 GetCentrePos()
    {
        return transform.position;
    }

    public ChunkType GetChunkType()
    {
        return chunkType;
    }

    private void OnCreate()
    {
        // Debug.Log("Chunk created at " + centrePos + " of type " + chunkType);
        //meshObject.GetComponent<MeshFilter>().mesh = 
    }

}
