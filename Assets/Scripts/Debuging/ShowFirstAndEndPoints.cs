using System;
using System.Linq;
using TrainProject.Assets.GridSystem;
using UnityEngine;

namespace TrainProject.Assets.Debug
{
    [RequireComponent(typeof(MeshFilter))]
    public class ShowFirstAndEndPoints : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        [SerializeField] [Range(0, 3)] private float radius;

        private void CreateGrid()
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
                return;
            var mesh = meshFilter.sharedMesh;
            var firstPos = mesh.vertices.Last();
            var lastPos = mesh.vertices.First();
            var cellSize = 1;
            var width = Mathf.FloorToInt(Mathf.Abs(lastPos.x - firstPos.x) / cellSize);
            var height = Mathf.FloorToInt(Mathf.Abs(lastPos.z - firstPos.z) / cellSize);
            var grid = new Grid<GridItem>(firstPos, width, height, cellSize, (grid, x, y) => new GridItem
            {
                CellSize = cellSize,
                X = x,
                Y = y
            }, false);

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Gizmos.DrawWireCube(grid.GetCentralizedWorldPosition(x, 0, y), Vector3.one);
                }
            }
        }

        private void OnDrawGizmos()
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
                return;

            var mesh = meshFilter.sharedMesh;
            var firstPos = mesh.vertices.Last();
            var lastPos = mesh.vertices.First();
            Gizmos.DrawSphere(firstPos, radius);
            Gizmos.DrawSphere(lastPos, radius);
            CreateGrid();
        }
    }
}