using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAir : Block {

    public BlockAir() : base() {

    }

    public override MeshChunk MeshBlock(Chunk c, int x, int y, int z, MeshChunk m) {
        return m;
    }

    public override bool IsSolid(Surface s) {
        return false; // no solid surfaces
    }
}
