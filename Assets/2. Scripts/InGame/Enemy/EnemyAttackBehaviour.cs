using PlayerScript;
using System.Collections;
using UnityEngine;

namespace EnemyScript
{
    public class EnemyAttackBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform hitBox;
        [SerializeField] private PlayerHitText debugText;
        private bool isComplete = true;

        public void Attack()
        {
            if (isComplete == false)
                return;

            StartCoroutine(Delay());
        }
        IEnumerator Delay()
        {
            //hitBox.gameObject.SetActive(true);
            Collider2D collider = Physics2D.OverlapBox(hitBox.position, hitBox.transform.localScale, 0, LayerMask.GetMask("Player"));
            if(collider != null)
            {
                Debug.Log(collider.gameObject.name);
                debugText.HitText();
            }
            isComplete = false;

            yield return new WaitForSeconds(1.5f);

            //hitBox.gameObject.SetActive(false);
            isComplete = true;
        }
    }
}