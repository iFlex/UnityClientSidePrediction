using Prediction.data;
using UnityEngine;

namespace Prediction.Simulation
{
    public class SimplePhysicsControllerKinematic : PhysicsController
    {
        //TODO: save velocity state before and after resim
        private Rigidbody[] bodies;
        private PhysicsStateRecord[] states;
        public void DetectAllBodies()
        {
            bodies = Object.FindObjectsOfType<Rigidbody>();
            states = new PhysicsStateRecord[bodies.Length];
            for (int i = 0; i < bodies.Length; i++)
            {
                states[i] = new PhysicsStateRecord();
            }
            SaveStates();
        }

        void SaveStates()
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                states[i].position = bodies[i].position;
                states[i].rotation = bodies[i].rotation;
                states[i].velocity = bodies[i].linearVelocity;
                states[i].angularVelocity = bodies[i].angularVelocity;
            }
        }

        void LoadStates(Rigidbody ignore)
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i] == ignore)
                {
                    continue;
                }
                
                states[i].position = bodies[i].position;
                states[i].rotation = bodies[i].rotation;
                states[i].velocity = bodies[i].linearVelocity;
                states[i].angularVelocity = bodies[i].angularVelocity;
            }
        }
        
        public void Setup(bool isServer)
        {
            Physics.simulationMode = SimulationMode.Script;
        }

        public void Simulate()
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }

        public void BeforeResimulate(ClientPredictedEntity entity)
        {
            SaveStates();
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].isKinematic = true;
            }
            
            entity.rigidbody.isKinematic = false;
        }

        public void Resimulate(ClientPredictedEntity entity)
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }

        public void AfterResimulate(ClientPredictedEntity entity)
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].isKinematic = false;
            }
            LoadStates(entity.rigidbody);
        }
    }
}