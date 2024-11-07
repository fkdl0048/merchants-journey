using System.Collections;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float unitPerSecond = 1;
    [SerializeField]
    private bool isPlayOnAwake = true;
    [SerializeField]
    private bool isLoop = true;

    private int wayPointCount;
    private int currentIndex = 0;

    private void Awake()
    {
        wayPointCount = wayPoints.Length;
        if (target == null)
            target = transform;
        if (isPlayOnAwake)
            Play();
    }
    private void Update()
    {
        Vector2 direction = wayPoints[currentIndex].position - target.position;
        // 방향에 따라 오브젝트 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));


        //Debug
        for (int i = 0; i < wayPointCount - 1; i++)
        {
            var nextPoint = wayPoints[i + 1];
            Debug.DrawLine(wayPoints[i].position, nextPoint.position, Color.white);
        }
    }

    private void Play()
    {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        var wait = new WaitForSeconds(waitTime);
        while(true)
        {
            yield return StartCoroutine(MoveToB(target.position, wayPoints[currentIndex].position));

            if (currentIndex < wayPointCount - 1)
                currentIndex++;
            else
            {
                if (isLoop)
                    currentIndex = 0;
                else
                    break;
            }
            
            //waitTime 시간 동안 대기
            yield return wait;
        }
    }

    private IEnumerator MoveToB(Vector2 start, Vector2 end)
    {
        float percent = 0;
        float moveTime = Vector2.Distance(start, end) / unitPerSecond;

        while(percent < 1)
        {
            percent += Time.deltaTime / moveTime;
            target.position = Vector2.Lerp(start, end, percent);

            yield return null;
        }
    }
}