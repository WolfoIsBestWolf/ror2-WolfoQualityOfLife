using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


namespace WolfoQualityOfLife
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


        public static void BuffColorChanger()
        {

            if (WConfig.cfgBuff_RepeatColors.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/AffixHauntedRecipient").buffColor = new Color32(148, 215, 214, 255); //94D7D6 Celestine Elite
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/SmallArmorBoost").buffColor = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/Slow60").buffColor;
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/WhipBoost").buffColor = new Color32(245, 158, 73, 255); //E8813D

                Texture2D texBuffRaincoat = new Texture2D(128, 128, TextureFormat.DXT5, false);
                texBuffRaincoat.LoadImage(Properties.Resources.texBuffRaincoat, true);
                texBuffRaincoat.filterMode = FilterMode.Bilinear;
                Sprite texBuffRaincoatS = Sprite.Create(texBuffRaincoat, new Rect(0, 0, 128, 128), v.half);
                Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/ImmuneToDebuff/bdImmuneToDebuffReady.asset").WaitForCompletion().iconSprite = texBuffRaincoatS;
            }





            BuffDef NormalBurn = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/OnFire");
            FakeHellFire = ScriptableObject.CreateInstance<BuffDef>();
            FakeHellFire.iconSprite = NormalBurn.iconSprite;
            FakeHellFire.buffColor = new Color32(50, 188, 255, 255);
            FakeHellFire.name = "visual_HelFire";
            FakeHellFire.isDebuff = false;
            FakeHellFire.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeHellFire);

            //Obsolete
            FakePercentBurn = ScriptableObject.CreateInstance<BuffDef>();
            FakePercentBurn.iconSprite = NormalBurn.iconSprite;
            //FakePercentBurn.buffColor = new Color32(226, 91, 69, 255); //E25B45 Overheat
            FakePercentBurn.buffColor = new Color32(203, 53, 38, 255); //CB3526 Blazing Elite
            FakePercentBurn.name = "visual_EnemyBurn";
            FakePercentBurn.isDebuff = false;
            FakePercentBurn.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakePercentBurn);
            //



            Texture2D BugUp = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BugUp.LoadImage(Properties.Resources.texBuffBeetleUp, true);
            BugUp.filterMode = FilterMode.Bilinear;
            Sprite BugUpS = Sprite.Create(BugUp, v.rec128, v.half);

            FakeBugWings = ScriptableObject.CreateInstance<BuffDef>();
            FakeBugWings.iconSprite = BugUpS;
            FakeBugWings.buffColor = new Color32(218, 136, 251, 255); //DA88FB Tesla Coil
            FakeBugWings.name = "visual_BugFlight";
            FakeBugWings.isDebuff = false;
            FakeBugWings.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeBugWings);

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


            Texture2D BWLunarShell = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BWLunarShell.LoadImage(Properties.Resources.texBuffLunarShellIcon, true);
            BWLunarShell.filterMode = FilterMode.Bilinear;
            Sprite BWLunarShellS = Sprite.Create(BWLunarShell, v.rec128, v.half);

            FakeStrides = ScriptableObject.CreateInstance<BuffDef>();
            FakeStrides.iconSprite = BWLunarShellS;
            FakeStrides.buffColor = new Color32(189, 176, 255, 255); //BDB0FF
            FakeStrides.name = "visual_ShadowIntangible";
            FakeStrides.isDebuff = false;
            FakeStrides.canStack = false;

            R2API.ContentAddition.AddBuffDef(FakeStrides);


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



            Texture2D RoundShieldTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
            RoundShieldTex.LoadImage(Properties.Resources.texBuffBodyArmor, true);
            RoundShieldTex.filterMode = FilterMode.Bilinear;
            Sprite RoundShieldS = Sprite.Create(RoundShieldTex, v.rec128, v.half);
            /*Texture2D RoundShieldTex2 = new Texture2D(128, 128, TextureFormat.DXT5, false);
            RoundShieldTex2.LoadImage(Properties.Resources.texBuffRoundShieldBuff, true);
            RoundShieldTex2.filterMode = FilterMode.Bilinear;
            Sprite RoundShieldS2 = Sprite.Create(RoundShieldTex2, v.rec128, v.half);*/


            FakeRoseBuckle = ScriptableObject.CreateInstance<BuffDef>();
            FakeRoseBuckle.iconSprite = RoundShieldS;
            FakeRoseBuckle.buffColor = new Color32(251, 199, 38, 255); //FBC726
            FakeRoseBuckle.name = "visual_SprintArmor";
            FakeRoseBuckle.isDebuff = false;
            FakeRoseBuckle.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeRoseBuckle);

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
                    if (UnityEngine.Networking.NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            if (!self.HasBuff(FakeRoseBuckle)) { return; }
                            self.RemoveBuff(FakeRoseBuckle);
                        }
                    }
                };
            }


            Texture2D CubeBroke = new Texture2D(128, 128, TextureFormat.DXT5, false);
            CubeBroke.LoadImage(Properties.Resources.texBuffBrokenCube, true);
            CubeBroke.filterMode = FilterMode.Bilinear;
            Sprite CubeBrokeS = Sprite.Create(CubeBroke, v.rec128, v.half);

            FakeFrozen = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrozen.iconSprite = CubeBrokeS;
            FakeFrozen.buffColor = new Color32(184, 216, 239, 255); //CFDBE0 // B8D8EF
            FakeFrozen.name = "visual_Frozen";
            FakeFrozen.isDebuff = false;
            FakeFrozen.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeFrozen);

            if (WConfig.cfgBuff_Frozen.Value == true)
            {
                On.EntityStates.FrozenState.OnEnter += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        for (float num5 = 0; num5 <= self.freezeDuration; num5 = num5 + 0.5f)
                        {
                            self.characterBody.AddTimedBuff(FakeFrozen, (float)num5);
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


            Texture2D HeadStompOn = new Texture2D(128, 128, TextureFormat.DXT5, false);
            HeadStompOn.LoadImage(Properties.Resources.texBuffHeadStompOn, true);
            HeadStompOn.filterMode = FilterMode.Bilinear;
            Sprite HeadStompOnS = Sprite.Create(HeadStompOn, v.rec128, v.half);
            Texture2D HeadStompOff = new Texture2D(128, 128, TextureFormat.DXT5, false);
            HeadStompOff.LoadImage(Properties.Resources.texBuffHeadStompOff, true);
            HeadStompOff.filterMode = FilterMode.Bilinear;
            Sprite HeadStompOffS = Sprite.Create(HeadStompOff, v.rec128, v.half);

            FakeHeadstompOn = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOn.iconSprite = HeadStompOnS;
            FakeHeadstompOn.buffColor = new Color32(255, 250, 250, 255);
            FakeHeadstompOn.name = "visual_HeadstomperReady";
            FakeHeadstompOn.isDebuff = false;
            FakeHeadstompOn.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeHeadstompOn);


            FakeHeadstompOff = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOff.iconSprite = HeadStompOffS;
            FakeHeadstompOff.buffColor = new Color32(250, 250, 255, 255);
            FakeHeadstompOff.name = "visual_HeadstomperCooldown";
            FakeHeadstompOff.isDebuff = false;
            FakeHeadstompOff.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeHeadstompOff);


            if (WConfig.cfgBuff_Headstomper.Value == true)
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




            BuffDef MedkitDelay = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/MedkitHeal");

            FakeShieldDelay = ScriptableObject.CreateInstance<BuffDef>();
            FakeShieldDelay.iconSprite = MedkitDelay.iconSprite;
            ///FakeShieldDelay.buffColor = new Color32(66, 100, 220, 255); //4264DC
            FakeShieldDelay.buffColor = new Color(0.3529f, 0.4863f, 0.9647f, 1f); //
            FakeShieldDelay.name = "visual_ShieldDelay";
            FakeShieldDelay.isDebuff = false;
            FakeShieldDelay.canStack = false;
            FakeShieldDelay.isCooldown = true;
            R2API.ContentAddition.AddBuffDef(FakeShieldDelay);

            FakeShieldDelayPink = ScriptableObject.CreateInstance<BuffDef>();
            FakeShieldDelayPink.iconSprite = MedkitDelay.iconSprite;
            //FakeShieldDelayPink.buffColor = RoR2.UI.HealthBar.voidShieldsColor;
            FakeShieldDelayPink.buffColor = new Color(1f, 0.2235f, 0.7804f, 1); //
            FakeShieldDelayPink.name = "visual_ShieldDelayPink";
            FakeShieldDelayPink.isDebuff = false;
            FakeShieldDelayPink.canStack = false;
            FakeShieldDelayPink.isCooldown = true;
            R2API.ContentAddition.AddBuffDef(FakeShieldDelayPink);


            BuffDef bdOutOfCombatArmorBuff = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion();
            FakeOpalCooldown = ScriptableObject.CreateInstance<BuffDef>();
            FakeOpalCooldown.iconSprite = bdOutOfCombatArmorBuff.iconSprite;
            FakeOpalCooldown.buffColor = new Color(0.4151f, 0.4014f, 0.4014f, 1); //4264DC
            FakeOpalCooldown.name = "visual_OutOfCombatArmorCooldown";
            FakeOpalCooldown.isDebuff = false;
            FakeOpalCooldown.canStack = false;
            FakeOpalCooldown.isCooldown = true;
            R2API.ContentAddition.AddBuffDef(FakeOpalCooldown);

            //If disabled
            //FakeOpalCooldown.isHidden = true;

 
            if (WConfig.cfgBuff_ShieldOpalCooldown.Value == true)
            {
                On.RoR2.CharacterBody.OnTakeDamageServer += (orig, self, damageReport) =>
                {
                    orig(self, damageReport);
                    if (NetworkServer.active  && self.teamComponent.teamIndex == TeamIndex.Player)
                    {
                        if (damageReport.damageDealt > 0f)
                        {
                            if (self.healthComponent.fullShield > 0 && self.healthComponent.shield < self.healthComponent.fullShield)
                            {
                                if (self.healthComponent.itemCounts.missileVoid > 0)
                                {
                                    self.AddTimedBuff(FakeShieldDelayPink, 7);
                                }
                                else
                                {
                                    self.AddTimedBuff(FakeShieldDelay, 7);
                                }
                            }
                            if (self.inventory.GetItemCount(DLC1Content.Items.OutOfCombatArmor) > 0)
                            {
                                self.AddTimedBuff(FakeOpalCooldown, 7);
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
            R2API.ContentAddition.AddBuffDef(FakeEgg);


            
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
            R2API.ContentAddition.AddBuffDef(FakeHellFireDuration);
            
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

            Texture2D FrostRelicIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            FrostRelicIcon.LoadImage(Properties.Resources.texBuffFrostRelic, true);
            FrostRelicIcon.filterMode = FilterMode.Bilinear;
            Sprite FrostRelicIconS = Sprite.Create(FrostRelicIcon, v.rec128, v.half);

            FakeFrostRelic = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrostRelic.iconSprite = FrostRelicIconS;
            FakeFrostRelic.buffColor = new Color32(202, 229, 255, 255); //CAE5FF
            FakeFrostRelic.name = "visual_FrostRelicGrowth";
            FakeFrostRelic.isDebuff = false;
            FakeFrostRelic.canStack = true;

            R2API.ContentAddition.AddBuffDef(FakeFrostRelic);

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

            //0.4706 0.1686 0.7765 1




            Texture2D FeatherBuff = new Texture2D(128, 128, TextureFormat.DXT5, false);
            FeatherBuff.LoadImage(Properties.Resources.texBuffFeather, true);
            FeatherBuff.filterMode = FilterMode.Bilinear;
            Sprite FeatherBuffS = Sprite.Create(FeatherBuff, v.rec128, v.half);

            FakeFeather = ScriptableObject.CreateInstance<BuffDef>();
            FakeFeather.iconSprite = FeatherBuffS;
            FakeFeather.buffColor = new Color32(99, 192, 255, 255); //3FC5E3
            FakeFeather.name = "visual_BonusJump";
            FakeFeather.isDebuff = false;
            FakeFeather.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeFeather);

            FakeVoidFeather = ScriptableObject.CreateInstance<BuffDef>();
            FakeVoidFeather.iconSprite = FakeFeather.iconSprite;
            //FakeVoidFeather.buffColor = new Color32(120, 43, 198, 255); //3FC5E3
            FakeVoidFeather.buffColor = new Color(0.7f, 0.45f, 1f, 1f); //3FC5E3
            FakeVoidFeather.name = "visual_BonusJumpVoid";
            FakeVoidFeather.isDebuff = false;
            FakeVoidFeather.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeVoidFeather);

            if (WConfig.cfgBuff_Feather.Value == true)
            {
                //Has effective authority or smth

                //These hooks are soemehow fuckin client only
                On.RoR2.CharacterMotor.OnLanded += (orig, self) =>
                {
                    orig(self);
                    if (self.body && self.body.isPlayerControlled)
                    {
                        self.body.SetBuffCount(FakeFeather.buffIndex, self.body.inventory.GetItemCount(RoR2Content.Items.Feather));
                    }
                };

                On.EntityStates.GenericCharacterMain.ApplyJumpVelocity += FeatherRemoveBuffs;
            }

        }


        private static void FeatherRemoveBuffs(On.EntityStates.GenericCharacterMain.orig_ApplyJumpVelocity orig, CharacterMotor characterMotor, CharacterBody characterBody, float horizontalBonus, float verticalBonus, bool vault)
        {
            orig(characterMotor, characterBody, horizontalBonus, verticalBonus, vault);
            if (characterBody.isPlayerControlled && characterMotor.jumpCount + 1 > characterBody.baseJumpCount)
            {
                characterBody.SetBuffCount(FakeFeather.buffIndex, characterBody.GetBuffCount(FakeFeather) - 1);
                characterBody.SetBuffCount(FakeVoidFeather.buffIndex, characterBody.GetBuffCount(FakeVoidFeather) - 1);
            }
        }




        public static BuffDef FakeVoidFeather;
        public static ItemIndex VV_VoidFeather = ItemIndex.None;

        public static void ModSupport()
        {
            //Cant add buffs this late don't add buffs here

            VV_VoidFeather = ItemCatalog.FindItemIndex("VV_ITEM_DASHQUILL_ITEM");
            //Debug.LogWarning("Void Feather "+VV_VoidFeather);

            if (VV_VoidFeather != ItemIndex.None && WConfig.cfgBuff_Feather.Value == true)
            {
                FakeVoidFeather.isHidden = false;
                On.RoR2.CharacterMotor.OnLanded += (orig, self) =>
                {
                    orig(self);
                    if (self.body && self.body.isPlayerControlled)
                    {
                        self.body.SetBuffCount(FakeVoidFeather.buffIndex, self.body.inventory.GetItemCount(VV_VoidFeather));
                    }
                };
            }


        }




        public static void GetDotDef()
        {
            DotController.GetDotDef(DotController.DotIndex.Helfire).associatedBuff = FakeHellFire;
            DotController.GetDotDef(DotController.DotIndex.PercentBurn).associatedBuff = FakePercentBurn;
        }


    }


}
