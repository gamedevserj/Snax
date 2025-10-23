using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private GameObject _cellPrefab;

    // serializing it so we can clean it after play mode, otherwise we would have to delete cells by hand
    [SerializeField, HideInInspector] private List<GameObject> _cells;
    public Vector2Int GridSize => _gridSize;

    [ContextMenu("Create grid")]
    private void CreateGrid()
    {
        if (_cells != null)
        {
            foreach (var cell in _cells)
            {
                DestroyImmediate(cell);
            }
        }

        _cells = new();
        for (int i = 0; i < _gridSize.x; i++)
        {
            for (int k = 0; k < _gridSize.y; k++)
            {
                var cell = GameObject.Instantiate(_cellPrefab, new Vector3(i, k, 0), Quaternion.identity, transform);
                cell.SetActive(true);
                _cells.Add(cell);
            }
        }
    }
}
