using Prediction.Interpolation;
using UnityEngine;

namespace Prediction.wrappers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PredictedEntityVisuals))]
    public class PredictedMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private int bufferSize;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PredictedEntityVisuals visuals;
        
        private ClientPredictedEntity _clientPredictedEntity;
        private ServerPredictedEntity _serverPredictedEntity;
        
        //TODO - wrap the prediction entities and configure
        void Awake()
        {
            //TODO: check visuals must be a child of this game object
        }

        void ConfigureAsServer()
        {
            //TODO: detect or wire components
            _serverPredictedEntity = new ServerPredictedEntity(bufferSize, _rigidbody, visuals.gameObject, new PredictableControllableComponent[0], new PredictableComponent[0]);
        }

        void ConfigureAsClient(bool controlledLocally)
        {
            //TODO: detect or wire components
            _clientPredictedEntity = new ClientPredictedEntity(30, _rigidbody, visuals.gameObject, new PredictableControllableComponent[0]{}, new PredictableComponent[0]{});
            _clientPredictedEntity.gameObject = gameObject;
            //TODO: configurable
            _clientPredictedEntity.interpolationsProvider = new MovingAverageInterpolator();
            visuals.SetClientPredictedEntity(_clientPredictedEntity, visuals);
        }
    }
}