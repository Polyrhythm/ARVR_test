/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.Networking;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : NetworkBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
		private GameObject arPlayer;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
				Debug.Log ("tracking found");
                OnTrackingFound();
            }
            else
            {
				Debug.Log ("tracking lost");
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

		private bool arPlayerAlive() {
			if (arPlayer)
				return true;

			if (!arPlayer) {
				arPlayer = GameObject.Find ("UserHeadAR(Clone)");

				if (!arPlayer)
					return false;
			}

			return true;
		}

        private void OnTrackingFound()
        {
			if (!arPlayerAlive ())
				return;

			arPlayer.SendMessage ("OnTrackingFound");
        }

        private void OnTrackingLost()
        {
			if (!arPlayerAlive ())
				return;

			arPlayer.SendMessage ("OnTrackingLost");
        }

        #endregion // PRIVATE_METHODS
    }
}
