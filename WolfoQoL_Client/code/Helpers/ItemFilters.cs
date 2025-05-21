using RoR2;
using System;
using System.Collections;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class ItemFilters
    {
        public static readonly Func<ItemIndex, bool> Tier1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier2DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier2DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier3DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier3DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> BossDeathItemFilterDelegate = new Func<ItemIndex, bool>(BossDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> LunarDeathItemFilterDelegate = new Func<ItemIndex, bool>(LunarDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> NoTierDeathItemFilterDelegate = new Func<ItemIndex, bool>(NoTierDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> AllowAllItemFilterDelegate = new Func<ItemIndex, bool>(AllowAllItemFilter);

        public static bool AllowAllItemFilter(ItemIndex itemIndex)
        {
            return true;
        }

        public static bool NoTierDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.NoTier) { return false; }
            if (!tempdef.pickupIconTexture || tempdef.pickupIconTexture.name.StartsWith("texNullIcon"))
            {
                return false;
            }
            return true;
        }
        public static bool Tier1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier1) { return false; }
            return true;
        }
        public static bool Tier2DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier2) { return false; }
            return true;
        }
        public static bool Tier3DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier3) { return false; }
            return true;
        }
        public static bool LunarDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Lunar) { return false; }
            return true;
        }
        public static bool BossDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Boss) { return false; }
            return true;
        }

        public static bool Void1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier == ItemTier.VoidTier1 |
                tempdef.tier == ItemTier.VoidTier2 |
                tempdef.tier == ItemTier.VoidTier3 |
                tempdef.tier == ItemTier.VoidBoss) { return true; }
            return false;
        }


    }
    public class Courtines
    {
        public static float LagDependentDelay()
        {
            return 1f;
        }

        public static IEnumerator Delayed_ChatAddMessage(string chatMessage, float Delay)
        {
            yield return new WaitForSeconds(Delay);
            Chat.AddMessage(chatMessage);
        }
        public static IEnumerator Delayed_ChatBroadcast(string token, float delay)
        {
            yield return new WaitForSeconds(delay);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = token
            });
            yield break;
        }
        public static IEnumerator Delayed_RebirthMessage(NetworkUser user, float delay)
        {
            yield return new WaitForSeconds(delay);


            string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(user.rebirthItem).pickupDef.baseColor);
            hex = "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(user.rebirthItem).nameToken) + "</color>";
            string result = string.Format(Language.GetString("REBIRTH_ENDING_CHAT_ITEM_2P"), hex);
            Chat.AddMessage("   " + string.Format(Language.GetString("WIN_FORMAT_REBIRTH"), result));
            yield break;
        }
    }

}
