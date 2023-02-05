/*       
          if (LysateCellCaptain.Value == true)
          {
              On.RoR2.CaptainDefenseMatrixController.TryGrantItem += (orig, self) =>
              {
                  orig(self);
                  CaptainSupplyDropController supplyController = self.characterBody.GetComponent<CaptainSupplyDropController>();
                  if (supplyController)
                  {
                      supplyController.CallCmdSetSkillMask(3);
                      if (self.characterBody.master.inventory)
                      {
                          int lysateCount = self.characterBody.master.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                          if (lysateCount > 0)
                          {
                              if (lysateCount % 2 == 0)
                              {
                                  //Debug.LogWarning("Number is even");
                                  supplyController.supplyDrop1Skill.bonusStockFromBody = lysateCount / 2;
                                  supplyController.supplyDrop2Skill.bonusStockFromBody = lysateCount / 2;
                                  //supplyController.supplyDrop1Skill.maxStock = supplyController.supplyDrop1Skill.skillDef.baseMaxStock + supplyController.supplyDrop1Skill.bonusStockFromBody;
                                  //supplyController.supplyDrop2Skill.maxStock = supplyController.supplyDrop2Skill.skillDef.baseMaxStock + supplyController.supplyDrop2Skill.bonusStockFromBody;
                              }
                              else
                              {
                                  //Debug.LogWarning("Number is odd");
                                  supplyController.supplyDrop1Skill.bonusStockFromBody = lysateCount / 2 + 1;
                                  supplyController.supplyDrop2Skill.bonusStockFromBody = lysateCount / 2;
                                  //supplyController.supplyDrop1Skill.maxStock = supplyController.supplyDrop1Skill.skillDef.baseMaxStock + supplyController.supplyDrop1Skill.bonusStockFromBody;
                                  //supplyController.supplyDrop2Skill.maxStock = supplyController.supplyDrop2Skill.skillDef.baseMaxStock + supplyController.supplyDrop2Skill.bonusStockFromBody;
                              }
                          }
                      }
                  }


              };

              On.EntityStates.Captain.Weapon.SetupSupplyDrop.OnExit += (orig, self) =>
              {
                  orig(self);
                  //Debug.LogWarning(self);
                  if (self.skillLocator.allSkills.Length > 5)
                  {
                      self.skillLocator.allSkills[5].SetBonusStockFromBody(self.skillLocator.allSkills[5].stock - 1);
                  }

              };

          }
           */

/*
On.RoR2.GenericPickupController.GrantEquipment += (orig, self, body, inventory) =>
{

    EquipmentIndex tempeq = PickupCatalog.GetPickupDef(self.pickupIndex).equipmentIndex;
    if (body.isPlayerControlled == true && tempeq != inventory.currentEquipmentIndex)
    {
        On.RoR2.CharacterModel.SetEquipmentDisplay += EquipmentHighlighter;
        body.modelLocator.modelTransform.gameObject.GetComponent<CharacterModel>().UpdateItemDisplay(inventory);
    }
    orig(self, body, inventory);
    //Debug.LogWarning("Pickup "+tempeq);
    //Debug.LogWarning("Current "+inventory.currentEquipmentIndex);
};
*/

/*
On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
{
    orig(self);
    if (self.inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment) > 0)
    {
        while (self.inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment) > 0)
        {
            self.inventory.RemoveItem(RoR2Content.Items.AutoCastEquipment);
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(ItemCatalog.FindItemIndex("AutoCastEquipment")), self.gameObject.transform.position, new Vector3 (0,15,0));
        }
    }
};
*/
/*
 * 
            /*
           On.EntityStates.BasicMeleeAttack.AuthorityFireAttack += MercStepMethod;

           On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += (orig, self) =>
           {
               orig(self);

           };

           */
/*
On.EntityStates.Merc.WhirlwindBase.OnEnter += (orig, self) =>
{
    orig(self);
    self.SetFieldValue("duration", self.baseDuration);
};
*/

/*
Debug.LogWarning(self);
Debug.LogWarning(self.outer);
Debug.LogWarning(self.outer.transform);
Debug.LogWarning(self.outer.transform.position);
Debug.LogWarning(self.outer);



DirectorCore instance = DirectorCore.instance;
if (instance)
{
    Xoroshiro128Plus rng = new Xoroshiro128Plus((ulong)Run.instance.stageRng.nextUint);
    DirectorPlacementRule placementRule = new DirectorPlacementRule
    {
        placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
        position = self.outer.gameObject.transform.position,
        minDistance = 0f,
        maxDistance = float.PositiveInfinity
    };
    instance.TrySpawnObject(new DirectorSpawnRequest(self.spawnCard, placementRule, rng));
}
*/

/*
On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += (orig, self) =>
{
    orig(self);

    GameObject tempmsportal = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/networkedobjects/PortalMS"));
    tempmsportal.transform.position = new Vector3(21f, -7f, 23f);
    tempmsportal.GetComponent<RoR2.ObjectScaleCurve>().baseScale = new Vector3(1.05f, 1.89f, 1.05f);
    tempmsportal.GetComponent<RoR2.SceneExitController>().useRunNextStageScene = true;
};
*/


/*Updates every frame even without the item
            On.RoR2.CharacterBody.UpdateTeslaCoil += (orig, self) =>
            {
                orig(self);
                


                Debug.LogWarning("UpdateTeslaCoil");
                if (self.HasBuff(RoR2Content.Buffs.TeslaField))
                {
                    self.AddTimedBuff(RoR2Content.Buffs.TeslaField, 10);
                }
            };

//only starts once
            On.RoR2.TeslaCoilAnimator.Start += (orig, self) =>
            {
                orig(self);
                Debug.LogWarning("Tesla Coil Animator Start");
                CharacterBody tempbody = self.GetFieldValue<CharacterBody>("characterBody");
                if (tempbody && tempbody.HasBuff(RoR2Content.Buffs.TeslaField))
                {
                    tempbody.AddTimedBuff(RoR2Content.Buffs.TeslaField, 10);
                }
            };

*/


//Stupid fucking idiot red sword

/*
On.RoR2.CharacterModel.OnEnable += (orig, self) =>
{
    orig(self);

    Debug.LogWarning(self.name);
    Debug.LogWarning(self.body);
    Debug.LogWarning(self.body.skinIndex);
    if (self.name == "mdlMerc" && self.body && self.body.skinIndex == 1)
    {
        self.baseLightInfos[0].defaultColor = new Color(1, 0.1f, 0f, 1);
        self.baseLightInfos[1].defaultColor = new Color(1, 0.1f, 0f, 1);
        self.baseRendererInfos[1].defaultMaterial = SwordMat;
    }
};
*/


/*
GameObject MercBody = Resources.Load<GameObject>("prefabs/characterbodies/MercBody");



GameObject OGRedSwordInstantiate = R2API.PrefabAPI.InstantiateClone(MercBody.transform.GetChild(0).GetChild(0).GetChild(2).gameObject, "OniMercSwordMesh",false);
Instantiate(OGRedSwordInstantiate, MercBody.transform.GetChild(0).GetChild(0));
//RedSwordInstantiate.SetActive(false);


GameObject LightBack = R2API.PrefabAPI.InstantiateClone(MercBody.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject, "OniPointLight", false);
LightBack.transform.SetParent(MercBody.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0));
LightBack.SetActive(false);


GameObject LightSword = R2API.PrefabAPI.InstantiateClone(MercBody.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0).GetChild(4).GetChild(0).GetChild(6).gameObject, "OniPointLight", false);
LightSword.transform.SetParent(MercBody.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0).GetChild(4).GetChild(0));
LightSword.SetActive(false);




SkinnedMeshRenderer MercSwordRenderer = MercBody.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>();

GameObject MercDisplay = Resources.Load<GameObject>("prefabs/characterdisplays/MercDisplay");
SkinnedMeshRenderer MercDisplaySwordRenderer = MercDisplay.transform.GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>();


Mesh RedSwordMesh = Instantiate(MercSwordRenderer.sharedMesh);


SkinDef OniMerc = MercBody.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1];
OniSwordMat = Instantiate(OniMerc.rendererInfos[1].defaultMaterial);
*/

/*
MercSwordRenderer.material = SwordMat;
MercSwordRenderer.sharedMaterial = SwordMat;

MercDisplaySwordRenderer.material = SwordMat;
MercDisplaySwordRenderer.sharedMaterial = SwordMat;

OniMerc.rendererInfos[1].defaultMaterial = SwordMat;

OGRedSwordInstantiate.GetComponent<SkinnedMeshRenderer>().material = SwordMat;
OGRedSwordInstantiate.GetComponent<SkinnedMeshRenderer>().sharedMaterial = SwordMat;
*/


/*
GameObject SoupGreenRed = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant");
GameObject SoupRedWhite = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant");

UnityEngine.Events.UnityEvent SoupGRPurchase = SoupGreenRed.GetComponent<RoR2.EntityLogic.DelayedEvent>().action;
UnityEngine.Events.UnityEvent SoupRWPurchase = SoupGreenRed.GetComponent<RoR2.EntityLogic.DelayedEvent>().action;

SoupRWPurchase.AddPersistentListener()
    */




/*
On.RoR2.CharacterModel.HighlightItemDisplay += (orig, self, itemIndex) =>
{
   ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
   if (itemDef != null && itemDef.tier == ItemTier.Boss)
   {

       typeof(CharacterModel).GetMethod("parentedPrefabDisplays", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new CharacterModel(), null).GetFieldValue;

       for (int i = this.parentedPrefabDisplays.Count - 1; i >= 0; i--)
       {
           if (this.parentedPrefabDisplays[i].itemIndex == itemIndex)
           {
               GameObject instance = this.parentedPrefabDisplays[i].instance;
               if (instance)
               {
                   Renderer componentInChildren = instance.GetComponentInChildren<Renderer>();
                   if (componentInChildren && self.body)
                   {
                       RoR2.UI.HighlightRect.CreateHighlight(self.body.gameObject, componentInChildren, Resources.Load<GameObject>(path), -1f, false);
                   }
               }
           }
       }

}
    else
    {
        orig(self, itemIndex);
    }
};
*/

/*
 *             On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += (orig, self) =>
            {
                float tempattack = self.outer.gameObject.GetComponent<CharacterBody>().attackSpeed;
                //EntityStates.Merc.Weapon.GroundLight2.baseDurationBeforeInterruptable = 0.45f * tempattack;
                //EntityStates.Merc.Weapon.GroundLight2.comboFinisherBaseDurationBeforeInterruptable = 0.85f * tempattack;
                orig(self);
            };
*/












/*


   if (MountainShrineStack.Value == true)
            {
                On.RoR2.TeleporterInteraction.AddShrineStack += (orig, self) =>
                {
                    orig(self);

                    GameObject tempbossicon = self.GetFieldValue<GameObject>("bossShrineIndicator");
                    if (tempbossicon.transform.childCount == 0)
                    {
                        GameObject tempbossiconclone = Instantiate(tempbossicon, tempbossicon.transform);

                        Destroy(tempbossicon.GetComponent<MeshRenderer>());
                        Destroy(tempbossicon.GetComponent<Billboard>());
                        tempbossicon.transform.localPosition = new Vector3(0, 0, 6);
                        tempbossiconclone.transform.localScale = new Vector3(1, 1, 1);
                        tempbossiconclone.transform.localPosition = new Vector3(0, 0, 0);
                        tempbossiconclone.SetActive(true);
                    }

                    if (self.shrineBonusStacks > 1)
                    {
                        tempbossicon = self.GetFieldValue<GameObject>("bossShrineIndicator").transform.GetChild(0).gameObject;


                        for (int i = tempbossicon.transform.parent.childCount; i < self.shrineBonusStacks; i++)
                        {
                            GameObject tempbossiconclone = Instantiate(tempbossicon, tempbossicon.transform.parent);
                            //tempbossiconclone.transform.localPosition = new Vector3(0, (self.shrineBonusStacks - 1), 0);
                            tempbossiconclone.transform.localPosition = new Vector3(0, (tempbossicon.transform.parent.childCount - 1), 0);
                        }
                    }

                };


            }






public static void RunStartMethod(On.RoR2.Run.orig_Start orig, Run self)
{

    orig(self);


    foreach (var playerController in PlayerCharacterMasterController.instances)
    {
        Debug.LogWarning(playerController.master.bodyPrefab);
        if (playerController.master.bodyPrefab.name.Contains("Heretic"))
        {
            playerController.master.inventory.GiveItem(RoR2Content.Items.LunarPrimaryReplacement);
            playerController.master.inventory.GiveItem(RoR2Content.Items.LunarSecondaryReplacement);
            playerController.master.inventory.GiveItem(RoR2Content.Items.LunarUtilityReplacement);
            playerController.master.inventory.GiveItem(RoR2Content.Items.LunarSpecialReplacement);
        }


    }
}

            On.RoR2.Run.Start += RunStartMethod;

            Texture2D texTreebotBlueSkinIcon = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            texTreebotBlueSkinIcon.LoadImage(Properties.Resources.texTreebotBlueSkinIcon, false);
            texTreebotBlueSkinIcon.filterMode = FilterMode.Bilinear;
            Sprite texTreebotBlueSkinIconS = Sprite.Create(texTreebotBlueSkinIcon, rec128, half);



            GameObject temphereticbod = Resources.Load<GameObject>("prefabs/characterbodies/HereticBody");
            CharacterModel temphereticmodel = temphereticbod.GetComponentInChildren<CharacterModel>();

            LoadoutAPI.SkinDefInfo HereticDefaultInfo = new LoadoutAPI.SkinDefInfo
            {
                NameToken = "Default",
                UnlockableDef = null,
                RootObject = temphereticmodel.gameObject,
                RendererInfos = temphereticmodel.baseRendererInfos,
                Name = "skinHereticNormal",
                Icon = texTreebotBlueSkinIconS,
            };
            LoadoutAPI.AddSkinToCharacter(temphereticbod, HereticDefaultInfo);
            


GivePickupsOnStart.ItemInfo LunarPart1 = new GivePickupsOnStart.ItemInfo { itemString = ("LunarPrimaryReplacement"), count = 1, };
GivePickupsOnStart.ItemInfo LunarPart2 = new GivePickupsOnStart.ItemInfo { itemString = ("LunarSecondaryReplacement"), count = 1, };
GivePickupsOnStart.ItemInfo LunarPart3 = new GivePickupsOnStart.ItemInfo { itemString = ("LunarUtilityReplacement"), count = 1, };
GivePickupsOnStart.ItemInfo LunarPart4 = new GivePickupsOnStart.ItemInfo { itemString = ("LunarSpecialReplacement"), count = 1, };
GivePickupsOnStart.ItemInfo[] HereticPickupDumb = new GivePickupsOnStart.ItemInfo[0];
HereticPickupDumb = HereticPickupDumb.Add(LunarPart1, LunarPart2, LunarPart3, LunarPart4);

Resources.Load<GameObject>("prefabs/characterbodies/HereticBody").AddComponent<GivePickupsOnStart>().itemInfos = HereticPickupDumb;

/*
            GameObject CommandoBody = BodyCatalog.FindBodyPrefab("Bandit2Body");
            GameObject HereticBody = BodyCatalog.FindBodyPrefab("HereticBody");

            SurvivorDef CommandoSurvivor = SurvivorCatalog.FindSurvivorDefFromBody(CommandoBody);
            SurvivorDef HereticSurvivor = SurvivorCatalog.FindSurvivorDefFromBody(HereticBody);

            HereticSurvivor.hidden = false;

            var HereticSkillDef1 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement"));
            var HereticSkillDef2 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement"));
            var HereticSkillDef3 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement"));
            var HereticSkillDef4 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarDetonatorSpecialReplacement"));
            
            GenericSkill[] hereticskill = HereticBody.GetComponents<GenericSkill>();

            RoR2.Skills.SkillFamily.Variant HereticVariant1 = new RoR2.Skills.SkillFamily.Variant
            {
                skillDef = HereticSkillDef1,
                unlockableName = ""
            };
            RoR2.Skills.SkillFamily.Variant HereticVariant2 = new RoR2.Skills.SkillFamily.Variant
            {
                skillDef = HereticSkillDef2,
                unlockableName = ""
            };
            RoR2.Skills.SkillFamily.Variant HereticVariant3 = new RoR2.Skills.SkillFamily.Variant
            {
                skillDef = HereticSkillDef3,
                unlockableName = ""
            };
            RoR2.Skills.SkillFamily.Variant HereticVariant4 = new RoR2.Skills.SkillFamily.Variant
            {
                skillDef = HereticSkillDef4,
                unlockableName = ""
            };


            hereticskill[0].skillFamily.variants = hereticskill[0].skillFamily.variants.Add(HereticVariant1);
            hereticskill[1].skillFamily.variants = hereticskill[1].skillFamily.variants.Add(HereticVariant2);
            hereticskill[2].skillFamily.variants = hereticskill[2].skillFamily.variants.Add(HereticVariant3);
            hereticskill[3].skillFamily.variants = hereticskill[3].skillFamily.variants.Add(HereticVariant4);

            GameObject HereticDisplay = R2API.PrefabAPI.InstantiateClone(CommandoSurvivor.displayPrefab, "HereticDisplay", false);
            HereticSurvivor.displayPrefab = HereticDisplay;
            Instantiate(HereticBody.transform.GetChild(0).GetChild(0).gameObject, HereticDisplay.transform);
            HereticDisplay.transform.GetChild(0).gameObject.SetActive(false);
            HereticDisplay.transform.GetChild(1).localScale = new Vector3(0.6f, 0.6f, 0.6f);
            HereticDisplay.transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = HereticDisplay.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController;
            */
/*
var HereticSkillDef1 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement"));
var HereticSkillDef2 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement"));
var HereticSkillDef3 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement"));
var HereticSkillDef4 = RoR2.Skills.SkillCatalog.GetSkillDef(RoR2.Skills.SkillCatalog.FindSkillIndexByName("LunarDetonatorSpecialReplacement"));

GenericSkill[] hereticskill = HereticBody.GetComponents<GenericSkill>();

RoR2.Skills.SkillFamily.Variant HereticVariant1 = new RoR2.Skills.SkillFamily.Variant
{
    skillDef = HereticSkillDef1,
    unlockableName = ""
};
RoR2.Skills.SkillFamily.Variant HereticVariant2 = new RoR2.Skills.SkillFamily.Variant
{
    skillDef = HereticSkillDef2,
    unlockableName = ""
};
RoR2.Skills.SkillFamily.Variant HereticVariant3 = new RoR2.Skills.SkillFamily.Variant
{
    skillDef = HereticSkillDef3,
    unlockableName = ""
};
RoR2.Skills.SkillFamily.Variant HereticVariant4 = new RoR2.Skills.SkillFamily.Variant
{
    skillDef = HereticSkillDef4,
    unlockableName = ""
};



            hereticskill[0].skillFamily.variants = hereticskill[0].skillFamily.variants.Add(HereticVariant1);
            hereticskill[1].skillFamily.variants = hereticskill[1].skillFamily.variants.Add(HereticVariant2);
            hereticskill[2].skillFamily.variants = hereticskill[2].skillFamily.variants.Add(HereticVariant3);
            hereticskill[3].skillFamily.variants = hereticskill[3].skillFamily.variants.Add(HereticVariant4);


            
            GameObject CommandoBody = BodyCatalog.FindBodyPrefab("Bandit2Body");
            GameObject HereticBody = BodyCatalog.FindBodyPrefab("HereticBody");

            SurvivorDef CommandoSurvivor = SurvivorCatalog.FindSurvivorDefFromBody(CommandoBody);
            SurvivorDef HereticSurvivor = SurvivorCatalog.FindSurvivorDefFromBody(HereticBody);

            HereticSurvivor.hidden = false;

            GameObject HereticDisplay = R2API.PrefabAPI.InstantiateClone(CommandoSurvivor.displayPrefab, "HereticDisplay", false);
            HereticSurvivor.displayPrefab = HereticDisplay;
            Instantiate(HereticBody.transform.GetChild(0).GetChild(0).gameObject, HereticDisplay.transform);
            HereticDisplay.transform.GetChild(0).gameObject.SetActive(false);
            HereticDisplay.transform.GetChild(1).localScale = new Vector3(0.6f, 0.6f, 0.6f);

            var tempclips = HereticDisplay.transform.GetChild(1).GetComponent<Animator>().parameters;


            for (int i = 0; i < tempclips.Length; i++)
            {
                Debug.LogWarning("read");
                    tempclips[i] = new AnimatorControllerParameter();
                
            }
                



*/



/*
On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += (orig, self, buffdef, duration) =>
{
    if (self.isPlayerControlled == true)
    {
        if (buffdef.isDebuff == true)
        {
            return;
        }
    }
    orig(self, buffdef,duration);
};

On.RoR2.DotController.AddDot += (orig, self, attackerObject, duration, dot, damageMultiplier) =>
{
    if (self.victimObject.GetComponent<CharacterBody>().isPlayerControlled == true)
    {
        return;
    }
    orig(self, attackerObject, duration, dot, damageMultiplier);
};
*/




/* GravitatePickup also needed
On.RoR2.AmmoPickup.OnTriggerStay += (orig, self, other) =>
{

   TeamIndex objectTeam = TeamComponent.GetObjectTeam(other.gameObject);
   if (objectTeam == TeamIndex.Player)
   {
       CharacterBody component = other.GetComponent<CharacterBody>();
       if (component && component.isPlayerControlled)
       {
           orig(self, other);
       }
       return;
   }

   orig(self, other);
};
On.RoR2.HealthPickup.OnTriggerStay += (orig, self, other) =>
{
   TeamIndex objectTeam = TeamComponent.GetObjectTeam(other.gameObject);
   if (objectTeam == TeamIndex.Player)
   {
       CharacterBody component = other.GetComponent<CharacterBody>();
       if (component && component.isPlayerControlled)
       {
           orig(self, other);
       }
       return;
   }

   orig(self, other);
};

On.RoR2.MoneyPickup.OnTriggerStay += (orig, self, other) =>
{
   orig(self, other);
};
*/


//Resources.Load<GameObject>("prefabs/characterbodies/CaptainBody").GetComponent<CharacterBody>().baseAcceleration = 5;
//Resources.Load<GameObject>("prefabs/characterbodies/CaptainBody").GetComponent<CharacterBody>().baseMoveSpeed = 10.5f;




//EntityStates.Merc.Weapon.GroundLight2.


/*
On.EntityStates.BasicMeleeAttack.AuthorityOnFinish += (orig, self) =>
{

    Debug.LogWarning(self.
        );
    Debug.LogWarning(self.GetPropertyValue<float>("fixedAge"));

    orig(self);
};
*/





/*
var UIMoneyBox = Resources.Load<GameObject>("prefabs/HUDSimple").transform.GetChild(0).GetChild(6).GetChild(2).GetChild(3);
UIMoneyBox.GetChild(0).GetChild(3).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>().color = new Color32(223, 223, 184, 255);
UIMoneyBox.GetChild(0).GetChild(4).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>().color = new Color32(223, 223, 184, 255);
UIMoneyBox.GetChild(1).GetChild(3).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>().color = new Color32(195, 215, 250, 255);
UIMoneyBox.GetChild(1).GetChild(4).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>().color = new Color32(195, 215, 250, 255);
On.RoR2.UI.HUD
 */



//Merc Tint Color

//ExposeConsume uses matMercExposedSlash
//ExposeConsume uses matMercExposedBackdrop 
//ExposeConsume uses matMercExposedSlash


//Slash 1 uses matMercSwipe1
//Slash 2 uses matMercSwipe1
//Slash 3 uses matMercSwipe2

//Whirlwind Ground uses matMercSwipe1
//Whirlwind Air uses matMercSwipe1
//Rising Thunder uses matMercSwipe1

//PreDashEffect uses matTracerBright
//PreDashEffect uses Light
//PreDashEffect uses matMercIgnition 
//PreDashEffect uses matMercIgnition

//MercAssaulterEffect uses matMercIgnition
//MercAssaulterEffect uses matTracerBright
//MercAssaulterEffect uses Light
//MercAssaulterEffect uses matMercSwipe1
//MercAssaulterEffect uses matMercIgnition x2
//Dash Overlay : matMercEnergized

//MercFocusedAssaultOrbEffect uses matMercFocusedAssaultIcon
//MercFocusedAssaultOrbEffect uses matMercExposedBackdrop
//MercFocusedAssaultOrbEffect uses matMercExposedSlash

//(Merc Clones)
//OmniImpactVFXSlashMercEvis uses matOmniHitspark3Merc 
//OmniImpactVFXSlashMercEvis uses matGenericFlash 
//OmniImpactVFXSlashMercEvis uses matTracerBright 
//OmniImpactVFXSlashMercEvis uses matOpaqueDustSpeckledLarge 
//OmniImpactVFXSlashMercEvis uses matOmniRadialSlash1Merc 
//OmniImpactVFXSlashMercEvis uses matOmniHitspark4Merc 
//OmniImpactVFXSlashMercEvis uses matMercHologram 

//Evis Overlay : matHuntressFlashBright
//Evis Overlay : matHuntressFlashExpanded

//SlicingWindsGhost uses matMercSwipe2
//SlicingWindsGhost uses matTracerBright
//SlicingWindsGhost uses Light

//EvisOverlapProjectileGhost uses matMercSwipe2
//EvisOverlapProjectileGhost uses matOmniRadialSlash1Merc 
//EvisOverlapProjectileGhost uses matOmniHitspark2Merc 
//EvisOverlapProjectileGhost uses matTracerBright
//EvisOverlapProjectileGhost uses Light
//EvisOverlapProjectileGhost uses matMercHologram

//public static Texture2D texRampHuntressRed = null;

//public static GameObject ImpactMercFocusedAssault = null;  //Utility2
//public static GameObject ImpactMercAssaulter = null;  //Utility2
//public static GameObject MercAssaulterEffect = null;  //Utility2

//public static GameObject EvisProjectile = Resources.Load<GameObject>("prefabs/projectiles/EvisProjectile");
//public static GameObject EvisProjectileGhost = Resources.Load<GameObject>("prefabs/projectileghosts/EvisProjectileGhost");
//public static GameObject EvisOverlapProjectile = Resources.Load<GameObject>("prefabs/projectiles/EvisOverlapProjectile");
//public static GameObject EvisOverlapProjectileGhost = Resources.Load<GameObject>("prefabs/projectileghosts/EvisOverlapProjectileGhost");



/*
        public static ClassicStageInfo.MonsterFamily ParentFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily Parent2FamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily Lunar2FamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily Lunar0FamilyEvent = new ClassicStageInfo.MonsterFamily();

        public static ClassicStageInfo.MonsterFamily VanillaBeetleFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaGolemFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaGolemDampCaveFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaJellyFishFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaWispFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaLemurianFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VanillaImpFamilyEvent = new ClassicStageInfo.MonsterFamily();

        public static ClassicStageInfo.MonsterFamily ClayFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily RoboBallFamilyEvent = new ClassicStageInfo.MonsterFamily();
        public static ClassicStageInfo.MonsterFamily VoidFamilyEvent = new ClassicStageInfo.MonsterFamily();



        private static CharacterSpawnCard TitanGoolake = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/titan/cscTitanGooLake");
        private static CharacterSpawnCard TitanPlains = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/titan/cscTitanGolemPlains");

        private static SpawnCard ISCScavLunarBag = Resources.Load<SpawnCard>("SpawnCards/interactablespawncard/iscScavLunarBackpack");

*/


/*
On.RoR2.Run.BuildDropTable += (orig, self) =>
{
    orig(self);
    self.availableTier1DropList.Clear();
    self.availableTier2DropList.Clear();
    self.availableTier3DropList.Clear();
    self.availableLunarDropList.Clear();
    self.availableEquipmentDropList.Clear();
    self.availableBossDropList.Clear();
    self.availableLunarEquipmentDropList.Clear();
    self.availableNormalEquipmentDropList.Clear();

    ItemIndex itemIndex = (ItemIndex)0;
    ItemIndex itemCount = (ItemIndex)ItemCatalog.itemCount;
    while (itemIndex < itemCount)
    {

            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);

            if (itemDef.tier != ItemTier.NoTier)
            {
                self.availableTier1DropList.Add(PickupCatalog.FindPickupIndex(itemIndex));
            }

        itemIndex++;
    }
    EquipmentIndex equipmentIndex = (EquipmentIndex)0;
    EquipmentIndex equipmentCount = (EquipmentIndex)EquipmentCatalog.equipmentCount;
    while (equipmentIndex < equipmentCount)
    {

        EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
        if (equipmentDef.canDrop)
        {
            self.availableTier1DropList.Add(PickupCatalog.FindPickupIndex(equipmentIndex));
        }
        else if (equipmentDef.isBoss || equipmentDef.isLunar)
        {
            self.availableTier1DropList.Add(PickupCatalog.FindPickupIndex(equipmentIndex));
        }


        equipmentIndex++;
    }
    self.availableTier1DropList.Add(PickupCatalog.FindPickupIndex(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex));




    self.availableTier2DropList.AddRange(self.availableTier1DropList);
    self.availableTier3DropList.AddRange(self.availableTier1DropList);
    self.availableLunarDropList.AddRange(self.availableTier1DropList);
    self.availableEquipmentDropList.AddRange(self.availableTier1DropList);
    self.availableBossDropList.AddRange(self.availableTier1DropList);
    self.availableLunarEquipmentDropList.AddRange(self.availableTier1DropList);
    self.availableNormalEquipmentDropList.AddRange(self.availableTier1DropList);

};
*/

/*
On.RoR2.UI.LogBook.PageBuilder.StatsPanel += (orig, builder) =>
{
    orig(builder);

    UserProfile userProfile = (UserProfile)builder.entry.extraData;



    Debug.LogWarning(builder.container);




    Transform StatDisplayParent = builder.container.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetChild(1);

    GameObject NewStatObj1 = Instantiate(StatDisplayParent.GetChild(2).gameObject, StatDisplayParent);
    GameObject NewStatObj2 = Instantiate(StatDisplayParent.GetChild(2).gameObject, StatDisplayParent);

    NewStatObj1.transform.GetChild(0).GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText(RoR2.Language.GetString(RoR2.Stats.StatDef.totalLunarPurchases.displayToken));
    NewStatObj1.transform.GetChild(2).GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("<color=#FFFF7F>" + userProfile.statSheet.GetStatValueAsDouble(RoR2.Stats.StatDef.totalLunarPurchases) + "</color>");
    NewStatObj1.transform.SetSiblingIndex(13);

    NewStatObj2.transform.GetChild(0).GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText(RoR2.Language.GetString(RoR2.Stats.StatDef.highestLunarPurchases.displayToken));
    NewStatObj2.transform.GetChild(2).GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("<color=#FFFF7F>" + userProfile.statSheet.GetStatValueAsDouble(RoR2.Stats.StatDef.highestLunarPurchases) + "</color>");
    NewStatObj2.transform.SetSiblingIndex(14);

    Debug.LogWarning(NewStatObj1);

    //Debug.LogWarning(userProfile.statSheet.GetStatValueAsDouble(RoR2.Stats.StatDef.totalHealthHealed));
    Debug.LogWarning(userProfile.statSheet.GetStatValueAsDouble(RoR2.Stats.StatDef.totalLunarPurchases));
    Debug.LogWarning(userProfile.statSheet.GetStatValueAsDouble(RoR2.Stats.StatDef.highestLunarPurchases));
};
*/



/*
 public void PingIconChanger(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            GameObjectAttatcher();

            orig(self);


            if (LastStageGoolake)
            {
                On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter -= FireMegaFireball_OnEnter;
                LastStageGoolake = false;
            }




            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "skymeadow":
                    break;

            }





            if (SceneInfo.instance.sceneDef.baseSceneName == "skymeadow")
            {
                GameObject.Find("/PortalDialerEvent/Final Zone/ButtonContainer/PortalDialer").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                Material CorrectGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser (2)/mdlGeyser").GetComponent<MeshRenderer>().material;
                Transform WrongGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser").transform;

                WrongGeyser.GetChild(0).GetComponent<MeshRenderer>().material = CorrectGeyser;
                WrongGeyser.GetChild(1).GetComponent<MeshRenderer>().material = CorrectGeyser;

            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "frozenwall" || SceneInfo.instance.sceneDef.baseSceneName == "itfrozenwall")
            {
                genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];

                for (var i = 0; i < genericlist.Length; i++)
                {
                    //Debug.LogWarning(genericlist[i]); ////DISABLE THIS
                    if (genericlist[i].name.Contains("HumanFan"))
                    {
                        genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                    }
                    else if (genericlist[i].name.Equals("TimedChest"))
                    {
                        genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = TimedChestIcon;
                    }

                }
                genericlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "blackbeach")
            {

                GameObject tempobj = GameObject.Find("/HOLDER: Preplaced Objects");
                if (tempobj != null)
                {
                    portalstatuelist = tempobj.GetComponentsInChildren<RoR2.UnlockableGranter>(true);
                    int unavailable = 0;
                    for (var i = 0; i < portalstatuelist.Length; i++)
                    {
                        //Debug.LogWarning(portalstatuelist[i]); ////DISABLE THIS
                        //Debug.LogWarning(portalstatuelist[i].gameObject.activeSelf); ////DISABLE THIS
                        if (portalstatuelist[i].gameObject.activeSelf == false)
                        {
                            unavailable++;
                        }
                        if (unavailable == 3)
                        {
                            portalstatuelist[1].gameObject.SetActive(true);
                        }
                    }
                    portalstatuelist = null;
                    GC.Collect();
                }
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "goolake")
            {
                LastStageGoolake = true;
                On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter += FireMegaFireball_OnEnter;

                //GameObject tempobj = GameObject.Find("/HOLDER: Misc Props/GooPlane, High");
                GameObject GooPlaneOriginal = GameObject.Find("/HOLDER: Misc Props/GooPlane");

                GameObject GooPlaneNew1 = Instantiate(GooPlaneOriginal, GooPlaneOriginal.transform.parent);
                GameObject GooPlaneNew2 = Instantiate(GooPlaneOriginal, GooPlaneOriginal.transform.parent);

                GooPlaneNew1.transform.localPosition = new Vector3(319.28f, -135.9f, 143f);
                GooPlaneNew1.transform.localScale = new Vector3(33.8897f, 126.1369f, 23.5065f);
                GooPlaneNew1.transform.localRotation = new Quaternion(0, 0.291f, 0, -0.9567f);

                GooPlaneNew2.transform.localPosition = new Vector3(134.3587f, -138.65f, 143f);
                GooPlaneNew2.transform.localScale = new Vector3(33.8897f, 126.1369f, 23.5065f);
                GooPlaneNew2.transform.localRotation = new Quaternion(0, 0.291f, 0, -0.9567f);


                DummyPingableInteraction[] desertplatelist = FindObjectsOfType(typeof(DummyPingableInteraction)) as DummyPingableInteraction[];
                for (var i = 0; i < desertplatelist.Length; i++)
                {
                    //Debug.LogWarning(desertplatelist[i]); ////DISABLE THIS
                    if (desertplatelist[i].name.Contains("GLPressurePlate"))
                    {
                        desertplatelist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                    }
                }
                Inventory[] InventoryList = FindObjectsOfType(typeof(RoR2.Inventory)) as RoR2.Inventory[];
                for (var i = 0; i < InventoryList.Length; i++)
                {
                    if (NetworkServer.active)
                    {
                        //Debug.LogWarning(desertplatelist[i]); ////DISABLE THIS
                        if (InventoryList[i].name.Equals("LemurianBruiserFireMaster") || InventoryList[i].name.Equals("LemurianBruiserIceMaster"))
                        {
                            int temp = Run.instance.stageClearCount * Run.instance.stageClearCount - 1;
                            if (temp > 0)
                            {
                                InventoryList[i].GiveItem(RoR2Content.Items.LevelBonus, temp);
                            }

                        }
                    }
                }
                desertplatelist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "sulfurpools")
            {
                GameObject GeyserHolder = GameObject.Find("/HOLDER: Geysers");
                MatGeyserSulfurPools = Instantiate(GeyserHolder.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
                MatGeyserSulfurPools.SetColor("_EmissionColor", new Color(0.15f, 0.2f, 0.08f));

                for (int i = 0; i < GeyserHolder.transform.childCount; i++)
                {
                    Transform LoopParticles = GeyserHolder.transform.GetChild(i).GetChild(2).GetChild(0);
                    LoopParticles.GetChild(1).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                    LoopParticles.GetChild(2).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                    LoopParticles.GetChild(3).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                }

            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "foggyswamp")
            {
                GameObjectUnlockableFilter[] dummylist = FindObjectsOfType(typeof(RoR2.GameObjectUnlockableFilter)) as RoR2.GameObjectUnlockableFilter[];
                for (var i = 0; i < dummylist.Length; i++)
                {
                    //Debug.LogWarning(dummylist[i]); ////DISABLE THIS

                    Destroy(dummylist[i]);

                }
                dummylist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "dampcavesimple")
            {
                purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                for (var i = 0; i < purchaserlist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (purchaserlist[i].name.Equals("TreebotUnlockInteractable"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        GameObject BrokenRexHoloPivot = new GameObject("BrokenRexHoloPivot");
                        BrokenRexHoloPivot.transform.localPosition = new Vector3(0.8f, 4f, -0.2f);
                        BrokenRexHoloPivot.transform.SetParent(purchaserlist[i].gameObject.transform, false);
                        purchaserlist[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = BrokenRexHoloPivot.transform;
                    }
                    else if (purchaserlist[i].name.Equals("GoldChest"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;
                    }


                }
                purchaserlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "itdampcave")
            {
                purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                for (var i = 0; i < purchaserlist.Length; i++)
                {
                    if (purchaserlist[i].name.Equals("GoldChest"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;
                    }
                }
                purchaserlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "rootjungle")
            {
                var purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];

                for (var i = 0; i < purchaserlist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (purchaserlist[i].name.Equals("GoldChest"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;
                    }
                }
                UnityEngine.LODGroup[] lodlist = GameObject.Find("HOLDER: Randomization/GROUP: Large Treasure Chests").GetComponentsInChildren<UnityEngine.LODGroup>();

                for (var i = 0; i < lodlist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (lodlist[i].name.Equals("RJpinkshroom"))
                    {
                        Destroy(lodlist[i]);
                    }
                }

                purchaserlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "wispgraveyard")
            {

            else if (SceneInfo.instance.sceneDef.baseSceneName == "bazaar")
            {
                purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];



                int mooned = 10;
                int instant = 10;
                if (ThirdLunarSeer.Value == true)
                {
                    instant = 0;
                }
                if (CommencementSeer.Value == true)
                {
                    mooned = 0;
                }
                //int voidportalpresent = 0;

                SceneDef nextStageScene = Run.instance.nextStageScene;
                List<SceneDef> nextscenelist = new List<SceneDef>();
                if (nextStageScene != null)
                {
                    int stageOrder = nextStageScene.stageOrder;
                    foreach (SceneDef sceneDef in SceneCatalog.allSceneDefs)
                    {
                        if (sceneDef.stageOrder == stageOrder && (sceneDef.requiredExpansion == null || Run.instance.IsExpansionEnabled(sceneDef.requiredExpansion)) && !sceneDef.baseSceneName.EndsWith("2"))
                        {
                            nextscenelist.Add(sceneDef);
                        }
                    }
                }

                //prevDestinationsClone = prevDestinations;
                for (var i = 0; i < purchaserlist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (purchaserlist[i].name.Contains("LunarCauldron,"))
                    {
                        purchaserlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
                    }
                    else if (purchaserlist[i].name.Contains("SeerStation"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = SeerIcon;
                        if (NetworkServer.active)
                        {
                            if (Run.instance.NetworkstageClearCount % Run.stagesPerLoop == 0)
                            {
                                if (mooned == 0)
                                {
                                    mooned++;
                                    purchaserlist[i].gameObject.GetComponent<SeerStationController>().SetTargetScene(SceneCatalog.GetSceneDefFromSceneName("moon2"));
                                }
                            }

                            if (nextscenelist.Count > 0)
                            {
                                nextscenelist.Remove(SceneCatalog.GetSceneDef((SceneIndex)purchaserlist[i].gameObject.GetComponent<SeerStationController>().NetworktargetSceneDefIndex));
                            }

                            if (instant == 1)
                            {
                                instant = 2;
                                GameObject newseer = GameObject.Instantiate(purchaserlist[i].gameObject, purchaserlist[i].gameObject.transform.parent);
                                newseer.transform.localPosition = new Vector3(10f, -0.81f, 4f);
                                newseer.transform.localRotation = new Quaternion(0f, 0.3827f, 0f, -0.9239f);
                                if (nextscenelist.Count > 0)
                                {
                                    newseer.GetComponent<SeerStationController>().SetTargetScene(nextscenelist[0]);
                                    newseer.GetComponent<SeerStationController>().OnStartClient();
                                }
                                else
                                {
                                    newseer.GetComponent<PurchaseInteraction>().SetAvailable(false);
                                    newseer.GetComponent<SeerStationController>().OnStartClient();
                                }



                            }
                            instant++;
                        }
                    }
                    else if (purchaserlist[i].name.Contains("LunarShopTerminal"))
                    {
                        purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;
                    }
                    else if (purchaserlist[i].name.Equals("LunarRecycler"))
                    {
                        GameObject LunarRecyclerPivot = new GameObject("LunarRecyclerPivot");
                        LunarRecyclerPivot.transform.localPosition = new Vector3(0.1f, -1f, 1f);
                        LunarRecyclerPivot.transform.localRotation = new Quaternion(0f, -0.7071f, -0.7071f, 0f);
                        LunarRecyclerPivot.transform.SetParent(purchaserlist[i].gameObject.transform, false);
                        purchaserlist[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LunarRecyclerPivot.transform;
                        purchaserlist[i].gameObject.GetComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
                    }
                    else if (purchaserlist[i].name.Contains("LockedMage"))
                    {
                        if (AlwaysDisplayLockedArti.Value == true)
                        {
                            Destroy(purchaserlist[i].gameObject.GetComponent<RoR2.GameObjectUnlockableFilter>());
                        }
                    }
                }
                for (var j = 0; j < genericlist.Length; j++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (genericlist[j].name.Contains("PortalArena"))
                    {
                        //voidportalpresent++;
                        genericlist[j].gameObject.transform.parent.GetChild(2).gameObject.GetComponent<SphereCollider>().radius = 130;
                        genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;

                    }
                    else if (genericlist[j].name.Contains("Portal"))
                    {
                        genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
                        genericlist[j].gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = "Leave the <style=cIsLunar>Bazaar Between Time</style>";
                    }
                }
                purchaserlist = null;
                genericlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "moon2")
            {
                highlightlist = FindObjectsOfType(typeof(Highlight)) as Highlight[];
                BossGroup[] bossgrouplist = FindObjectsOfType(typeof(BossGroup)) as BossGroup[];
                int cauldronthing = 0;
                int cauldronRWcount = 0;
                Transform tempcauldronparent = null;
                Transform tempcauldron1 = null;
                for (var i = 0; i < highlightlist.Length; i++)
                {
                    //Debug.LogWarning(highlightlist[i]); ////DISABLE THIS
                    if (highlightlist[i].name.Contains("MoonBatteryDesign") || highlightlist[i].name.Contains("MoonBatteryBlood") || highlightlist[i].name.Contains("MoonBatteryMass") || highlightlist[i].name.Contains("MoonBatterySoul"))
                    {
                        highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
                    }
                    else if (highlightlist[i].name.Contains("LunarCauldron,"))
                    {
                        if (cauldronthing == 0)
                        {
                            tempcauldron1 = highlightlist[i].gameObject.transform;
                            tempcauldronparent = highlightlist[i].gameObject.transform.parent;
                        };
                        cauldronthing++;
                        if (highlightlist[i].name.Contains("LunarCauldron, RedToWhite Variant"))
                        {
                            cauldronRWcount++;
                            highlightlist[i].gameObject.GetComponent<ShopTerminalBehavior>().dropVelocity = new Vector3(5, 10, 5);
                        };
                        highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
                        if (cauldronthing == 5 && cauldronRWcount == 0 && tempcauldronparent != null)
                        {
                            if (GuaranteedRedToWhite.Value == true)
                            {
                                Instantiate<GameObject>(RedToWhiteSoup, tempcauldron1.position, tempcauldron1.rotation);
                                tempcauldron1.gameObject.SetActive(false);
                                Debug.Log("No White Soup, making one");
                            }
                            else
                            {
                                Debug.Log("No White Soup");
                            }
                        };
                        //RedToWhiteSoup
                    }
                    else if (highlightlist[i].name.Equals("MoonElevator"))
                    {
                        highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                    }
                    else if (highlightlist[i].name.Equals("LunarChest"))
                    {
                        highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;
                    }
                    else if (highlightlist[i].name.Equals("ShrineRestack"))
                    {
                        highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
                    }
                }

                for (var i = 0; i < bossgrouplist.Length; i++)
                {
                    if (bossgrouplist[i].name.Equals("BrotherEncounter, Phase 2"))
                    {
                        bossgrouplist[i].bestObservedName = Language.GetString("LUNARGOLEM_BODY_NAME");
                        bossgrouplist[i].bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
                    }
                }

                highlightlist = null;
                bossgrouplist = null;


                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "arena")
            {
                GameObject PortalArena = GameObject.Find("/PortalArena");
                PortalArena.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
                PortalArena.AddComponent<GenericObjectiveProvider>().objectiveToken = "Leave the <style=cIsVoid>Void Fields</style>";

                purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                for (var i = 0; i < purchaserlist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (purchaserlist[i].name.Contains("NullSafeZone"))
                    {
                        purchaserlist[i].gameObject.GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;
                    }
                }
                purchaserlist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "mysteryspace")
            {
                GameObject MSObelisk = GameObject.Find("/MSObelisk");
                MSObelisk.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                MSObelisk.AddComponent<GenericObjectiveProvider>().objectiveToken = "Obliterate yourself from existence";
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "artifactworld")
            {

                pickuplist = FindObjectsOfType(typeof(GenericPickupController)) as GenericPickupController[];
                for (var i = 0; i < pickuplist.Length; i++)
                {
                    //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                    if (pickuplist[i].name.Contains("SetpiecePickup"))
                    {
                        pickuplist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
                        pickuplist[i].gameObject.AddComponent<RoR2.GenericObjectiveProvider>().objectiveToken = "Free the <style=cArtifact>Artifact</style>";


                        if (ArtifactOutline.Value == true)
                        {
                            RoR2.Highlight tempartifact = pickuplist[i].gameObject.GetComponent<Highlight>();
                            tempartifact.pickupIndex = pickuplist[i].pickupIndex;
                            tempartifact.highlightColor = Highlight.HighlightColor.pickup;
                            tempartifact.isOn = true;
                        }

                    }
                }
                pickuplist = null;
                GC.Collect();
            }
            else if (SceneInfo.instance.sceneDef.baseSceneName == "voidstage")
            {
                GameObject MissionController = GameObject.Find("/MissionController");
                MissionController.GetComponent<VoidStageMissionController>().deepVoidPortalObjectiveProvider = null;
            }



            if (self.teleporterSpawnCard != null)
            {
                if (self.teleporterSpawnCard.name == "iscLunarTeleporter")
                {
                    RoR2.UI.ChargeIndicatorController[] tempchargelist = FindObjectsOfTypeAll(typeof(RoR2.UI.ChargeIndicatorController)) as RoR2.UI.ChargeIndicatorController[];
                    for (var i = 0; i < tempchargelist.Length; i++)
                    {
                        if (tempchargelist[i].name.Equals("TeleporterChargingPositionIndicator(Clone)"))
                        {
                            tempchargelist[i].iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                        }
                    }
                    tempchargelist = null;
                }
            }



        }

*/