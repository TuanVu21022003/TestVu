using DragonBones;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ANIM_WEAPON
{
    public static string STAND = "stand";
    public static string APPEAR = "appear";
    public static string DISAPPEAR = "disappear";
    public static string FIRE = "fire";
}

public class Weapon : BaseGame
{
    private UnityArmatureComponent dataWeapon = null;
    private string wingBasePath = "Player/Weapon/";

    private int indexWeapon = 1;

    public void OnInit(int level)
    {
        indexWeapon = level;
        LoadWeaponWithAppear();
    }


    public void ChangeWeapon(int index)
    {
        if (index < 1 || index > 6)
        {
            Debug.LogError("Invalid wing index! Must be between 1 and 6.");
            return;
        }

        indexWeapon = index;

        if (dataWeapon != null)
        {
            // Play the disappear animation
            dataWeapon.animation.Play(ANIM_WEAPON.DISAPPEAR);

            // Add listener for the animation complete event
            dataWeapon.AddDBEventListener(EventObject.COMPLETE, OnDisappearComplete);
        }
        else
        {
            // If no existing wing, directly load the new one with the appear animation
            LoadWeaponWithAppear();
        }
    }

    private void OnDisappearComplete(string type, EventObject eventObject)
    {
        // Check if the completed animation is "disappear"
        if (eventObject.animationState.name == ANIM_WEAPON.DISAPPEAR)
        {
            // Remove the listener to avoid multiple calls
            dataWeapon.RemoveDBEventListener(EventObject.COMPLETE, OnDisappearComplete);

            // Destroy the old wing after the disappear animation is complete
            DestroyImmediate(dataWeapon);

            // Load the new wing and play the appear animation
            LoadWeaponWithAppear();
        }
    }

    public void LoadWeaponWithAppear()
    {
        // Load the new wing data
        string dragonBonesJSONPath = $"{wingBasePath}kyby_paotai{indexWeapon}_ske";
        string textureAtlasJSONPath = $"{wingBasePath}kyby_paotai{indexWeapon}_tex";
        Utils._LoadData(dragonBonesJSONPath, textureAtlasJSONPath);
        this.dataWeapon = UnityFactory.factory.BuildArmatureComponent($"kyby_paotai{indexWeapon}", "", "", "", this.gameObject);
        dataWeapon.sortingMode = SortingMode.SortByOrder;
        dataWeapon.sortingLayerName = "TopLayerTable";
        dataWeapon.sortingOrder = 3;
        // Play the appear animation
        dataWeapon.animation.Play(ANIM_WEAPON.APPEAR);

        // Add listener for the animation complete event
        dataWeapon.AddDBEventListener(EventObject.COMPLETE, OnAppearComplete);
    }

    private void OnAppearComplete(string type, EventObject eventObject)
    {
        // Check if the completed animation is "appear"
        if (eventObject.animationState.name == ANIM_WEAPON.APPEAR)
        {
            // Remove the listener to avoid multiple calls
            dataWeapon.RemoveDBEventListener(EventObject.COMPLETE, OnAppearComplete);

            // Transition to the flap animation after appear completes
            dataWeapon.animation.Play(ANIM_WEAPON.STAND);
        }
    }
    public void RotateWeapon(float angle)
    {
        TF.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void FireWeapon()
    {
        dataWeapon.animation.Play(ANIM_WEAPON.FIRE);
    }
}
