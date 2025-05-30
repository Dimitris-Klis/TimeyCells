#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectChildrenByParentName : EditorWindow
{
    private string parentName = "";
    private bool selectAll = true;
    private int childIndex = 0;
    private List<GameObject> lastMatchedChildren = new List<GameObject>();

    [MenuItem("Tools/Select Children By Parent Name")]
    public static void ShowWindow()
    {
        GetWindow<SelectChildrenByParentName>("Select Children");
    }

    private void OnGUI()
    {
        GUILayout.Label("Find Children of Parent by Name", EditorStyles.boldLabel);
        parentName = EditorGUILayout.TextField("Parent Name", parentName);

        selectAll = EditorGUILayout.Toggle("Select All Children", selectAll);

        if (!selectAll && lastMatchedChildren.Count > 0)
        {
            childIndex = EditorGUILayout.IntSlider("Child Index", childIndex, 0, lastMatchedChildren.Count - 1);
        }

        if (GUILayout.Button("Select Children"))
        {
            SelectChildren();
        }
    }

    private void SelectChildren()
    {
        lastMatchedChildren.Clear();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == parentName && !EditorUtility.IsPersistent(obj))
            {
                foreach (Transform child in obj.transform)
                {
                    lastMatchedChildren.Add(child.gameObject);
                }
            }
        }

        if (lastMatchedChildren.Count == 0)
        {
            Debug.LogWarning($"No children found for parent named '{parentName}'.");
            Selection.objects = new GameObject[0];
            return;
        }

        if (selectAll)
        {
            Selection.objects = lastMatchedChildren.ToArray();
            Debug.Log($"Selected {lastMatchedChildren.Count} children of '{parentName}'.");
        }
        else
        {
            childIndex = Mathf.Clamp(childIndex, 0, lastMatchedChildren.Count - 1);
            Selection.activeObject = lastMatchedChildren[childIndex];
            Debug.Log($"Selected child at index {childIndex} of parent '{parentName}': {lastMatchedChildren[childIndex].name}");
        }
    }
}
#endif