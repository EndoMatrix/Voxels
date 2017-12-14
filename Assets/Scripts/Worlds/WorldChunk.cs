using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WorldChunk {

    public int x, y, z; // world co-ordinates

    public WorldChunk(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(object input) {
        if (!(input is WorldChunk)) {
            return false;
        }

        WorldChunk index = (WorldChunk)(input);

        if (index.x != x || index.y != y || index.z != z) {
            return false;
        }

        return true;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
