using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PolyXData : ScriptableObject {

	//holds on to inverted vertex lists and all of hte kind.
    //lets first fill it with vertices then inverted index lists
    //for reconstruction
    #region VARIABLES

    Vector3[] vertices;
    Vector3[] normals;
    ArrayData[] inverted;
    Hashtable indexHash;
    int[] indices;
    private Mesh registeredMesh;
    private Vector3[] originalVertices;
    
    #endregion VARIABLES
    //helper 
    [System.Serializable]
    private class ArrayData
    {
        //these connections are considered clusters of vertices (vertex splits).
        public int[] connections;
        public List<int> tmpConnections;
        public List<int> tmpAdjacentIndices;
        //these are teh connected indices to those vertex splits.
        public int[] adjacentIndices;
        public float maxConnectionDistance;
        public float minConnectionDistance;
        public void UpdateArray()
        {
            connections = tmpConnections.ToArray();
        }
        public void UpdateAdjacents()
        {
            adjacentIndices = tmpAdjacentIndices.ToArray();
        }
    }
    //Served class
    [System.Serializable]
    public class MeshDataPacket
    {
        public int[] segIndices;
        private Vector3 segVertex;
        public Vector3 avgSplitNormal;
        public float maxConnectionDistance;
        public float minConnectionDistance;
    }
    public Vector3[] ResetVertices()
    {
        Vector3[] nVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; ++i)
            nVertices[i] = originalVertices[i];
        return nVertices;
    }
    public Vector3[] ResetVertices(Vector3[] mverts)
    {
        Vector3[] nVertices = new Vector3[mverts.Length];
        for (int i = 0; i < mverts.Length; ++i)
            nVertices[i] = mverts[i];
        return nVertices;
    }
    public int[] GetSplitIndices(int index)
    {
        return inverted[index].connections;
    }
    public int[] GetSplitAdjacent(int index)
    {
        return inverted[index].adjacentIndices;
    }
    public MeshDataPacket[] GetDataBlocks()
    {
        List<MeshDataPacket> packets = new List<MeshDataPacket>();
        for(int i = 0; i < inverted.Length; ++i)
        {
            if(inverted[i].connections.Length > 0)
            {
                MeshDataPacket packet = new MeshDataPacket();
                packet.avgSplitNormal = GetSplitNormal(i);
                packet.segIndices = inverted[i].connections;
                packet.minConnectionDistance = inverted[i].minConnectionDistance;
                packet.maxConnectionDistance = inverted[i].maxConnectionDistance;
                //packet.
                packets.Add(packet);
            }
        }
        return packets.ToArray();
    }
    //to calculate the normal of a vertex cluster.
    public Vector3 GetSplitNormal(int index)
    {
        Vector3 n = Vector3.zero;
        if (inverted.Length > index)
        { 
            for (int i = 0; i < inverted[index].connections.Length; ++i )
            {
                
                n += normals[inverted[index].connections[i]];
            }
            
        }


        return n.normalized ;
    }
    public int GetIndex(Vector3 vertex)
    {
        if (indexHash.Contains(vertex))
            return (int)indexHash[vertex];
        return -1;

    }
    public Mesh GetRegisteredMesh()
    {
        return registeredMesh;
    }
    public void SetRegisteredMesh(Mesh mesh)
    {
        registeredMesh = mesh;
    }
    public void Initialize(Mesh m, Vector3[] _vertices, int[] _indices, Vector3[] _normals)
    {
        indices = _indices;
        vertices = _vertices;
        normals = _normals;
        registeredMesh = m;
        originalVertices = new Vector3[vertices.Length];
        for (int i = 0; i < originalVertices.Length; ++ i )
        {
            originalVertices[i] = vertices[i];
        }
        GenerateInvertedIndices();
    }
    private void CleanUp()
    {

    }
    private void GenerateConnections()
    {
        //take get all vertices that are in the 'split'
        //then all of the index connections

        ICollection values = indexHash.Values;
        foreach (int v in values)
        {
            Vector3 origin = vertices[v];
            float min = float.MaxValue;
            float max = 0.0f;
            int[] splits = GetSplitIndices(v);
            for (int i = 0; i < splits.Length; ++i)
            {
                int[] adj = GetSplitAdjacent(splits[i]);
                for (int j = 0; j < adj.Length; ++j)
                {
                    Vector3 pos = vertices[adj[j]];
                    //Vector3 pos = transform.TransformPoint(m.vertices[adj[j]]);
                    float t = Vector3.Distance(pos, origin);
                    if (t < min)
                        min = t;
                    if (t > max)
                        max = t;
                }
            }
            inverted[v].maxConnectionDistance = max;
            inverted[v].minConnectionDistance = min;
        }


        
    }
    private void GenerateInvertedIndices()
    {
        indexHash = new Hashtable();
        inverted = new ArrayData[vertices.Length];
        for (int i = 0; i < vertices.Length; ++i)
        {
            if(!indexHash.ContainsKey(vertices[i]))
                indexHash.Add(vertices[i], i);
            inverted[i] = new ArrayData();
            if (inverted[i].tmpConnections == null)
            {
                inverted[i].tmpConnections = new List<int>();
            }
            if (inverted[i].tmpAdjacentIndices == null)
            {
                inverted[i].tmpAdjacentIndices = new List<int>();
            }
           
        }
        //could probably do this on the fly in the previous loop
        for (int i = 0; i < vertices.Length; ++i)
        {
            if(indexHash.ContainsKey(vertices[i]))
            {
                int index = (int)indexHash[vertices[i]];
                inverted[index].tmpConnections.Add(i);
            }
        }
        for (int i = 0; i < indices.Length; i+=3 )
        {
            //for each index add to the adj
            inverted[indices[i]].tmpAdjacentIndices.Add(indices[i + 1]);
            inverted[indices[i]].tmpAdjacentIndices.Add(indices[i + 2]);

            inverted[indices[i+1]].tmpAdjacentIndices.Add(indices[i + 0]);
            inverted[indices[i+1]].tmpAdjacentIndices.Add(indices[i + 2]);

            inverted[indices[i+2]].tmpAdjacentIndices.Add(indices[i + 1]);
            inverted[indices[i+2]].tmpAdjacentIndices.Add(indices[i + 0]);
        }
        for (int i = 0; i < vertices.Length; ++i)
        {
            inverted[i].UpdateArray();
            inverted[i].UpdateAdjacents();
        }
        GenerateConnections();
        

    }
}
