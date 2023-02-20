using System;
using UnityEngine;

public class BlockMaker : MonoBehaviour
{
    [SerializeField] Collider original;
    [SerializeField] GameObject blockPrototype;
    [SerializeField] float blockScale = 1f;
    [SerializeField] float breakForce = 1000f;

    public void GenerateBlocks()
    {
        if (blockPrototype == null) return;

        Collider originalCollider = original.GetComponent<Collider>();
        if (originalCollider == null) return;

        original.gameObject.SetActive(true);
        DestroyChildren();

        Bounds bounds = originalCollider.bounds;
        int xCount = Mathf.RoundToInt(bounds.size.x / blockScale);
        int yCount = Mathf.RoundToInt(bounds.size.y / blockScale);
        int zCount = Mathf.RoundToInt(bounds.size.z / blockScale);

        var blocks = new GameObject[xCount, yCount, zCount];
        Vector3 scale = Vector3.one * blockScale;

        for (int x = 0; x < xCount; x++) // X dimenzió
        for (int y = 0; y < yCount; y++) // Y dimenzió
        for (int z = 0; z < zCount; z++) // Z dimenzió
        {
            Vector3 position = bounds.min + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) * blockScale;
            bool inside = Physics.CheckBox(position, scale / 2f);
            if (!inside) continue;

            GameObject block = Instantiate(blockPrototype, transform);
            block.transform.position = position;
            block.transform.localScale = scale;
            blocks[x, y, z] = block;
        }

        for (int x = 0; x < xCount; x++)
        for (int y = 0; y < yCount; y++)
        for (int z = 0; z < zCount; z++)
        {
            GameObject block = blocks[x, y, z];
            if (x > 0)
                TryConnectWithFixJoint(block, blocks[x - 1, y, z]);
            if (y > 0)
                TryConnectWithFixJoint(block, blocks[x, y - 1, z]);
            if (z > 0)
                TryConnectWithFixJoint(block, blocks[x, y, z - 1]);
        }

        original.gameObject.SetActive(false);
    }

    void TryConnectWithFixJoint(GameObject block, GameObject block2)
    {
        if (block != null && block2 != null)
        {
            FixedJoint joint = block.AddComponent<FixedJoint>();
            joint.connectedBody = block2.GetComponent<Rigidbody>();
            joint.breakForce = breakForce;
        }
    }
    
    public void DestroyChildren()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            try
            {
                DestroyImmediate(child.gameObject);
            }
            catch (InvalidOperationException) { }
        }
    }
}