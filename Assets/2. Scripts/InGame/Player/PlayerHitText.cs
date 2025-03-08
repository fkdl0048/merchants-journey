using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PlayerScript
{
    public class PlayerHitText : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private TMP_Text text;
        public void HitText()
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(player.position);
            text.transform.position = pos;
            Sequence seq = DOTween.Sequence();
            seq
                .SetAutoKill(true)
                .Append(text.transform.DOScale(1, 0.25f))
                .Append(text.transform.DOScale(0, 0.125f));
        }
    }
}