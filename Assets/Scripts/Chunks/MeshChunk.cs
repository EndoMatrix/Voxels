using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChunk {

    public List<Vector2> uv = new List<Vector2>(); // uv co-ordinates list

    public List<Vector3> vx = new List<Vector3>(); // vertex co-ordinates list
    public List<Vector3> vc = new List<Vector3>(); // vertex collision list

    public List<int> tx = new List<int>();         // triangle co-ordinates list
    public List<int> tc = new List<int>();         // triangle collision list

    public bool isShared; // same mesh for renderer and collider

    public MeshChunk() {

    }

    public void AddSurface() {
        tx.Add(vx.Count - 4); // 0
        tx.Add(vx.Count - 3); // 1
        tx.Add(vx.Count - 2); // 2
        tx.Add(vx.Count - 4); // 0
        tx.Add(vx.Count - 2); // 2
        tx.Add(vx.Count - 1); // 3

        // adds equivalent collision triangles if true
        if (isShared) {
            tc.Add(vc.Count - 4); // 0
            tc.Add(vc.Count - 3); // 1
            tc.Add(vc.Count - 2); // 2
            tc.Add(vc.Count - 4); // 0
            tc.Add(vc.Count - 2); // 2
            tc.Add(vc.Count - 1); // 3
        }
    }

    public void AddVertex(Vector3 vertex) {
        vx.Add(vertex);

        // adds equivalent collision vertices if true 
        if (isShared) {
            vc.Add(vertex);
        }
    }

    public void AddTriangle(int triangle) {
        tx.Add(triangle);

        // adds equivalent collision triangles if true
        if (isShared) {
            tc.Add(triangle - (vx.Count - vc.Count)); // adjusts for vertex indices
        }
    }
}
