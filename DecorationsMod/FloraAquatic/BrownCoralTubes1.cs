﻿using DecorationsMod.Controllers;
using DecorationsMod.Fixers;
using System.Collections.Generic;
using UnityEngine;

namespace DecorationsMod.FloraAquatic
{
    public class BrownCoralTubes1 : DecorationItem, ICustomFlora
    {
        public CustomFlora Config = new CustomFlora();
        CustomFlora ICustomFlora.Config
        {
            get => this.Config;
            set => this.Config = value;
        }

        public BrownCoralTubes1()
        {
            this.ClassID = "BrownCoralTubes1"; // 361bea51-183b-4eab-998d-fd61d07d6a65
            this.PrefabFileName = DecorationItem.DefaultResourcePath + this.ClassID;

            this.GameObject = new GameObject(this.ClassID);

            this.TechType = SMLHelper.V2.Handlers.TechTypeHandler.AddTechType(this.ClassID,
                                                        LanguageHelper.GetFriendlyWord("BrownCoralTubesName") + " (1)",
                                                        LanguageHelper.GetFriendlyWord("BrownCoralTubesDescription"),
                                                        true);

            CrafterLogicFixer.BrownTubes1 = this.TechType;
            KnownTechFixer.AddedNotifications.Add((int)this.TechType, false);

#if SUBNAUTICA
            this.Recipe = new SMLHelper.V2.Crafting.TechData()
            {
                craftAmount = 1,
                Ingredients = new List<SMLHelper.V2.Crafting.Ingredient>(new SMLHelper.V2.Crafting.Ingredient[1]
                {
                    new SMLHelper.V2.Crafting.Ingredient(ConfigSwitcher.FloraRecipiesResource, ConfigSwitcher.FloraRecipiesResourceAmount)
                }),
            };
#else
            this.Recipe = new SMLHelper.V2.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                    new Ingredient(ConfigSwitcher.FloraRecipiesResource, ConfigSwitcher.FloraRecipiesResourceAmount)
                }),
            };
#endif

            this.Config = ConfigSwitcher.config_BrownCoralTubes1;
        }

        public override void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                // Associate recipe to the new TechType
                SMLHelper.V2.Handlers.CraftDataHandler.SetTechData(this.TechType, this.Recipe);

                // Set item occupies 1 slot
                SMLHelper.V2.Handlers.CraftDataHandler.SetItemSize(this.TechType, new Vector2int(1, 1));

                // Add the new TechType to Harvest types
                SMLHelper.V2.Handlers.CraftDataHandler.SetHarvestType(this.TechType, HarvestType.DamageAlive);
                SMLHelper.V2.Handlers.CraftDataHandler.SetHarvestOutput(this.TechType, this.TechType);

                // Change item background to water-plant seed
                SMLHelper.V2.Handlers.CraftDataHandler.SetBackgroundType(this.TechType, CraftData.BackgroundType.PlantWaterSeed);

                // Set item bioreactor charge
                BaseBioReactorHelper.SetBioReactorCharge(this.TechType, this.Config.Charge);

                // Specify bonus on final cut
                SMLHelper.V2.Handlers.CraftDataHandler.SetHarvestFinalCutBonus(this.TechType, 1);

                // Set the buildable prefab
                SMLHelper.V2.Handlers.PrefabHandler.RegisterPrefab(this);

                // Set the custom sprite
                SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, AssetsHelper.Assets.LoadAsset<Sprite>("flora_browncoraltubes0203icon"));

                this.IsRegistered = true;
            }
        }

        private static GameObject _brownCoralTubes1 = null;

        public override GameObject GetGameObject()
        {
            if (_brownCoralTubes1 == null)
#if SUBNAUTICA
                _brownCoralTubes1 = PrefabsHelper.LoadGameObjectFromFilename("WorldEntities/Doodads/Coral_reef/coral_reef_brown_coral_tubes_02_03.prefab");
#else
                _brownCoralTubes1 = PrefabsHelper.LoadGameObjectFromFilename("WorldEntities/flora/shared/coral_reef_brown_coral_tubes_02_03.prefab");
#endif

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T1");
#endif
            GameObject prefab = GameObject.Instantiate(_brownCoralTubes1);

            prefab.name = this.ClassID;

            PrefabsHelper.AddNewGenericSeed(ref prefab);

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T2");
            Logger.PrintTransform(prefab.transform);
            Logger.Log("DEBUG: BrownCoralTube1 T2b");
#endif
            // Scale models
            prefab.FindChild("coral_reef_brown_coral_tubes_02_03").transform.localScale *= 0.4f;
            prefab.FindChild("coral_reef_brown_coral_tubes_02_03_LOD3").transform.localScale *= 0.4f;

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T3");
#endif
            // Scale and shrink colliders
            BoxCollider[] colliders = prefab.GetComponentsInChildren<BoxCollider>();
            if (colliders.Length > 0)
            {
                foreach (BoxCollider collider in colliders)
                {
                    collider.size *= 0.4f;
                    collider.size *= 0.001f;
                }
            }

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T4");
#endif
            // Update rigid body
            var rb = prefab.GetComponent<Rigidbody>();
            rb.mass = 0.5f;
            rb.drag = 1.0f;
            rb.angularDrag = 1.0f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.constraints = RigidbodyConstraints.None;

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T5");
#endif
            // Add EntityTag
            var entityTag = prefab.AddComponent<EntityTag>();
            entityTag.slotType = EntitySlot.Type.Small;

            // Add TechTag
            var techTag = prefab.AddComponent<TechTag>();
            techTag.type = this.TechType;

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T6");
#endif
            // Update prefab identifier
            var prefabId = prefab.GetComponent<PrefabIdentifier>();
            prefabId.ClassId = this.ClassID;

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T7");
#endif
            // Update large world entity
            var lwe = prefab.GetComponent<LargeWorldEntity>();
            lwe.cellLevel = LargeWorldEntity.CellLevel.Near;

            // Add box collider
            //BoxCollider collider = prefab.AddComponent<BoxCollider>();
            //collider.size = new Vector3(0.2f, 0.2f, 0.2f);

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T8");
#endif
            // Add world forces
            var worldForces = prefab.AddComponent<WorldForces>();
            worldForces.handleGravity = true;
            worldForces.aboveWaterGravity = 9.81f;
            worldForces.underwaterGravity = 1.0f;
            worldForces.handleDrag = true;
            worldForces.aboveWaterDrag = 0.0f;
            worldForces.underwaterDrag = 10.0f;
            worldForces.useRigidbody = rb;

            // Add pickupable
            var pickupable = prefab.AddComponent<Pickupable>();
            pickupable.isPickupable = false;
            pickupable.destroyOnDeath = true;
            pickupable.isLootCube = false;
            pickupable.randomizeRotationWhenDropped = true;
            pickupable.usePackUpIcon = false;

            // Add eatable
            Eatable eatable = null;
            if (Config.Eatable)
            {
                eatable = prefab.AddComponent<Eatable>();
                eatable.foodValue = Config.FoodValue;
                eatable.waterValue = Config.WaterValue;
#if SUBNAUTICA
                eatable.stomachVolume = 10.0f;
                eatable.allowOverfill = false;
#endif
                eatable.decomposes = Config.Decomposes;
                eatable.kDecayRate = Config.KDecayRate;
                eatable.despawns = Config.Despawns;
                eatable.despawnDelay = Config.DespawnDelay;
            }

            // Add plantable
            var plantable = prefab.AddComponent<Plantable>();
            plantable.aboveWater = false;
            plantable.underwater = true;
            plantable.isSeedling = true;
            plantable.plantTechType = this.TechType;
            plantable.size = Plantable.PlantSize.Small;
            plantable.pickupable = pickupable;
            plantable.eatable = eatable;
            plantable.model = prefab;
            plantable.linkedGrownPlant = new GrownPlant();
            plantable.linkedGrownPlant.seed = plantable;
            plantable.linkedGrownPlant.seedUID = "BrownCoralTubes1";

            // Add generic plant controller
            PlantGenericController landPlant1Controller = prefab.AddComponent<PlantGenericController>();
            landPlant1Controller.GrowthDuration = Config.GrowthDuration;
            landPlant1Controller.Health = Config.Health;
            landPlant1Controller.Knifeable = Config.Knifeable;
            landPlant1Controller.RestoreBoxColliders = true;

            // Add flora serializer/deserializer
            CustomFloraSerializer customSerializer = prefab.AddComponent<CustomFloraSerializer>();

            // Add live mixin
            var liveMixin = prefab.AddComponent<LiveMixin>();
            liveMixin.health = Config.Health;
            liveMixin.data = ScriptableObject.CreateInstance<LiveMixinData>();
            liveMixin.data.broadcastKillOnDeath = false;
            liveMixin.data.canResurrect = false;
            liveMixin.data.destroyOnDeath = true;
            liveMixin.data.invincibleInCreative = false;
            liveMixin.data.minDamageForSound = 10.0f;
            liveMixin.data.passDamageDataOnDeath = true;
            liveMixin.data.weldable = false;
            liveMixin.data.knifeable = false;
            liveMixin.data.maxHealth = Config.Health;
            //liveMixin.startHealthPercent = 1.0f;

#if DEBUG_CORALS
            Logger.Log("DEBUG: BrownCoralTube1 T9");
#endif
            // Hide plant and show seed
            PrefabsHelper.HidePlantAndShowSeed(prefab.transform, this.ClassID);

            return prefab;
        }
    }
}
