using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]   // adds components to object class
[RequireComponent(typeof(MeshRenderer))] // adds components to object class
[RequireComponent(typeof(MeshCollider))] // adds components to object class

public class Chunk : MonoBehaviour {
    Block[ , , ] blocks = new Block[sizeChunk, sizeChunk, sizeChunk]; // volume of chunks
//  Block[ , , ] blocks;

    public static int sizeChunk = 16; // size of chunks
    public bool update = true; // update flag

    public World      world;
    public WorldChunk index;

    MeshFilter mf;
    MeshCollider mc;

    // use this for initialization
    void Start() {
        mf = gameObject.GetComponent<MeshFilter>();
        mc = gameObject.GetComponent<MeshCollider>();

        //      blocks = new Block[chunkSize, chunkSize, chunkSize];

        //      for (int x = 0; x < chunkSize; x++) {
        //          for (int y = 0; y < chunkSize; y++) {
        //              for (int z = 0; z < chunkSize; z++) {
        //                  blocks[x, y, z] = new BlockAir(); // fills chunk with air
        //              }
        //          }
        //      }

        //      blocks[3, 5, 2] = new Block(); // example block
        //      blocks[4, 5, 2] = new BlockGrass(); // example grass block

        //      UpdateChunk();
    }
	
	// update is called once per frame
	void Update() {
		if (update) {
            update = false;
            UpdateChunk();
        }
	}

    // updates the chunk based on its contents
    void UpdateChunk() {
        MeshChunk m = new MeshChunk();

        for (int x = 0; x < sizeChunk; x++) {
            for (int y = 0; y < sizeChunk; y++) {
                for (int z = 0; z < sizeChunk; z++) {
                    m = blocks[x, y, z].MeshBlock(this, x, y, z, m);
                }
            }
        }

        RenderMesh(m);
    }

    // sends calculated mesh data to renderer and collider
    void RenderMesh(MeshChunk m) {

        // renderer features
        mf.mesh.Clear();

        mf.mesh.vertices  = m.vx.ToArray(); // adds renderer vertices
        mf.mesh.triangles = m.tx.ToArray(); // adds renderer triangles
        mf.mesh.uv        = m.uv.ToArray(); // adds uv-maps

        mf.mesh.RecalculateNormals();

        // collider features
        mc.sharedMesh = null; // removes default collision mesh

        Mesh temp = new Mesh(); // temporary mesh

        temp.vertices  = m.vc.ToArray(); // adds collider vertices
        temp.triangles = m.tc.ToArray(); // adds collider triangles

        temp.RecalculateNormals();

        mc.sharedMesh = temp; // inserts new collision mesh

    }

    // retrieves specific block from chunk
    public Block GetBlock(int x, int y, int z) {
        if (InRange(x) && InRange(y) && InRange(z)) {
            return blocks[x, y, z]; // retrieves block from within chunk
        }

        return world.GetBlock(index.x + x, index.y + y, index.z + z); // retrieves block from different chunk
    }

    // introduces specific block to chunk
    public void SetBlock(int x, int y, int z, Block block) {
        if (InRange(x) && InRange(y) && InRange(z)) {
            blocks[x, y, z] = block; // introduces block to within chunk
        }
        else {
            world.SetBlock(index.x + x, index.y + y, index.z + z, block); // introduces block to different chunk
        }
    }

    public static bool InRange(int value) {
        if (value < 0 || value >= sizeChunk) {
            return false;
        }

        return true;
    } 
}
