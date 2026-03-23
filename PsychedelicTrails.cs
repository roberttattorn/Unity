using UnityEngine;

public class PsychedelicTrails : MonoBehaviour
{
    [Range(0.01f, 0.99f)]
    public float ghostRetention = 0.9f; // Higher = longer trails
    private RenderTexture accumTexture;
    private Material feedbackMat;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
        {
            if (accumTexture != null) accumTexture.Release();
            accumTexture = new RenderTexture(source.width, source.height, 0);
            Graphics.Blit(source, accumTexture);
        }

        if (feedbackMat == null)
        {
            // Make sure the shader name matches exactly
            feedbackMat = new Material(Shader.Find("Hidden/GhostFeedback"));
        }

        // Apply ghostRetention (how much of the NEW frame replaces the old)
        feedbackMat.SetColor("_Color", new Color(1, 1, 1, 1.0f - ghostRetention));

        // THE TRICK: Draw the CURRENT frame onto the HISTORY with transparency
        // This is what creates the "smear"
        Graphics.Blit(source, accumTexture, feedbackMat);

        // Finally, show the accumulated history on the screen
        Graphics.Blit(accumTexture, destination);
    }

    void OnDisable()
    {
        if (accumTexture != null) accumTexture.Release();
    }
}


