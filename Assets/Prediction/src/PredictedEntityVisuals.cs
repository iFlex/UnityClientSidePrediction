using Prediction.data;
using Prediction.Interpolation;
using UnityEngine;

namespace Prediction
{
    public class PredictedEntityVisuals : MonoBehaviour
    {
        [SerializeField] public GameObject visualsEntity;
        [SerializeField] private bool debug = false;
        [SerializeField] private GameObject serverGhostPrefab;
        [SerializeField] private GameObject clientGhostPrefab;
        
        private ClientPredictedEntity clientPredictedEntity;
        [SerializeField] private GameObject follow;
        
        //public VisualsInterpolationsProvider visualsInterpolationsProvider;
        private GameObject serverGhost;
        private GameObject clientGhost;
        
        public void SetClientPredictedEntity(ClientPredictedEntity clientPredictedEntity)
        {
            this.clientPredictedEntity = clientPredictedEntity;
            follow = clientPredictedEntity.gameObject;
            //TODO: listen for destruction events
            visualsEntity.transform.SetParent(null);
            
            //visualsInterpolationsProvider?.SetInterpolationTarget(visualsEntity.transform);
            if (debug)
            {
                serverGhost = Instantiate(serverGhostPrefab, Vector3.zero, Quaternion.identity);
                clientGhost = Instantiate(clientGhostPrefab, Vector3.zero, Quaternion.identity, follow.transform);
            }
        }

        //TODO: configurable
        private float defaultLerpFactor = 20f;
        void Update()
        {
            if (!follow)
                return;

            //TODO: proper integration
            //if (visualsInterpolationsProvider != null)
            //{
            //    visualsInterpolationsProvider.Update();
            //}
            //else
            {
                visualsEntity.transform.position = Vector3.Lerp(visualsEntity.transform.position, follow.transform.position, Time.deltaTime * 20);
                visualsEntity.transform.rotation = Quaternion.Lerp(visualsEntity.transform.rotation, follow.transform.rotation, Time.deltaTime * 20);
            }
            
            if (debug)
            {
                PhysicsStateRecord rec = clientPredictedEntity.serverStateBuffer.GetEnd();
                if (rec != null)
                {
                    serverGhost.transform.position = rec.position;
                    serverGhost.transform.rotation = rec.rotation;   
                }
            }
        }
    }
}