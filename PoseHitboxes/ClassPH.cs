using MelonLoader;
using RUMBLE.Tutorial.MoveLearning;
using RumbleModdingAPI;
using static RumbleModdingAPI.Calls;
using UnityEngine;
using static RUMBLE.Players.Subsystems.PlayerVR;


namespace PoseHitboxes
{
    public class ClassPH : MelonMod
    {
        private string currentScene = "Loader";
        private bool sceneChanged = false;
        private bool loaded = false;
        PoseGhost poseGhost;
        Material ghosty;
        Material redy;
        string currentPoseName = "";
        int delayCycle = 0;

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneChanged = true;
            delayCycle = 180;
            //MelonLogger.Msg("delay frames:");
            //MelonLogger.Msg(delayCycle);
        }

        public override void OnUpdate()
        {
            if (currentScene != "Gym") { return; }
            if (sceneChanged)
            {
                if (delayCycle > 1)
                {
                    delayCycle--; // wait 180 frames before trying to fetch anything
                    //MelonLogger.Msg(delayCycle);
                }
                else
                {
                    if (!loaded)
                    {
                        try
                        {
                            //MelonLogger.Msg("loading items");
                            poseGhost = Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning.GetGameObject().GetComponentInChildren<PoseGhost>();//GameObject.Find("--------------LOGIC--------------/Heinhouser products/MoveLearning/Ghost").GetComponent<PoseGhost>();
                            ghosty = poseGhost.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material;
                            redy = Calls.GameObjects.Gym.Scene.GymProduction.MainStaticGroup.GymArena.GetGameObject().transform.GetChild(0).GetComponent<Renderer>().material;

                            loaded = true;
                            //MelonLogger.Msg("items loaded");
                        }
                        catch { return; }
                    }
                    sceneChanged = false;
                }
            }
            if (loaded)
            {

                //if pose switched
                if (currentPoseName != poseGhost.showCurrentPoseData.name)
                {
                    
                    if (poseGhost.transform.GetChild(0).GetChild(0).childCount > 0)
                    {
                       // MelonLogger.Msg("Destroying Existing");
                        while (poseGhost.transform.GetChild(0).GetChild(0).childCount > 0 || poseGhost.transform.GetChild(0).GetChild(1).childCount > 0)
                        {
                            GameObject.DestroyImmediate(poseGhost.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                            GameObject.DestroyImmediate(poseGhost.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
                        }
                    }
                   // MelonLogger.Msg("Creating Primitives");
                    GameObject LCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LCube.transform.parent = poseGhost.transform.GetChild(0).GetChild(0);
                    //LCube.transform.position = poseGhost.transform.GetChild(0).GetChild(1).position;
                    LCube.transform.position = poseGhost.leftHand.position;//showCurrentPoseData.leftControllerCondition.DesiredPose.position;
                    LCube.transform.localRotation = poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation;
                    LCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold);
                    LCube.GetComponent<Renderer>().material = ghosty;
                    poseGhost.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = redy;

                    //poseGhost.transform.GetChild(0).GetChild(0).localScale = new Vector3(poseGhost.showCurrentPoseData.LeftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.LeftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.LeftControllerCondition.PositionTreshold);
                    GameObject RCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RCube.transform.parent = poseGhost.transform.GetChild(0).GetChild(1);
                    //RCube.transform.position = poseGhost.transform.position;
                    RCube.transform.position = poseGhost.rightHand.position;
                    RCube.transform.localRotation = poseGhost.showCurrentPoseData.rightControllerCondition.DesiredPose.rotation;
                    RCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold);
                    RCube.GetComponent<Renderer>().material = ghosty;
                    poseGhost.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = redy;
                    currentPoseName = poseGhost.showCurrentPoseData.name;
                }
            }
        }
    }
}