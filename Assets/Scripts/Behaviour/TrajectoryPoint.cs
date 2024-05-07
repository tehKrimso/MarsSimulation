using UnityEngine;

namespace Behaviour
{
    public class TrajectoryPoint : MonoBehaviour
    {
        public int parentBotId;
        
        //debug
        public bool isCollisionDetected;

        public void SetParentId(int id) => parentBotId = id;
    }
}
