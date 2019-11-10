//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR
{
    public class SteamVR_Loop : MonoBehaviour
    {
        private static SteamVR_Loop _instance;
        public static SteamVR_Loop Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<SteamVR_Loop>();

                return _instance;
            }
        }

        private List<SteamVR_Behaviour_Pose> _behaviourPoses = new List<SteamVR_Behaviour_Pose>();

        private void Awake()
        {
            if (!_instance)
                _instance = this;
        }

        public void AddBehaviourPose(SteamVR_Behaviour_Pose poseAction)
        {
            _behaviourPoses.Add(poseAction);
        }

        public void RemoveBehaviourPose(SteamVR_Behaviour_Pose poseAction)
        {
            _behaviourPoses.Remove(poseAction);
        }

        /// <summary>Gets called by SteamVR_Behaviour every Update and updates actions if the steamvr settings are configured to update then.</summary>
        private void Update()
        {
            if (SteamVR.settings.IsPoseUpdateMode(SteamVR_UpdateModes.OnUpdate))
            {
                UpdateVisualActions();
            }
        }

        /// <summary>
        /// Gets called by SteamVR_Behaviour every LateUpdate and updates actions if the steamvr settings are configured to update then.
        /// Also updates skeletons regardless of settings are configured to so we can account for animations on the skeletons.
        /// </summary>
        private void LateUpdate()
        {
            if (SteamVR.settings.IsPoseUpdateMode(SteamVR_UpdateModes.OnLateUpdate))
            {
                //update poses and skeleton
                UpdateVisualActions();
            }
            else
            {
                //force skeleton update so animation blending sticks
                UpdateSkeletonActions(true);
            }
        }

        /// <summary>Gets called by SteamVR_Behaviour every FixedUpdate and updates actions if the steamvr settings are configured to update then.</summary>
        private void FixedUpdate()
        {
            if (SteamVR.settings.IsPoseUpdateMode(SteamVR_UpdateModes.OnFixedUpdate))
            {
                UpdateVisualActions();
            }
        }

        /// <summary>Gets called by SteamVR_Behaviour every OnPreCull and updates actions if the steamvr settings are configured to update then.</summary>
        private void OnPreCull()
        {
            if (SteamVR.settings.IsPoseUpdateMode(SteamVR_UpdateModes.OnPreCull))
            {
                UpdateVisualActions();
            }
        }

        /// <summary>
        /// Updates the states of all the visual actions (pose / skeleton)
        /// </summary>
        /// <param name="skipStateAndEventUpdates">Controls whether or not events are fired from this update call</param>
        public void UpdateVisualActions(bool skipStateAndEventUpdates = false)
        {
            UpdatePoseActions(skipStateAndEventUpdates);

            UpdateSkeletonActions(skipStateAndEventUpdates);
        }

        /// <summary>
        /// Updates the states of all the pose actions
        /// </summary>
        /// <param name="skipSendingEvents">Controls whether or not events are fired from this update call</param>
        public void UpdatePoseActions(bool skipSendingEvents = false)
        {
            for (int actionIndex = 0; actionIndex < _behaviourPoses.Count; actionIndex++)
            {
                //SteamVR_Action_Pose action = actionsPose[actionIndex];
                _behaviourPoses[actionIndex].UpdateValues(skipSendingEvents);
            }

            //if (onPosesUpdated != null)
            //    onPosesUpdated(false);
        }


        /// <summary>
        /// Updates the states of all the skeleton actions
        /// </summary>
        /// <param name="skipSendingEvents">Controls whether or not events are fired from this update call</param>
        public void UpdateSkeletonActions(bool skipSendingEvents = false)
        {
            /*for (int actionIndex = 0; actionIndex < actionsSkeleton.Length; actionIndex++)
            {
                SteamVR_Action_Skeleton action = actionsSkeleton[actionIndex];

                action.UpdateValue(skipSendingEvents);
            }

            if (onSkeletonsUpdated != null)
                onSkeletonsUpdated(skipSendingEvents);*/
        }
    }
}