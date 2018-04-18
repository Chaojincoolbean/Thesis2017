using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PolyMenu
{
    [MenuItem("Tools/2Ginge/LowPolyRandomizer/Select Randomized")]
    private static void SelectRandomPoly()
    {
        //basically we need to find all PolyRandomizer.cs in selection then make the selection THAT
        Transform[] ts = Selection.transforms;
        //incase we have nothing selected, lets select EVERYTHING that has the script on it.
        if(ts.Length <= 0)
        {
            ts = GameObject.FindObjectsOfType<Transform>();
        }
        List<GameObject> allPolys = new List<GameObject>();
        for (int i = 0; i < ts.Length; ++i)
        {
            PolyRandomizer[] prs = ts[i].GetComponentsInChildren<PolyRandomizer>();
            for (int j = 0; j < prs.Length; ++j)
            {
                if (allPolys.Contains(prs[j].gameObject))
                {
                    continue;
                }
                allPolys.Add(prs[j].gameObject);
            }
        }

        Selection.objects = allPolys.ToArray();
    }
    [MenuItem("Tools/2Ginge/LowPolyRandomizer/Select Meshes")]
    private static void SelectItemsRandoms()
    {
        //basically we need to find all PolyRandomizer.cs in selection then make the selection THAT
        Transform[] ts = Selection.transforms;
        List<GameObject> allPolys = new List<GameObject>();
        for (int i = 0; i < ts.Length; ++i)
        {
            MeshFilter[] prs = ts[i].GetComponentsInChildren<MeshFilter>();
            for (int j = 0; j < prs.Length; ++j)
            {
                if (allPolys.Contains(prs[j].gameObject))
                {
                    continue;
                }
                allPolys.Add(prs[j].gameObject);
            }
        }

        Selection.objects = allPolys.ToArray();
    }
}