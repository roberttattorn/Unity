using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshLineExtruder : MonoBehaviour
{
    [Header("Source Mesh Settings")]
    public Mesh sourceMesh;       // The 3D segment (e.g., a road piece)
    public float meshLength = 1f; // The length of the source mesh (Z-axis)
    public float roadWidth = 2f;  // Total width of the source mesh

    [Header("Placement Settings")]
    public List<Vector3> pathPoints = new List<Vector3>();
    public float hoverOffset = 0.02f; // Height above terrain to prevent flickering
    public bool lateralLeveling = true;

    [Header("Physics")]
    public bool autoUpdateCollider = true;
    public bool generateColliderOnlyAtEnd = false;

    public void ClearPath()
    {
        pathPoints.Clear();
        GetComponent<MeshFilter>().sharedMesh = null;
        if (GetComponent<MeshCollider>()) GetComponent<MeshCollider>().sharedMesh = null;
    }

    // This is the core function that builds the road
    public void Generate(bool forceCollider = false)
    {
        if (pathPoints.Count < 2 || sourceMesh == null) return;

        CombineInstance[] combine = new CombineInstance[pathPoints.Count - 1];
        
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Vector3 start = pathPoints[i];
            Vector3 end = pathPoints[i + 1];

            // 1. Calculations for Hill-Safety (Sampling Edges)
            Vector3 direction = (end - start).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;

            // Sample 3 points (Left, Center, Right) to find the highest ground
            float hL = GetTerrainHeight(start - right * (roadWidth / 2f));
            float hR = GetTerrainHeight(start + right * (roadWidth / 2f));
            float hC = GetTerrainHeight(start);
            float maxStart = Mathf.Max(hL, Mathf.Max(hR, hC)) + hoverOffset;

            float nhL = GetTerrainHeight(end - right * (roadWidth / 2f));
            float nhR = GetTerrainHeight(end + right * (roadWidth / 2f));
            float nhC = GetTerrainHeight(end);
            float maxEnd = Mathf.Max(nhL, Mathf.Max(nhR, nhC)) + hoverOffset;

            Vector3 pStart = new Vector3(start.x, maxStart, start.z);
            Vector3 pEnd = new Vector3(end.x, maxEnd, end.z);

            // 2. Create Transform Matrix
            float segmentDist = Vector3.Distance(pStart, pEnd);
            Quaternion rotation = lateralLeveling ? 
                Quaternion.LookRotation((pEnd - pStart).normalized, Vector3.up) : 
                Quaternion.LookRotation((pEnd - pStart).normalized);

            combine[i].mesh = sourceMesh;
            combine[i].transform = Matrix4x4.TRS(pStart, rotation, new Vector3(1, 1, segmentDist / meshLength));
        }

        // 3. Create the Final Mesh
        Mesh finalMesh = new Mesh();
        finalMesh.name = "ExtrudedRoadMesh";
        finalMesh.CombineMeshes(combine, true, true);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        // 4. Update Collider Logic
        bool shouldUpdateCollider = forceCollider || (autoUpdateCollider && !generateColliderOnlyAtEnd);
        if (shouldUpdateCollider)
        {
            UpdateCollider(finalMesh);
        }
    }

    public void UpdateCollider(Mesh mesh)
    {
        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc == null) mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = null; // Flush old data
        mc.sharedMesh = mesh;
    }

    private float GetTerrainHeight(Vector3 pos)
    {
        // Fires a ray from the sky downwards to find the ground
        if (Physics.Raycast(new Vector3(pos.x, 2000f, pos.z), Vector3.down, out RaycastHit hit))
        {
            return hit.point.y;
        }
        return pos.y;
    }
}
