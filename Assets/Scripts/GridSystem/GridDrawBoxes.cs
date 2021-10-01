using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class GridDrawBoxes
    {
        private GameObject[,] blocks;
        private BlockSpawner spawner;
        private Grid<PathNode> grid;
        private float cellSize;
        
        public GridDrawBoxes(BlockSpawner spawner, Grid<PathNode> grid, float cellSize)
        {
            blocks = new GameObject[grid.Width, grid.Height];
            this.spawner = spawner;
            this.cellSize = cellSize;
            this.grid = grid;
        }
        
        public void FillBlocks()
        {
            
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
                blocks[x, y] = spawner.SpawnBlock(grid.GridArray[x, y].worldPosIn3d, cellSize);

        }

        public void PaintReachableBlocks(int[,] costs, int maxCost)
        {
            
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var block = blocks[x, y];
                if (grid.GridArray[x, y].isWalkable) block.GetComponent<Renderer>().material.color = Color.black;
                else
                    continue;

                if(costs[x, y] <= maxCost && grid.GridArray[x, y].isWalkable)
                    block.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
        public void PaintUnWalkableBlocks()
        {
            
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var block = blocks[x, y];
                if (grid.GridArray[x, y].isWalkable) 
                    block.GetComponent<Renderer>().material.color = Color.black;;
                if (!grid.GridArray[x, y].isWalkable) 
                    block.GetComponent<Renderer>().material.color = Color.red;;
            }
        }
    }
}
