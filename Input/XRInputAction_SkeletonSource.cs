using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Valve.VR
{
     /// <summary>
    /// Skeleton Actions are our best approximation of where your hands are while holding vr controllers and pressing buttons. We give you 31 bones to help you animate hand models.
    /// For more information check out this blog post: https://steamcommunity.com/games/250820/announcements/detail/1690421280625220068
    /// </summary>
    public class XRInputAction_SkeletonSource
    {
        public const int numBones = 31;

        public delegate void ActiveChangeHandler(XRInputAction_SkeletonSource fromAction, bool active);
        public delegate void ChangeHandler(XRInputAction_SkeletonSource fromAction);
        public delegate void UpdateHandler(XRInputAction_SkeletonSource fromAction);
        public delegate void TrackingChangeHandler(XRInputAction_SkeletonSource fromAction, ETrackingResult trackingState);
        public delegate void ValidPoseChangeHandler(XRInputAction_SkeletonSource fromAction, bool validPose);
        public delegate void DeviceConnectedChangeHandler(XRInputAction_SkeletonSource fromAction, bool deviceConnected);

        public float[] GetFingerCurls(bool copy = false)
        {
            if (copy)
                return (float[])fingerCurls.Clone();
            else
                return fingerCurls;
        }

        public float GetFingerCurl(int finger)
        {
            return fingerCurls[finger];
        }

        public float GetFingerCurl(SteamVR_Skeleton_FingerIndexEnum finger)
        {
            return GetFingerCurl((int)finger);
        }

        public void SetRangeOfMotion(EVRSkeletalMotionRange range)
        {
            rangeOfMotion = range;
        }

        public void UpdateValueWithoutEvents()
        {
            UpdateValue(true);
        }

        /// <summary>
        /// Gets the bone positions in local space. This array may be modified later so if you want to hold this data then pass true to get a copy of the data instead of the actual array
        /// </summary>
        /// <param name="copy">This array may be modified later so if you want to hold this data then pass true to get a copy of the data instead of the actual array</param>
        public Vector3[] GetBonePositions(bool copy = false)
        {
            if (copy)
                return (Vector3[])bonePositions.Clone();

            return bonePositions;
        }

        /// <summary>
        /// Gets the bone rotations in local space. This array may be modified later so if you want to hold this data then pass true to get a copy of the data instead of the actual array
        /// </summary>
        /// <param name="copy">This array may be modified later so if you want to hold this data then pass true to get a copy of the data instead of the actual array</param>
        public Quaternion[] GetBoneRotations(bool copy = false)
        {
            if (copy)
                return (Quaternion[])boneRotations.Clone();

            return boneRotations;
        }

        /// <summary>The local position of the pose relative to the universe origin</summary>
        public Vector3 GetLocalPosition()
        {
            return localPosition;
        }

        /// <summary>The local rotation of the pose relative to the universe origin</summary>
        public Quaternion GetLocalRotation()
        {
            return localRotation;
        }

        protected static uint skeletonActionData_size = 0;

        /// <summary>Event fires when the active state (ActionSet active and binding active) changes</summary>
        public event ActiveChangeHandler onActiveChange;

        /// <summary>Event fires when the active state of the binding changes</summary>
        public event ActiveChangeHandler onActiveBindingChange;

        /// <summary>Event fires when the orientation of the pose or bones changes more than the changeTolerance</summary>
        public event ChangeHandler onChange;

        /// <summary>Event fires when the action is updated</summary>
        public event UpdateHandler onUpdate;

        /// <summary>Event fires when the state of the tracking system that is used to create pose data (position, rotation, etc) changes</summary>
        public event TrackingChangeHandler onTrackingChanged;

        /// <summary>Event fires when the state of the pose data retrieved for this action changes validity (good/bad data from the tracking source)</summary>
        public event ValidPoseChangeHandler onValidPoseChanged;

        /// <summary>Event fires when the device bound to this action is connected or disconnected</summary>
        public event DeviceConnectedChangeHandler onDeviceConnectedChanged;


        /// <summary>True if the action is bound</summary>
        public bool activeBinding { get { return true; } } //return skeletonActionData.bActive;

        /// <summary>True if the action's binding was active during the previous update</summary>
        public bool lastActiveBinding { get { return true; } } //return lastSkeletonActionData.bActive;

        /// <summary>An array of the positions of the bones from the most recent update. Relative to skeletalTransformSpace. See SteamVR_Skeleton_JointIndexes for bone indexes.</summary>
        public Vector3[] bonePositions { get; protected set; }

        /// <summary>An array of the rotations of the bones from the most recent update. Relative to skeletalTransformSpace. See SteamVR_Skeleton_JointIndexes for bone indexes.</summary>
        public Quaternion[] boneRotations { get; protected set; }

        /// <summary>From the previous update: An array of the positions of the bones from the most recent update. Relative to skeletalTransformSpace. See SteamVR_Skeleton_JointIndexes for bone indexes.</summary>
        public Vector3[] lastBonePositions { get; protected set; }

        /// <summary>From the previous update: An array of the rotations of the bones from the most recent update. Relative to skeletalTransformSpace. See SteamVR_Skeleton_JointIndexes for bone indexes.</summary>
        public Quaternion[] lastBoneRotations { get; protected set; }


        /// <summary>The range of motion the we're using to get bone data from. With Controller being your hand while holding the controller.</summary>
        public EVRSkeletalMotionRange rangeOfMotion { get; set; }

        /// <summary>The space to get bone data in. Parent space by default</summary>
        public EVRSkeletalTransformSpace skeletalTransformSpace { get; set; }


        /// <summary>The type of summary data that will be retrieved by default. FromAnimation is smoothed data to based on the skeletal animation system. FromDevice is as recent from the device as we can get - may be different data from smoothed. </summary>
        public EVRSummaryType summaryDataType { get; set; }


        /// <summary>A 0-1 value representing how curled the thumb is. 0 being straight, 1 being fully curled.</summary>
        public float thumbCurl { get { return fingerCurls[SteamVR_Skeleton_FingerIndexes.thumb]; } }

        /// <summary>A 0-1 value representing how curled the index finger is. 0 being straight, 1 being fully curled.</summary>
        public float indexCurl { get { return fingerCurls[SteamVR_Skeleton_FingerIndexes.index]; } }

        /// <summary>A 0-1 value representing how curled the middle finger is. 0 being straight, 1 being fully curled.</summary>
        public float middleCurl { get { return fingerCurls[SteamVR_Skeleton_FingerIndexes.middle]; } }

        /// <summary>A 0-1 value representing how curled the ring finger is. 0 being straight, 1 being fully curled.</summary>
        public float ringCurl { get { return fingerCurls[SteamVR_Skeleton_FingerIndexes.ring]; } }

        /// <summary>A 0-1 value representing how curled the pinky finger is. 0 being straight, 1 being fully curled.</summary>
        public float pinkyCurl { get { return fingerCurls[SteamVR_Skeleton_FingerIndexes.pinky]; } }


        /// <summary>A 0-1 value representing the size of the gap between the thumb and index fingers</summary>
        public float thumbIndexSplay { get { return fingerSplays[SteamVR_Skeleton_FingerSplayIndexes.thumbIndex]; } }

        /// <summary>A 0-1 value representing the size of the gap between the index and middle fingers</summary>
        public float indexMiddleSplay { get { return fingerSplays[SteamVR_Skeleton_FingerSplayIndexes.indexMiddle]; } }

        /// <summary>A 0-1 value representing the size of the gap between the middle and ring fingers</summary>
        public float middleRingSplay { get { return fingerSplays[SteamVR_Skeleton_FingerSplayIndexes.middleRing]; } }

        /// <summary>A 0-1 value representing the size of the gap between the ring and pinky fingers</summary>
        public float ringPinkySplay { get { return fingerSplays[SteamVR_Skeleton_FingerSplayIndexes.ringPinky]; } }


        /// <summary>[Previous Update] A 0-1 value representing how curled the thumb is. 0 being straight, 1 being fully curled.</summary>
        public float lastThumbCurl { get { return lastFingerCurls[SteamVR_Skeleton_FingerIndexes.thumb]; } }

        /// <summary>[Previous Update] A 0-1 value representing how curled the index finger is. 0 being straight, 1 being fully curled.</summary>
        public float lastIndexCurl { get { return lastFingerCurls[SteamVR_Skeleton_FingerIndexes.index]; } }

        /// <summary>[Previous Update] A 0-1 value representing how curled the middle finger is. 0 being straight, 1 being fully curled.</summary>
        public float lastMiddleCurl { get { return lastFingerCurls[SteamVR_Skeleton_FingerIndexes.middle]; } }

        /// <summary>[Previous Update] A 0-1 value representing how curled the ring finger is. 0 being straight, 1 being fully curled.</summary>
        public float lastRingCurl { get { return lastFingerCurls[SteamVR_Skeleton_FingerIndexes.ring]; } }

        /// <summary>[Previous Update] A 0-1 value representing how curled the pinky finger is. 0 being straight, 1 being fully curled.</summary>
        public float lastPinkyCurl { get { return lastFingerCurls[SteamVR_Skeleton_FingerIndexes.pinky]; } }


        /// <summary>[Previous Update] A 0-1 value representing the size of the gap between the thumb and index fingers</summary>
        public float lastThumbIndexSplay { get { return lastFingerSplays[SteamVR_Skeleton_FingerSplayIndexes.thumbIndex]; } }

        /// <summary>[Previous Update] A 0-1 value representing the size of the gap between the index and middle fingers</summary>
        public float lastIndexMiddleSplay { get { return lastFingerSplays[SteamVR_Skeleton_FingerSplayIndexes.indexMiddle]; } }

        /// <summary>[Previous Update] A 0-1 value representing the size of the gap between the middle and ring fingers</summary>
        public float lastMiddleRingSplay { get { return lastFingerSplays[SteamVR_Skeleton_FingerSplayIndexes.middleRing]; } }

        /// <summary>[Previous Update] A 0-1 value representing the size of the gap between the ring and pinky fingers</summary>
        public float lastRingPinkySplay { get { return lastFingerSplays[SteamVR_Skeleton_FingerSplayIndexes.ringPinky]; } }


        /// <summary>0-1 values representing how curled the specified finger is. 0 being straight, 1 being fully curled. For indexes see: SteamVR_Skeleton_FingerIndexes</summary>
        public float[] fingerCurls { get; protected set; }

        /// <summary>0-1 values representing how splayed the specified finger and it's next index'd finger is. For indexes see: SteamVR_Skeleton_FingerIndexes</summary>
        public float[] fingerSplays { get; protected set; }

        /// <summary>[Previous Update] 0-1 values representing how curled the specified finger is. 0 being straight, 1 being fully curled. For indexes see: SteamVR_Skeleton_FingerIndexes</summary>
        public float[] lastFingerCurls { get; protected set; }

        /// <summary>[Previous Update] 0-1 values representing how splayed the specified finger and it's next index'd finger is. For indexes see: SteamVR_Skeleton_FingerIndexes</summary>
        public float[] lastFingerSplays { get; protected set; }

        /// <summary>Separate from "changed". If the pose for this skeleton action has changed (root position/rotation)</summary>
        public bool poseChanged { get; protected set; }

        /// <summary>Skips processing the full per bone data and only does the summary data</summary>
        public bool onlyUpdateSummaryData { get; set; }
        public bool active => true;
        public bool lastActive { get; protected set; }
        public bool changed { get; protected set; }

        /// <summary>The value of the action's 'changed' during the previous update</summary>
        public bool lastChanged { get; protected set; }
        /// <summary>The Time.realtimeSinceStartup that this action was last changed.</summary>
        public float changedTime { get; protected set; }
        public float changeTolerance = Mathf.Epsilon;
         /// <summary>The local position of this action relative to the universe origin</summary>
        public Vector3 localPosition { get; protected set; }

        /// <summary>The local rotation of this action relative to the universe origin</summary>
        public Quaternion localRotation { get; protected set; }

        protected VRSkeletalSummaryData_t skeletalSummaryData = new VRSkeletalSummaryData_t();
        protected VRSkeletalSummaryData_t lastSkeletalSummaryData = new VRSkeletalSummaryData_t();
        //protected SteamVR_Action_Skeleton skeletonAction;

        protected VRBoneTransform_t[] tempBoneTransforms = new VRBoneTransform_t[SteamVR_Action_Skeleton.numBones];

        protected InputSkeletalActionData_t skeletonActionData = new InputSkeletalActionData_t();

        protected InputSkeletalActionData_t lastSkeletonActionData = new InputSkeletalActionData_t();

        protected InputSkeletalActionData_t tempSkeletonActionData = new InputSkeletalActionData_t();

        public void Preinitialize()
        {
            bonePositions = new Vector3[SteamVR_Action_Skeleton.numBones];
            lastBonePositions = new Vector3[SteamVR_Action_Skeleton.numBones];
            boneRotations = new Quaternion[SteamVR_Action_Skeleton.numBones];
            lastBoneRotations = new Quaternion[SteamVR_Action_Skeleton.numBones];

            rangeOfMotion = EVRSkeletalMotionRange.WithController;
            skeletalTransformSpace = EVRSkeletalTransformSpace.Parent;

            fingerCurls = new float[SteamVR_Skeleton_FingerIndexes.enumArray.Length];
            fingerSplays = new float[SteamVR_Skeleton_FingerSplayIndexes.enumArray.Length];

            lastFingerCurls = new float[SteamVR_Skeleton_FingerIndexes.enumArray.Length];
            lastFingerSplays = new float[SteamVR_Skeleton_FingerSplayIndexes.enumArray.Length];
        }

        /// <summary>
        /// <strong>[Should not be called by user code]</strong>
        /// Initializes the handle for the inputSource, the skeletal action data size, and any other related SteamVR data.
        /// </summary>
        public void Initialize()
        {
            if (skeletonActionData_size == 0)
                skeletonActionData_size = (uint)Marshal.SizeOf(typeof(InputSkeletalActionData_t));
        }

        /// <summary><strong>[Should not be called by user code]</strong>
        /// Updates the data for this action and this input source. Sends related events.
        /// </summary>
        public void UpdateValue()
        {
            UpdateValue(false);
        }

        /// <summary><strong>[Should not be called by user code]</strong>
        /// Updates the data for this action and this input source. Sends related events.
        /// </summary>
        public void UpdateValue(bool skipStateAndEventUpdates)
        {
            lastActive = active;
            lastSkeletonActionData = skeletonActionData;
            lastSkeletalSummaryData = skeletalSummaryData;

            if (onlyUpdateSummaryData == false)
            {
                for (int boneIndex = 0; boneIndex < SteamVR_Action_Skeleton.numBones; boneIndex++)
                {
                    lastBonePositions[boneIndex] = bonePositions[boneIndex];
                    lastBoneRotations[boneIndex] = boneRotations[boneIndex];
                }
            }

            for (int fingerIndex = 0; fingerIndex < SteamVR_Skeleton_FingerIndexes.enumArray.Length; fingerIndex++)
            {
                lastFingerCurls[fingerIndex] = fingerCurls[fingerIndex];
            }

            for (int fingerIndex = 0; fingerIndex < SteamVR_Skeleton_FingerSplayIndexes.enumArray.Length; fingerIndex++)
            {
                lastFingerSplays[fingerIndex] = fingerSplays[fingerIndex];
            }

            poseChanged = changed;

            if (active)
            {
                if (onlyUpdateSummaryData == false)
                {
                    for (int boneIndex = 0; boneIndex < tempBoneTransforms.Length; boneIndex++)
                    {
                        // SteamVR's coordinate system is right handed, and Unity's is left handed.  The FBX data has its
                        // X axis flipped when Unity imports it, so here we need to flip the X axis as well
                        bonePositions[boneIndex].x = -tempBoneTransforms[boneIndex].position.v0;
                        bonePositions[boneIndex].y = tempBoneTransforms[boneIndex].position.v1;
                        bonePositions[boneIndex].z = tempBoneTransforms[boneIndex].position.v2;

                        boneRotations[boneIndex].x = tempBoneTransforms[boneIndex].orientation.x;
                        boneRotations[boneIndex].y = -tempBoneTransforms[boneIndex].orientation.y;
                        boneRotations[boneIndex].z = -tempBoneTransforms[boneIndex].orientation.z;
                        boneRotations[boneIndex].w = tempBoneTransforms[boneIndex].orientation.w;
                    }

                    // Now that we're in the same handedness as Unity, rotate the root bone around the Y axis
                    // so that forward is facing down +Z

                    boneRotations[0] = SteamVR_Action_Skeleton.steamVRFixUpRotation * boneRotations[0];
                }

                UpdateSkeletalSummaryData(summaryDataType, true);
            }

            if (changed == false)
            {
                for (int boneIndex = 0; boneIndex < tempBoneTransforms.Length; boneIndex++)
                {
                    if (Vector3.Distance(lastBonePositions[boneIndex], bonePositions[boneIndex]) > changeTolerance)
                    {
                        changed = true;
                        break;
                    }

                    if (Mathf.Abs(Quaternion.Angle(lastBoneRotations[boneIndex], boneRotations[boneIndex])) > changeTolerance)
                    {
                        changed = true;
                        break;
                    }
                }
            }

            if (changed)
                changedTime = Time.realtimeSinceStartup;

            if (skipStateAndEventUpdates == false)
                CheckAndSendEvents();
        }

        /// <summary>
        /// The number of bones in the skeleton for this action
        /// </summary>
        public int boneCount { get { return (int)GetBoneCount(); } }

        /// <summary>
        /// Gets the number of bones in the skeleton for this action
        /// </summary>
        public uint GetBoneCount()
        {
            /*uint boneCount = 0;
            EVRInputError error = OpenVR.Input.GetBoneCount(handle, ref boneCount);
            if (error != EVRInputError.None)
                Debug.LogError("<b>[SteamVR]</b> GetBoneCount error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
            */
            return numBones;
        }

        /// <summary>
        /// Gets the ordering of the bone hierarchy
        /// </summary>
        public int[] boneHierarchy { get { return GetBoneHierarchy(); } }

        /// <summary>
        /// Gets the ordering of the bone hierarchy
        /// </summary>
        public int[] GetBoneHierarchy()
        {
            int boneCount = (int)GetBoneCount();
            int[] parentIndicies = new int[boneCount];

            /*EVRInputError error = OpenVR.Input.GetBoneHierarchy(handle, parentIndicies);
            if (error != EVRInputError.None)
                Debug.LogError("<b>[SteamVR]</b> GetBoneHierarchy error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
            */
            return parentIndicies;
        }

        /// <summary>
        /// Gets the name for a bone at the specified index
        /// </summary>
        public string GetBoneName(int boneIndex)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            /*EVRInputError error = OpenVR.Input.GetBoneName(handle, boneIndex, stringBuilder, 255);
            if (error != EVRInputError.None)
                Debug.LogError("<b>[SteamVR]</b> GetBoneName error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
*/
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns an array of positions/rotations that represent the state of each bone in a reference pose.
        /// </summary>
        /// <param name="transformSpace">What to get the position/rotation data relative to, the model, or the bone's parent</param>
        /// <param name="referencePose">Which reference pose to return</param>
        /// <returns></returns>
        public SteamVR_Utils.RigidTransform[] GetReferenceTransforms(EVRSkeletalTransformSpace transformSpace, EVRSkeletalReferencePose referencePose)
        {
            SteamVR_Utils.RigidTransform[] transforms = new SteamVR_Utils.RigidTransform[GetBoneCount()];

            VRBoneTransform_t[] boneTransforms = new VRBoneTransform_t[transforms.Length];

            /*EVRInputError error = OpenVR.Input.GetSkeletalReferenceTransforms(handle, transformSpace, referencePose, boneTransforms);
            if (error != EVRInputError.None)
                Debug.LogError("<b>[SteamVR]</b> GetSkeletalReferenceTransforms error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
            */
            for (int transformIndex = 0; transformIndex < boneTransforms.Length; transformIndex++)
            {
                Vector3 position = new Vector3(-boneTransforms[transformIndex].position.v0, boneTransforms[transformIndex].position.v1, boneTransforms[transformIndex].position.v2);
                Quaternion rotation = new Quaternion(boneTransforms[transformIndex].orientation.x, -boneTransforms[transformIndex].orientation.y, -boneTransforms[transformIndex].orientation.z, boneTransforms[transformIndex].orientation.w);
                transforms[transformIndex] = new SteamVR_Utils.RigidTransform(position, rotation);
            }

            if (transforms.Length > 0)
            {
                // Now that we're in the same handedness as Unity, rotate the root bone around the Y axis
                // so that forward is facing down +Z
                Quaternion qFixUpRot = Quaternion.AngleAxis(Mathf.PI * Mathf.Rad2Deg, Vector3.up);

                transforms[0].rot = qFixUpRot * transforms[0].rot;
            }

            return transforms;
        }

        /// <summary>
        /// Get the accuracy level of the skeletal tracking data.
        /// <para/>* Estimated: Body part location can’t be directly determined by the device. Any skeletal pose provided by the device is estimated based on the active buttons, triggers, joysticks, or other input sensors. Examples include the Vive Controller and gamepads.
        /// <para/>* Partial: Body part location can be measured directly but with fewer degrees of freedom than the actual body part.Certain body part positions may be unmeasured by the device and estimated from other input data.Examples include Knuckles or gloves that only measure finger curl
        /// <para/>* Full: Body part location can be measured directly throughout the entire range of motion of the body part.Examples include hi-end mocap systems, or gloves that measure the rotation of each finger segment.
        /// </summary>
        public EVRSkeletalTrackingLevel skeletalTrackingLevel { get { return GetSkeletalTrackingLevel(); } }

        /// <summary>
        /// Get the accuracy level of the skeletal tracking data.
        /// <para/>* Estimated: Body part location can’t be directly determined by the device. Any skeletal pose provided by the device is estimated based on the active buttons, triggers, joysticks, or other input sensors. Examples include the Vive Controller and gamepads.
        /// <para/>* Partial: Body part location can be measured directly but with fewer degrees of freedom than the actual body part.Certain body part positions may be unmeasured by the device and estimated from other input data.Examples include Knuckles or gloves that only measure finger curl
        /// <para/>* Full: Body part location can be measured directly throughout the entire range of motion of the body part.Examples include hi-end mocap systems, or gloves that measure the rotation of each finger segment.
        /// </summary>
        public EVRSkeletalTrackingLevel GetSkeletalTrackingLevel()
        {
            EVRSkeletalTrackingLevel skeletalTrackingLevel = EVRSkeletalTrackingLevel.VRSkeletalTracking_Estimated;

            /*EVRInputError error = OpenVR.Input.GetSkeletalTrackingLevel(handle, ref skeletalTrackingLevel);
            if (error != EVRInputError.None)
                Debug.LogError("<b>[SteamVR]</b> GetSkeletalTrackingLevel error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
*/
            return skeletalTrackingLevel;
        }

        /// <summary>
        /// Get the skeletal summary data structure from OpenVR.
        /// Contains curl and splay data in finger order: thumb, index, middlg, ring, pinky.
        /// Easier access at named members: indexCurl, ringSplay, etc.
        /// </summary>
        protected VRSkeletalSummaryData_t GetSkeletalSummaryData(EVRSummaryType summaryType = EVRSummaryType.FromAnimation, bool force = false)
        {
            UpdateSkeletalSummaryData(summaryType, force);
            return skeletalSummaryData;
        }

        /// <summary>
        /// Updates the skeletal summary data structure from OpenVR.
        /// Contains curl and splay data in finger order: thumb, index, middlg, ring, pinky.
        /// Easier access at named members: indexCurl, ringSplay, etc.
        /// </summary>
        protected void UpdateSkeletalSummaryData(EVRSummaryType summaryType = EVRSummaryType.FromAnimation, bool force = false)
        {
            if (force || this.summaryDataType != summaryDataType && active)
            {
                /*EVRInputError error = OpenVR.Input.GetSkeletalSummaryData(handle, summaryType, ref skeletalSummaryData);
                if (error != EVRInputError.None)
                    Debug.LogError("<b>[SteamVR]</b> GetSkeletalSummaryData error (" + fullPath + "): " + error.ToString() + " handle: " + handle.ToString());
*/
                fingerCurls[0] = skeletalSummaryData.flFingerCurl0;
                fingerCurls[1] = skeletalSummaryData.flFingerCurl1;
                fingerCurls[2] = skeletalSummaryData.flFingerCurl2;
                fingerCurls[3] = skeletalSummaryData.flFingerCurl3;
                fingerCurls[4] = skeletalSummaryData.flFingerCurl4;

                //no splay data for thumb
                fingerSplays[0] = skeletalSummaryData.flFingerSplay0;
                fingerSplays[1] = skeletalSummaryData.flFingerSplay1;
                fingerSplays[2] = skeletalSummaryData.flFingerSplay2;
                fingerSplays[3] = skeletalSummaryData.flFingerSplay3;
            }
        }

        protected void CheckAndSendEvents()
        {
            /*if (trackingState != lastTrackingState && onTrackingChanged != null)
                onTrackingChanged.Invoke(this, trackingState);

            if (poseIsValid != lastPoseIsValid && onValidPoseChanged != null)
                onValidPoseChanged.Invoke(this, poseIsValid);

            if (deviceIsConnected != lastDeviceIsConnected && onDeviceConnectedChanged != null)
                onDeviceConnectedChanged.Invoke(this, deviceIsConnected);
*/
            if (changed && onChange != null)
                onChange.Invoke(this);

            if (active != lastActive && onActiveChange != null)
                onActiveChange.Invoke(this, active);

            if (activeBinding != lastActiveBinding && onActiveBindingChange != null)
                onActiveBindingChange.Invoke(this, activeBinding);

            if (onUpdate != null)
                onUpdate.Invoke(this);
        }
    }
}