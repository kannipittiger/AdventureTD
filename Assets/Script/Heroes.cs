    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Heroes : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform rangeObject;

        [Header("Attribute")]
        [SerializeField] protected float aps; // Attacks per second
        [SerializeField] protected float damage; // Base damage
        [SerializeField] protected float upgradeCost;
        [SerializeField] protected float orginalCost; 
        [SerializeField] protected int towerIndex; 
        public float CurrentDamage => damage; // Current damage (base)
        public float TargetingRange => targetingRange; // Targeting range
        public float UpgradeCost => upgradeCost; // Targeting range
        public float OriginalCost => upgradeCost; // Targeting range
        public int TowerIndex => towerIndex; // Targeting range
        


        public float targetingRange; // Field for targeting range
        private SpriteRenderer spriteRenderer;
        public int countUpgrade = 0;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on the hero.");
            }

            if (rangeObject != null)
            {
                targetingRange = Mathf.Max(rangeObject.localScale.x, rangeObject.localScale.y) / 2f;
            }
            // Damage = damage; // Initialize the Damage property
        }

        public Sprite GetHeroSprite()
        {
            return spriteRenderer != null ? spriteRenderer.sprite : null;
        }

        public void UpgradeStats()
        {
            damage *= 1.3f; // Increase damage by 20%
            targetingRange *= 1.1f; // Increase targeting range by 10%
            upgradeCost *= 1.2f;
            // Damage = damage; // Update the Damage property with the new value

            if (rangeObject != null)
            {
                rangeObject.localScale = new Vector3(targetingRange * 2, targetingRange * 2, rangeObject.localScale.z);
            }
        }
    }
