using UnityEngine;

namespace Prediction.wrappers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PredictedEntityVisuals))]
    public class PredictedNetworkBehaviour : MonoBehaviour
    {
        [SerializeField] private int bufferSize;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PredictedEntityVisuals visuals;
        
        private ClientPredictedEntity _clientPredictedEntity;
        private ServerPredictedEntity _serverPredictedEntity;
        
        void Awake()
        {
            //TODO
            //TODO: use common methods instead of duplicating the code here...
        }
    }
}