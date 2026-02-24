using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshLineExtruder))]
public class MeshLineExtruderEditor : Editor
{
    void OnSceneGUI()
    {
        MeshLineExtruder script = (MeshLineExtruder)target;
        Event e = Event.current;

        // Shift + Left Click to draw the road
        if (e.type == EventType.MouseDown && e.button == 0 && e.shift)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Undo.RecordObject(script, "Add Road Point");
                script.pathPoints.Add(hit.point);
                script.Generate();
                e.Use();
            }
        }

        // Handles for moving points manually
        for (int i = 0; i < script.pathPoints.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(script.pathPoints[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Move Point");
                script.pathPoints[i] = newPos;
                script.Generate();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        MeshLineExtruder script = (MeshLineExtruder)target;

        EditorGUILayout.HelpBox("Shift + Click in Scene to add points. The mesh will automatically stay above sloped terrain.", MessageType.Info);
        
        DrawDefaultInspector();

        if (GUILayout.Button("Manual Re-Generate Mesh")) script.Generate();

        if (GUILayout.Button("Finalize & Bake Collider"))
        {
            script.Generate(true);
            Debug.Log("Collider Baked Successfully!");
        }

        if (GUILayout.Button("Clear All")) script.ClearPath();
    }
}
