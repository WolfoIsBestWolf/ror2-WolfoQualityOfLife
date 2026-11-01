using RoR2;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Hex
    {
        public static void Start()
        {
            //Maybe if we put it on the white hand?
            On.HallowingShrineSplitNode.PreStartClient += AddObjective_Hand;
            //On.HallowingShrineSplitNode.Close += HallowingShrineSplitNode_Close;
            On.HallowingShrineSplitNode.RpcClose += HallowingShrineSplitNode_RpcClose;
        }

        private static void AddObjective_Hand(On.HallowingShrineSplitNode.orig_PreStartClient orig, HallowingShrineSplitNode self)
        {
            orig(self);
            //Hand of Life
            if (self.GetComponent<DrainItemsAndDropMultiChoicePotentialController>() != null)
            {
                bool hex = WConfig.cfgReminder_HexShrine.Value == WConfig.HexShrine.Always || LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.HasAtLeastXTotalRemovablePermanentItemsOfTier(ItemTier.Hex, 1);
                if (hex)
                {
                    self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_HALLOWED";
                }
            }

        }


        private static void HallowingShrineSplitNode_RpcClose(On.HallowingShrineSplitNode.orig_RpcClose orig, HallowingShrineSplitNode self)
        {
            orig(self);
            //Clients don't have a "owner" here.
            if (self.TryGetComponent<GenericObjectiveProvider>(out var obj))
            {
                obj.enabled = false;
            }
        }
    }
}
