using System.Collections.Generic;
using Mirror;
using Prediction.data;
using UnityEngine;

namespace Prediction.Interpolation
{
    public class MirrorSnapshotInterpolationBridge : VisualsInterpolationsProvider
    {
        public readonly SortedList<double, TransformSnapshot> snapshots = new SortedList<double, TransformSnapshot>(16);
        private Transform transform;
        
        public void SetInterpolationTarget(Transform t)
        {
            transform = t;
        }
        
        public void Update()
        {
            if (snapshots.Count == 0)
                return;
            
            // step the interpolation without touching time.
            // NetworkClient is responsible for time globally.
            SnapshotInterpolation.StepInterpolation(
                snapshots,
                NetworkTime.time, // == NetworkClient.localTimeline from snapshot interpolation
                out TransformSnapshot from,
                out TransformSnapshot to,
                out double t);

            // interpolate & apply
            TransformSnapshot computed = TransformSnapshot.Interpolate(from, to, t);
            transform.position = computed.position;
            transform.rotation = computed.rotation;
        }
        
        public void Add(uint localTickId, PhysicsStateRecord record)
        {
            Debug.Log($"[MirrorSnapshotInterpolationBridge][Add] time:{NetworkTime.localTime} srvTime:{record.tmpServerTime} record:{record}");
            // insert transform snapshot
            SnapshotInterpolation.InsertIfNotExists(
                snapshots,
                NetworkClient.snapshotSettings.bufferLimit,
                new TransformSnapshot(
                    record.tmpServerTime, //GetTime(record), // arrival remote timestamp. NOT remote time.
                    NetworkTime.localTime, //GetTime(localTickId),
                    record.position,
                    record.rotation,
                    Vector3.one
                )
            );
        }
        
        double GetTime(PhysicsStateRecord record)
        {
            return GetTime(record.tickId);
        }
        
        double GetTime(uint tickId)
        {
            return tickId * Time.fixedTime;
        }
    }
}