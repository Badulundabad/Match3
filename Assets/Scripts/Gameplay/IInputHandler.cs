using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IInputHandler
    {
        bool TryToPick(out GameObject obj);
        Direction GetMoveDirection(Vector3 position);
    }
}
