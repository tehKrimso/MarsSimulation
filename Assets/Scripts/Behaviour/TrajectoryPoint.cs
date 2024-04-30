using UnityEngine;

namespace Behaviour
{
    public class TrajectoryPoint : MonoBehaviour
    {
        public int parentBotId;

        public void SetParentId(int id) => parentBotId = id;
    }
}
