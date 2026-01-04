using RoR2;

namespace WolfoQoL_Client.ModSupport
{
    public static class QualitySupport
    {
        public static bool QualityModInstalled;
        public static bool PreBaseItemIndex(ItemIndex newItem, ItemDef isThisItem)
        {
            if (QualityModInstalled)
            {
                return BaseItemIndex(newItem, isThisItem.itemIndex);
            }
            return newItem == isThisItem.itemIndex;
        }

        public static bool BaseItemIndex(ItemIndex newItem, ItemIndex itemIndex)
        {
            var qualityIndex = ItemQualities.QualityCatalog.FindItemQualityGroupIndex(newItem);
            if (qualityIndex != ItemQualities.ItemQualityGroupIndex.Invalid)
            {
                return ItemQualities.QualityCatalog.GetItemQualityGroup(qualityIndex).BaseItemIndex == itemIndex;
            }
            return false;
        }


        public static int QualiyItemCountPermanent(ItemDef itemDef, Inventory inv)
        {

            var qualityIndex = ItemQualities.QualityCatalog.FindItemQualityGroupIndex(itemDef.itemIndex);
            if (qualityIndex != ItemQualities.ItemQualityGroupIndex.Invalid)
            {
                return ItemQualities.QualityCatalog.GetItemQualityGroup(qualityIndex).GetItemCountsPermanent(inv).TotalCount;
            }
            return 0;
        }
        public static int QualiyItemCountEffective(ItemDef itemDef, Inventory inv)
        {

            var qualityIndex = ItemQualities.QualityCatalog.FindItemQualityGroupIndex(itemDef.itemIndex);
            if (qualityIndex != ItemQualities.ItemQualityGroupIndex.Invalid)
            {
                return ItemQualities.QualityCatalog.GetItemQualityGroup(qualityIndex).GetItemCountsPermanent(inv).TotalCount;
            }
            return 0;
        }

    }


}
