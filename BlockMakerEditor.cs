# if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockMaker))]
class BlockMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BlockMaker blockMaker = (BlockMaker) target;
        if (GUILayout.Button("Generate Blocks"))
            blockMaker.GenerateBlocks();
    }
}
#endif