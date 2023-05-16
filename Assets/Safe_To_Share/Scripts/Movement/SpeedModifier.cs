using System.Collections;
using AvatarStuff.Holders;
using Character.StatsStuff.Mods;
using Movement.ECM2.Source.Characters;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement
{
    public class SpeedModifier : MonoBehaviour
    {
        public float speedMultiplier = 2.0f;

        public float accelerationMultiplier = 2.0f;
        public float decelerationMultiplier = 2.0f;

        public float frictionMultiplier = 2.0f;
        [SerializeField] TempIntMod sprintMod = new(88, 50, "Road", ModType.Percent);
        float _savedDecelerationWalking;
        float _savedGroundFriction;
        float _savedMaxAcceleration;
        ECM2Character playerChar;

        bool stopBoosting;
        readonly WaitForSeconds waitForSeconds = new(1.6f);

        void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                !other.gameObject.TryGetComponent(out PlayerHolder holder))
                return;
            if (stopBoosting)
            {
                stopBoosting = false;
                return;
            }

           // TODO playerChar = holder.PersonEcm2Character;
           // holder.MoveModHandler.AddWalkTempEffect(sprintMod);
            _savedMaxAcceleration = playerChar.maxAcceleration;
            _savedDecelerationWalking = playerChar.brakingDecelerationWalking;
            _savedGroundFriction = playerChar.groundFriction;

            playerChar.maxAcceleration *= accelerationMultiplier;
            playerChar.brakingDecelerationWalking *= decelerationMultiplier;
            playerChar.groundFriction *= frictionMultiplier;
        }

        void OnCollisionExit(Collision other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                !other.gameObject.TryGetComponent(out PlayerHolder holder))
                return;
            stopBoosting = true;
            StartCoroutine(SmallDelay(playerChar, holder));
        }

        IEnumerator SmallDelay(ECM2Character character, PlayerHolder holder)
        {
            yield return waitForSeconds;
            if (!stopBoosting)
                yield break;
            stopBoosting = false;
            // Restore character's saved settings

            // character.maxWalkSpeed = _savedMaxWalkSpeed;
       //    holder.MoveModHandler.RemoveWalkTempEffect(sprintMod);
            character.maxAcceleration = _savedMaxAcceleration;
            character.brakingDecelerationWalking = _savedDecelerationWalking;
            character.groundFriction = _savedGroundFriction;
        }
    }
}