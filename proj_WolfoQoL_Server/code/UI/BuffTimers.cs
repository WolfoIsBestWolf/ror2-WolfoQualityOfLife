using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Server
{
    public class BuffTimers
    {
        public static BuffDef bdHellFire;
        public static BuffDef bdPercentBurn;
        public static BuffDef bdBugWings;
        public static BuffDef bdStrides;
        public static BuffDef bdRoseBuckle;

        public static BuffDef bdHeadstompOn;
        public static BuffDef bdHeadstompOff;
        public static BuffDef bdFeather;
        public static BuffDef bdFeatherVV;
        public static ItemIndex VV_VoidFeather = ItemIndex.None;

        public static BuffDef bdShieldDelay;
        public static BuffDef bdShieldDelayPink;
        //public static BuffDef bdEgg;
        //public static BuffDef bdHellFireDuration;
        public static BuffDef bdFrostRelic;
        public static BuffDef bdOpalCooldown;
        public static BuffDef bdFrozen;

        public static void Buffs_NewBuffs()
        {
            bool riskyModEnabled = false;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod"))
            {
                riskyModEnabled = true;
            }


            #region Helfire Burn
            BuffDef NormalBurn = LegacyResourcesAPI.Load<BuffDef>("buffdefs/OnFire");
            bdHellFire = ScriptableObject.CreateInstance<BuffDef>();
            bdHellFire.iconSprite = NormalBurn.iconSprite;
            bdHellFire.buffColor = new Color32(50, 188, 255, 255);
            bdHellFire.name = "wqol_HelFire";
            bdHellFire.isDebuff = false;
            bdHellFire.canStack = true;
            bdHellFire.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdHellFire);
            #endregion
            #region
            //Obsolete
            bdPercentBurn = ScriptableObject.CreateInstance<BuffDef>();
            bdPercentBurn.iconSprite = NormalBurn.iconSprite;
            //bdPercentBurn.buffColor = new Color32(226, 91, 69, 255); //E25B45 Overheat
            bdPercentBurn.buffColor = new Color32(203, 53, 38, 255); //CB3526 Blazing Elite
            bdPercentBurn.name = "wqol_EnemyBurn";
            bdPercentBurn.isDebuff = false;
            bdPercentBurn.canStack = true;
            bdPercentBurn.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdPercentBurn);
            #endregion


            #region Bug Wings Jetpack
            bdBugWings = ScriptableObject.CreateInstance<BuffDef>();
            bdBugWings.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffBeetleUp.png");
            bdBugWings.buffColor = new Color32(218, 136, 251, 255); //DA88FB Tesla Coil
            bdBugWings.name = "wqol_BugFlight";
            bdBugWings.isDebuff = false;
            bdBugWings.canStack = false;
            bdBugWings.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(bdBugWings);

            if (WConfig.cfgBuff_BugFlight.Value == true)
            {
                On.RoR2.JetpackController.StartFlight += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.targetBody.AddTimedBuff(bdBugWings, self.duration);
                    }
                };
                On.RoR2.JetpackController.OnDestroy += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.targetBody)
                        {
                            self.targetBody.ClearTimedBuffs(bdBugWings);
                        }
                    }
                };
            }
            #endregion
            #region Strides of Heresy

            bdStrides = ScriptableObject.CreateInstance<BuffDef>();
            bdStrides.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffLunarShellIcon.png");
            bdStrides.buffColor = new Color32(189, 176, 255, 255); //BDB0FF
            bdStrides.name = "wqol_ShadowIntangible";
            bdStrides.isDebuff = false;
            bdStrides.canStack = false;
            bdStrides.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(bdStrides);


            if (WConfig.cfgBuff_Strides.Value == true)
            {
                On.EntityStates.GhostUtilitySkillState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        float tempdur = EntityStates.GhostUtilitySkillState.baseDuration * self.outer.GetComponentInParent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement);
                        self.outer.GetComponentInParent<CharacterBody>().AddTimedBuff(bdStrides, tempdur);
                    }
                };

                On.EntityStates.GhostUtilitySkillState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.outer.GetComponentInParent<CharacterBody>().ClearTimedBuffs(bdStrides);
                    }
                };
            }
            #endregion
            #region Rose Buckler

            bdRoseBuckle = ScriptableObject.CreateInstance<BuffDef>();
            bdRoseBuckle.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffBodyArmor.png");
            bdRoseBuckle.buffColor = new Color32(251, 199, 38, 255); //FBC726
            bdRoseBuckle.name = "wqol_SprintArmor";
            bdRoseBuckle.isDebuff = false;
            bdRoseBuckle.canStack = false;
            bdRoseBuckle.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(bdRoseBuckle);

            if (WConfig.cfgBuff_SprintArmor.Value == true)
            {
                On.RoR2.CharacterBody.OnSprintStart += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            self.AddBuff(bdRoseBuckle);
                        }
                    }
                };

                On.RoR2.CharacterBody.OnSprintStop += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            if (!self.HasBuff(bdRoseBuckle)) { return; }
                            self.RemoveBuff(bdRoseBuckle);
                        }
                    }
                };
            }
            #endregion
            #region Frozen


            bdFrozen = ScriptableObject.CreateInstance<BuffDef>();
            bdFrozen.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffBrokenCube.png");
            bdFrozen.buffColor = new Color32(184, 216, 239, 255); //CFDBE0 // B8D8EF
            bdFrozen.name = "wqol_Frozen";
            bdFrozen.isDebuff = false;
            bdFrozen.canStack = true;
            bdFrozen.ignoreGrowthNectar = true;

            ContentAddition.AddBuffDef(bdFrozen);

            if (WConfig.cfgBuff_Frozen.Value == true && riskyModEnabled == false)
            {
                On.EntityStates.FrozenState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        for (float num5 = 0; num5 <= self.freezeDuration; num5 += 0.5f)
                        {
                            //self.characterBody.AddTimedBuff(bdFrozen, (float)num5);
                            self.characterBody.AddTimedBuffAuthority(bdFrozen.buffIndex, (float)num5);
                        }
                    }
                };

                On.EntityStates.FrozenState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.characterBody.ClearTimedBuffs(bdFrozen);
                    }
                };
            }
            #endregion
            #region HeadStomper

            bdHeadstompOn = ScriptableObject.CreateInstance<BuffDef>();
            bdHeadstompOn.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffHeadStompOn.png");
            bdHeadstompOn.buffColor = new Color32(255, 250, 250, 255);
            bdHeadstompOn.name = "wqol_HeadstomperReady";
            bdHeadstompOn.isDebuff = false;
            bdHeadstompOn.canStack = false;
            bdHeadstompOn.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(bdHeadstompOn);


            bdHeadstompOff = ScriptableObject.CreateInstance<BuffDef>();
            bdHeadstompOff.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffHeadStompOff.png");
            bdHeadstompOff.buffColor = new Color32(250, 250, 255, 255);
            bdHeadstompOff.name = "wqol_HeadstomperCooldown";
            bdHeadstompOff.isDebuff = false;
            bdHeadstompOff.canStack = true;
            bdHeadstompOff.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdHeadstompOff);

            if (WConfig.cfgBuff_Headstomper.Value == true && riskyModEnabled == false)
            {
                On.EntityStates.Headstompers.BaseHeadstompersState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self is EntityStates.Headstompers.HeadstompersIdle)
                        {
                            if (self.body.GetBuffCount(bdHeadstompOn) == 0)
                            {
                                self.body.AddBuff(bdHeadstompOn);
                            }
                        }
                    }
                };

                On.EntityStates.Headstompers.HeadstompersCooldown.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.body.RemoveBuff(bdHeadstompOn);
                        for (float newtimer = self.duration; 0 < newtimer; newtimer--)
                        {
                            self.body.AddTimedBuff(bdHeadstompOff, (float)newtimer);
                        }
                    }
                };

                On.RoR2.Items.HeadstomperBodyBehavior.OnDisable += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.body != null)
                        {
                            self.body.RemoveBuff(bdHeadstompOn);
                        }
                    }
                };
            }
            #endregion
            #region Shield Delay and Opal Delay
            BuffDef MedkitDelay = LegacyResourcesAPI.Load<BuffDef>("buffdefs/MedkitHeal");

            bdShieldDelay = ScriptableObject.CreateInstance<BuffDef>();
            bdShieldDelay.iconSprite = MedkitDelay.iconSprite;
            ///bdShieldDelay.buffColor = new Color32(66, 100, 220, 255); //4264DC
            bdShieldDelay.buffColor = new Color(0.3529f, 0.4863f, 0.9647f, 1f); //
            bdShieldDelay.name = "wqol_ShieldDelay";
            bdShieldDelay.isDebuff = false;
            bdShieldDelay.canStack = false;
            bdShieldDelay.isCooldown = false;
            bdShieldDelay.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdShieldDelay);

            bdShieldDelayPink = ScriptableObject.CreateInstance<BuffDef>();
            bdShieldDelayPink.iconSprite = MedkitDelay.iconSprite;
            //bdShieldDelayPink.buffColor = RoR2.UI.HealthBar.voidShieldsColor;
            bdShieldDelayPink.buffColor = new Color(1f, 0.2235f, 0.7804f, 1); //
            bdShieldDelayPink.name = "wqol_ShieldDelayPink";
            bdShieldDelayPink.isDebuff = false;
            bdShieldDelayPink.canStack = false;
            bdShieldDelayPink.isCooldown = false;
            bdShieldDelayPink.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdShieldDelayPink);


            BuffDef bdOutOfCombatArmorBuff = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion();
            bdOpalCooldown = ScriptableObject.CreateInstance<BuffDef>();
            bdOpalCooldown.iconSprite = bdOutOfCombatArmorBuff.iconSprite;
            bdOpalCooldown.buffColor = new Color(0.4151f, 0.4014f, 0.4014f, 1); //4264DC
            bdOpalCooldown.name = "wqol_OutOfCombatArmorCooldown";
            bdOpalCooldown.isDebuff = false;
            bdOpalCooldown.canStack = false;
            bdOpalCooldown.isCooldown = false;
            bdOpalCooldown.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdOpalCooldown);

            //If disabled
            //bdOpalCooldown.isHidden = true;
            if (WConfig.cfgBuff_ShieldOpalCooldown.Value == true)
            {
                On.RoR2.CharacterBody.OnTakeDamageServer += ShieldAndOpal;

                On.RoR2.HealthComponent.ForceShieldRegen += (orig, self) =>
                {
                    orig(self);
                    if (self.body && self.body.HasBuff(bdShieldDelay))
                    {
                        self.body.ClearTimedBuffs(bdShieldDelay);
                        self.body.ClearTimedBuffs(bdShieldDelayPink);
                    }
                };
            }
            #endregion



            /*
            Texture2D EggIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            EggIcon.LoadImage(Properties.Resources.texBuffEgg, true);
            EggIcon.filterMode = FilterMode.Bilinear;
            Sprite EggIconS = Sprite.Create(EggIcon, v.rec128, v.half);

            bdEgg = ScriptableObject.CreateInstance<BuffDef>();
            bdEgg.iconSprite = EggIconS;
            //bdEgg.buffColor = new Color32(255, 157, 174, 255); //FF9DAE
            bdEgg.buffColor = new Color32(255, 180, 137, 255); //FFB489
            bdEgg.name = "wqol_VolcanoEgg";
            bdEgg.isDebuff = false;
            bdEgg.canStack = false;
            ContentAddition.AddBuffDef(bdEgg);


            
            if (WConfig.EggVisualA.Value == true)
            {
                On.RoR2.FireballVehicle.OnPassengerEnter += (orig, self, passenger) =>
                {
                    orig(self, passenger);
                    if (NetworkServer.active)
                    {
                        passenger.GetComponent<CharacterBody>().AddTimedBuff(bdEgg, self.duration);
                    }
                };
                On.RoR2.FireballVehicle.FixedUpdate += (orig, self) =>
                {
                    if (NetworkServer.active)
                    {
                        if ((self.overlapFireAge + Time.fixedDeltaTime) > 1f / (self.overlapFireFrequency += Time.fixedDeltaTime))
                        {
                            self.vehicleSeat.currentPassengerBody.AddTimedBuff(bdEgg, self.duration - self.age);
                        }
                    }
                    orig(self);
                };

                On.RoR2.FireballVehicle.OnPassengerExit += (orig, self, passenger) =>
                {
                    orig(self, passenger);
                    if (NetworkServer.active)
                    {
                        passenger.GetComponent<CharacterBody>().ClearTimedBuffs(bdEgg);
                    }
                };
            }
            */
            /*
            Texture2D TinctureIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TinctureIcon.LoadImage(Properties.Resources.texBuffTincture, true);
            TinctureIcon.filterMode = FilterMode.Bilinear;
            Sprite TinctureIconS = Sprite.Create(TinctureIcon, v.rec128, v.half);

            bdHellFireDuration = ScriptableObject.CreateInstance<BuffDef>();
            bdHellFireDuration.iconSprite = TinctureIconS;
            bdHellFireDuration.buffColor = new Color32(115, 182, 165, 255); //
            bdHellFireDuration.name = "wqol_TinctureIgnition";
            bdHellFireDuration.isDebuff = false;
            bdHellFireDuration.canStack = false;
            ContentAddition.AddBuffDef(bdHellFireDuration);
            
            if (WConfig.TinctureVisual.Value == true)
            {
                On.RoR2.CharacterBody.AddHelfireDuration += (orig, self, duration) =>
                {
                    orig(self, duration);
                    if (NetworkServer.active)
                    {
                        self.AddTimedBuff(bdHellFireDuration, duration);
                    }
                };

            }
            */
            #region Frost Relic

            bdFrostRelic = ScriptableObject.CreateInstance<BuffDef>();
            bdFrostRelic.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffFrostRelic.png");
            bdFrostRelic.buffColor = new Color32(202, 229, 255, 255); //CAE5FF
            bdFrostRelic.name = "wqol_FrostRelicGrowth";
            bdFrostRelic.isDebuff = false;
            bdFrostRelic.canStack = true;
            bdFrostRelic.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(bdFrostRelic);

            if (WConfig.cfgBuff_FrostRelic.Value == true)
            {
                On.RoR2.IcicleAuraController.OnOwnerKillOther += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        if (self.cachedOwnerInfo.characterBody.GetBuffCount(bdFrostRelic) + 1 > self.maxIcicleCount)
                        {
                            self.cachedOwnerInfo.characterBody.RemoveOldestTimedBuff(bdFrostRelic);
                        }
                        self.cachedOwnerInfo.characterBody.AddTimedBuff(bdFrostRelic, self.icicleDuration);
                    }
                };
            }
            #endregion
            #region Feather
            //0.4706 0.1686 0.7765 1

            bdFeather = ScriptableObject.CreateInstance<BuffDef>();
            bdFeather.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffFeather.png");
            bdFeather.buffColor = new Color32(99, 192, 255, 255); //3FC5E3
            bdFeather.name = "wqol_BonusJump";
            bdFeather.isDebuff = false;
            bdFeather.canStack = true;
            bdFeather.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdFeather);

            bdFeatherVV = ScriptableObject.CreateInstance<BuffDef>();
            bdFeatherVV.iconSprite = bdFeather.iconSprite;
            //bdVoidFeather.buffColor = new Color32(120, 43, 198, 255); //3FC5E3
            bdFeatherVV.buffColor = new Color(0.7f, 0.45f, 1f, 1f); //3FC5E3
            bdFeatherVV.name = "wqol_BonusJumpVoid";
            bdFeatherVV.isDebuff = false;
            bdFeatherVV.canStack = true;
            bdFeatherVV.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(bdFeatherVV);
            #endregion

            //Bro this is so annoying
            //On.RoR2.BuffCatalog.SetBuffDefs += ReplaceBuffOrderIGuess;
        }

        private static void ShieldAndOpal(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (NetworkServer.active && self.teamComponent.teamIndex == TeamIndex.Player)
            {
                if (!self._outOfDanger)
                {
                    if (self.healthComponent.fullShield > 0 && self.healthComponent.shield < self.healthComponent.fullShield)
                    {
                        float duration = 7f - self.outOfDangerStopwatch;
                        if (self.healthComponent.itemCounts.missileVoid > 0)
                        {
                            self.AddTimedBuff(bdShieldDelayPink, duration);
                        }
                        else
                        {
                            self.AddTimedBuff(bdShieldDelay, duration);
                        }
                    }
                    if (self.inventory)
                    {
                        float duration = 7f - self.outOfDangerStopwatch;
                        if (self.inventory.GetItemCount(DLC1Content.Items.OutOfCombatArmor) > 0)
                        {
                            self.AddTimedBuff(bdOpalCooldown, duration);
                        }
                    }
                }
            }
        }
 

        public static void ModSupport()
        {
            VV_VoidFeather = ItemCatalog.FindItemIndex("VV_ITEM_DASHQUILL_ITEM");

            if (WConfig.cfgBuff_Feather.Value || WConfig.cfgBuff_VVFeather.Value)
            {
                

                On.EntityStates.GenericCharacterMain.ApplyJumpVelocity += FeatherRemoveBuffs;
                On.RoR2.CharacterBody.ReadBuffs += FeatherClient;
                CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            }


        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (!NetworkServer.active)
            {
                if (obj.isPlayerControlled)
                {
                    obj.gameObject.AddComponent<FeatherTrackerClients>();
                }
            }
        }

        private static void FeatherClient(On.RoR2.CharacterBody.orig_ReadBuffs orig, CharacterBody self, NetworkReader reader)
        {
            orig(self, reader);
            if (self.isPlayerControlled)
            {
                FeatherTrackerClients feather = self.GetComponent<FeatherTrackerClients>();
                if (feather)
                {
                    self.SetBuffCount(bdFeather.buffIndex, feather.amount);
                    self.SetBuffCount(bdFeatherVV.buffIndex, feather.amountVoid);
                }
            }
        }

        private static void FeatherRemoveBuffs(On.EntityStates.GenericCharacterMain.orig_ApplyJumpVelocity orig, CharacterMotor characterMotor, CharacterBody characterBody, float horizontalBonus, float verticalBonus, bool vault)
        {
            orig(characterMotor, characterBody, horizontalBonus, verticalBonus, vault);
            if (characterBody.isPlayerControlled && characterMotor.jumpCount + 1 > characterBody.baseJumpCount)
            {
                characterBody.SetBuffCount(bdFeather.buffIndex, characterBody.GetBuffCount(bdFeather) - 1);
                characterBody.SetBuffCount(bdFeatherVV.buffIndex, characterBody.GetBuffCount(bdFeatherVV) - 1);
                if (!NetworkServer.active)
                {
                    characterBody.GetComponent<FeatherTrackerClients>().SetVal(characterBody.GetBuffCount(bdFeather), characterBody.GetBuffCount(bdFeatherVV));
                }
            }
        }

 

    }
    public class FeatherTrackerClients : MonoBehaviour
    {
        public int amount = 0;
        public int amountVoid = 0;
        public CharacterBody body;

        public void Start()
        {
            body = GetComponent<CharacterBody>();
            CharacterMotor motor = this.GetComponent<CharacterMotor>();
            if (motor)
            {
                motor.onHitGroundAuthority += Motor_onHitGroundAuthority;
            }
        }

        private void Motor_onHitGroundAuthority(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            int hopoo = body.inventory.GetItemCountEffective(RoR2Content.Items.Feather);
            int voids = body.inventory.GetItemCountEffective(BuffTimers.VV_VoidFeather);
            if (WConfig.cfgBuff_Feather.Value)
            {
                body.SetBuffCount(BuffTimers.bdFeather.buffIndex, hopoo);
                amount = hopoo;
            }
            if (WConfig.cfgBuff_VVFeather.Value)
            {
                body.SetBuffCount(BuffTimers.bdFeatherVV.buffIndex, voids);
                amountVoid = voids;
            }
        }
        public void SetVal(int one, int two)
        {
            amount = one;
            amountVoid = two;
        }


    }

}
