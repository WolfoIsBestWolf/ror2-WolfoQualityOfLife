using RoR2;
using System;
using UnityEngine;


namespace WolfoQualityOfLife
{
    public class Assets
    {

        public static AssetBundle DuplicatorOldModelBundle = LoadAssetBundle(Properties.Resources.oldduplicatorbundle);
        public static AssetBundle MainBundle = LoadAssetBundle(Properties.Resources.wolfoqualityoflife);

        static AssetBundle LoadAssetBundle(Byte[] resourceBytes)
        {
            //Check to make sure that the byte array supplied is not null, and throw an appropriate exception if they are.
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            //Actually load the bundle with a Unity function.
            var bundle = AssetBundle.LoadFromMemory(resourceBytes);

            return bundle;
        }
 
    }

}
