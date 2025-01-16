using System.Linq;
using UnityEngine;

namespace Scripts.InGame.System
{
    public class ClickSystem : MonoBehaviour
    {
        public GameObject GetMouseDownGameobject(string tag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = Physics.RaycastAll(ray);

            var targetObj = hit.Where(x => x.transform.CompareTag(tag));
            if (targetObj == null)
                return null;
            return targetObj.ToArray()[0].transform.gameObject;
        }
    }
}