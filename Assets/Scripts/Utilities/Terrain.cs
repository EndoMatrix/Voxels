using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain {

    /// <summary>
    /// Used to create a WorldChunk variable from the rounded components of a
    /// Vector3 input.
    /// </summary>
    /// <param name="block"></param>
    /// <returns>A WorldChunk object of the nearest index</returns>
    public static WorldChunk SelectBlock(Vector3 block) {
        WorldChunk index = new WorldChunk(
            Mathf.RoundToInt(block.x),
            Mathf.RoundToInt(block.y),
            Mathf.RoundToInt(block.z)
        );

        return index;
    }

    public static WorldChunk SelectBlock(RaycastHit rc, bool adjacent = false) {
        Vector3 block = new Vector3(
            MoveWithinBlock(rc.point.x, rc.normal.x, adjacent),
            MoveWithinBlock(rc.point.y, rc.normal.y, adjacent),
            MoveWithinBlock(rc.point.z, rc.normal.z, adjacent)
        );

        return SelectBlock(block);
    }

    static float MoveWithinBlock(float loc, float normal, bool adjacent = false) {
        if (adjacent) {
            loc = loc + normal / 2; // if adjacent, centers on adjacent block
        }
        else {
            loc = loc - normal / 2; // if not adjacent, centers on selected block
        }

        return (float)(loc);
    }

    public static bool SetBlock(RaycastHit rc, Block block, bool adjacent = false) {
        Chunk chunk = rc.collider.GetComponent<Chunk>();

        if (chunk == null) {
            return false;
        }

        WorldChunk index = SelectBlock(rc, adjacent);

        chunk.world.SetBlock(index.x, index.y, index.z, block);

        return true;
    }

    public static Block GetBlock(RaycastHit rc, bool adjacent = false) {
        Chunk chunk = rc.collider.GetComponent<Chunk>();

        if (chunk == null) {
            return null;
        }

        WorldChunk index = SelectBlock(rc, adjacent);

        Block block = chunk.world.GetBlock(index.x, index.y, index.z);

        return block;
    }
}
