using Newtonsoft.Json.Utilities;
using RoR2;
using RoR2.EntitlementManagement;
using RoR2.ExpansionManagement;
using RoR2.UI;
using RoR2.UI.LogBook;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client
{

    public static class Logbook_AddRecipes
    {

        public static void Add()
        {
            //+ Recipe Quantity
            //+ No Cycle Images when recipes is about 1 specific one
            //- Duplicate Recipes in same category
            //- Prevent same recipe from showing in both categories
            //- Recipes that result in a DLC item you cant get
            //- Recipes that require a DLC item you cant get

            //TODO HIDE CYCLE IMAGES OF ITEMS OF UNOWNED DLCS
            //Decided against ^

            if (!WConfig.cfgLogbook_Recipes.Value)
            {
                return;
            }


            //Recipe stuff that is unused in game but can easily be reimplemented
            On.RoR2.UI.LogBook.PageBuilder.AddSimplePickup += AddRecipe;
            On.RoR2.UI.RecipeInfoContainer.PopulateRecipes += RemoveUnneededCycleImages;
            On.RoR2.UI.RecipeDisplay.PopulateImages += RemoveDuplicateRecipes;
            On.RoR2.UI.RecipeStrip.PopulateImages += RecipeStrip_PopulateImages;

            //RecipeInfoContainer TopDog
            //RecipeDisplay MiddleDog
            //RecipeStrip BottomDog
            GameObject recipeDisplay = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC3/UI/RecipeDisplayUpdated.prefab").WaitForCompletion();
            recipeDisplay.GetComponentInChildren<RecipeDisplay>().recipeStripPrefab.GetChild(1).GetChild(0).localPosition = new Vector3(-20, 0, 0);

            foreach (var img in recipeDisplay.GetComponentsInChildren<CyclingImage>())
            {
                img.beatBetweenSwaps = 0.8f;
            }


            //For some fucking reason, it just refuses to update the spacing sometimes
            //And cant manually force an update, for some reason
            //So turn off the Scrollbar
            //Which automatically turns on again if needed, which forces an update
            //SURE why not
            //This is a bug with literally EVERY FUCKING THING GAH
            //self.container.parent.parent.GetChild(1).gameObject.SetActive(true);

            On.RoR2.UI.LogBook.LogBookPage.SetEntry += LogBookPage_SetEntry;

        }

        private static void LogBookPage_SetEntry(On.RoR2.UI.LogBook.LogBookPage.orig_SetEntry orig, LogBookPage self, UserProfile userProfile, Entry entry)
        {
            orig(self, userProfile, entry);
            self.pageBuilder.container.parent.parent.GetChild(1).gameObject.SetActive(true);
            self.pageBuilder.container.parent.parent.parent.GetComponent<LayoutGroup>().SetDirty();

        }

        public static void RecipeStrip_PopulateNew(RecipeStrip self, PickupIndex itemRecipesFor, bool usedIn)
        {
            //Bison + Mocha = Dont Cycle, because,
            //Is any of this even relevant for "Created From"
            //Or is Cycle just for Used In
            PickupIndex first = self.recipe.possibleIngredients[0].pickups[0];
            bool isUsed0 = self.recipe.possibleIngredients[0].pickups.IndexOf(first) != -1;
            bool isUsed1 = self.recipe.possibleIngredients[1].pickups.IndexOf(first) != -1;


            bool needsDLCItem = false;

            for (int i = 0; i < self.recipe.possibleIngredients.Length; i++)
            {
                RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(self.imagePrefab);
                rectTransform.gameObject.SetActive(true);
                rectTransform.parent = self.ingredientContainer;

                //Filter out unavilable DLC items
                List<PickupIndex> list = new List<PickupIndex>();
                foreach (var pickup in self.recipe.possibleIngredients[i].pickups)
                {
                    if (PickupAvailable(pickup))
                    {
                        list.Add(pickup);
                    }
                }

                needsDLCItem = needsDLCItem || list.Count == 0;
                rectTransform.GetComponent<CyclingImage>().SetSpritesFromPickupIndices(list.ToArray());
            }
            if (self.resultIcon != null)
            {
                PickupDef pickupDef = PickupCatalog.GetPickupDef(self.recipe.result);
                if (pickupDef != null)
                {
                    self.resultIcon.sprite = pickupDef.iconSprite;
                }
                if (self.resultBackground != null)
                {
                    Image[] componentsInChildren = self.resultBackground.GetComponentsInChildren<Image>();
                    Color darkColor = pickupDef.darkColor;
                    foreach (Image image in componentsInChildren)
                    {
                        float a = image.color.a;
                        image.color = new Color(darkColor.r, darkColor.g, darkColor.b, a);
                    }
                }
            }
        }


        private static void RecipeStrip_PopulateImages(On.RoR2.UI.RecipeStrip.orig_PopulateImages orig, RecipeStrip self)
        {

            //If already populated, Don't
            if (self.transform.GetChild(0).childCount != 2)
            {
                return;
            }
            if (self.recipe == null)
            {
                return;
            }
            if (self.recipe.possibleIngredients == null)
            {
                return;
            }
            orig(self);
            if (self.recipe.amountToDrop > 1)
            {
                GameObject quantity = new GameObject("Quantity");
                quantity.AddComponent<HGTextMeshProUGUI>().text = self.recipe.amountToDrop + "x";
                quantity.GetComponent<HGTextMeshProUGUI>().fontSize = 30;
                quantity.transform.SetParent(self.resultIcon.transform, false);
                quantity.transform.localPosition = new Vector3(24, -8, 0);
            }


        }

        private static void RemoveUnneededCycleImages(On.RoR2.UI.RecipeInfoContainer.orig_PopulateRecipes orig, RecipeInfoContainer self, PickupIndex pickupIndex)
        {
            //ie if Monster Tooth + Steak
            //Dont show Steak + Rotating list of items, just show steak + tooth.

            orig(self, pickupIndex);
            try
            {
                Fixture(self.createdFromDisplay.container, pickupIndex, 1);
                Fixture(self.usedToMakeDisplay.container, pickupIndex, 0);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        public static void Fixture(Transform recipeHolder, PickupIndex pickup, int start)
        {
            for (int i = start; i < recipeHolder.childCount; i++)
            {
                RecipeStrip strip = recipeHolder.GetChild(i).GetComponent<RecipeStrip>();
                if (!strip.recipe.IsUsedInMultipleSlots(pickup))
                {
                    bool isUsed = strip.recipe.possibleIngredients[1].pickups.Length != 1 && strip.recipe.possibleIngredients[1].pickups.IndexOf(pickup) != -1;
                    if (isUsed)
                    {
                        strip.transform.GetChild(0).GetChild(3).GetComponent<CyclingImage>().SetSprites(new Sprite[] { PickupCatalog.GetPickupDef(pickup).iconSprite });
                    }
                    else
                    {
                        isUsed = strip.recipe.possibleIngredients[0].pickups.Length != 1 && strip.recipe.possibleIngredients[0].pickups.IndexOf(pickup) != -1;
                        if (isUsed)
                        {
                            strip.transform.GetChild(0).GetChild(2).GetComponent<CyclingImage>().SetSprites(new Sprite[] { PickupCatalog.GetPickupDef(pickup).iconSprite });
                        }
                    }
                }
            }
        }

        internal static bool PickupAvailable(PickupIndex index)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(index);
            if (pickupDef.itemIndex != ItemIndex.None)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(pickupDef.itemIndex);
                if (itemDef != null)
                {
                    ExpansionDef requiredExpansion = itemDef.requiredExpansion;
                    if (requiredExpansion != null)
                    {
                        return EntitlementManager.localUserEntitlementTracker.AnyUserHasEntitlement(requiredExpansion.requiredEntitlement);
                    }
                }
            }
            else if (pickupDef.equipmentIndex != EquipmentIndex.None)
            {
                EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(pickupDef.equipmentIndex);
                if (equipmentDef != null)
                {
                    ExpansionDef requiredExpansion = equipmentDef.requiredExpansion;
                    if (requiredExpansion != null)
                    {
                        return EntitlementManager.localUserEntitlementTracker.AnyUserHasEntitlement(requiredExpansion.requiredEntitlement);
                    }
                }
            }
            return true;
        }

        private static void RemoveDuplicateRecipes(On.RoR2.UI.RecipeDisplay.orig_PopulateImages orig, RecipeDisplay self, List<CraftableCatalog.RecipeEntry> newRecipeEntries)
        {
            //Remove duplicate entries.

            //If Used to make
            bool used = self.name.StartsWith("U");
            used = false;

            var distinct = newRecipeEntries.Distinct().ToList();
            var newList = new List<CraftableCatalog.RecipeEntry>();
            foreach (CraftableCatalog.RecipeEntry recipeEntry in distinct)
            {
                //If result needs unavailable DLC -> Dont
                if (PickupAvailable(recipeEntry.result))
                {
                    //If isIngredient && isResult
                    //ie to Duplicate scrap
                    //Then only show in "created from"

                    if (!(used && recipeEntry.Required(recipeEntry.result) != 0))
                    {
                        //If ingredient1 is list OR ingredient1 available
                        if (recipeEntry.possibleIngredients[0].pickups.Length != 1 || PickupAvailable(recipeEntry.possibleIngredients[0].pickups[0]))
                        {
                            //If ingredient2 needs unavailable DLC -> Dont
                            if (recipeEntry.possibleIngredients[1].pickups.Length != 1 || PickupAvailable(recipeEntry.possibleIngredients[1].pickups[0]))
                            {
                                newList.Add(recipeEntry);
                            }
                        }
                    }


                }
            }
            if (newList.Count > 1)
            {
                newList.Sort((x, y) => x.RecipeComparer(y));
            }

            orig(self, newList);
            if (self.container)
            {
                for (int i = 0; i < self.container.childCount; i++)
                {
                    self.container.GetChild(i).GetComponent<RecipeStrip>().PopulateImages();
                }
            }

        }

        private static void AddRecipe(On.RoR2.UI.LogBook.PageBuilder.orig_AddSimplePickup orig, PageBuilder self, PickupIndex pickupIndex)
        {
            orig(self, pickupIndex);
            //self.AddRecipeDisplay(pickupIndex); //Does not work cuz wrong asset path
            GameObject RecipeDisplay = self.AddPrefabInstance(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC3/UI/RecipeDisplayUpdated.prefab").WaitForCompletion());

            RecipeDisplay.GetComponent<RecipeInfoContainer>().PopulateRecipes(pickupIndex);
            RecipeDisplay.GetComponent<Image>().color = new Color(0.3255f, 0.4039f, 0.4706f, 0.8706f);
            RecipeDisplay.GetComponent<Image>().sprite = Addressables.LoadAssetAsync<Sprite>("03f90788916529e459ba4d7f40bb32ee").WaitForCompletion();
            RecipeDisplay.SetActive(CraftableCatalog.resultToRecipeSearchTable.ContainsKey(pickupIndex) || CraftableCatalog.pickupToIngredientSearchTable.ContainsKey(pickupIndex));

        }

        public static int RecipeComparer(this CraftableCatalog.RecipeEntry first, in CraftableCatalog.RecipeEntry other)
        {
            PickupDef def1 = null;
            PickupDef def2 = null;

            if (first.result != other.result)
            {
                def1 = PickupCatalog.GetPickupDef(first.result);
                def2 = PickupCatalog.GetPickupDef(other.result);
            }
            else if (first.possibleIngredients[0].pickups[0] != other.possibleIngredients[0].pickups[0])
            {
                def1 = PickupCatalog.GetPickupDef(first.possibleIngredients[0].pickups[0]);
                def2 = PickupCatalog.GetPickupDef(other.possibleIngredients[0].pickups[0]);
            }
            else if (first.possibleIngredients[1].pickups[0] != other.possibleIngredients[1].pickups[0])
            {
                def1 = PickupCatalog.GetPickupDef(first.possibleIngredients[1].pickups[0]);
                def2 = PickupCatalog.GetPickupDef(other.possibleIngredients[1].pickups[0]);
            }
            if (def1 == null)
            {
                return 0;
            }

            //If both are items, compare ItemTier
            if (def1.itemIndex != ItemIndex.None && def2.itemIndex != ItemIndex.None)
            {
                int compareTier = def2.itemTier.CompareTo(def1.itemTier);
                if (compareTier == 0)
                {
                    //If both are the same tier, compare internalName
                    return def1.internalName.CompareTo(def2.internalName);
                }
                return compareTier;
            }
            else if (def1.equipmentIndex != EquipmentIndex.None && def2.equipmentIndex != EquipmentIndex.None)
            {
                if (def1.isBoss && !def2.isBoss)
                {
                    return -1;
                }
                else if (def1.isLunar && !def2.isLunar)
                {
                    return -1;
                }
                else
                {
                    return def1.internalName.CompareTo(def2.internalName);
                }
            }
            else
            {
                //Just sort by PickupIndex, which is item first equipment second anyhow.
                if (first.result < other.result)
                {
                    return -1;
                }
                if (first.result > other.result)
                {
                    return 1;
                }
            }
            return 0;
        }

    }

}