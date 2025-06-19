//using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;

namespace WolfoQoL_Client
{
    public class PlayerPing
    {

        public static void PlayerPingsIL(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.Before,
             x => x.MatchLdcI4(1),
            x => x.MatchStfld("RoR2.UI.PingIndicator", "pingType"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchCall("RoR2.UI.PingIndicator", "get_pingTarget"),
                x => x.MatchCall("UnityEngine.Object", "op_Implicit")
                ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, PingIndicator, bool>>((doesEx, self) =>
                {
                    if (WConfig.cfgPlayerPing.Value)
                    {
                        if (self.pingTarget)
                        {
                            CharacterBody body = self.pingTarget.GetComponent<CharacterBody>();
                            if (body && body.teamComponent && body.teamComponent.teamIndex == TeamIndex.Player)
                            {
                                Debug.LogFormat("Ping playerTeam target {0}", new object[]
                                    {
                                    self.pingTarget
                                    });
                                self.pingType = playerPingType;
                                return false;
                            }
                        }
                    }
                    return doesEx;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: PlayerPingsIL");
            }

            if (c.TryGotoNext(MoveType.Before,
               x => x.MatchLdfld("RoR2.UI.PingIndicator", "pingType")
               ))
            {
                c.EmitDelegate<System.Func<PingIndicator, PingIndicator>>((self) =>
                {
                    BuildPlayerPing(self);
                    return self;
                });

            }
            else
            {
                Debug.LogWarning("IL Failed: PlayerPingsILPart2");
            }


        }
        public static PingIndicator.PingType playerPingType = (PingIndicator.PingType)73;
        //public static Color playerTeamPingColor3 = new Color(0.6f, 0.7f, 0.933f, 1);
        //public static Color playerTeamPingColor2 = new Color(0.4784f, 0.8745f, 0.8902f, 1f);
        public static Color playerTeamPingColor2 = new Color(0.125f, 0.425f, 0.65f, 1f);
        public static Color playerTeamPingColor = new Color(0.49f, 0.843f, 0.96f, 1f);

        public static void BuildPlayerPing(PingIndicator self)
        {
            Transform playerObjects1 = self.defaultPingGameObjects[0].transform.parent.Find("Player");
            Transform playerObjects2 = self.enemyPingGameObjects[1].transform.parent.Find("PlayerPositionRing");
            if (playerObjects1)
            {
                playerObjects1.gameObject.SetActive(false);
                playerObjects2.gameObject.SetActive(false);
            }
            if (self.pingType == playerPingType)
            {
                string ownerName = self.GetOwnerName();
                string text = Util.GetBestBodyName(self.pingTarget);

                self.pingColor = playerTeamPingColor;
                self.pingDuration = self.enemyPingDuration;
                //self.pingText.color = self.textBaseColor * self.pingColor;

                ModelLocator modelLocator = self.pingTarget.GetComponent<ModelLocator>();
                if (modelLocator)
                {
                    Transform modelTransform = modelLocator.modelTransform;
                    if (modelTransform)
                    {
                        CharacterModel characterModel = modelTransform.GetComponent<CharacterModel>();
                        if (characterModel)
                        {
                            self.targetTransformToFollow = characterModel.coreTransform;
                            Renderer target = characterModel.mainSkinnedMeshRenderer;
                            if (!target)
                            {
                                foreach (CharacterModel.RendererInfo rendererInfo in characterModel.baseRendererInfos)
                                {
                                    if (!rendererInfo.ignoreOverlays)
                                    {
                                        target = rendererInfo.renderer;
                                        break;
                                    }
                                }
                            }
                            if (target)
                            {
                                self.pingHighlight.highlightColor = Highlight.HighlightColor.custom;
                                self.pingHighlight.CustomColor = playerTeamPingColor;
                                self.pingHighlight.targetRenderer = target;
                                self.pingHighlight.strength = 1f;
                                self.pingHighlight.isOn = true;
                                self.pingHighlight.enabled = true;
                            }
                            /*foreach (CharacterModel.RendererInfo rendererInfo in characterModel.baseRendererInfos)
                            {
                                if (!rendererInfo.ignoreOverlays)
                                {
                                    self.pingHighlight.highlightColor = Highlight.HighlightColor.custom;
                                    self.pingHighlight.CustomColor = playerTeamPingColor;
                                    self.pingHighlight.targetRenderer = rendererInfo.renderer;
                                    self.pingHighlight.strength = 1f;
                                    self.pingHighlight.isOn = true;
                                    self.pingHighlight.enabled = true;
                                    break;
                                }
                            }*/
                        }
                    }

                    string token = "PLAYER_PING_PLAYER";
                    if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.FriendlyFire))
                    {
                        token = "PLAYER_PING_ENEMY";
                        token = Language.GetString(token).Replace("Health", "Utility");
                    }
                    else
                    {
                        token = Language.GetString(token);
                    }

                    if (playerObjects1)
                    {
                        playerObjects1.gameObject.SetActive(true);
                        playerObjects2.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject playerObject1 = GameObject.Instantiate(self.defaultPingGameObjects[0], self.defaultPingGameObjects[0].transform.parent);
                        GameObject playerObject2 = GameObject.Instantiate(self.enemyPingGameObjects[1], self.enemyPingGameObjects[1].transform.parent);
                        playerObject1.name = "Player";
                        playerObject2.name = "PlayerPositionRing";
                        playerObject1.SetActive(true);
                        playerObject2.SetActive(true);
                        playerObject1.GetComponent<SpriteRenderer>().color = playerTeamPingColor;
                        playerObject2.GetComponent<ParticleSystem>().startColor = playerTeamPingColor2;
                    }

                    Chat.AddMessage(string.Format(token, ownerName, text));
                }

            }
        }


    }
}
