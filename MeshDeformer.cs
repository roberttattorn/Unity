using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    [Range(0.1f, 2f)] public float deformRadius = 0.5f; // How wide the dent is
    [Range(0.1f, 2f)] public float maxDeform = 0.5f;    // Max depth of a single dent
    public float damageMultiplier = 1f;                // Overall sensitivity
    public float minVelocity = 2f;                     // Ignore light taps

    private Mesh mesh;
    private Vector3[] originalVertices, modifiedVertices;

    void Start()
    {
        // Clone the mesh so we don't modify the original asset file
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only deform if the impact is hard enough
        if (collision.relativeVelocity.magnitude > minVelocity)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                // Convert world impact point to the object's local space
                Vector3 point = transform.InverseTransformPoint(contact.point);
                Vector3 velocity = transform.InverseTransformDirection(collision.relativeVelocity);
                
                DeformMesh(point, velocity * 0.01f * damageMultiplier);
            }
        }
    }

    void DeformMesh(Vector3 contactPoint, Vector3 impactVelocity)
    {
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            float distance = Vector3.Distance(contactPoint, modifiedVertices[i]);

            if (distance < deformRadius)
            {
                // Push vertex based on proximity to impact and force
                float falloff = 1f - (distance / deformRadius);
                modifiedVertices[i] += impactVelocity * falloff;

                // Clamp deformation so it doesn't look like a mess
                Vector3 totalDeform = modifiedVertices[i] - originalVertices[i];
                if (totalDeform.magnitude > maxDeform)
                {
                    modifiedVertices[i] = originalVertices[i] + (totalDeform.normalized * maxDeform);
                }
            }
        }

        // Apply changes to the mesh
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Optional: Update the collider so physics stays accurate
        if (GetComponent<MeshCollider>())
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
