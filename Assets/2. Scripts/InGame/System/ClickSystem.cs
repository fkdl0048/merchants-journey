using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.InGame.System
{
    public class ClickSystem : MonoBehaviour
    {
        public GameObject GetMouseDownGameobject(string tag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 디버그 레이 그리기 (씬 뷰에서만 보임)
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            Debug.Log($"Mouse Position: {Input.mousePosition}, Ray Origin: {ray.origin}, Ray Direction: {ray.direction}");

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}, Tag: {hit.collider.tag}, Position: {hit.point}");

                // 유닛 선택
                if (hit.collider.CompareTag(tag))
                {
                    Debug.Log("Unit selected");
                    return hit.collider.gameObject;
                }
            }
            else
                Debug.Log("No hit detected");

            return null;
        }
    }
}