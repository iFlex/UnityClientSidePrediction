using Prediction.data;
using UnityEngine;

namespace Prediction.Interpolation
{
    public interface VisualsInterpolationsProvider
    {
        void Update();
        void Add(uint localTickId, PhysicsStateRecord record);
        void SetInterpolationTarget(Transform t);
    }
}