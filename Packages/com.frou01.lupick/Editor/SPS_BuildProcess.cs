using System;
using System.Collections;
using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase.Editor.BuildPipeline;

public class SPS_BuildProcess : IProcessSceneWithReport
{
    public int callbackOrder => 0;
    LUP_RC_ColliderManager RCCManager = null;

    List<LUP_RC_CatcherCollider> RCCatchers = new List<LUP_RC_CatcherCollider>();
    List<LUPickUpRC_RootChangeable> RCPicks = new List<LUPickUpRC_RootChangeable>();

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        RCCatchers.Clear();
        RCPicks.Clear();
        RCCManager = null;
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            RCCManager = obj.GetComponent<LUP_RC_ColliderManager>();
            if (RCCManager != null) break;
        }
        if(RCCManager == null)
        {
            GameObject go = new GameObject();
            go.AddUdonSharpComponent<LUP_RC_ColliderManager>();
            RCCManager = go.GetComponent<LUP_RC_ColliderManager>();
        }
        int pickID = 0;
        int catchID = 0;
        if (RCCManager != null)
        {
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                LUPickUpRC_RootChangeable[] Picks = obj.GetComponentsInChildren<LUPickUpRC_RootChangeable>(true);
                foreach (LUPickUpRC_RootChangeable pick in Picks)
                {
                    pick.RCCManager = RCCManager;
                    pick.ID = pickID;
                    RCPicks.Add(pick);
                    pickID++;
                }
                LUP_RC_CatcherCollider[] Catches = obj.GetComponentsInChildren<LUP_RC_CatcherCollider>(true);
                foreach (LUP_RC_CatcherCollider catchcol in Catches)
                {
                    catchcol.ID = catchID;
                    RCCatchers.Add(catchcol);
                    catchID++;
                }
            }

            RCCManager.RCCatchers = RCCatchers.ToArray();
            RCCManager.RCPicks = RCPicks.ToArray();
            //Debug.Log(SPSCatchers.ToArray().Length);
            //Debug.Log(SPSCatchers[0].name);
        }


    }

    public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
    {
        return true;
    }
}
