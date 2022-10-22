using UnityEngine;

namespace GlobalInterface
{
    public interface IDamgeable {
        void TakeHit(float damage,Vector3 localHitPos);
    }
}