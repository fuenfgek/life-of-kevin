using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Sopra.ECS;
using Sopra.Logic.KI;

namespace Sopra.Logic.Health
{
    /// <summary>
    /// Component for storing health data.
    /// </summary>
    /// <inheritdoc cref="IComponent"/>
    /// <author>Felix Vogt</author>
    [Serializable]
    public sealed class HealthC : IComponent
    {
        public static ComponentType Type { get; } = ComponentType.Of <HealthC>();
        
        public const int MaxCoffeeCharges = 3;
        [XmlElement]
        public int CurrentCoffeeCharges { get; set; }
        private float mMaxHealth;
        private float mCurrentHealth;

        [XmlElement]
        public bool HasSpecialDamageLogic { get; }
        [XmlElement]
        public SerialzableDictionary<int, float> UnhandeldDamage { get; }

        [XmlElement]
        public float MaxHealth
        {
            get { return mMaxHealth; }
            set
            {
                mMaxHealth = value;
                CurrentHealth = value;
            }
        }


        [XmlElement]
        public float CurrentHealth
        {
            get { return mCurrentHealth; }
            set
            {
                mCurrentHealth = value < 0
                    ? 0
                    : value > MaxHealth
                        ? MaxHealth
                        : value;
            }
        }

        
        public HealthC()
        {
            CurrentCoffeeCharges = MaxCoffeeCharges;
            HasSpecialDamageLogic = false;
            UnhandeldDamage = new SerialzableDictionary<int, float>();
        }

        public HealthC(float maxHealth, bool hasSpecialDamageLogic = false)
        {
            MaxHealth = maxHealth;
            CurrentCoffeeCharges = MaxCoffeeCharges;
            HasSpecialDamageLogic = hasSpecialDamageLogic;
            UnhandeldDamage = new SerialzableDictionary<int, float>();
        }

        public void ApplyDamageSimple(float damage)
        {
            if (!HasSpecialDamageLogic)
            {
                CurrentHealth -= damage;
            }
        }

        public void ApplyDamage(Entity entity, float damage, int attackingEntityId, Vector2 attackingEntityPos, bool attackingEntityIsCar)
        {
            if (entity.HasComponent(Enemy.Type))
            {
                entity.GetComponent<Enemy>(Enemy.Type).SetAgressiv(entity, attackingEntityPos, attackingEntityIsCar);
            }

            if (!HasSpecialDamageLogic)
            {
                CurrentHealth -= damage;
                return;
            }

            if (UnhandeldDamage.ContainsKey(attackingEntityId))
            {
                UnhandeldDamage[attackingEntityId] += damage;
            }
            else
            {
                UnhandeldDamage.Add(attackingEntityId, damage);
            }
        }
    }
}
