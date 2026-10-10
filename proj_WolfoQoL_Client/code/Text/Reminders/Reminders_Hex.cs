using RoR2;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Hex
    {
        public static void Start()
        {
            On.HallowingShrineSplitBase.PreStartClient += AddObjective;
            On.HallowingShrineSplitNode.Close += HallowingShrineSplitNode_Close;
            On.HallowingShrineSplitNode.RpcClose += HallowingShrineSplitNode_RpcClose;
        }

        private static void AddObjective(On.HallowingShrineSplitBase.orig_PreStartClient orig, HallowingShrineSplitBase self)
        {
            orig(self);

            bool hex = WConfig.cfgReminder_HexShrine.Value == WConfig.HexShrine.Always || LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.HasAtLeastXTotalRemovablePermanentItemsOfTier(ItemTier.Hex, 1);
            if (hex)
            {
               self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_HALLOWED";
            }
        }

        private static void HallowingShrineSplitNode_Close(On.HallowingShrineSplitNode.orig_Close orig, HallowingShrineSplitNode self)
        {
            orig(self);

            //Maybe we aught to check like, if YOU have the hexes removed for the hex one?
            if (self.owner && self.owner.TryGetComponent<GenericObjectiveProvider>(out var obj))
            {
                obj.enabled = false;
            }
        }

        private static void HallowingShrineSplitNode_RpcClose(On.HallowingShrineSplitNode.orig_RpcClose orig, HallowingShrineSplitNode self)
        {
            orig(self);
            if (self.owner && self.owner.TryGetComponent<GenericObjectiveProvider>(out var obj))
            {
                obj.enabled = false;
            }
        }
    }
}
