using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuckyballMosaicGenerator))]
public class BuckyballMosaicGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BuckyballMosaicGenerator generator = (BuckyballMosaicGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            generator.GenerateMosaicBuckyball();
        }

        if (GUILayout.Button("Clear"))
        {
            generator.ClearTiles();
        }
    }
}
