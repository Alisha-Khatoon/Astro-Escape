using UnityEngine;

public class camera : MonoBehaviour
{
    private Board board;
    public float cameraOffset = 10f;
    public float aspectRatio = 0.625f;
    public float padding = 2f;

    void Start()
    {
        board = FindObjectOfType<Board>();
        if (board != null)
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPosition = new Vector3(x / 2f, y / 2f, -cameraOffset);
        transform.position = tempPosition;

        float orthoSize;
        if (board.width >= board.height)
        {
            orthoSize = (board.width / 2f + padding) / aspectRatio;
        }
        else
        {
            orthoSize = (board.height / 2f + padding);
        }

        Camera.main.orthographicSize = orthoSize;

        Debug.Log("Camera Position: " + tempPosition);
        Debug.Log("Camera Orthographic Size: " + Camera.main.orthographicSize);
    }
}
