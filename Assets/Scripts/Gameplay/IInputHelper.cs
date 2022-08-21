using UnityEngine;

namespace Match3.Gameplay
{
    public interface IInputHelper
    {
        bool TryToPick(out GameObject obj);
        Direction GetSwapDirection();
    }
}
