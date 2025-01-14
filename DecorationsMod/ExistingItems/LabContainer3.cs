﻿using System.Collections.Generic;
using UnityEngine;

namespace DecorationsMod.ExistingItems
{
    public class LabContainer3 : DecorationItem
    {
        public LabContainer3() // Feeds abstract class
        {
            this.ClassID = "7f601dd4-0645-414d-bb62-5b0b62985836";
#if SUBNAUTICA
            this.PrefabFileName = "WorldEntities/Doodads/Debris/Wrecks/Decoration/biodome_lab_containers_tube_01.prefab";
#else
            this.PrefabFileName = "WorldEntities/Alterra/Base/biodome_lab_containers_tube_01.prefab";
#endif

            this.TechType = TechType.LabContainer3;

            this.GameObject = new GameObject(this.ClassID);

#if SUBNAUTICA
            this.Recipe = new SMLHelper.V2.Crafting.TechData()
            {
                craftAmount = 1,
                Ingredients = new List<SMLHelper.V2.Crafting.Ingredient>(new SMLHelper.V2.Crafting.Ingredient[1]
                    {
                        new SMLHelper.V2.Crafting.Ingredient(TechType.Glass, 1)
                    }),
            };
#else
            this.Recipe = new SMLHelper.V2.Crafting.RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                    {
                        new Ingredient(TechType.Glass, 1)
                    }),
            };
#endif
        }

        private static GameObject _labContainer3 = null;

        public override GameObject GetGameObject()
        {
            if (_labContainer3 == null)
                _labContainer3 = PrefabsHelper.LoadGameObjectFromFilename(this.PrefabFileName);

            //GameObject prefab = GameObject.Instantiate(this.GameObject);
            GameObject prefab = GameObject.Instantiate(_labContainer3);

            // Add fabricating animation
            var fabricating = prefab.FindChild("biodome_lab_containers_tube_01").AddComponent<VFXFabricating>();
            fabricating.localMinY = -0.1f;
            fabricating.localMaxY = 0.36f;
            fabricating.posOffset = new Vector3(0f, 0f, 0.04f);
            fabricating.eulerOffset = new Vector3(0f, 0f, 0f);
            fabricating.scaleFactor = 1f;

            return prefab;
        }
    }
}
