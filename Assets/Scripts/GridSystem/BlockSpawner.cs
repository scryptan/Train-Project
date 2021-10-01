using UnityEngine;

namespace TrainProject.Assets.GridSystem
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject blockPrefab;
        
        public GameObject SpawnBlock(Vector3 position, float cellSize)
        {
            var result = Instantiate(blockPrefab, position, Quaternion.identity);
            result.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);
            return result;
        }
    }
}
