using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.Audio;
using Sopra.ECS;

namespace Sopra.Logic.KI
{
    /// <inheritdoc/>
    /// <summary>
    /// Mark an entity as basic enemy.
    /// </summary>
    /// <author>Michael Fleig</author>
    [Serializable]
    public sealed class Enemy : IComponent
    {
        private static readonly ComponentType sType = ComponentType.Of<Enemy>();
        public static ComponentType Type => sType;

        [XmlElement]
        public EnemyStance Stance { get; set; } = EnemyStance.Idle;
        [XmlElement]
        public float TriggerLevel { get; set; }

        [XmlElement]
        public int SightDistance { get; set; } = 200;

        /// <summary>
        /// Contains the value where the enemy was spawned.
        /// </summary>
        /// <author>Felix Vogt</author>
        [XmlElement]
        public Vector2 SpawnPosition { get; set; }

        /// <summary>
        /// This value is only relevant if the Stance is != idle.
        /// Contains the value the Entity should go to.
        /// </summary>
        /// <author>Felix Vogt</author>
        [XmlElement]
        public Vector2 DesiredPos { get; set; }

        [XmlElement]
        public bool IsAlwaysAgressiv { get; set; }

        [XmlElement]
        public bool IsAgressivOnCar { get; set; }


        public void SetSpawnPos(Vector2 spawnPos)
        {
            SpawnPosition = spawnPos;
            DesiredPos = spawnPos;
        }

        public void IncreaseTriggerLevel(float value, Vector2 triggerPos)
        {
            if (IsAlwaysAgressiv)
            {
                return;
            }

            TriggerLevel += value;

            if (TriggerLevel > 100f)
            {
                SetAlerted(triggerPos);
            }

            if (TriggerLevel < 0f)
            {
                SetIdle();
            }
        }

        public void SetAgressiv(Entity enemy, Vector2 playerPos, bool targetIsCar)
        {
            if (IsAlwaysAgressiv)
            {
                return;
            }

            if (Stance != EnemyStance.Aggresive)
            {
                Stance = EnemyStance.Aggresive;
                SoundManager.Instance.PlaySound("hi_kevin");
                enemy.AddComponent(new CallForHelp(playerPos));
                
                IsAgressivOnCar = targetIsCar;
            }

            TriggerLevel = 100;
        }

        public void SetAlerted(Vector2 targetPos)
        {
            if (IsAlwaysAgressiv || Stance == EnemyStance.Aggresive)
            {
                return;
            }
            
            if (Stance != EnemyStance.Alerted)
            {
                SoundManager.Instance.PlaySound("alert");
                Stance = EnemyStance.Alerted;
            }

            TriggerLevel = 100;
            DesiredPos = targetPos;
        }

        private void SetIdle()
        {
            if (IsAlwaysAgressiv)
            {
                return;
            }
            
            if (Stance != EnemyStance.Idle)
            {
                SoundManager.Instance.PlaySound("calm_down");
                Stance = EnemyStance.Idle;
                DesiredPos = SpawnPosition;
            }

            TriggerLevel = 0;
        }

        public void AlwaysAgressiv()
        {
            IsAlwaysAgressiv = true;
            Stance = EnemyStance.Aggresive;
        }
    }
}