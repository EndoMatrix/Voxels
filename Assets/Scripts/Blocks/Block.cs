using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as the core block class, from which any block-based objects may be
/// derived. Contains all methods for rendering an individual block.
/// </summary>
/// <remarks>
/// This class stores the surface solidity and texture co-ordinates of the
/// block, and can supply the appropriate vertices for the renderer and
/// collider meshes to the encapsulating chunk.
/// </remarks>
public class Block {

    /// <summary>
    /// Used to describe each of the six faces of the block.
    /// </summary>
    /// <remarks>
    ///   <para>North and South corresponds to the z-axis.</para>
    ///   <para>East and West corresponds to the x-axis.</para>
    ///   <para>Above and Below corresponds to the y-axis.</para>
    /// </remarks>
    public enum Surface {
        NORTH,
        SOUTH,
        WEST,
        EAST,
        ABOVE,
        BELOW
    }

    /// <summary>
    /// Used to describe the texture co-ordinates of the block.
    /// </summary>
    /// <remarks>
    ///   <para>The bottom-left corner of the tile-sheet is defined as the
    ///   origin; and each corner (starting bottom-left) of the respective tile
    ///   is retrieved in a clockwise order (to match the vertices).</para>
    /// </remarks>
    public struct Tile {
        public int x;
        public int y;
    }

    const float tileSize = 0.25f; // inverse # of textures on asset

    // default block constructor
    public Block() {

    }

    // defines solidity of block
    public virtual bool IsSolid(Surface s) {
        switch(s) {
            case Surface.NORTH:
            case Surface.SOUTH:
            case Surface.WEST:
            case Surface.EAST:
            case Surface.ABOVE:
            case Surface.BELOW:
                return true;
        }

        return false;
    }

    // retrieves mesh data
    public virtual MeshChunk MeshBlock(Chunk c, int x, int y, int z, MeshChunk m) {
        m.isShared = true; // uses same mesh for renderer and collider

        // check west-connected block
        if (!c.GetBlock(x - 1, y, z).IsSolid(Surface.EAST)) {
            m = SurfaceWest(c, x, y, z, m);
        }

        // check east-connected block
        if (!c.GetBlock(x + 1, y, z).IsSolid(Surface.WEST)) {
            m = SurfaceEast(c, x, y, z, m);
        }

        // check below-connected block
        if (!c.GetBlock(x, y - 1, z).IsSolid(Surface.ABOVE)) {
            m = SurfaceBelow(c, x, y, z, m);
        }

        // check above-connected block
        if (!c.GetBlock(x, y + 1, z).IsSolid(Surface.BELOW)) {
            m = SurfaceAbove(c, x, y, z, m);
        }

        // check south-connected block
        if (!c.GetBlock(x, y, z - 1).IsSolid(Surface.NORTH)) {
            m = SurfaceSouth(c, x, y, z, m);
        }

        // check north-connected block
        if (!c.GetBlock(x, y, z + 1).IsSolid(Surface.SOUTH)) {
            m = SurfaceNorth(c, x, y, z, m);
        }

        return m;
    }

    // vertices for opposite sides have one inverted axis, and reversed vertex order

    //   6---------5 //
    //  /|        /| //
    // 1-+-------2 | //
    // | |       | | //
    // | 7-------+-4 //
    // |/        |/  //
    // 0---------3   //

    protected virtual MeshChunk SurfaceWest(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f)); // 7
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f)); // 6
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f)); // 1
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f)); // 0

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.WEST));

        return m;
    }

    protected virtual MeshChunk SurfaceEast(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f)); // 3
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f)); // 2
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f)); // 5
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f)); // 4

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.EAST));

        return m;
    }

    protected virtual MeshChunk SurfaceBelow(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f)); // 0
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f)); // 3
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f)); // 4
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f)); // 7

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.BELOW));

        return m;
    }

    protected virtual MeshChunk SurfaceAbove(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f)); // 6
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f)); // 5
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f)); // 2
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f)); // 1

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.ABOVE));

        return m;
    }

    protected virtual MeshChunk SurfaceSouth(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f)); // 0
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f)); // 1
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f)); // 2
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f)); // 3

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.SOUTH));

        return m;
    }

    protected virtual MeshChunk SurfaceNorth(Chunk c, int x, int y, int z, MeshChunk m) {
        m.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f)); // 4
        m.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f)); // 5
        m.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f)); // 6
        m.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f)); // 7

        m.AddSurface();

        m.uv.AddRange(FaceUVs(Surface.NORTH));

        return m;
    }

    // defines tile positions
    public virtual Tile TileSurface(Surface s) {
        Tile tile = new Tile();

        tile.x = 0; // x-position of texture on asset
        tile.y = 0; // y-position of texture on asset

        return tile;
    }

    public virtual Vector2[] FaceUVs(Surface s) {
        Vector2[] UVs = new Vector2[4];

        Tile tile = TileSurface(s); // obtains texture location for face

        // lower-left
        UVs[0] = new Vector2(
            tileSize * tile.x,
            tileSize * tile.y
        );

        // upper-left
        UVs[1] = new Vector2(
            tileSize * tile.x,
            tileSize * tile.y + tileSize
        );

        // upper-right
        UVs[2] = new Vector2(
            tileSize * tile.x + tileSize,
            tileSize * tile.y + tileSize
        );

        // lower-right
        UVs[3] = new Vector2(
            tileSize * tile.x + tileSize,
            tileSize * tile.y
        );

        return UVs;
    }
}
