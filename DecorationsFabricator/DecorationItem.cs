﻿using SMLHelper;
using SMLHelper.Patchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DecorationsFabricator
{
    public abstract class DecorationItem
    {
        #region Attributes

        // This is used as the default path when we add a new resource to the game
        public const string DefaultResourcePath = "WorldEntities/Environment/Wrecks/";

        // This is used to know if we already registered our item in the game
        public bool IsRegistered = false;

        // The item class ID
        public string ClassID { get; set; }

        // The item resource path
        public string ResourcePath { get; set; }
        
        // The item root GameObject
        public GameObject GameObject { get; set; }

        // The item TechType
        public TechType TechType { get; set; }

        // The item recipe
        public TechDataHelper Recipe { get; set; }

        #endregion
        #region Abstract and virtual methods

        public abstract GameObject GetPrefab();

        public virtual void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                // Update PlaceTool parameters
                var placeTool = this.GameObject.GetComponent<PlaceTool>();
                if (placeTool != null)
                {
                    if (this.TechType == TechType.Poster ||
                        this.TechType == TechType.PosterAurora ||
                        this.TechType == TechType.PosterExoSuit1 ||
                        this.TechType == TechType.PosterExoSuit2 ||
                        this.TechType == TechType.PosterKitty)
                    {
                        placeTool.allowedInBase = true;
                        placeTool.allowedOnBase = true;
                        placeTool.allowedOnCeiling = false;
                        placeTool.allowedOnConstructable = true;
                        placeTool.allowedOnGround = false;
                        placeTool.allowedOnRigidBody = true;
                        placeTool.allowedOnWalls = true;
                        placeTool.allowedOutside = false;
                        placeTool.rotationEnabled = true;
                        placeTool.enabled = true;
                        placeTool.hasAnimations = false;
                        placeTool.hasBashAnimation = false;
                        placeTool.hasFirstUseAnimation = false;
                    }
                    else
                    {
                        placeTool.allowedInBase = true;
                        placeTool.allowedOnBase = true;
                        placeTool.allowedOnCeiling = false;
                        placeTool.allowedOnConstructable = true;
                        placeTool.allowedOnGround = true;
                        placeTool.allowedOnRigidBody = true;
                        placeTool.allowedOnWalls = false;
                        placeTool.allowedOutside = false;
                        placeTool.rotationEnabled = true;
                        placeTool.enabled = true;
                    }
                }

                // Set the buildable prefab
                CustomPrefabHandler.customPrefabs.Add(new CustomPrefab(this.ClassID, this.ResourcePath, this.TechType, this.GetPrefab));

                // Associate new recipe
                CraftDataPatcher.customTechData[this.TechType] = this.Recipe;

                this.IsRegistered = true;
            }
        }

        #endregion
    }
}