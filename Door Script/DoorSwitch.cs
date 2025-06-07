using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
namespace Packer
{
	[RequireComponent(typeof(AudioSource), typeof(BoxCollider))]

	public class DoorSwitch : MonoBehaviour
	{
        [Tooltip("All the doors that will be triggered when this switch is hit")]
		public Door[] doors;
        [Tooltip("All Switches that will be set to this switches state")]
        public DoorSwitch[] switches;
        [Tooltip("Default switch state")]
        public bool switchState = false;
        [Tooltip("Does this button accept only an empty hand, or any collision")]
        public bool physicsActive = false;

		public AudioClip openSound;
		public AudioClip closeSound;
        private BoxCollider triggerZone;
		private LayerMask mask;
		private AudioSource audioSource;

        private float timeout = 0;

        void Start()
		{
            audioSource = GetComponent<AudioSource>();
            triggerZone = GetComponent<BoxCollider>();
			triggerZone.isTrigger = true;
			gameObject.layer = LayerMask.NameToLayer("Enviroment");
        }

		void Update()
		{
			if(timeout > 0)
				timeout = Mathf.Clamp(timeout -= Time.deltaTime, 0, 1);
        }

        // Made public so you can use player body trigger volumes instead of a button if desired
		public void ToggleDoors()
		{
			switchState = !switchState;

            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] != null)
                        doors[i].SetDoor(!doors[i].open); // Switched from .switchState to .open so each door isn't tied to it's button state (for auto closing doors)
			}

			for (int i = 0; i < switches.Length; i++)
			{
				if(switches[i] != null)
					switches[i].switchState = switchState;
            }

            //Audio
            if (switchState)
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

        // New function to toggle a single door so auto close doesn't effect every connected door
        public void ToggleSingleDoor(int doorIndex)
        {
            doors[doorIndex].SetDoor(!doors[doorIndex].open);
        }

        void OnTriggerEnter(Collider collider)
        {
            // Tweaked this so anything can trigger if desired, makes for some more 
            // interesting interactions with the world and means you don't need to
            // drop your weapon every time you want to push a button.
            if (physicsActive) {
                if (timeout > 0)
                    return;
                timeout = 1;
                ToggleDoors();
            } else {
                if (collider.gameObject.tag == "GameController") {
                    if (timeout > 0)
                        return;
                    timeout = 1;
                    ToggleDoors();
                }
            }     
        }

    }
}
