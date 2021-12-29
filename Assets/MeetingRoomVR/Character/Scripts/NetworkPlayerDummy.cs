using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeetingRoomVR.Character
{
    public class NetworkPlayerDummy : MonoBehaviour
    {
        public Transform trackingHead;
        public Transform trackingLeftHand;
        public Transform trackingRight;
        [Space(100f)]
        private AnimatedAvatar avatar;
        [SerializeField] private Transform dummyHead;
        [SerializeField] private Transform dummyLeftHand;
        [SerializeField] private Transform dummyRightHand;

        void Awake()
        {
            avatar = GetComponentInChildren<AnimatedAvatar>();
        }

        void Start()
        {
            SetupDummyFollowing();
        }

        void Update()
        {
            TestTracking();
        }

        private void TestTracking()
        {
            var trackingTransforms = new[] { trackingHead, trackingLeftHand, trackingRight };
            var dummyTransforms = new[] { dummyHead, dummyLeftHand, dummyRightHand };
            for (var i = 0; i < dummyTransforms.Length; i++)
            {
                if (trackingTransforms[i] != null)
                {
                    dummyTransforms[i].localPosition = trackingTransforms[i].localPosition;
                    dummyTransforms[i].rotation = trackingTransforms[i].rotation;
                }
            }
        }

        private void SetupDummyFollowing()
        {
            var trackingTransforms = new[] { trackingHead, trackingLeftHand, trackingRight };
            var dummyTransforms = new[] { dummyHead, dummyLeftHand, dummyRightHand };
            var trackers = new[] { avatar.Head, avatar.LeftHand, avatar.RightHand };
            for (var i = 0; i < dummyTransforms.Length; i++)
            {
                if (trackingTransforms[i] != null)
                    trackers[i].StartFollowing(dummyHead);
                else
                    trackers[i].StopFollowing();
            }
        }
    }
}