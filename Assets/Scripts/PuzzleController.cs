using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    public delegate void PlayerMoved(Vector3 position);
    public static event PlayerMoved OnPlayerMoved;

    [SerializeField] private GridController _gridController;
    [SerializeField] private Transform _cellHighlight;
    [SerializeField] private KeyCode _up = KeyCode.W;
    [SerializeField] private KeyCode _down = KeyCode.S;
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;

    void Update()
    {
        if (Input.GetKeyDown(_up))
        {
            Move(Vector3.up);
        }
        if (Input.GetKeyDown(_down))
        {
            Move(Vector3.down);
        }
        if (Input.GetKeyDown(_left))
        {
            Move(Vector3.left);
        }
        if (Input.GetKeyDown(_right))
        {
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {
        // keeping it within the grid
        if (_cellHighlight.position.x + direction.x < 0
            || _cellHighlight.position.y + direction.y < 0
            || _cellHighlight.position.x + direction.x > _gridController.GridSize.x - 1
            || _cellHighlight.position.y + direction.y > _gridController.GridSize.y - 1)
        {
            return;
        }

        _cellHighlight.position += direction;
        OnPlayerMoved?.Invoke(_cellHighlight.position);
    }

}
