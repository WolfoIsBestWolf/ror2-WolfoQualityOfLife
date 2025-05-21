using UnityEngine;

namespace WolfoQoL_Server
{
    public static class Assets
    {
        public static AssetBundle Bundle
        {
            get
            {
                return WolfoQoL_Client.Assets.Bundle;
            }
        }
    }
}