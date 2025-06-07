using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Packer
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        [Tooltip("Switches connected to this door")]
        public DoorSwitch[] switches;
        [Tooltip("How fast the door opens (Meters a second)")]
        public float doorMoveSpeed = 1.0f;
        [Tooltip("How fast the door opens (Degrees a second")]
        public float doorRotateSpeed = 1.0f;
        [Tooltip("Auto close timing (>0 for no auto)")]
        public float autoClose = 0f;
        [Tooltip("Default State of this door")]
        public bool open = false;
        [Tooltip("Does this door move?")]
        public bool mover = false;
        [Tooltip("The closed local position of the door")]
        public Vector3 closeOffset;
        [Tooltip("The opened local position of the door")]
        public Vector3 openOffset;
        [Tooltip("Does this door rotate?")]
        public bool rotator = false;
        [Tooltip("The closed local rotation of the door, would reccomend not modifying these values directly unless you really know what you're doing")]
        public Quaternion closeRotation;
        [Tooltip("The opened local rotation of the door, would reccomend not modifying these values directly unless you really know what you're doing")]
        public Quaternion openRotation;

        public AudioClip openSound;
        public AudioClip closeSound;
        private AudioSource audioSource;

        private bool moving = false;
        private int doorIndex;

        List<IEnumerator> WaitList = new List<IEnumerator>();

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            transform.hasChanged = false;
        }

        void Update()
		{            
            while (WaitList.Count > 0)
            {
                StartCoroutine(WaitList[0]);
                WaitList.RemoveAt(0);
            }

            if (mover){
                if (open && transform.localPosition != openOffset)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, openOffset, doorMoveSpeed * Time.deltaTime);
                    moving = true;
                }
                else if (!open && transform.localPosition != closeOffset)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, closeOffset, doorMoveSpeed * Time.deltaTime);
                    moving = true;
                } else {
                    moving = false;
                }
            }

            if (rotator){
                if (open && transform.rotation != openRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, openRotation, doorRotateSpeed * Time.deltaTime);
                    moving = true;
                }
                else if (!open && transform.rotation != closeRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, closeRotation, doorRotateSpeed * Time.deltaTime);
                    moving = true;
                } else {
                    moving = false;
                }
            }
            
            if (open && autoClose > 0 && !moving){
                new Thread(() =>
                {
                    WaitList.Add(Wait());
                }).Start();
            }
		}

		public void SetDoor(bool state)
		{
			open = state;

            //Audio
            if (open)
            {
                if (openSound != null)
                    audioSource.PlayOneShot(openSound);
            }
            else
            {
                if (closeSound != null)
                    audioSource.PlayOneShot(closeSound);
            }
        }

        public void SetOpenOffset()
        {
            if (ErrorCheck())
                return;

            openOffset = transform.localPosition;
        }

        public void SetCloseOffset()
        {
            if (ErrorCheck())
                return;

            closeOffset = transform.localPosition;
        }


        public void MoveToOpenOffset()
        {
            if (ErrorCheck())
                return;

            transform.localPosition = openOffset;
        }

        public void MoveToCloseOffset()
        {
            if (ErrorCheck())
                return;

            transform.localPosition = closeOffset;
        }

        public void SetOpenRotation()
        {
            if (ErrorCheck())
                return;

            openRotation = transform.rotation;
        }

        public void SetCloseRotation()
        {
            if (ErrorCheck())
                return;

            closeRotation = transform.rotation;
        }

        public void MoveToOpenRotation()
        {
            if (ErrorCheck())
                return;

            transform.rotation = openRotation;
        }

        public void MoveToCloseRotation()
        {
            if (ErrorCheck())
                return;

            transform.rotation = closeRotation;
        }

        bool ErrorCheck()
        {
            if (transform.parent == null)
            {
                Debug.LogError(name + " is missing a parent gameobject");
                return true;
            }


            return false;
        }

        IEnumerator Wait()
        {
            // This could probably be implemented a lot better, but it seems to be working in all the nonsensical use cases I've thrown at it.
            // Pressing a button whilst a door is automatically closing will open it again then re-trigger the auto close but it doesn't cause
            // any actual problems as far as I can tell.
            yield return new WaitForSeconds(autoClose);
            while (open && !moving) 
            {
                // ToggleSingleDoor doesn't change the switches state so it doesn't matter which switch is used
                if (switches[0] != null)
                {
                    for (int i = 0; i < switches[0].doors.Length; i++)
                    {
                        if (switches[0].doors[i].name == this.name)
                            doorIndex = i;
                    }
                    switches[0].ToggleSingleDoor(doorIndex);
                }
            }
        }
    }
}