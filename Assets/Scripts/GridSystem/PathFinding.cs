using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class PathFinding
    {
        private const int MOVE_DIAGONAL_COST = 15;
        private const int MOVE_STRAIGHT_COST = 10;
        
        private readonly Grid<PathNode> grid;
        private List<PathNode> openList;
        private List<PathNode> closeList;

        public Grid<PathNode> Grid => grid;
        
        public PathFinding(Vector3 originPos, int width, int height, float cellSize)
        {
            grid = new Grid<PathNode>(originPos, width, height, cellSize, (g, x ,y) => new PathNode(g, x, y));
        }

        public int[,] CalculateAllCosts(int startX, int startY)
        {
            var costs = new int[grid.Width, grid.Height];
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                costs[x, y] = CalculateDistanceCost(grid.GetGridObject(startX, startY), grid.GetGridObject(x, y));
            }
            return costs;
        }
        
        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            var startNode = grid.GetGridObject(startX,  startY);
            var endNode = grid.GetGridObject(endX, endY);
            if (endNode == default)
                return null;
            openList = new List<PathNode>{startNode};
            closeList = new List<PathNode>();
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var pathNode = grid.GetGridObject(x,  y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.CameFromNode = null;
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if(closeList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        closeList.Add(neighbourNode);
                        continue;
                    }
                    
                    var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        
                        if(!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            var neighbourList = new List<PathNode>();
            if (currentNode.X - 1 >= 0)
            {
                neighbourList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y));
                if(currentNode.Y - 1 >= 0) 
                    neighbourList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y - 1));
                if(currentNode.Y + 1 < grid.Height) 
                    neighbourList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y + 1));
            }
            
            if (currentNode.X + 1 < grid.Width)
            {
                neighbourList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y));
                if(currentNode.Y - 1 >= 0) 
                    neighbourList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y - 1));
                if(currentNode.Y + 1 < grid.Height) 
                    neighbourList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y + 1));
            }
            
            if(currentNode.Y - 1 >= 0) 
                neighbourList.Add(grid.GetGridObject(currentNode.X, currentNode.Y - 1));
            if(currentNode.Y + 1 < grid.Height) 
                neighbourList.Add(grid.GetGridObject(currentNode.X, currentNode.Y + 1));
            return neighbourList;
        }
        
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            var path = new List<PathNode>();
            path.Add(endNode);
            var currentNode = endNode;
            while (currentNode.CameFromNode != null)
            {   
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }

            path.Reverse();
            return path;
        }
        
        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            var xDistance = Mathf.Abs(a.X - b.X);
            var yDistance = Mathf.Abs(a.Y - b.Y);
            var reamining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * reamining;
        }

        private PathNode GetLowestFCostNode(IReadOnlyList<PathNode> pathNodes)
        {
            var lowestPathNodes = pathNodes[0];
            foreach (var pathNode in pathNodes.Where(pathNode => pathNode.fCost < lowestPathNodes.fCost))
                lowestPathNodes = pathNode;
            return lowestPathNodes;
        }
    }
}
