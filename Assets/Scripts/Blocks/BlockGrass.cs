using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrass : Block {

    public BlockGrass() : base() {

    }

    public override Tile TileSurface(Surface s) {

        Tile tile = new Tile();

        switch(s) {
            case Surface.ABOVE:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Surface.BELOW:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }

        tile.x = 3;
        tile.y = 0;
        return tile;
    }
}
