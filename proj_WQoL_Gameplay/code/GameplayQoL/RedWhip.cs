using UnityEngine.AddressableAssets;

namespace WQoL_Gameplay
{
    public class RedWhip
    {

        public static void BetterRedWhipCheck()
        {
            if (!WConfig.cfgRedWhip.Value)
            {
                return;
            }

            //Commando
            //Huntress
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Huntress/AimArrowSnipe.asset").WaitForCompletion().isCombatSkill = false;
            //Bandit2
            //MulT
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Toolbot/ToolbotBodyToolbotDash.asset").WaitForCompletion().isCombatSkill = false;

            ////Engi
            //Engi Turrets placing doesn't count?
            //Engi Harpoon painting doesn't count?
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Engi/EngiBodyPlaceBubbleShield.asset").WaitForCompletion().isCombatSkill = false;

            //Artificer
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Mage/MageBodyFlyUp.asset").WaitForCompletion().isCombatSkill = false;

            //Merc
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyAssaulter.asset").WaitForCompletion().isCombatSkill = false;
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyFocusedAssault.asset").WaitForCompletion().isCombatSkill = false;

            //REX
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Treebot/TreebotBodyPlantSonicBoom.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Treebot/TreebotBodySonicBoom.asset").WaitForCompletion().isCombatSkill = false;

            //Loader
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Loader/FireHook.asset").WaitForCompletion().isCombatSkill = false;
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Loader/FireYankHook.asset").WaitForCompletion().isCombatSkill = false;

            //Acrid
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Croco/CrocoLeap.asset").WaitForCompletion().isCombatSkill = false;
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Croco/CrocoChainableLeap.asset").WaitForCompletion().isCombatSkill = false;

            //Captain
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Captain/PrepSupplyDrop.asset").WaitForCompletion().isCombatSkill = false;

            //Railgunner
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC1/Railgunner/RailgunnerBodyScopeLight.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC1/Railgunner/RailgunnerBodyScopeHeavy.asset").WaitForCompletion().isCombatSkill = false;

            //Void Fiend
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC1/VoidSurvivor/VoidBlinkDown.asset").WaitForCompletion().isCombatSkill = false;


            //Seeker
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC2/Seeker/SeekerBodySojourn.asset").WaitForCompletion().isCombatSkill = false;

            //Chef
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC2/Chef/ChefRolyPoly.asset").WaitForCompletion().isCombatSkill = false;
            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC2/Chef/ChefRolyPolyBoosted.asset").WaitForCompletion().isCombatSkill = false;


            //False Son

        }





    }

}
