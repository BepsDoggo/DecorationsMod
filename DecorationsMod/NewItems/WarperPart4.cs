﻿using DecorationsMod.Controllers;
using DecorationsMod.Fixers;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DecorationsMod.NewItems
{
    public class WarperPart4 : DecorationItem
    {
        public WarperPart4()
        {
            this.ClassID = "WarperPart4";
            this.PrefabFileName = DecorationItem.DefaultResourcePath + this.ClassID;

            //this.GameObject = AssetsHelper.Assets.LoadAsset<GameObject>("warper_part_4");
            this.GameObject = new GameObject(this.ClassID);

            this.TechType = SMLHelper.V2.Handlers.TechTypeHandler.AddTechType(this.ClassID,
                                                        LanguageHelper.GetFriendlyWord("HangingWarperPartName") + " (3)",
                                                        LanguageHelper.GetFriendlyWord("HangingWarperPartDescription"),
                                                        true);

            CrafterLogicFixer.WarperPart4 = this.TechType;
            KnownTechFixer.AddedNotifications.Add((int)this.TechType, false);

#if SUBNAUTICA
            this.Recipe = new SMLHelper.V2.Crafting.TechData()
            {
                craftAmount = 1,
                Ingredients = new List<SMLHelper.V2.Crafting.Ingredient>(new SMLHelper.V2.Crafting.Ingredient[3]
                    {
                        new SMLHelper.V2.Crafting.Ingredient(TechType.Titanium, 1),
                        new SMLHelper.V2.Crafting.Ingredient(TechType.Silicone, 1),
                        new SMLHelper.V2.Crafting.Ingredient(TechType.FiberMesh, 1)
                    }),
            };
#else
            this.Recipe = new SMLHelper.V2.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[3]
                    {
                        new Ingredient(TechType.Titanium, 1),
                        new Ingredient(TechType.Silicone, 1),
                        new Ingredient(TechType.FiberMesh, 1)
                    }),
            };
#endif
        }

        private static GameObject _warperPart4 = null;

        public override void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                if (_warperPart4 == null)
                    _warperPart4 = AssetsHelper.Assets.LoadAsset<GameObject>("warper_part_4");

                GameObject model = _warperPart4.FindChild("Model");

                // Rotate model
                model.transform.localEulerAngles = new Vector3(model.transform.localEulerAngles.x, model.transform.localEulerAngles.y - 180f, model.transform.localEulerAngles.z);

                // Set tech tag
                var techTag = _warperPart4.AddComponent<TechTag>();
                techTag.type = this.TechType;

                // Add prefab identifier
                _warperPart4.AddComponent<PrefabIdentifier>().ClassId = this.ClassID;

                // Add collider
                var collider = _warperPart4.AddComponent<BoxCollider>();
                collider.size = new Vector3(0.1f, 0.5f, 0.1f);
                collider.center = new Vector3(collider.center.x, collider.center.y - 0.25f, collider.center.z);

                // Set proper shaders (for crafting animation)
                Shader marmosetUber = Shader.Find("MarmosetUBER");
                Texture normal1 = AssetsHelper.Assets.LoadAsset<Texture>("precursor_lab_warper_normal");
                Texture spec1 = AssetsHelper.Assets.LoadAsset<Texture>("precursor_lab_warper_spec");
                Texture illum1 = AssetsHelper.Assets.LoadAsset<Texture>("precursor_lab_warper_illum");
                Texture normal2 = AssetsHelper.Assets.LoadAsset<Texture>("warper_normal");
                Texture spec2 = AssetsHelper.Assets.LoadAsset<Texture>("warper_spec");
                Texture illum2 = AssetsHelper.Assets.LoadAsset<Texture>("warper_illum");
                Texture normal3 = AssetsHelper.Assets.LoadAsset<Texture>("warper_entrails_normal");
                Texture spec3 = AssetsHelper.Assets.LoadAsset<Texture>("warper_entrails_spec");
                Texture illum3 = AssetsHelper.Assets.LoadAsset<Texture>("warper_entrails_illum");
                var renderers = _warperPart4.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers)
                    if (rend.materials.Length > 0)
                        foreach (Material tmpMat in rend.materials)
                        {
                            tmpMat.shader = marmosetUber;
                            if (string.Compare(tmpMat.name, "precursor_lab_warper (Instance)", true, CultureInfo.InvariantCulture) == 0)
                            {
                                tmpMat.SetTexture("_SpecTex", spec1);
                                tmpMat.SetTexture("_BumpMap", normal1);
                                tmpMat.SetTexture("_Illum", illum1);
                                tmpMat.SetFloat("_EmissionLM", 1f);

                                tmpMat.EnableKeyword("MARMO_SPECMAP");
                                tmpMat.EnableKeyword("MARMO_NORMALMAP");
                                tmpMat.EnableKeyword("MARMO_EMISSION");
                                tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
                            }
                            else if (string.Compare(tmpMat.name, "Warper (Instance)", true, CultureInfo.InvariantCulture) == 0 || 
                                string.Compare(tmpMat.name, "Warper_alpha (Instance)", true, CultureInfo.InvariantCulture) == 0)
                            {
                                tmpMat.SetTexture("_SpecTex", spec2);
                                tmpMat.SetTexture("_BumpMap", normal2);
                                tmpMat.SetTexture("_Illum", illum2);
                                tmpMat.SetFloat("_EmissionLM", 1f);

                                tmpMat.EnableKeyword("MARMO_SPECMAP");
                                tmpMat.EnableKeyword("MARMO_NORMALMAP");
                                tmpMat.EnableKeyword("MARMO_EMISSION");
                                tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
                            }
                            else if (string.Compare(tmpMat.name, "warper_entrails (Instance)", true, CultureInfo.InvariantCulture) == 0)
                            {
                                tmpMat.SetTexture("_SpecTex", spec3);
                                tmpMat.SetTexture("_BumpMap", normal3);
                                tmpMat.SetTexture("_Illum", illum3);
                                tmpMat.SetFloat("_EmissionLM", 1f);

                                tmpMat.EnableKeyword("MARMO_SPECMAP");
                                tmpMat.EnableKeyword("MARMO_NORMALMAP");
                                tmpMat.EnableKeyword("MARMO_EMISSION");
                                tmpMat.EnableKeyword("_ZWRITE_ON"); // Enable Z write
                            }
                        }

                // Add large world entity
                PrefabsHelper.SetDefaultLargeWorldEntity(_warperPart4);

                // Add sky applier
                PrefabsHelper.SetDefaultSkyApplier(_warperPart4, renderers);

                // We can pick this item
                PrefabsHelper.SetDefaultPickupable(_warperPart4);

                // We can place this item
                _warperPart4.AddComponent<CustomPlaceToolController>();
                var placeTool = _warperPart4.AddComponent<GenericPlaceTool>();
                placeTool.allowedInBase = true;
                placeTool.allowedOnCeiling = true;
                placeTool.allowedOnConstructable = true;
                placeTool.allowedOnRigidBody = true;
                placeTool.allowedOnWalls = true;
                placeTool.allowedOnBase = false;
                placeTool.allowedOnGround = false;
                placeTool.allowedOutside = ConfigSwitcher.AllowPlaceOutside;
                placeTool.rotationEnabled = true;
                placeTool.enabled = true;
                placeTool.hasAnimations = false;
                placeTool.hasBashAnimation = false;
                placeTool.hasFirstUseAnimation = false;
                placeTool.ghostModelPrefab = _warperPart4;
                placeTool.mainCollider = collider;
                placeTool.pickupable = _warperPart4.GetComponent<Pickupable>();
                placeTool.drawTime = 0.5f;
                placeTool.dropTime = 1;
                placeTool.holsterTime = 0.35f;

                // Define unlock conditions
                if (ConfigSwitcher.AddItemsWhenDiscovered)
                    SMLHelper.V2.Handlers.KnownTechHandler.SetAnalysisTechEntry(TechType.PrecursorLostRiverWarperParts, new TechType[] { this.TechType });

                // Associate recipe to the new TechType
                SMLHelper.V2.Handlers.CraftDataHandler.SetTechData(this.TechType, this.Recipe);

                // Add the new TechType to Hand Equipment type.
                SMLHelper.V2.Handlers.CraftDataHandler.SetEquipmentType(this.TechType, EquipmentType.Hand);

                // Set quick slot type.
                SMLHelper.V2.Handlers.CraftDataHandler.SetQuickSlotType(this.TechType, QuickSlotType.Selectable);

                // Set the buildable prefab
                SMLHelper.V2.Handlers.PrefabHandler.RegisterPrefab(this);

                // Set the custom sprite
                SMLHelper.V2.Handlers.SpriteHandler.RegisterSprite(this.TechType, AssetsHelper.Assets.LoadAsset<Sprite>("warper_icon_4"));

                this.IsRegistered = true;
            }
        }

        public override GameObject GetGameObject()
        {
            GameObject prefab = GameObject.Instantiate(_warperPart4);

            prefab.name = this.ClassID;

            // Add fabricating animation
            var fabricatingA = prefab.AddComponent<VFXFabricating>();
            fabricatingA.localMinY = -0.2f;
            fabricatingA.localMaxY = 0.3f;
            fabricatingA.posOffset = new Vector3(0.3f, 0.09f, 0.04f);
            fabricatingA.eulerOffset = new Vector3(90f, 90f, 0f);
            fabricatingA.scaleFactor = 1f;

            return prefab;
        }
    }
}
