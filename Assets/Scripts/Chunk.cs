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
    GOAL = 7,
    WALL = 8
}

public class Chunk : MonoBehaviour{


    [SerializeField] int chunksize = 5; // size of the chunnk (square so 5x5)
    Vector3 worldPosition; //  maybe not necessary
    Vector3Int gridIndex;
    [SerializeField] ChunkType chunkType;
    [SerializeField] GameObject meshObject;
    [SerializeField] Mesh CubeMesh;
    [SerializeField] Mesh PlaneMesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField] Material[] materials; // materials for different chunk types 


    private void Start()
    {
       
    }
    public Chunk(Vector3 centrePos, ChunkType chunkType_)
    {
        //this.centrePos = centrePos;
        chunkType = chunkType_;
    }

    public void SetCentrePos(int x, int y, int z)
    {
        transform.position = new Vector3(x*chunksize, y*chunksize, z*chunksize);
        //Debug.Log("X:" + x + " Y:" + y + " Z:" + z);
        //Debug.Log("Chunksize:" + chunksize);
        //Debug.Log("Chunk position set to " + transform.position);
        gridIndex = new Vector3Int(x, y, z);

        //if (x == 0 && z == 0)
        //{
        //    var meshRenderer = meshObject.GetComponent<MeshRenderer>();
        //    meshRenderer.enabled = false;
        //}
    }

    public void SetChunkType(ChunkType type_)
    {
        chunkType = type_;
        meshFilter = meshObject.GetComponent<MeshFilter>();
        meshRenderer = meshObject.GetComponent<MeshRenderer>();
        // set mesh or material based on type
        switch (chunkType)
        {
            case ChunkType.AIR:
                //meshObject.SetActive(false);
                meshRenderer.enabled = false; // set as invisible
                break;
            case ChunkType.GOAL: // need to set rat to true
            case ChunkType.GROUND:
            case ChunkType.START:
            case ChunkType.PIT:
                meshFilter.mesh = PlaneMesh;
                meshObject.transform.localPosition= new Vector3(0, -2.5f, 0); // set at ground level
                meshObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // scale to chunk size
                break;
            case ChunkType.HEDGE:
            case ChunkType.AIR_HEDGE:
            case ChunkType.ROCK:
            case ChunkType.WALL:
                meshFilter.mesh = CubeMesh; // set as cube
                meshObject.transform.localPosition = new Vector3(0, 0, 0); // set at center
                meshObject.transform.localScale = new Vector3(chunksize,chunksize,chunksize); // scale to chunk size
                break;
        }

        switch (chunkType)
        {
            case ChunkType.AIR:
                // do nothing as invisible
                break;
            case ChunkType.GROUND:
                meshRenderer.material = materials[1];
                break;
            case ChunkType.HEDGE:
            case ChunkType.AIR_HEDGE:
                meshRenderer.material = materials[2];
                break;
            case ChunkType.ROCK:
                meshRenderer.material = materials[3];
                break;
            case ChunkType.PIT:
                meshRenderer.material = materials[4];
                break;
            case ChunkType.START:
            case ChunkType.GOAL:
                meshRenderer.material = materials[5];
                break;    
            case ChunkType.WALL:
                meshRenderer.material = materials[6];
                break;
        }
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
