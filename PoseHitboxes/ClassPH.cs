using MelonLoader;
using RUMBLE.Tutorial.MoveLearning;
using RumbleModdingAPI;
using static RumbleModdingAPI.Calls;
using UnityEngine;
using static RUMBLE.Players.Subsystems.PlayerVR;
using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning.MoveLearnSelector;
using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning;
using static RumbleModdingAPI.Calls.GameObjects.Gym.Logic.HeinhouserProducts;
using MelonLoader.TinyJSON;


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
        Material glassy;
        string currentPoseName = "";
        int delayCycle = 0;
        GameObject LRotate;
        GameObject RRotate;
        GameObject LRotateLimit;
        GameObject RRotateLimit;
        GameObject LHandRef;
        GameObject RHandRef;
        float deg2rad = 3.14159f / 180f;
        float LThresh = 0f;
        float RThresh = 0f;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneChanged = true;
            delayCycle = 5;
            loaded = false;
            //MelonLogger.Msg("loaded = ");
            //MelonLogger.Msg(loaded);
        }

        public override void OnUpdate()
        {
            if (currentScene != "Gym") { return; }
            if (sceneChanged)
            {
                if (delayCycle > 1)
                {
                    delayCycle--; // wait 5 frames before trying to fetch anything
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
                            //glassy = Calls.GameObjects.Gym.Logic.HeinhouserProducts.MoveLearning.MoveLearnSelector.TotemPedistalCompact.GetGameObject().transform.GetChild(0).GetComponent<Renderer>().material;
                            LHandRef = GameObject.Find("Player Controller(Clone)/Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_L/Bone_Shoulder_L/Bone_Lowerarm_L/Bone_Hand_L");
                            RHandRef = GameObject.Find("Player Controller(Clone)/Visuals/RIG/Bone_Pelvis/Bone_Spine/Bone_Chest/Bone_Shoulderblade_R/Bone_Shoulder_R/Bone_Lowerarm_R/Bone_Hand_R");
                            //--------------LOGIC-------------- / Heinhouser products / MoveLearning / MoveLearnSelector / TotemPedistalCompact /
                            loaded = true;
                            currentPoseName = "";
                            //MelonLogger.Msg("items loaded");
                        }
                        catch { return; }
                    }
                    sceneChanged = false;
                }
            }
            if (loaded)
            {
                //MelonLogger.Msg("chkpt 1");

                //if pose switched
                if (currentPoseName != poseGhost.showCurrentPoseData.name)
                {
                    //MelonLogger.Msg("chkpt 2");

                    if (poseGhost.transform.GetChild(0).GetChild(0).childCount > 0)
                    {
                        //MelonLogger.Msg("chkpt 3");
                        // MelonLogger.Msg("Destroying Existing");
                        while (poseGhost.transform.GetChild(0).GetChild(0).childCount > 0 || poseGhost.transform.GetChild(0).GetChild(1).childCount > 0)
                        {
                            GameObject.DestroyImmediate(poseGhost.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
                            GameObject.DestroyImmediate(poseGhost.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
                            GameObject.DestroyImmediate(LRotate.gameObject);
                            GameObject.DestroyImmediate(RRotate.gameObject);
                        }
                    }
                   // MelonLogger.Msg("Creating Primitives");
                    GameObject LCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject RCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    LCube.transform.parent = poseGhost.transform.GetChild(0).GetChild(0);
                    RCube.transform.parent = poseGhost.transform.GetChild(0).GetChild(1);
                    LCube.transform.position = poseGhost.leftHand.position;//showCurrentPoseData.leftControllerCondition.DesiredPose.position;
                    RCube.transform.position = poseGhost.rightHand.position;


                    //MelonLogger.Msg(poseGhost.leftHand.rotation.x);
                    //MelonLogger.Msg(poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation.x);
                    LCube.GetComponent<Renderer>().material = ghosty;
                    RCube.GetComponent<Renderer>().material = ghosty;
                    poseGhost.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = redy;
                    poseGhost.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material = redy;
                    currentPoseName = poseGhost.showCurrentPoseData.name;


                    //setup gyro objects and details
                    LRotate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    RRotate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //GameObject LXBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject LYBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject LZBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //GameObject RXBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject RYBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject RZBar = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    LRotate.transform.parent = poseGhost.transform.GetChild(0).GetChild(0);
                    RRotate.transform.parent = poseGhost.transform.GetChild(0).GetChild(1);

                    LRotate.GetComponent<Renderer>().material = redy;
                    RRotate.GetComponent<Renderer>().material = redy;
                    LRotate.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
                    RRotate.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);

                    //LXBar.transform.parent = LRotate.transform;
                    LYBar.transform.parent = LRotate.transform;
                    LZBar.transform.parent = LRotate.transform;
                    //RXBar.transform.parent = RRotate.transform;
                    RYBar.transform.parent = RRotate.transform;
                    RZBar.transform.parent = RRotate.transform;

                    //LXBar.transform.localScale = new Vector3(40f, 1f, 1f);
                    LYBar.transform.localScale = new Vector3(1f, 25f, 1f);
                    LZBar.transform.localScale = new Vector3(1f, 1f, 25f);
                    //RXBar.transform.localScale = new Vector3(40f, 1f, 1f);
                    RYBar.transform.localScale = new Vector3(1f, 25f, 1f);
                    RZBar.transform.localScale = new Vector3(1f, 1f, 25f);

                    //LXBar.GetComponent<Renderer>().material = ghosty;
                    //LYBar.GetComponent<Renderer>().material = ghosty;
                    //LZBar.GetComponent<Renderer>().material = ghosty;
                    //RXBar.GetComponent<Renderer>().material = ghosty;
                    //RYBar.GetComponent<Renderer>().material = ghosty;
                    //RZBar.GetComponent<Renderer>().material = ghosty;

                    Color red8 = new Color(1.0f, 0.0f, 0.0f, 0.8f);
                    Color green8 = new Color(0.0f, 1.0f, 0.0f, 0.8f);
                    Color blue8 = new Color(0.0f, 0.0f, 1.0f, 0.8f);


                    //LXBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    LYBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    LZBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    //RXBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    RYBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    RZBar.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");

                    //LXBar.GetComponent<Renderer>().material.color = red8;
                    LYBar.GetComponent<Renderer>().material.color = green8;
                    LZBar.GetComponent<Renderer>().material.color = blue8;
                    //RXBar.GetComponent<Renderer>().material.color = red8;
                    RYBar.GetComponent<Renderer>().material.color = green8;
                    RZBar.GetComponent<Renderer>().material.color = blue8;

                    //RRotate.transform.localScale = new Vector3(0.01f, 0.01f, 0.4f);
                    //RRotate.GetComponent<Renderer>().material = redy;
                    //RRotate.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.8f);


                    LRotate.transform.position = poseGhost.leftHand.position + new Vector3(-1.0f, 0.00f, -1.0f);
                    RRotate.transform.position = poseGhost.rightHand.position + new Vector3(-1.0f, 0.00f, -1.0f);

                    //generate reference cylinders

                    LRotateLimit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    RRotateLimit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //GameObject LXBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    GameObject LYBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    GameObject LZBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    //GameObject RXBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    GameObject RYBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    GameObject RZBarLimit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

                    //calculate trig for degree limits
 

                    if (poseGhost.leftHand.rotation.eulerAngles.x == poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation.eulerAngles.x) // if the hands are not mirrored, use the normal map
                    {
                        LThresh = poseGhost.showCurrentPoseData.LeftControllerCondition.RotationTreshold;
                        RThresh = poseGhost.showCurrentPoseData.RightControllerCondition.RotationTreshold;
                        LCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold);
                        RCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold);
                        
                       // MelonLogger.Msg("right hand mode");
                    }
                    else // they need to be swapped
                    {
                        RThresh = poseGhost.showCurrentPoseData.LeftControllerCondition.RotationTreshold;
                        LThresh = poseGhost.showCurrentPoseData.RightControllerCondition.RotationTreshold;
                        RCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.leftControllerCondition.PositionTreshold);
                        LCube.transform.localScale = new Vector3(poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold, poseGhost.showCurrentPoseData.RightControllerCondition.PositionTreshold);
                        
                        //MelonLogger.Msg("left hand mode");
                    }

                    // tan(theta)/A = X // X is the radiaus we need, A is the length of the cylinder from the orgin, theta is angle tolerance
                    //default cylinder diamter is 1, so the radius is 0.5, default height from origin is 1
                    // we are not give theta in degrees, is is a percent like 0.8f, so (1 - 0.8) * 360 = 72 degrees
                    // if 72 degrees is the total range, then 36 degrees is our theta, we can shorten the math by going to (1-0.8)*180 to get our tolernace in degrees
                    // Tangent of this will give us a tolerance R in a scale of 1 we can apply to the cylinder
                    //float LRotationLimit = Mathf.Tan((1 - poseGhost.showCurrentPoseData.LeftControllerCondition.RotationTreshold) * 180 * deg2rad); // 72 degrees
                    //float RRotationLimit = Mathf.Tan((1 - poseGhost.showCurrentPoseData.RightControllerCondition.RotationTreshold) * 180 * deg2rad); // 126

                    float LRotationLimit = ((1 - LThresh) * 3.14159f); // 72 degrees
                    float RRotationLimit = ((1 - RThresh) * 3.14159f); // 126

                    LRotateLimit.transform.parent = poseGhost.transform.GetChild(0).GetChild(0);
                    RRotateLimit.transform.parent = poseGhost.transform.GetChild(0).GetChild(1);

                    LRotateLimit.GetComponent<Renderer>().material = redy;
                    RRotateLimit.GetComponent<Renderer>().material = redy;
                    LRotateLimit.transform.localScale = new Vector3(0.03f * LThresh, 0.03f * LThresh, 0.03f * LThresh);
                    RRotateLimit.transform.localScale = new Vector3(0.03f * RThresh, 0.03f * RThresh, 0.03f * RThresh);

                    //LXBarLimit.transform.parent = LRotateLimit.transform;
                    LYBarLimit.transform.parent = LRotateLimit.transform;
                    LZBarLimit.transform.parent = LRotateLimit.transform;
                    //RXBarLimit.transform.parent = RRotateLimit.transform;
                    RYBarLimit.transform.parent = RRotateLimit.transform;
                    RZBarLimit.transform.parent = RRotateLimit.transform;
                    //MelonLogger.Msg("LRotationLimit");
                    //MelonLogger.Msg(LRotationLimit);
                    //MelonLogger.Msg("RRotationLimit");
                    //MelonLogger.Msg(RRotationLimit);
                    //LXBarLimit.transform.localScale = new Vector3(LRotationLimit, 0.5f, LRotationLimit);
                    LYBarLimit.transform.localScale = new Vector3(LRotationLimit, 0.5f, LRotationLimit);
                    LZBarLimit.transform.localScale = new Vector3(LRotationLimit, 0.5f, LRotationLimit);
                    //RXBarLimit.transform.localScale = new Vector3(RRotationLimit, 0.5f, RRotationLimit);
                    RYBarLimit.transform.localScale = new Vector3(RRotationLimit, 0.5f, RRotationLimit);
                    RZBarLimit.transform.localScale = new Vector3(RRotationLimit, 0.5f, RRotationLimit);
                    //LXBarLimit.transform.Rotate(new Vector3(0f, 0f, 90f), Space.Self);
                    LYBarLimit.transform.Rotate(new Vector3(0f, 90f, 0f), Space.Self);
                    LZBarLimit.transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);
                    //RXBarLimit.transform.Rotate(new Vector3(0f, 0f, 90f), Space.Self);
                    RYBarLimit.transform.Rotate(new Vector3(0f, 90f, 0f), Space.Self);
                    RZBarLimit.transform.Rotate(new Vector3(90f, 0f, 0f), Space.Self);

                    //LXBarLimit.GetComponent<Renderer>().material = ghosty;
                    //LYBarLimit.GetComponent<Renderer>().material = ghosty;
                    //LZBarLimit.GetComponent<Renderer>().material = ghosty;
                    //RXBarLimit.GetComponent<Renderer>().material = ghosty;
                    //RYBarLimit.GetComponent<Renderer>().material = ghosty;
                    //RZBarLimit.GetComponent<Renderer>().material = ghosty;

                    //LXBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    LYBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    LZBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    //RXBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    RYBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                    RZBarLimit.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");

                    Color red1 = new Color(1.0f, 0.0f, 0.0f, 0.1f);
                    Color green1 = new Color(0.0f, 1.0f, 0.0f, 0.1f);
                    Color blue1 = new Color(0.0f, 0.0f, 1.0f, 0.1f);

                    //LXBarLimit.GetComponent<Renderer>().material.color = red1;
                    LYBarLimit.GetComponent<Renderer>().material.color = green1;
                    LZBarLimit.GetComponent<Renderer>().material.color = blue1;
                    //RXBarLimit.GetComponent<Renderer>().material.color = red1;
                    RYBarLimit.GetComponent<Renderer>().material.color = green1;
                    RZBarLimit.GetComponent<Renderer>().material.color = blue1;

                    //RRotate.transform.localScale = new Vector3(0.01f, 0.01f, 0.4f);
                    //RRotate.GetComponent<Renderer>().material = redy;
                    //RRotate.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 0.8f);

                    LRotateLimit.transform.position = poseGhost.leftHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
                    RRotateLimit.transform.position = poseGhost.rightHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
                    LRotate.transform.position = poseGhost.leftHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
                    RRotate.transform.position = poseGhost.rightHand.position + new Vector3(-0.3f, 0.15f, -0.3f);
                    if (poseGhost.leftHand.rotation.eulerAngles.x == poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation.eulerAngles.x) // if the hands are not mirrored, use the normal map
                    {
                        LCube.transform.localRotation = poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation;
                        RCube.transform.localRotation = poseGhost.showCurrentPoseData.rightControllerCondition.DesiredPose.rotation;
                    }
                    else // they need to be swapped
                    {
                        RCube.transform.localRotation = poseGhost.showCurrentPoseData.leftControllerCondition.DesiredPose.rotation;
                        LCube.transform.localRotation = poseGhost.showCurrentPoseData.rightControllerCondition.DesiredPose.rotation;
                    }
                    LRotateLimit.transform.rotation = LCube.transform.rotation;
                    RRotateLimit.transform.rotation = RCube.transform.rotation;
                }
            }
            if(LRotate != null)
             {
                LRotate.transform.rotation = LHandRef.GetComponent<Transform>().rotation;
                RRotate.transform.rotation = RHandRef.GetComponent<Transform>().rotation;
                //LRotate.transform.position = LHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                //RRotate.transform.position = RHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                //LRotateLimit.transform.position = LHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                //RRotateLimit.transform.position = RHandRef.GetComponent<Transform>().position + new Vector3(0.0f, 0.2f, 0.0f);
                //LRotate.transform.localRotation = LHandRef.GetComponent<Transform>().localRotation;
                //RRotate.transform.localRotation = RHandRef.GetComponent<Transform>().localRotation;
            }
        }
    }
}