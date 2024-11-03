using DragonBones;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ANIM_WING
{
    public static string APPEAR = "appear";
    public static string DISAPPEAR = "disappear";
    public static string FLAP = "flap";
}

public class Wing : BaseGame
{
    private UnityArmatureComponent dataWing = null;
    private string wingBasePath = "Player/Wing/";

    private int indexWing = 1;

    public void OnInit(int level)
    {
        indexWing = level;
        LoadWingWithAppear();
    }


    public void ChangeWing(int index)
    {
        if (index < 1 || index > 6)
        {
            Debug.LogError("Invalid wing index! Must be between 1 and 6.");
            return;
        }

        indexWing = index;

        if (dataWing != null)
        {
            // Play the disappear animation
            dataWing.animation.Play(ANIM_WING.DISAPPEAR);

            // Add listener for the animation complete event
            dataWing.AddDBEventListener(EventObject.COMPLETE, OnDisappearComplete);
        }
        else
        {
            // If no existing wing, directly load the new one with the appear animation
            LoadWingWithAppear();
        }
    }

    private void OnDisappearComplete(string type, EventObject eventObject)
    {
        // Check if the completed animation is "disappear"
        if (eventObject.animationState.name == ANIM_WING.DISAPPEAR)
        {
            // Remove the listener to avoid multiple calls
            dataWing.RemoveDBEventListener(EventObject.COMPLETE, OnDisappearComplete);

            // Destroy the old wing after the disappear animation is complete
            DestroyImmediate(dataWing);

            // Load the new wing and play the appear animation
            LoadWingWithAppear();
        }
    }

    public void LoadWingWithAppear()
    {
        // Load the new wing data
        string dragonBonesJSONPath = $"{wingBasePath}kyby_chibang{indexWing}_ske";
        string textureAtlasJSONPath = $"{wingBasePath}kyby_chibang{indexWing}_tex";
        Utils._LoadData(dragonBonesJSONPath, textureAtlasJSONPath);
        this.dataWing = UnityFactory.factory.BuildArmatureComponent($"kyby_chibang{indexWing}", "", "", "", this.gameObject);
        dataWing.sortingMode = SortingMode.SortByOrder;
        dataWing.sortingLayerName = "TopLayerTable";
        dataWing.sortingOrder = 1;

        // Play the appear animation
        dataWing.animation.Play(ANIM_WING.APPEAR);

        // Add listener for the animation complete event
        dataWing.AddDBEventListener(EventObject.COMPLETE, OnAppearComplete);
    }

    private void OnAppearComplete(string type, EventObject eventObject)
    {
        // Check if the completed animation is "appear"
        if (eventObject.animationState.name == ANIM_WING.APPEAR)
        {
            // Remove the listener to avoid multiple calls
            dataWing.RemoveDBEventListener(EventObject.COMPLETE, OnAppearComplete);

            // Transition to the flap animation after appear completes
            dataWing.animation.Play(ANIM_WING.FLAP);
        }
    }

    public void RotateWing(float angle)
    {
        TF.rotation = Quaternion.Euler(0, 0, angle);
    }
}
