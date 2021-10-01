using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class Grid<TGridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public TGridObject[,] GridArray => _gridArray;

        private readonly int _width;
        private readonly int _height;
        private readonly TGridObject[,] _gridArray;
        private readonly float _cellSize;
        private readonly TextMesh[,] _debugTextMeshArray;
        private readonly Vector3 _originPosition;

        public Grid(Vector3 originPosition, int width, int height, float cellSize,
            Func<Grid<TGridObject>, int, int, TGridObject> createObject, bool showDebug = true)
        {
            _height = height;
            _width = width;
            _cellSize = cellSize;
            _originPosition = originPosition;

            _gridArray = new TGridObject[width, height];
            _debugTextMeshArray = new TextMesh[width, height];

            for (var x = 0; x < _gridArray.GetLength(0); x++)
            for (var y = 0; y < _gridArray.GetLength(1); y++)
                _gridArray[x, y] = createObject(this, x, y);

            if (showDebug)
            {
                for (var x = 0; x < _gridArray.GetLength(0); x++)
                for (var y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _debugTextMeshArray[x, y] = UtilsClass.CreateWorldText(_gridArray[x, y]?.ToString(), null,
                        GetWorldPosition(x, y, 0) + new Vector3(cellSize, cellSize) * 0.5f,
                        30, Color.red, TextAnchor.MiddleCenter);
                    UnityEngine.Debug.DrawLine(GetWorldPosition(x, y, 0), GetWorldPosition(x, y + 1, 0), Color.red,
                        100f);
                    UnityEngine.Debug.DrawLine(GetWorldPosition(x, y, 0), GetWorldPosition(x + 1, y, 0), Color.red,
                        100f);
                }

                UnityEngine.Debug.DrawLine(GetWorldPosition(0, height, 0), GetWorldPosition(width, height, 0),
                    Color.red, 100f);
                UnityEngine.Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, height, 0),
                    Color.red, 100f);
            }
        }

        public Vector3 GetWorldPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * _cellSize + _originPosition;
        }

        public Vector3 GetCentralizedWorldPosition(int x, int y, int z)
        {
            var point = new Vector3(x, y, z);
            return point * _cellSize + _originPosition + new Vector3(_cellSize / 2, 0, _cellSize / 2);
        }

        public void GetXyz(Vector3 worldPosition, out int x, out int y)
        {
            worldPosition -= _originPosition;
            x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            y = Mathf.FloorToInt(worldPosition.y / _cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
                _debugTextMeshArray[x, y].text = _gridArray[x, y].ToString();
                if (OnGridObjectChanged != null)
                    OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {X = x, Y = y});
            }
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridObjectChanged != null)
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {X = x, Y = y});
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXyz(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
                return _gridArray[x, y];
            return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXyz(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
    }
}