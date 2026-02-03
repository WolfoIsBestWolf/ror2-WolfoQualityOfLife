using RoR2;
using UnityEngine.AddressableAssets;

namespace WQoL_Gameplay
{
    public class ItemStuff
    {

        public static void ItemFixes()
        {
            RoR2Content.Buffs.LunarSecondaryRoot.flags = 0;
            RoR2Content.Buffs.LunarDetonationCharge.flags = 0;
            DLC2Content.Buffs.Oiled.isDebuff = true;
        }





    }

}
