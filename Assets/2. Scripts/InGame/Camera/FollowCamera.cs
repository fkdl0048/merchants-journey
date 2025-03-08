using UnityEngine;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.DOMove(new Vector3(
            target.transform.position.x,
            target.transform.position.y,
            transform.position.z), 0.125f);
    }
}
