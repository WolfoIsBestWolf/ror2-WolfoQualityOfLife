
/*
ItemDef ScrapGreen = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ScrapGreen");
ItemDef ScrapRed = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ScrapRed");
ItemDef ScrapYellow = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ScrapYellow");

GameObject tempScrapGreenModel = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/pickupmodels/PickupScrap"), "PickupScrapGreen", false);
GameObject tempScrapRedModel = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/pickupmodels/PickupScrap"), "PickupScrapRed", false);
GameObject tempScrapYellowModel = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/pickupmodels/PickupScrap"), "PickupScrapYellow", false);

GameObject PickupScrap = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/pickupmodels/PickupScrap");
GameObject PickupSpleen = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/pickupmodels/PickupBleedOnHitAndExplode");


Texture2D TexRedMetal = new Texture2D(128, 128, TextureFormat.DXT5, false);
TexRedMetal.LoadImage(Properties.Resources.texTrimSheetVeryRed, false);
TexRedMetal.filterMode = FilterMode.Bilinear;
TexRedMetal.name = "TexRedMetal";

GameObject BackupDroneBody = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BackupDroneBody");
GameObject CaptainSupplyDrop = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Healing");
GameObject YellowPrinter = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild");
GameObject RedPrinter = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary");
GameObject RoboBall = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallMiniBody");

SkinnedMeshRenderer smallDamageMesh = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestDamage").GetComponentInChildren<SkinnedMeshRenderer>();
SkinnedMeshRenderer smallHealingeMesh = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestHealing").GetComponentInChildren<SkinnedMeshRenderer>();
SkinnedMeshRenderer smallUtilityMesh = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestUtility").GetComponentInChildren<SkinnedMeshRenderer>();

string[] newshadercategory = { "DITHER", "FORCE_SPEC", "USE_VERTEX_COLORS" };

Material matCaptainSupplyDrop = CaptainSupplyDrop.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;
Material matTrimSheetConstructionRedLight = BackupDroneBody.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;
Material matTrimSheetConstructionWild = YellowPrinter.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material;
Material matRedPrinter = RedPrinter.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().material;
Material matRoboBall = RoboBall.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material;
Material matSpleen = PickupSpleen.transform.GetChild(0).GetComponent<MeshRenderer>().material;
matSpleen.mainTexture = TexRedMetal;
matSpleen.color = new Color(0.5f, 0.5f, 0.5f, 1);

tempScrapGreenModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = smallUtilityMesh.material;
tempScrapGreenModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial = smallUtilityMesh.material;
tempScrapGreenModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.shaderKeywords = newshadercategory;
ScrapGreen.pickupModelPrefab = tempScrapGreenModel;

tempScrapRedModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = matTrimSheetConstructionRedLight;
tempScrapRedModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial = matTrimSheetConstructionRedLight;
tempScrapRedModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = matRedPrinter.mainTexture;
tempScrapRedModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.shaderKeywords = new string[0];
ScrapRed.pickupModelPrefab = tempScrapRedModel;

tempScrapYellowModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = matSpleen;
tempScrapYellowModel.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial = matSpleen;
ScrapYellow.pickupModelPrefab = tempScrapYellowModel;
*/

