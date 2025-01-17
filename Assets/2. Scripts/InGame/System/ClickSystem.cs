using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.InGame.System
{
    public class ClickSystem : MonoBehaviour
    {
        public GameObject GetMouseDownGameobject(string tag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitUnit = Physics.RaycastAll(ray)
                .Where(x => x.collider.CompareTag(tag)).ToArray();
            if (hitUnit.Length == 0)
                return null;
            //광선에 맞은 첫번째 unit을 색적
            return hitUnit[0].collider.gameObject;
        }
    }
}