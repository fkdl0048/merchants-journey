using UnityEngine;


namespace ObjectAI
{
    public enum FSM
    {
        Idle,       //화물을 호위하는, 또는 화물에 접근하고 있는 상태
        Encounter,  //적을 맞닥뜨린 상태
        Fight,      //사정거리 안으로 들어온 상태, 공격을 준비한다.
        Attack,     //공격 애니메이션, 공격을 완료한 후, Idle 상태로 전이
        Dead,       //사망 상태
    }
    public class ObjectAI : MonoBehaviour
    {
        [SerializeField] ObjectStatus status;
        [SerializeField] FSM fsm;
    }
}
