﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button ("Generate"))
        {
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("Run Seeds"))
        {
            for(int i = 0; i<= 100; i++)
            {
                mapGen.seed = i;
                mapGen.GenerateMap();
            }
        }      
    }
}
