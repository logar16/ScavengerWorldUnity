using Assets.SharedAssets.Scripts.ScavengerEntity;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public class ActorAgent : Agent
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Unit unit;

        public UnityAction OnNewEpisode;
        public UnityAction<ActionSegment<int>> OnActionsReceived;

        private EnvironmentParameters ResetParams;

        public override void Initialize()
        {
            mover = GetComponent<Mover>();
            unit = GetComponent<Unit>();
            ResetParams = Academy.Instance.EnvironmentParameters;
        }

        public override void OnEpisodeBegin()
        {
            OnNewEpisode?.Invoke();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(mover.CheckForTarget());
            sensor.AddObservation(unit.HealthRemaining);

            var summary = unit.Summarize();
            //Color helps identify friend/foe
            var color = summary.Color;
            sensor.AddObservation(color.r);
            sensor.AddObservation(color.g);
            sensor.AddObservation(color.b);
            //Positional data may or may not be useful (e.g. finding home base which is at [0,0,0])
            sensor.AddObservation(summary.Position);

            //print("collected observations");
        }


        public override void OnActionReceived(ActionBuffers actions)
        {
            //Discrete Actions: Need to figure out what these will be
            //  1. Set new position
            //  2. Rotate
            //  3. Set current action
            
            // Pass this data along to relevant gameplay controllers
            ActionSegment<int> discrete = actions.DiscreteActions;
            OnActionsReceived?.Invoke(discrete);
        }

        protected void AddReward(string name, float defaultValue)
        {
            var reward = FindReward(name, defaultValue);
            AddReward(reward);
        }

        protected float FindReward(string name, float defaultValue)
        {
            return ResetParams.GetWithDefault(name, defaultValue);
        }
    }
}