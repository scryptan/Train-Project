using System.Collections.Generic;
using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class GridPlayerMovement : MonoBehaviour
    {
        private List<PathNode> myPath;
        private Vector3 pos;
        [SerializeField] private float moveSpeed = 15;
        [SerializeField] private float rotationSpeed = 15;
        private void Start()
        {
            myPath = new List<PathNode>();
        }

        private void Update()
        {
            if (myPath.Count > 0)
            {
                pos = transform.position;
                var tempPos = pos;
                tempPos.y = 0;
                var temp = myPath[0].worldPosIn3d;
                temp.y = 0;
                if (Vector3.Distance(temp, tempPos) > 0.1f)
                {
                    var dir = myPath[0].worldPosIn3d - pos;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * rotationSpeed);
                    
                    transform.position += dir.normalized * (Time.deltaTime * moveSpeed);
                }
                else  myPath.RemoveAt(0);
            }
        }

        public void SetMyPath(List<PathNode> newPath)
        {
            myPath.AddRange(newPath);
        }

        public Vector3 GetMyCurrentPoint()
        {
            var result = transform.position;
            var temp = result.y;
            result.y = result.z;
            result.z = temp;
            return result;
        }

        public Vector3 GetMyLastPoint()
        {
            if (myPath.Count == 0)
                return GetMyCurrentPoint();
            return myPath[myPath.Count - 1].worldPosIn3d;
        }
    }
}
