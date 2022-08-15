using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IInputHelper
    {
        bool TryToPick(out GameObject obj);
        Direction GetSwapDirection();
    }
}
