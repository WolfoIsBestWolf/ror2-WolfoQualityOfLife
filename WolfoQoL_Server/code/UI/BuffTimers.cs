using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Server
{
    public class BuffTimers
    {
        public static BuffDef FakeHellFire;
        public static BuffDef FakePercentBurn;
        public static BuffDef FakeBugWings;
        public static BuffDef FakeStrides;
        public static BuffDef FakeRoseBuckle;
        public static BuffDef FakeFrozen;
        public static BuffDef FakeHeadstompOn;
        public static BuffDef FakeHeadstompOff;
        public static BuffDef FakeFeather;
        public static BuffDef FakeShieldDelay;
        public static BuffDef FakeShieldDelayPink;
        //public static BuffDef FakeEgg;
        //public static BuffDef FakeHellFireDuration;
        public static BuffDef FakeFrostRelic;
        public static BuffDef FakeOpalCooldown;


        public static void Buffs_NewBuffs()
        {
            bool riskyModEnabled = false;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod"))
            {
                riskyModEnabled = true;
            }


            #region Helfire Burn
            BuffDef NormalBurn = LegacyResourcesAPI.Load<BuffDef>("buffdefs/OnFire");
            FakeHellFire = ScriptableObject.CreateInstance<BuffDef>();
            FakeHellFire.iconSprite = NormalBurn.iconSprite;
            FakeHellFire.buffColor = new Color32(50, 188, 255, 255);
            FakeHellFire.name = "visual_HelFire";
            FakeHellFire.isDebuff = false;
            FakeHellFire.canStack = true;
            FakeHellFire.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeHellFire);
            #endregion
            #region
            //Obsolete
            FakePercentBurn = ScriptableObject.CreateInstance<BuffDef>();
            FakePercentBurn.iconSprite = NormalBurn.iconSprite;
            //FakePercentBurn.buffColor = new Color32(226, 91, 69, 255); //E25B45 Overheat
            FakePercentBurn.buffColor = new Color32(203, 53, 38, 255); //CB3526 Blazing Elite
            FakePercentBurn.name = "visual_EnemyBurn";
            FakePercentBurn.isDebuff = false;
            FakePercentBurn.canStack = true;
            FakePercentBurn.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakePercentBurn);
            #endregion


            #region Bug Wings Jetpack
            Texture2D BugUp = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffBeetleUp.png");
            Sprite BugUpS = Sprite.Create(BugUp, v.rec128, v.half);

            FakeBugWings = ScriptableObject.CreateInstance<BuffDef>();
            FakeBugWings.iconSprite = BugUpS;
            FakeBugWings.buffColor = new Color32(218, 136, 251, 255); //DA88FB Tesla Coil
            FakeBugWings.name = "visual_BugFlight";
            FakeBugWings.isDebuff = false;
            FakeBugWings.canStack = false;
            FakeBugWings.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(FakeBugWings);

            if (WConfig.cfgBuff_BugFlight.Value == true)
            {
                On.RoR2.JetpackController.StartFlight += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.targetBody.AddTimedBuff(FakeBugWings, self.duration);
                    }
                };
                On.RoR2.JetpackController.OnDestroy += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.targetBody)
                        {
                            self.targetBody.ClearTimedBuffs(FakeBugWings);
                        }
                    }
                };
            }
            #endregion
            #region Strides of Heresy
            Texture2D BWLunarShell = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffLunarShellIcon.png");
            Sprite BWLunarShellS = Sprite.Create(BWLunarShell, v.rec128, v.half);

            FakeStrides = ScriptableObject.CreateInstance<BuffDef>();
            FakeStrides.iconSprite = BWLunarShellS;
            FakeStrides.buffColor = new Color32(189, 176, 255, 255); //BDB0FF
            FakeStrides.name = "visual_ShadowIntangible";
            FakeStrides.isDebuff = false;
            FakeStrides.canStack = false;
            FakeStrides.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(FakeStrides);


            if (WConfig.cfgBuff_Strides.Value == true)
            {
                On.EntityStates.GhostUtilitySkillState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        float tempdur = EntityStates.GhostUtilitySkillState.baseDuration * self.outer.GetComponentInParent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement);
                        self.outer.GetComponentInParent<CharacterBody>().AddTimedBuff(FakeStrides, tempdur);
                    }
                };

                On.EntityStates.GhostUtilitySkillState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.outer.GetComponentInParent<CharacterBody>().ClearTimedBuffs(FakeStrides);
                    }
                };
            }
            #endregion
            #region Rose Buckler
            Texture2D RoundShieldTex = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffBodyArmor.png");
            Sprite RoundShieldS = Sprite.Create(RoundShieldTex, v.rec128, v.half);

            FakeRoseBuckle = ScriptableObject.CreateInstance<BuffDef>();
            FakeRoseBuckle.iconSprite = RoundShieldS;
            FakeRoseBuckle.buffColor = new Color32(251, 199, 38, 255); //FBC726
            FakeRoseBuckle.name = "visual_SprintArmor";
            FakeRoseBuckle.isDebuff = false;
            FakeRoseBuckle.canStack = false;
            FakeRoseBuckle.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(FakeRoseBuckle);

            if (WConfig.cfgBuff_SprintArmor.Value == true)
            {
                On.RoR2.CharacterBody.OnSprintStart += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            self.AddBuff(FakeRoseBuckle);
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
                            if (!self.HasBuff(FakeRoseBuckle)) { return; }
                            self.RemoveBuff(FakeRoseBuckle);
                        }
                    }
                };
            }
            #endregion
            #region Frozen
            Texture2D CubeBroke = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffBrokenCube.png");
            Sprite CubeBrokeS = Sprite.Create(CubeBroke, v.rec128, v.half);

            FakeFrozen = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrozen.iconSprite = CubeBrokeS;
            FakeFrozen.buffColor = new Color32(184, 216, 239, 255); //CFDBE0 // B8D8EF
            FakeFrozen.name = "visual_Frozen";
            FakeFrozen.isDebuff = false;
            FakeFrozen.canStack = true;
            FakeFrozen.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeFrozen);

            if (WConfig.cfgBuff_Frozen.Value == true && riskyModEnabled == false)
            {
                On.EntityStates.FrozenState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        for (float num5 = 0; num5 <= self.freezeDuration; num5 = num5 + 0.5f)
                        {
                            //self.characterBody.AddTimedBuff(FakeFrozen, (float)num5);
                            self.characterBody.AddTimedBuffAuthority(FakeFrozen.buffIndex, (float)num5);
                        }
                    }
                };

                On.EntityStates.FrozenState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.characterBody.ClearTimedBuffs(FakeFrozen);
                    }
                };
            }
            #endregion
            #region HeadStomper
            Texture2D HeadStompOn = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffHeadStompOn.png");
            Sprite HeadStompOnS = Sprite.Create(HeadStompOn, v.rec128, v.half);

            Texture2D HeadStompOff = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffHeadStompOff.png");
            Sprite HeadStompOffS = Sprite.Create(HeadStompOff, v.rec128, v.half);

            FakeHeadstompOn = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOn.iconSprite = HeadStompOnS;
            FakeHeadstompOn.buffColor = new Color32(255, 250, 250, 255);
            FakeHeadstompOn.name = "visual_HeadstomperReady";
            FakeHeadstompOn.isDebuff = false;
            FakeHeadstompOn.canStack = false;
            FakeHeadstompOn.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(FakeHeadstompOn);


            FakeHeadstompOff = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOff.iconSprite = HeadStompOffS;
            FakeHeadstompOff.buffColor = new Color32(250, 250, 255, 255);
            FakeHeadstompOff.name = "visual_HeadstomperCooldown";
            FakeHeadstompOff.isDebuff = false;
            FakeHeadstompOff.canStack = true;
            FakeHeadstompOff.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeHeadstompOff);

            if (WConfig.cfgBuff_Headstomper.Value == true && riskyModEnabled == false)
            {
                On.EntityStates.Headstompers.BaseHeadstompersState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self is EntityStates.Headstompers.HeadstompersIdle)
                        {
                            if (self.body.GetBuffCount(FakeHeadstompOn) == 0)
                            {
                                self.body.AddBuff(FakeHeadstompOn);
                            }
                        }
                    }
                };

                On.EntityStates.Headstompers.HeadstompersCooldown.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.body.RemoveBuff(FakeHeadstompOn);
                        for (float newtimer = self.duration; 0 < newtimer; newtimer--)
                        {
                            self.body.AddTimedBuff(FakeHeadstompOff, (float)newtimer);
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
                            self.body.RemoveBuff(FakeHeadstompOn);
                        }
                    }
                };
            }
            #endregion
            #region Shield Delay and Opal Delay
            BuffDef MedkitDelay = LegacyResourcesAPI.Load<BuffDef>("buffdefs/MedkitHeal");

            FakeShieldDelay = ScriptableObject.CreateInstance<BuffDef>();
            FakeShieldDelay.iconSprite = MedkitDelay.iconSprite;
            ///FakeShieldDelay.buffColor = new Color32(66, 100, 220, 255); //4264DC
            FakeShieldDelay.buffColor = new Color(0.3529f, 0.4863f, 0.9647f, 1f); //
            FakeShieldDelay.name = "visual_ShieldDelay";
            FakeShieldDelay.isDebuff = false;
            FakeShieldDelay.canStack = false;
            FakeShieldDelay.isCooldown = false;
            FakeShieldDelay.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeShieldDelay);

            FakeShieldDelayPink = ScriptableObject.CreateInstance<BuffDef>();
            FakeShieldDelayPink.iconSprite = MedkitDelay.iconSprite;
            //FakeShieldDelayPink.buffColor = RoR2.UI.HealthBar.voidShieldsColor;
            FakeShieldDelayPink.buffColor = new Color(1f, 0.2235f, 0.7804f, 1); //
            FakeShieldDelayPink.name = "visual_ShieldDelayPink";
            FakeShieldDelayPink.isDebuff = false;
            FakeShieldDelayPink.canStack = false;
            FakeShieldDelayPink.isCooldown = false;
            FakeShieldDelayPink.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeShieldDelayPink);


            BuffDef bdOutOfCombatArmorBuff = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion();
            FakeOpalCooldown = ScriptableObject.CreateInstance<BuffDef>();
            FakeOpalCooldown.iconSprite = bdOutOfCombatArmorBuff.iconSprite;
            FakeOpalCooldown.buffColor = new Color(0.4151f, 0.4014f, 0.4014f, 1); //4264DC
            FakeOpalCooldown.name = "visual_OutOfCombatArmorCooldown";
            FakeOpalCooldown.isDebuff = false;
            FakeOpalCooldown.canStack = false;
            FakeOpalCooldown.isCooldown = false;
            FakeOpalCooldown.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeOpalCooldown);

            //If disabled
            //FakeOpalCooldown.isHidden = true;
            if (WConfig.cfgBuff_ShieldOpalCooldown.Value == true)
            {
                On.RoR2.CharacterBody.OnTakeDamageServer += (orig, self, damageReport) =>
                {
                    orig(self, damageReport);
                    if (NetworkServer.active && self.teamComponent.teamIndex == TeamIndex.Player)
                    {
                        if (self.outOfDangerStopwatch < 0.5f)
                        {
                            if (self.healthComponent.fullShield > 0 && self.healthComponent.shield < self.healthComponent.fullShield)
                            {
                                float duration = 7f - self.outOfDangerStopwatch;
                                if (self.healthComponent.itemCounts.missileVoid > 0)
                                {
                                    self.AddTimedBuff(FakeShieldDelayPink, duration);
                                }
                                else
                                {
                                    self.AddTimedBuff(FakeShieldDelay, duration);
                                }
                            }
                            if (self.inventory)
                            {
                                float duration = 7f - self.outOfDangerStopwatch;
                                if (self.inventory.GetItemCount(DLC1Content.Items.OutOfCombatArmor) > 0)
                                {
                                    self.AddTimedBuff(FakeOpalCooldown, duration);
                                }
                            }
                        }
                    }
                };
                //On.RoR2.HealthComponent.HandleDamageDealt Runs slightly less often but some server message shit idk

                On.RoR2.HealthComponent.ForceShieldRegen += (orig, self) =>
                {
                    orig(self);
                    if (self.body && self.body.HasBuff(FakeShieldDelay))
                    {
                        self.body.ClearTimedBuffs(FakeShieldDelay);
                        self.body.ClearTimedBuffs(FakeShieldDelayPink);
                    }
                };
            }
            #endregion



            /*
            Texture2D EggIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            EggIcon.LoadImage(Properties.Resources.texBuffEgg, true);
            EggIcon.filterMode = FilterMode.Bilinear;
            Sprite EggIconS = Sprite.Create(EggIcon, v.rec128, v.half);

            FakeEgg = ScriptableObject.CreateInstance<BuffDef>();
            FakeEgg.iconSprite = EggIconS;
            //FakeEgg.buffColor = new Color32(255, 157, 174, 255); //FF9DAE
            FakeEgg.buffColor = new Color32(255, 180, 137, 255); //FFB489
            FakeEgg.name = "visual_VolcanoEgg";
            FakeEgg.isDebuff = false;
            FakeEgg.canStack = false;
            ContentAddition.AddBuffDef(FakeEgg);


            
            if (WConfig.EggVisualA.Value == true)
            {
                On.RoR2.FireballVehicle.OnPassengerEnter += (orig, self, passenger) =>
                {
                    orig(self, passenger);
                    if (NetworkServer.active)
                    {
                        passenger.GetComponent<CharacterBody>().AddTimedBuff(FakeEgg, self.duration);
                    }
                };
                On.RoR2.FireballVehicle.FixedUpdate += (orig, self) =>
                {
                    if (NetworkServer.active)
                    {
                        if ((self.overlapFireAge + Time.fixedDeltaTime) > 1f / (self.overlapFireFrequency += Time.fixedDeltaTime))
                        {
                            self.vehicleSeat.currentPassengerBody.AddTimedBuff(FakeEgg, self.duration - self.age);
                        }
                    }
                    orig(self);
                };

                On.RoR2.FireballVehicle.OnPassengerExit += (orig, self, passenger) =>
                {
                    orig(self, passenger);
                    if (NetworkServer.active)
                    {
                        passenger.GetComponent<CharacterBody>().ClearTimedBuffs(FakeEgg);
                    }
                };
            }
            */
            /*
            Texture2D TinctureIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TinctureIcon.LoadImage(Properties.Resources.texBuffTincture, true);
            TinctureIcon.filterMode = FilterMode.Bilinear;
            Sprite TinctureIconS = Sprite.Create(TinctureIcon, v.rec128, v.half);

            FakeHellFireDuration = ScriptableObject.CreateInstance<BuffDef>();
            FakeHellFireDuration.iconSprite = TinctureIconS;
            FakeHellFireDuration.buffColor = new Color32(115, 182, 165, 255); //
            FakeHellFireDuration.name = "visual_TinctureIgnition";
            FakeHellFireDuration.isDebuff = false;
            FakeHellFireDuration.canStack = false;
            ContentAddition.AddBuffDef(FakeHellFireDuration);
            
            if (WConfig.TinctureVisual.Value == true)
            {
                On.RoR2.CharacterBody.AddHelfireDuration += (orig, self, duration) =>
                {
                    orig(self, duration);
                    if (NetworkServer.active)
                    {
                        self.AddTimedBuff(FakeHellFireDuration, duration);
                    }
                };

            }
            */
            #region Frost Relic
            Texture2D FrostRelicIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffFrostRelic.png");
            Sprite FrostRelicIconS = Sprite.Create(FrostRelicIcon, v.rec128, v.half);

            FakeFrostRelic = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrostRelic.iconSprite = FrostRelicIconS;
            FakeFrostRelic.buffColor = new Color32(202, 229, 255, 255); //CAE5FF
            FakeFrostRelic.name = "visual_FrostRelicGrowth";
            FakeFrostRelic.isDebuff = false;
            FakeFrostRelic.canStack = true;
            FakeFrostRelic.ignoreGrowthNectar = !WConfig.BuffsAffectNectar.Value;
            ContentAddition.AddBuffDef(FakeFrostRelic);

            if (WConfig.cfgBuff_FrostRelic.Value == true)
            {
                On.RoR2.IcicleAuraController.OnOwnerKillOther += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        if (self.cachedOwnerInfo.characterBody.GetBuffCount(FakeFrostRelic) + 1 > self.maxIcicleCount)
                        {
                            self.cachedOwnerInfo.characterBody.RemoveOldestTimedBuff(FakeFrostRelic);
                        }
                        self.cachedOwnerInfo.characterBody.AddTimedBuff(FakeFrostRelic, self.icicleDuration);
                    }
                };
            }
            #endregion
            #region Feather
            //0.4706 0.1686 0.7765 1
            Texture2D FeatherBuff = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffFeather.png");
            Sprite FeatherBuffS = Sprite.Create(FeatherBuff, v.rec128, v.half);

            FakeFeather = ScriptableObject.CreateInstance<BuffDef>();
            FakeFeather.iconSprite = FeatherBuffS;
            FakeFeather.buffColor = new Color32(99, 192, 255, 255); //3FC5E3
            FakeFeather.name = "visual_BonusJump";
            FakeFeather.isDebuff = false;
            FakeFeather.canStack = true;
            FakeFeather.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeFeather);

            FakeVoidFeather = ScriptableObject.CreateInstance<BuffDef>();
            FakeVoidFeather.iconSprite = FakeFeather.iconSprite;
            //FakeVoidFeather.buffColor = new Color32(120, 43, 198, 255); //3FC5E3
            FakeVoidFeather.buffColor = new Color(0.7f, 0.45f, 1f, 1f); //3FC5E3
            FakeVoidFeather.name = "visual_BonusJumpVoid";
            FakeVoidFeather.isDebuff = false;
            FakeVoidFeather.canStack = true;
            FakeVoidFeather.ignoreGrowthNectar = true;
            ContentAddition.AddBuffDef(FakeVoidFeather);
            #endregion

            //Bro this is so annoying
            //On.RoR2.BuffCatalog.SetBuffDefs += ReplaceBuffOrderIGuess;
        }


        private static void ReplaceBuffOrderIGuess(On.RoR2.BuffCatalog.orig_SetBuffDefs orig, BuffDef[] newBuffDefs)
        {
            try
            {
                BuffDef[] new2Buffs = new BuffDef[newBuffDefs.Length + 1];
                int j = 0;
                for (int i = 0; i < newBuffDefs.Length; i++)
                {
                    if (newBuffDefs[i] == DLC1Content.Buffs.OutOfCombatArmorBuff)
                    {
                        new2Buffs[j] = newBuffDefs[i];
                        j++;
                        new2Buffs[j] = FakeOpalCooldown;
                    }
                    else
                    {
                        new2Buffs[j] = newBuffDefs[i];
                    }
                    Debug.Log(newBuffDefs[i]);
                    Debug.Log(new2Buffs[j]);
                    j++;
                }
                orig(new2Buffs);
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
                orig(newBuffDefs);
                return;
            }
            Debug.LogWarning("How did this run");
            orig(newBuffDefs);
        }

        public static BuffDef FakeVoidFeather;
        public static ItemIndex VV_VoidFeather = ItemIndex.None;

        public static void ModSupport()
        {
            //Cant add buffs this late don't add buffs here

            VV_VoidFeather = ItemCatalog.FindItemIndex("VV_ITEM_DASHQUILL_ITEM");
            //Debug.LogWarning("Void Feather "+VV_VoidFeather);


            if (WConfig.cfgBuff_Feather.Value == true)
            {
                if (VV_VoidFeather != ItemIndex.None)
                {
                    FakeVoidFeather.isHidden = false;
                    On.RoR2.CharacterMotor.OnLanded += (orig, self) =>
                    {
                        orig(self);
                        if (self.body && self.body.isPlayerControlled)
                        {
                            self.body.SetBuffCount(FakeFeather.buffIndex, self.body.inventory.GetItemCount(RoR2Content.Items.Feather));
                            self.body.SetBuffCount(FakeVoidFeather.buffIndex, self.body.inventory.GetItemCount(VV_VoidFeather));
                            if (!NetworkServer.active)
                            {
                                self.GetComponent<FeatherTrackerClients>().SetVal(self.body.GetBuffCount(FakeFeather), self.body.GetBuffCount(FakeVoidFeather));
                            }
                        }
                    };
                }
                else
                {
                    On.RoR2.CharacterMotor.OnLanded += (orig, self) =>
                    {
                        orig(self);
                        if (self.body && self.body.isPlayerControlled)
                        {
                            self.body.SetBuffCount(FakeFeather.buffIndex, self.body.inventory.GetItemCount(RoR2Content.Items.Feather));
                            if (!NetworkServer.active)
                            {
                                self.GetComponent<FeatherTrackerClients>().SetVal(self.body.GetBuffCount(FakeFeather), self.body.GetBuffCount(FakeVoidFeather));
                            }
                        }
                    };
                }

                On.EntityStates.GenericCharacterMain.ApplyJumpVelocity += FeatherRemoveBuffs;
                On.RoR2.CharacterBody.ReadBuffs += FeatherClient;
                RoR2.CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
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
                    self.SetBuffCount(FakeFeather.buffIndex, feather.amount);
                    self.SetBuffCount(FakeVoidFeather.buffIndex, feather.amountVoid);
                }
            }
        }

        private static void FeatherRemoveBuffs(On.EntityStates.GenericCharacterMain.orig_ApplyJumpVelocity orig, CharacterMotor characterMotor, CharacterBody characterBody, float horizontalBonus, float verticalBonus, bool vault)
        {
            orig(characterMotor, characterBody, horizontalBonus, verticalBonus, vault);
            if (characterBody.isPlayerControlled && characterMotor.jumpCount + 1 > characterBody.baseJumpCount)
            {
                characterBody.SetBuffCount(FakeFeather.buffIndex, characterBody.GetBuffCount(FakeFeather) - 1);
                characterBody.SetBuffCount(FakeVoidFeather.buffIndex, characterBody.GetBuffCount(FakeVoidFeather) - 1);
                if (!NetworkServer.active)
                {
                    characterBody.GetComponent<FeatherTrackerClients>().SetVal(characterBody.GetBuffCount(FakeFeather), characterBody.GetBuffCount(FakeVoidFeather));
                }
            }
        }





        public static void GetDotDef()
        {
            DotController.GetDotDef(DotController.DotIndex.Burn).terminalTimedBuff = null;
            DotController.GetDotDef(DotController.DotIndex.Burn).terminalTimedBuffDuration = 0;
            DotController.GetDotDef(DotController.DotIndex.StrongerBurn).terminalTimedBuff = null;
            DotController.GetDotDef(DotController.DotIndex.StrongerBurn).terminalTimedBuffDuration = 0;

            DotController.GetDotDef(DotController.DotIndex.Helfire).associatedBuff = FakeHellFire;
            DotController.GetDotDef(DotController.DotIndex.PercentBurn).associatedBuff = FakePercentBurn;
        }


    }
    public class FeatherTrackerClients : MonoBehaviour
    {
        public int amount = 0;
        public int amountVoid = 0;
        public void SetVal(int one, int two)
        {
            amount = one;
            amountVoid = two;
        }
    }

}
