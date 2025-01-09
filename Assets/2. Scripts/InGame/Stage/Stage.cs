using Unity.AI.Navigation;
using UnityEngine;

namespace Scripts.InGame.Stage
{
    // 임시 클래스 (사용안하면 아마 삭제 객체지향 때문에 둠 근데 강제 접근이 더 좋을 듯) 
    public class Stage : MonoBehaviour
    {
        [SerializeField] private Cargo cargoPrefab;
        [SerializeField] private NavMeshSurface surface;

        [SerializeField] public bool aiEnable = false;
        private void Awake()
        {
            if(surface != null)
                surface.BuildNavMesh();
        }
    }
}