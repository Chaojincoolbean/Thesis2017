using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class PolyRandomizer : MonoBehaviour {

	// Use this for initialization
    private Mesh m;
    private MeshFilter mf;
    private Vector3[] original_verts;
    private PolyXData data;
    public int indexToCheck = 0;
    public Vector2 randomizationScale = Vector2.one;
    public float scale = 0.1f;
    public PolyXData.MeshDataPacket[] packets;
    public bool ResetOnRandomize = true;
    public Mesh baseMesh;
    public Texture2D DisplacementMap;
    public bool UseAlpha = false;
    public enum RandomizationScale { Uniform, EdgeLength }
    public enum RandomizationType { Random, RandomAlongNormal }
    public RandomizationScale RandomScale = RandomizationScale.EdgeLength;
    public RandomizationType RandomType;
    public bool OptimizeMesh = false;
    [Range((int)0,(int)3)]
    public int uvChannel = 0;
	public void Initialize () {

        if (ResetOnRandomize)
            ResetVertNorms();
        //lets generate the data if its not loaded
        bool newData = false;
        if (mf == null)
        {
            mf = GetComponent<MeshFilter>();
        }
        if (m == null || m != mf.sharedMesh)
        {
            if (m != null)
            {
                DestroyImmediate(m);
            }
            Mesh m_pre = Instantiate(mf.sharedMesh);
            if (m == null)
            {
                baseMesh = Instantiate(m_pre);
                baseMesh.name = baseMesh.name.Replace("(Clone)", "");
            }
            m = m_pre;
            m.name = m.name.Replace("(Clone)", "");
            mf.mesh = m;
            newData = true;
        }
        if (data == null || newData)
        {
            data = ScriptableObject.CreateInstance<PolyXData>();
            data.Initialize(m, m.vertices, m.GetIndices(0), m.normals);
            packets = data.GetDataBlocks();
            //loaded = true;
        }

	}

    public Mesh GetMesh()
    {
        return m;
    }
    public void SetMesh(Mesh mesh)
    {
        m = mesh;
        mf.sharedMesh = mesh;
    }
    public void RecalculateNormals()
    {
        if (m != null)
        {
            m.RecalculateNormals();
            //caluclate hard normals;
        }
    }
    /*//this code needs to be fixed when we want to convert from smooth to hard shading and vice versa.
    private void CalculateHardVertices()
    {
        int[] indices = m.GetIndices(0);
        m.RecalculateNormals();
        Vector3[] newNormals = new Vector3[m.vertices.Length];
        for (int i = 2; i < indices.Length - 2; i += 3)
        {
            Vector3 t1 = m.vertices[indices[i]];
            Vector3 t2 = m.vertices[indices[i-1]];
            Vector3 t3 = m.vertices[indices[i-2]];
            Vector3 center = (t1 + t2 + t3) / 3;
            //Should be able 3 points only;
            Vector3 norm = Vector3.Cross((t2 - center).normalized, (t3 - center).normalized).normalized * -1;
    
            newNormals[indices[i]] = norm;
            newNormals[indices[i - 1]] = norm;
            newNormals[indices[i - 2]] = norm;
            
        }
        m.normals = newNormals;
    }
     * */
    public void ResetVertNorms()
    {
        if(data != null && m != null)
        {
            if(baseMesh == null)
            {
                baseMesh = Instantiate(m);
            }
            m.vertices = data.ResetVertices(baseMesh.vertices);
            m.normals = data.ResetVertices(baseMesh.normals);
        
        }
    }
    
    public void Randomize()
    {
        //for each data block, randomize its position
        if (ResetOnRandomize)
            ResetVertNorms();
        //lets generate the data if its not loaded
        bool newData = false;
        if (mf == null)
        {
            mf = GetComponent<MeshFilter>();
        }
        if (m == null || m != mf.sharedMesh)
        {
            if(m!=null)
            {
                DestroyImmediate(m);
            }
            Mesh m_pre = Instantiate(mf.sharedMesh);
            if(m == null)
            {
                baseMesh = Instantiate(m_pre);
                baseMesh.name = baseMesh.name.Replace("(Clone)", "");
            }
            m = m_pre;
            m.name = m.name.Replace("(Clone)", "");
            mf.mesh = m;
            newData = true;
        }
        if (data == null || newData)
        {
            data = ScriptableObject.CreateInstance<PolyXData>();
            data.Initialize(m, m.vertices, m.GetIndices(0), m.normals);
            packets = data.GetDataBlocks();
            //loaded = true;
        }

        ///if the registered mesh is not the same, regenerate it
        //lets also use the texture
        
        Vector3[] newVertices = new Vector3[m.vertices.Length];
        float scalar = 1;
        Vector2 uvs;
        for (int i = 0; i < newVertices.Length; ++i)
        {
            newVertices[i] = m.vertices[i];
        }
        //Get Uvs
        Vector3 rDir;
        
        for (int i = 0; i < packets.Length; ++i)
        {
            int[] indices = packets[i].segIndices;
            
            switch(RandomType)
            {
                case RandomizationType.Random:
                    rDir = Random.insideUnitSphere;
                    break;
                case RandomizationType.RandomAlongNormal:
                    rDir = packets[i].avgSplitNormal;
                    break;
                default:
                    rDir = Random.insideUnitSphere;
                    break;

            }
            Vector3 r = rDir * Random.Range(randomizationScale.x, randomizationScale.y);
            if (DisplacementMap != null)
            {
                //to generate an average per vertex cluster
                scalar = 1;
                for (int index = 0; index < indices.Length; ++index)
                {
                    uvs = Vector2.zero;
                    switch(uvChannel)
                    {
                        case 0:
                            if (m.uv.Length != 0)
                                uvs = m.uv[indices[index]];
                            break;
                        case 1:
                            if (m.uv2.Length != 0)
                                uvs = m.uv2[indices[index]];
                            break;
                        case 2:
                            if (m.uv3.Length != 0)
                                uvs = m.uv3[indices[index]];
                            break;
                        case 3:
                            if (m.uv4.Length != 0)
                                uvs = m.uv4[indices[index]];
                            break;
                        default:
                            if (m.uv.Length != 0)
                                uvs = m.uv[indices[index]];
                            break;
                    }
                    //uvs = UVsArray[indices[index]];
                    scalar *= (UseAlpha) ? DisplacementMap.GetPixelBilinear(uvs.x, uvs.y).a : DisplacementMap.GetPixelBilinear(uvs.x, uvs.y).grayscale;

                }
            }
            float vScale = 1;
            switch(RandomScale)
            {
                case RandomizationScale.EdgeLength:
                    vScale = (packets[i].minConnectionDistance / 2);
                    break;
                case RandomizationScale.Uniform:
                    vScale = transform.lossyScale.magnitude;
                    break;
                default:
                    vScale = transform.lossyScale.magnitude;
                    break;
            }

            for (int index = 0; index < indices.Length; ++index)
            {
                
                newVertices[indices[index]] += r * scalar * vScale;
            }
        }
        m.vertices = newVertices;
        
        m.RecalculateBounds();


    }
	
}
