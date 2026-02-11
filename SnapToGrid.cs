public float gridSize = 1.0f;

void Update() {
    Vector3 currentPos = transform.position;
    transform.position = new Vector3(
        Mathf.Round(currentPos.x / gridSize) * gridSize,
        Mathf.Round(currentPos.y / gridSize) * gridSize,
        Mathf.Round(currentPos.z / gridSize) * gridSize
    );
}
