using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PolyRandomizer))]
[CanEditMultipleObjects]
public class PolyEditor : Editor
{
    private SerializedProperty fxn;
    private SerializedProperty randomizedAmount;
    //private SerializedProperty reset;
    private SerializedProperty baseMesh;
    private SerializedProperty sampleTex;
    private SerializedProperty alpha;
    private SerializedProperty randomScale;
    private SerializedProperty randomType;
    private SerializedProperty optimize;
    private SerializedProperty uvChan;
    void OnEnable()
    {
        //fxn = serializedObject.FindProperty("")
        randomizedAmount = serializedObject.FindProperty("randomizationScale");
        //reset = serializedObject.FindProperty("ResetOnRandomize");
        baseMesh = serializedObject.FindProperty("baseMesh");
        sampleTex = serializedObject.FindProperty("DisplacementMap");
        alpha = serializedObject.FindProperty("UseAlpha");
        randomScale = serializedObject.FindProperty("RandomScale");
        randomType = serializedObject.FindProperty("RandomType");
        optimize = serializedObject.FindProperty("OptimizeMesh");
        uvChan = serializedObject.FindProperty("uvChannel");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //The scale at which the randomization should occur.
        EditorGUILayout.PropertyField(randomizedAmount, new GUIContent("Randomization Scale"));
        //Also known as the limits or the range clamp, dunno I am a fan of limsip.
        EditorGUILayout.PropertyField(randomScale, new GUIContent("Scale Method: "));
        //which method do we randomize by?
        EditorGUILayout.PropertyField(randomType, new GUIContent("Type Method: "));
       

        for (int i = 0; i < serializedObject.targetObjects.Length; ++i)
        {
            PolyRandomizer pr = serializedObject.targetObjects[i] as PolyRandomizer;
            if (pr.baseMesh == null)
                pr.Initialize();
        }
        EditorGUILayout.PropertyField(baseMesh, new GUIContent("Base Mesh"));
        //Really the big bad "ART" Button.
        if(GUILayout.Button("Randomize", GUILayout.Height(40)))
        {
            for (int i = 0; i < serializedObject.targetObjects.Length; ++i)
            {
                PolyRandomizer pr = serializedObject.targetObjects[i] as PolyRandomizer;
                pr.Randomize();
            }
        }
        
        EditorGUILayout.BeginHorizontal();
        //recalculates the normals.
        if (GUILayout.Button("Recalc-Normals"))
        {
            for (int i = 0; i < serializedObject.targetObjects.Length; ++i)
            {
                PolyRandomizer pr = serializedObject.targetObjects[i] as PolyRandomizer;
                pr.RecalculateNormals();
            }
        }
        //resets part of hte mesh buffer.
        if (GUILayout.Button("Reset"))
        {
            for (int i = 0; i < serializedObject.targetObjects.Length; ++i)
            {
                PolyRandomizer pr = serializedObject.targetObjects[i] as PolyRandomizer;
                pr.ResetVertNorms();
            }
        }
        //Saves out the meshes such that they can be used as prefabs.
        if (GUILayout.Button("Save Mesh(es)"))
        {
            string path = EditorUtility.SaveFilePanel("Save Mesh", Application.dataPath, "Mesh", "asset");
            if (path != "")
            {
                path = "Assets" + path.Replace(Application.dataPath, "");

                for (int i = 0; i < serializedObject.targetObjects.Length; ++i)
                {
                    string tPath = path;
                    PolyRandomizer pr = serializedObject.targetObjects[i] as PolyRandomizer;
                    Mesh tMesh = Instantiate(pr.GetMesh());
                    if (pr.OptimizeMesh)
                        MeshUtility.Optimize(tMesh);
                    pr.SetMesh(tMesh);


                    string str = "_" + i.ToString() + ".";
                    tPath = tPath.Replace(".", str);
                    try
                    {
                        AssetDatabase.CreateAsset(tMesh, tPath);
                    }
                    catch
                    {
                        AssetDatabase.DeleteAsset(tPath);
                        AssetDatabase.CreateAsset(tMesh, tPath);
                    }

                    //AssetDatabase.
                    AssetDatabase.SaveAssets();
                }
                AssetDatabase.Refresh();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(optimize, GUIContent.none, GUILayout.MaxWidth(10));
        GUILayout.Label(("Optimize? (on Save)"));

        EditorGUILayout.EndHorizontal();
        //incase there are specific vertices that are to be altered.
        EditorGUILayout.PropertyField(sampleTex, new GUIContent("Displacement Map"));
        //is the texture being used?
        if (sampleTex.objectReferenceValue != null)
        {
            //which uv channel does the texture use?
            EditorGUILayout.PropertyField(uvChan, new GUIContent("uvChannel"));
            // or do we only care about the alpha? eh? One of us, One of us.... 
            EditorGUILayout.PropertyField(alpha, new GUIContent("Use Alpha?"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
