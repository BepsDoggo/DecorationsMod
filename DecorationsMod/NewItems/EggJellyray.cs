﻿using DecorationsMod.Fixers;
using System.Collections.Generic;
using UnityEngine;

namespace DecorationsMod.NewItems
{
    public class EggJellyray : DecorationItem
    {
        public EggJellyray()
        {
            this.ClassID = "344b5156-838c-4054-b9bb-0f065a2488f8";
#if SUBNAUTICA
            this.PrefabFileName = "WorldEntities/Eggs/JellyrayEgg.prefab";
#else
            this.PrefabFileName = "WorldEntities/Eggs/Legacy/JellyrayEgg.prefab";
#endif

            this.TechType = TechType.JellyrayEgg;

            KnownTechFixer.AddedNotifications.Add((int)this.TechType, false);

            this.GameObject = new GameObject(this.ClassID);

#if SUBNAUTICA
            this.Recipe = new SMLHelper.V2.Crafting.TechData()
            {
                craftAmount = 1,
                Ingredients = new List<SMLHelper.V2.Crafting.Ingredient>(new SMLHelper.V2.Crafting.Ingredient[1]
                    {
                        new SMLHelper.V2.Crafting.Ingredient(ConfigSwitcher.CreatureEggsResource, ConfigSwitcher.CreatureEggsResourceAmount)
                    }),
            };
#else
            this.Recipe = new SMLHelper.V2.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                    {
                        new Ingredient(ConfigSwitcher.CreatureEggsResource, ConfigSwitcher.CreatureEggsResourceAmount)
                    }),
            };
#endif
        }

        public override void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                // Set unlock conditions
                if (ConfigSwitcher.EnableEggsAtStart || ConfigSwitcher.EnableEggsWhenCreatureScanned)
                    SMLHelper.V2.Handlers.KnownTechHandler.UnlockOnStart(this.TechType);

                // Associate recipe to the new TechType
                SMLHelper.V2.Handlers.CraftDataHandler.SetTechData(this.TechType, this.Recipe);

                // Add the new TechType to the hand-equipments
                SMLHelper.V2.Handlers.CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Hand);

                // Set quick slot type.
                SMLHelper.V2.Handlers.CraftDataHandler.SetQuickSlotType(this.TechType, QuickSlotType.Selectable);

                // Set the buildable prefab
                SMLHelper.V2.Handlers.PrefabHandler.RegisterPrefab(this);

                SMLHelper.V2.Handlers.CraftDataHandler.SetEquipmentType(TechType.JellyrayEggUndiscovered, EquipmentType.Hand);
                SMLHelper.V2.Handlers.CraftDataHandler.SetQuickSlotType(TechType.JellyrayEggUndiscovered, QuickSlotType.Selectable);

                this.IsRegistered = true;
            }
        }

        private static GameObject _eggJellyray = null;

        public override GameObject GetGameObject()
        {
            if (_eggJellyray == null)
                _eggJellyray = PrefabsHelper.LoadGameObjectFromFilename(this.PrefabFileName);

            GameObject prefab = GameObject.Instantiate(_eggJellyray);

            prefab.name = this.ClassID;

            // Update TechTag
            var techTag = prefab.GetComponent<TechTag>();
            if (techTag == null)
                if ((techTag = prefab.GetComponentInChildren<TechTag>()) == null)
                    techTag = prefab.AddComponent<TechTag>();
            techTag.type = this.TechType;

            // Update prefab ID
            var prefabId = prefab.GetComponent<PrefabIdentifier>();
            if (prefabId == null)
                if ((prefabId = prefab.GetComponentInChildren<PrefabIdentifier>()) == null)
                    prefabId = prefab.AddComponent<PrefabIdentifier>();
            prefabId.ClassId = this.ClassID;

            // We can pick this item
            PrefabsHelper.SetDefaultPickupable(prefab);

            // We can place this item
            PrefabsHelper.SetDefaultPlaceTool(prefab);

            // Add fabricating animation
            var fabricating = prefab.AddComponent<VFXFabricating>();
            fabricating.localMinY = -0.2f;
            fabricating.localMaxY = 0.8f;
            fabricating.posOffset = new Vector3(0f, 0.05f, 0.04f);
            fabricating.eulerOffset = new Vector3(0f, 0f, 0f);
            fabricating.scaleFactor = 1f;

            return prefab;
        }
    }
}
