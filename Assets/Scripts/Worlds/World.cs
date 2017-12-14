using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Dictionary<WorldChunk, Chunk> chunks = new Dictionary<WorldChunk, Chunk>();

    public GameObject baseChunk;

	// use this for initialization
	void Start () {
        int size = Chunk.sizeChunk; // retrieves chunk size

        for (int x = -2; x <= 2; x++) {
            for (int y = 0; y < 1; y++) {
                for (int z = -2; z <= 2; z++) {
                    CreateChunk(x * size, y * size, z * size); // specifies chunk co-ordinates
                }
            }
        }
	}
	
	// update is called once per frame
	void Update () {
		
	}

    public Block GetBlock(int x, int y, int z) {
        Chunk tempChunk = GetChunk(x, y, z); // finds chunk at specified index

        if (tempChunk != null) {
            Block block = tempChunk.GetBlock(
                x - tempChunk.index.x,
                y - tempChunk.index.y,
                z - tempChunk.index.z
            );

            return block; // returns block within chunk
        }

        return new BlockAir(); // returns air block
    }

    public Chunk GetChunk(int x, int y, int z) {
        float size = Chunk.sizeChunk; // retrieves chunk size, as float

        WorldChunk index = new WorldChunk();
        index.x = Mathf.FloorToInt(x / size) * Chunk.sizeChunk; // returns x in multiples of chunk size
        index.y = Mathf.FloorToInt(y / size) * Chunk.sizeChunk; // returns y in multiples of chunk size
        index.z = Mathf.FloorToInt(z / size) * Chunk.sizeChunk; // returns z in multiples of chunk size

        Chunk tempChunk = null;

        chunks.TryGetValue(index, out tempChunk); // looks for existing chunk at index

        return tempChunk;
    }

    public void SetBlock(int x, int y, int z, Block block) {
        Chunk tempChunk = GetChunk(x, y, z);

        if (tempChunk != null) {
            tempChunk.SetBlock(
                x - tempChunk.index.x,
                y - tempChunk.index.y,
                z - tempChunk.index.z,
                block
            );
            tempChunk.update = true;
        }
    }

    public void CreateChunk(int x, int y, int z) {
        WorldChunk index = new WorldChunk(x, y, z); // co-ordinate index

        Vector3 thisChunk = new Vector3(x, y, z); // co-ordinate vector
        Quaternion zeroChunk = Quaternion.Euler(Vector3.zero); // zero vector

        // instantiates chunk at co-ordinates using base chunk
        GameObject newChunkEntity = Instantiate(baseChunk, thisChunk, zeroChunk) as GameObject;

        Chunk newChunk = newChunkEntity.GetComponent<Chunk>();

        newChunk.index = index; // stores index reference in chunk
        newChunk.world = this; // stores world reference in chunk

        chunks.Add(index, newChunk); // adds chunk to dictionary with index key

        // creates chunks

        int size = Chunk.sizeChunk; // retrieves chunk size

        for (int xi = 0; xi < size; xi++) {
            for (int yi = 0; yi < size; yi++) {
                for (int zi = 0; zi < size; zi++) {
                    if (yi <= xi && yi <= zi) {
                        SetBlock(x + xi, y + yi, z + zi, new BlockGrass());
                    }
                    else {
                        SetBlock(x + xi, y + yi, z + zi, new BlockAir());
                    }
                }
            }
        }
    }
}
