using RoR2;
using System;
using System.Collections;
using UnityEngine;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    public static class ItemFilters
    {
        public static readonly Func<ItemIndex, bool> NoTierDeathItemFilterDelegate = new Func<ItemIndex, bool>(NoTierDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> AllowAllItemFilterDelegate = new Func<ItemIndex, bool>(AllowAllItemFilter);

        public static bool NoTierDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.hidden)
            {
                return false;
            }
            if (tempdef.tier != ItemTier.NoTier)
            {
                return true;
            }
            if (tempdef.isConsumed) //If untiered, but marked as consumed, show it.
            {
                return true;
            }
            if (!tempdef.pickupIconSprite || tempdef.pickupIconSprite.name == "texNullIcon")
            {
                return false;
            }
            return true;
        }
        public static bool AllowAllItemFilter(ItemIndex itemIndex)
        {
            return true;
        }
    }
    public static class Courtines
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
            Networker.SendWQoLMessage(new Chat.SimpleChatMessage
            {
                baseToken = token
            });
            yield break;
        }
        public static IEnumerator Delayed_RebirthMessage(NetworkUser user, float delay)
        {
            yield return new WaitForSeconds(delay);
            string result = string.Format(Language.GetString("REBIRTH_ENDING_CHAT_ITEM_2P"), Help.GetColoredName(user.rebirthItem));
            Chat.AddMessage("   " + string.Format(Language.GetString("WIN_FORMAT_REBIRTH"), result));
            yield break;
        }
    }

}
