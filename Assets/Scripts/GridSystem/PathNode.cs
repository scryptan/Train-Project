using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class PathNode
    {
        private Grid<PathNode> grid;
        private int x;
        private int y;

        public int X => x;
        public int Y => y;

        public Vector3 worldPosIn3d => grid.GetWorldPosition(x, 0, y) + new Vector3(grid.CellSize, 0, grid.CellSize) * 0.5f;
        public Vector3 worldPosIn2d => grid.GetWorldPosition(x, y, 0) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f;
        
        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;
        public PathNode CameFromNode;
        
        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }        
        
        public override string ToString()
        {
            return gCost.ToString();
        }
    }
}