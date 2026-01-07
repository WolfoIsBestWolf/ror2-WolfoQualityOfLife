using RoR2;
using RoR2.Projectile;
using UnityEngine;


namespace WolfoQoL_Client.Skins
{
    //EffectData has no Owner or anything like this
    //Most effects have no origin or any way to deduce where the come from
    //Circumventing indexing is the least of issues

    //So if we want to we'd need to do it the previous method of :
    //Check every entity state for if the Merc is Red/Green

    public static class SkinChanges
    {
        public enum Case
        {
            None = 0,
            Merc,
            Acrid,
            Brass,
        }

        public static void Start()
        {
            On.RoR2.ProjectileGhostReplacementManager.FindProjectileGhostPrefab += ApplyConditionalProjectileGhostReplacement;

            Mercenary_Main.Start();
            Acrid_Main.Start();
            //EliteBell.Start();
            EliteEffects.Start();

        }


        private static GameObject ApplyConditionalProjectileGhostReplacement(On.RoR2.ProjectileGhostReplacementManager.orig_FindProjectileGhostPrefab orig, ProjectileController projectileController)
        {
            ProjectileGhostReplacer replacer = projectileController.GetComponent<ProjectileGhostReplacer>();
            if (replacer)
            {
                GameObject replacement = replacer.FindReplacement(projectileController);
                if (replacement != null)
                {
                    return replacement;
                }
            }
            return orig(projectileController);
        }


        public static int DetectCaseMerc(GameObject owner)
        {
            //I wonder if shouldn't do it like this...
            if (owner.TryGetComponent<ColorThisMerc>(out var merc))
            {
                return merc.ThisColor;
            }
            return 0;
        }
        public static MercColors DetectCaseMercEnum(GameObject owner)
        {
            //I wonder if shouldn't do it like this...
            if (owner.TryGetComponent<ColorThisMerc>(out var merc))
            {
                return merc.ThisColorEnum;
            }
            return MercColors.Blue;
        }
        public static int DetectCaseAcrid(GameObject owner)
        {
            if (owner.GetComponent<ColorThisAcrid>())
            {
                return 1;
            }
            return 0;
        }

    }


    /*public class PrefabReplacer : MonoBehaviour
    {
        public GameObject Prefab_1;
        public GameObject Prefab_2;

        public static GameObject FindReplacement(GameObject gameObject, int group)
        {
            PrefabReplacer prefab = gameObject.GetComponent<PrefabReplacer>();
            if (prefab != null)
            {
                switch (group)
                {
                    case 1:
                        return prefab.Prefab_1;
                    case 2:
                        return prefab.Prefab_2;
                    default:
                        return gameObject;
                }
            }
            return gameObject;
        }
    }*/


    public class ProjectileGhostReplacer : MonoBehaviour
    {
        public GameObject ghostPrefab_1;
        public GameObject impactPrefab_1;
        public GameObject ghostPrefab_2;
        public GameObject impactPrefab_2;
        public GameObject ghostPrefab_3;
        public GameObject impactPrefab_3;
        public bool useReplacement = true;
        public bool orbOnEnd = false;
        private ProjectileController pjC = null;

        public SkinChanges.Case condition;
        public GameObject FindReplacement(ProjectileController pjCo)
        {
            this.pjC = pjCo;
            if (!pjC.owner)
            {
                return null;
            }

            if (condition == SkinChanges.Case.Merc)
            {
                int i = SkinChanges.DetectCaseMerc(pjC.owner);
                if (useReplacement)
                {
                    EffectReplacer.Activate(pjC.GetComponent<ProjectileOverlapAttack>().impactEffect, i);
                }
                if (i == 1)
                {
                    if (!useReplacement)
                    {
                        pjC.GetComponent<ProjectileImpactExplosion>().impactEffect = impactPrefab_1;
                    }
                    return ghostPrefab_1;
                }
                else if (i == 2)
                {
                    if (!useReplacement)
                    {
                        pjC.GetComponent<ProjectileImpactExplosion>().impactEffect = impactPrefab_2;
                    }
                    return ghostPrefab_2;
                }
                else if (i == 3)
                {
                    if (!useReplacement)
                    {
                        pjC.GetComponent<ProjectileImpactExplosion>().impactEffect = impactPrefab_3;
                    }
                    return ghostPrefab_3;
                }
            }
            else if (condition == SkinChanges.Case.Acrid)
            {
                if (useReplacement)
                {
                    EffectReplacer.ActivateAcrid(pjC.GetComponent<ProjectileImpactExplosion>().impactEffect, pjC.owner);

                }
                if (pjC.owner.GetComponent<ColorThisAcrid>())
                {
                    return ghostPrefab_1;
                }
            }
            else if (condition == SkinChanges.Case.Brass)
            {
                CharacterBody component = pjC.owner.GetComponent<CharacterBody>();
                if (component && component.isElite)
                {
                    ghostPrefab_1.transform.GetChild(0).GetComponent<MeshRenderer>().material = component.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                    return ghostPrefab_1;
                }
            }
            return null;
        }


        public void OnDestroy()
        {
            if (orbOnEnd)
            {
                if (condition == SkinChanges.Case.Acrid)
                {
                    EffectReplacer.ActivateAcrid(Acrid_Hooks.CrocoDiseaseImpactEffect, pjC.owner);
                    EffectReplacer.ActivateAcrid(Acrid_Hooks.CrocoDiseaseOrbEffect, pjC.owner);
                }

            }
        }

    }

    public class EffectReplacer : MonoBehaviour
    {
        int lastSeenCase = -1;

        bool resetOnStart = false;
        public GameObject originalPrefab;
        public GameObject replacementPrefab1;
        public GameObject replacementPrefab2;
        public GameObject replacementPrefab3;
        public EffectDef effectDef;

        public void Start()
        {
            if (resetOnStart)
            {
                SetReplacement(0);
            }
        }
        public static void SetupComponent(GameObject main, GameObject replacementPrefab1, bool restart = false)
        {
            SetupComponent(main, replacementPrefab1, null, null, restart);
        }
        public static void SetupComponent(GameObject main, GameObject replacementPrefab1, GameObject replacementPrefab2, GameObject replacementPrefab3, bool restart)
        {
            //Even host does | GameObject -> EffectIndex -> Catalog -> NewGameobject
            //Set them all to the same EffectIndex?
            EffectReplacer replacer = main.AddComponent<EffectReplacer>();
            replacer.originalPrefab = main;
            replacer.replacementPrefab1 = replacementPrefab1;
            replacer.replacementPrefab2 = replacementPrefab2;
            replacer.replacementPrefab3 = replacementPrefab3;
            replacer.resetOnStart = restart;

            EffectIndex effectIndex = main.GetComponent<EffectComponent>().effectIndex;
            replacer.effectDef = EffectCatalog.GetEffectDef(effectIndex);
            //Better like this, because if we don't they fail to spawn, instead of spawning the default case which would be detectable
            replacementPrefab1.GetComponent<EffectComponent>().effectIndex = effectIndex;
            if (replacementPrefab2)
            {
                replacementPrefab2.GetComponent<EffectComponent>().effectIndex = effectIndex;
            }
            if (replacementPrefab3)
            {
                replacementPrefab3.GetComponent<EffectComponent>().effectIndex = effectIndex;
            }
        }

        public static void ActivateMerc(GameObject prefab, GameObject owner)
        {
            Activate(prefab, SkinChanges.DetectCaseMerc(owner));
        }
        public static void ActivateAcrid(GameObject prefab, GameObject owner)
        {
            if (owner.GetComponent<ColorThisAcrid>())
            {
                Activate(prefab, 1);
            }
            else
            {
                Activate(prefab, 0);
            }
        }
        public static void Activate(GameObject prefab, int i)
        {
            //IF any saftey checks needed, we can all uniformly put them here.
            //WolfoMain.log.LogMessage(prefab + " | " + i);
            if (prefab.TryGetComponent<EffectReplacer>(out EffectReplacer effectReplacer))
            {
                effectReplacer.SetReplacement(i);
            }
        }

        private void SetReplacement(int i)
        {
            //WolfoMain.log.LogMessage("ActivateReplacement "+i);
            if (i == lastSeenCase)
            {
                return;
            }
            lastSeenCase = i;
            switch (i)
            {
                case 0:
                    effectDef.prefab = originalPrefab;
                    return;
                case 1:
                    effectDef.prefab = replacementPrefab1;
                    return;
                case 2:
                    effectDef.prefab = replacementPrefab2;
                    return;
                case 3:
                    effectDef.prefab = replacementPrefab3;
                    return;
            }
        }
    }

    public enum MercColors
    {
        Blue = 0,
        Red = 1,
        Green = 2,
        Pink = 3,
    } //Doubtful that i'd make Yellow/Orange unless someone really requested it or dlc colored that way.
    public enum AcridDamageTypes
    {
        Poison,
        Blgith,
    }//Mod support for this would be a bitch and a half I imagine.
    public class ColorThisMerc : MonoBehaviour
    {
        public int ThisColor;
        public MercColors ThisColorEnum;
    }

    public class ColorThisAcrid : MonoBehaviour
    {
        public void Start()
        {
            //How reliable would this be on Client?
            CrocoDamageTypeController controller = this.gameObject.GetComponent<CrocoDamageTypeController>();
            if (controller == null)
            {
                Destroy(this);
            }
            if (controller.GetDamageType() != DamageType.BlightOnHit)
            {
                Destroy(this);
            }
        }
    }
}