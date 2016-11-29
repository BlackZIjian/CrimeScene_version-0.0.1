/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using System.Collections;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        CheckManager m_check_manager;
        public Transform model_transform;
        Transform child_transform;
        GameObject[] relatedGameObj; 
        #region PRIVATE_MEMBER_VARIABLES

        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            //m_check_manager = GameObject.Find("CheckManager").GetComponent<CheckManager>();
            child_transform = transform.GetChild(0);
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
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

       

        private void OnTrackingFound()
        {
            //for(int i=0;i<transform.childCount;i++)
            //{
            //    GameObject ga = transform.GetChild(i).gameObject;
            //    if(m_check_manager.isCanInitModel(ga))
            //    {
            //        Renderer renderer = ga.GetComponent<Renderer>();
            //        Collider collider = ga.GetComponent<Collider>();
            //        if(renderer != null)
            //        {
            //            renderer.enabled = true;
            //        }
            //        if(collider != null)
            //        {
            //            collider.enabled = true;
            //        }
            //    }
            //}

            //if(model_transform != null && child_transform != null)
            //{
            //    model_transform.gameObject.SetActive(true);
            //    model_transform.position = child_transform.position;
            //    model_transform.rotation = child_transform.rotation;
            //    Animation ani = model_transform.GetComponent<Animation>();
            //    if(ani != null)
            //    {
            //        ani.Play();
            //    }
            //}
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Animation[] animationComponents = GetComponentsInChildren<Animation>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            foreach(Animation component in animationComponents)
            {
                
                StartCoroutine(PlayWait(1, component));
               
            }
            foreach(GameObject gObj in relatedGameObj)
            {
                gObj.SetActive(true);
            }
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            //Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            //Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            //// Disable rendering:
            //foreach (Renderer component in rendererComponents)
            //{
            //    component.enabled = false;
            //}

            //// Disable colliders:
            //foreach (Collider component in colliderComponents)
            //{
            //    component.enabled = false;
            //}

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS

        IEnumerator DelModel(float waittime,Transform trans)
        {
            yield return new WaitForSeconds(waittime);
            Renderer[] rendererComponents = trans.GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = trans.GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }
            foreach (GameObject gObj in relatedGameObj)
            {
                gObj.SetActive(false);
            }
        }
        IEnumerator PlayWait(float waittime,Animation ani)
        {
            yield return new WaitForSeconds(waittime);
            ani.Play();
            StartCoroutine(DelModel(ani.clip.length + 1, transform));
        }
    }
}
