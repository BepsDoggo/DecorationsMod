﻿using DecorationsMod.Controllers;
using DecorationsMod.Fixers;
using System.Collections.Generic;
using UnityEngine;

namespace DecorationsMod.NewItems
{
    public class AlienArtefact9 : DecorationItem
    {
        public AlienArtefact9() // Feeds abstract class
        {
            this.ClassID = "AlienArtefact9"; // f111c882-4ef6-4ad0-aeba-d123568ad3fc
            this.PrefabFileName = DecorationItem.DefaultResourcePath + this.ClassID;

            this.GameObject = new GameObject(this.ClassID);

            this.TechType = SMLHelper.V2.Handlers.TechTypeHandler.AddTechType(this.ClassID,
                                                        LanguageHelper.GetFriendlyWord("AlienRelic9Name"),
                                                        LanguageHelper.GetFriendlyWord("AlienRelic9Description"),
                                                        true);

            CrafterLogicFixer.AlienArtefact9 = this.TechType;
            KnownTechFixer.AddedNotifications.Add((int)this.TechType, false);

#if SUBNAUTICA
            this.Recipe = new SMLHelper.V2.Crafting.TechData()
            {
                craftAmount = 1,
                Ingredients = new List<SMLHelper.V2.Crafting.Ingredient>(new SMLHelper.V2.Crafting.Ingredient[1]
                    {
                        new SMLHelper.V2.Crafting.Ingredient(ConfigSwitcher.RelicRecipiesResource, ConfigSwitcher.RelicRecipiesResourceAmount)
                    }),
            };
#else
            this.Recipe = new SMLHelper.V2.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                    {
                        new Ingredient(ConfigSwitcher.RelicRecipiesResource, ConfigSwitcher.RelicRecipiesResourceAmount)
                    }),
            };
#endif
        }

        public override void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                // Associate recipe to the new TechType
                SMLHelper.V2.Handlers.CraftDataHandler.SetTechData(this.TechType, this.Recipe);

                // Add the new TechType to the hand-equipments
                SMLHelper.V2.Handlers.CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Hand);

                // Set quick slot type.
                SMLHelper.V2.Handlers.CraftDataHandler.SetQuickSlotType(this.TechType, QuickSlotType.Selectable);

                // Set the buildable prefab
                SMLHelper.V2.Handlers.PrefabHandler.RegisterPrefab(this);

                // Set the custom sprite
                SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, AssetsHelper.Assets.LoadAsset<Sprite>("relic_10_b"));

                this.IsRegistered = true;
            }
        }

        private static GameObject _alienArtefact9 = null;

        public override GameObject GetGameObject()
        {
            if (_alienArtefact9 == null)
#if SUBNAUTICA
                _alienArtefact9 = PrefabsHelper.LoadGameObjectFromFilename("WorldEntities/Doodads/Precursor/Prison/Relics/Alien_relic_10.prefab");
#else
                _alienArtefact9 = PrefabsHelper.LoadGameObjectFromFilename("WorldEntities/Precursor/Relics/Alien_relic_10.prefab");
#endif

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T1");
#endif
            GameObject prefab = GameObject.Instantiate(_alienArtefact9);
            prefab.name = this.ClassID;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T2");
#endif
            if (!ConfigSwitcher.AlienRelic9Animation)
                prefab.GetComponentInChildren<Animator>().enabled = false;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T3");
#endif
            // Scale
            foreach (Transform tr in prefab.transform)
                tr.transform.localScale *= 0.6f;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T4");
#endif
            // Update TechTag
            var techTag = prefab.GetComponent<TechTag>();
            if (techTag == null)
                if ((techTag = prefab.GetComponentInChildren<TechTag>()) == null)
                    techTag = prefab.AddComponent<TechTag>();
            techTag.type = this.TechType;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T5");
#endif
            // Update prefab ID
            var prefabId = prefab.GetComponent<PrefabIdentifier>();
            if (prefabId == null)
                if ((prefabId = prefab.GetComponentInChildren<PrefabIdentifier>()) == null)
                    prefabId = prefab.AddComponent<PrefabIdentifier>();
            prefabId.ClassId = this.ClassID;

            // Remove rigid body to prevent bugs
            /*
            var rbs = prefab.GetComponentsInChildren<Rigidbody>();
            if (rbs != null && rbs.Length > 0)
                foreach (var rbp in rbs)
                    GameObject.DestroyImmediate(rbp);
            var rb = prefab.GetComponent<Rigidbody>();
            if (rb != null)
                GameObject.DestroyImmediate(rb);
            */

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T6");
#endif
            // Update sky applier
            PrefabsHelper.ReplaceSkyApplier(prefab);

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T7");
#endif
            // Scale colliders
            var collider = prefab.GetComponent<CapsuleCollider>();
            collider.radius = 0.15f;
            collider.height = 0.4f;
            collider.contactOffset = 0.1f;
            collider.isTrigger = true;
            foreach (CapsuleCollider c in prefab.GetComponentsInChildren<CapsuleCollider>())
            {
                c.radius *= 0.4f;
                c.height *= 0.4f;
                c.isTrigger = true;
            }

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T8");
#endif
            // We can pick this item
            var pickupable = prefab.GetComponent<Pickupable>();
            if (pickupable == null)
                pickupable = prefab.AddComponent<Pickupable>();
            pickupable.isPickupable = true;
            pickupable.randomizeRotationWhenDropped = true;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T9");
#endif
            // We can place this item
            prefab.AddComponent<CustomPlaceToolController>();
            //var placeTool = prefab.GetComponent<PlaceTool>();
            //if (placeTool != null)
            //    GameObject.DestroyImmediate(placeTool);
            var placeTool = prefab.AddComponent<GenericPlaceTool>();
            placeTool.allowedInBase = true;
            placeTool.allowedOnBase = true;
            placeTool.allowedOnCeiling = false;
            placeTool.allowedOnConstructable = true;
            placeTool.allowedOnGround = true;
            placeTool.allowedOnRigidBody = true;
            placeTool.allowedOnWalls = true;
            placeTool.allowedOutside = ConfigSwitcher.AllowPlaceOutside;
            placeTool.rotationEnabled = true;
            placeTool.enabled = true;
            placeTool.hasAnimations = false;
            placeTool.hasBashAnimation = false;
            placeTool.hasFirstUseAnimation = false;
            placeTool.mainCollider = collider;
            placeTool.pickupable = pickupable;

            // Add fabricating animation
            var fabricating = prefab.AddComponent<VFXFabricating>();
            fabricating.localMinY = -0.2f;
            fabricating.localMaxY = 0.8f;
            fabricating.posOffset = new Vector3(0f, 0f, 0f);
            fabricating.eulerOffset = new Vector3(0f, 0f, 0f);
            fabricating.scaleFactor = 0.7f;

#if DEBUG_ALIENARTEFACTS
            Logger.Log("DEBUG: ALientArtefact9 T10");
#endif
            return prefab;
        }
    }
}
