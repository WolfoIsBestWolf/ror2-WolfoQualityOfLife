using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace WolfoFixes
{
    public class Glass
    {
        public static void Start()
        {
            IL.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlayStates;
            IL.RoR2.CharacterModel.UpdateOverlayStates += CharacterModel_UpdateOverlayStates;

        }

        private static void CharacterModel_UpdateOverlayStates(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LunarDagger"));
            if (a && c.TryGotoNext(MoveType.Before,
            x => x.MatchBr(out _)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, CharacterModel, bool>>((yes, model) =>
                {
                    if (model.body.isGlass)
                    {
                        return true;
                    }
                    return yes;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: IL.CharacterModel_UpdateOverlays");
            }
        }


    }
}