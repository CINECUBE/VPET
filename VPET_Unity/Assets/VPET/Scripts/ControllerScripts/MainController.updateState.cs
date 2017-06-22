/*
-----------------------------------------------------------------------------
This source file is part of VPET - Virtual Production Editing Tool
http://vpet.research.animationsinstitut.de/
http://github.com/FilmakademieRnd/VPET

Copyright (c) 2016 Filmakademie Baden-Wuerttemberg, Institute of Animation

This project has been realized in the scope of the EU funded project Dreamspace
under grant agreement no 610005.
http://dreamspaceproject.eu/

This program is free software; you can redistribute it and/or modify it under
the terms of the MIT License as published by the Open Source Initiative.

This program is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE. See the MIT License for more details.

You should have received a copy of the MIT License along with
this program; if not go to
https://opensource.org/licenses/MIT
-----------------------------------------------------------------------------
*/
﻿using UnityEngine;
using System.Collections;

//!
//! MainController part handling the update of states (executes state machine)
//!
namespace vpet
{
	public partial class MainController : MonoBehaviour {
		
	    // cache old state
	    private Mode oldState = Mode.idle;
	    private Transform oldSelection = null;
		public Transform oldParent = null;

	    //!
		//! Update is called once per frame
		//!
	    void Update ()
	    {
	        //position modifiers if neccessary
	        if (currentSelection)
	        {
	            if (activeMode == Mode.translationMode || activeMode == Mode.animationEditing)
	            {
	                translateModifier.transform.position = currentSelection.position;
	                //translateModifier.transform.position = currentSelection.GetComponent<Collider>().bounds.center;
	
                    translateModifier.transform.localScale = getModifierScale();
                    translateModifier.transform.GetChild(9).position = new Vector3(currentSelection.position.x, 0.001f, currentSelection.position.z);
	            }
	            else if (activeMode == Mode.rotationMode)
	            {
	                rotationModifier.transform.position = currentSelection.position;
	                rotationModifier.transform.localScale = getModifierScale() ;
	            }
	            else if (activeMode == Mode.scaleMode)
	            {
	                scaleModifier.transform.position = currentSelection.position;
	                scaleModifier.transform.rotation = currentSelection.rotation;
	                scaleModifier.transform.localScale = getModifierScale();
	            }
	        }

            //state machine
            if (oldState != activeMode || currentSelection != oldSelection)
            {
                print(oldState.ToString() + " >>>>> " + activeMode);

                if (hasUpdatedProjectionMatrix)
                {
                    UpdateProjectionMatrixSecondaryCameras();
                    hasUpdatedProjectionMatrix = false;
                }

                // unlock if active mode is none editing mode
                //if (activeMode!=Mode.translationMode && activeMode!=Mode.rotationMode && activeMode != Mode.scaleMode && activeMode != Mode.objectLinkCamera && activeMode != Mode.animationEditing && activeMode != Mode.pointToMoveMode && activeMode != Mode.lightSettingsMode)
                if (activeMode != oldState)
                {
                    serverAdapter.sendLock(currentSelection, false);
                }

                animationController.deactivate();

                if (ui.LayoutUI == layouts.ANIMATION)
                {
                    animationController.activate(currentSelection != oldSelection);
                }

                //properly disable old mode
                switch (oldState) {
                    case (Mode.idle):
                        break;
                    case (Mode.translationMode):
                        translateModifier.GetComponent<Modifier>().setVisible(false);
                        //if ( activeMode != Mode.objectLinkCamera && activeMode != Mode.pointToMoveMode && ui.LayoutUI != layouts.ANIMATION )
                        //   ui.drawSecondaryMenu(layouts.EDIT);
                        ui.hideRangeSlider();
                        if (activeMode != Mode.objectLinkCamera && activeMode != Mode.translationMode && activeMode != Mode.pointToMoveMode)
                            ui.hideParameterMenu();
                        break;
	                case (Mode.rotationMode):
	                    rotationModifier.GetComponent<Modifier>().setVisible(false);
                        ui.hideRangeSlider();
                        ui.hideParameterMenu();
                        break;
	                case (Mode.scaleMode):
	                    scaleModifier.GetComponent<Modifier>().setVisible(false);
                        ui.hideRangeSlider();
                        ui.hideParameterMenu();
                        break;
	                case (Mode.objectLinkCamera):
                        if (currentSelection)
                        {
                            if (currentSelection.GetComponent<SceneObject>().isSpotLight ||
                                currentSelection.GetComponent<SceneObject>().isPointLight ||
                                currentSelection.GetComponent<SceneObject>().isDirectionalLight)
                            {
                                currentSelection.parent = oldParent;
                            }
                            else
                            {
                                //if (ui.LayoutUI != layouts.ANIMATION)
                                //    currentSelection.GetComponent<SceneObject>().setKinematic(false);
                                currentSelection.parent = oldParent;
                            }
                            //if (ui.LayoutUI == layouts.ANIMATION)
                            //    animationController.setKeyFrame();
                        }
                        //if (activeMode != Mode.objectLinkCamera && activeMode != Mode.translationMode && activeMode != Mode.pointToMoveMode) ui.drawSecondaryMenu(layouts.EDIT);
                        if (activeMode != Mode.objectLinkCamera &&  activeMode != Mode.translationMode && activeMode != Mode.pointToMoveMode)
                            ui.hideParameterMenu();
                        break;
	                case (Mode.pointToMoveMode):
                        if (activeMode != Mode.objectLinkCamera && activeMode != Mode.translationMode && activeMode != Mode.pointToMoveMode)
                            ui.hideParameterMenu();
                        //if (activeMode != Mode.objectLinkCamera && activeMode != Mode.translationMode && activeMode != Mode.pointToMoveMode) ui.drawSecondaryMenu(layouts.EDIT);
                        break;
	                case (Mode.objectMenuMode):
	                    break;
	                case (Mode.lightMenuMode):
	                    break;
	                case (Mode.lightSettingsMode):
	                    ui.hideLightSettingsWidget();
                        ui.hideRangeSlider();
                        ui.hideParameterMenu();
                        break;
	                case (Mode.addMode):
	                    break;
	                default:
	                    break;
	            }
	            //enable new mode
	            switch (activeMode)
	            {
	                case (Mode.idle):
	                    ui.hideCenterMenu();
	                    break;
	                case (Mode.translationMode):
                        serverAdapter.sendLock(currentSelection, true);
                        translateModifier.GetComponent<Modifier>().setVisible(true);
                        if ( ui.LayoutUI != layouts.ANIMATION)  ui.drawSecondaryMenu(layouts.TRANSLATION);
                        ui.resetRangeSlider();
                        ConnectRangeSlider( currentSelection.GetComponent<SceneObject>(), "TranslateX", 2f * VPETSettings.Instance.sceneScale);
                        ui.drawParameterMenu(layouts.TRANSLATION);
                        break;
	                case (Mode.rotationMode):
                        serverAdapter.sendLock(currentSelection, true);
                        rotationModifier.GetComponent<Modifier>().setVisible(true);
                        ui.resetRangeSlider();
                        ConnectRangeSlider(currentSelection.GetComponent<SceneObject>(), "RotateX", 1f);
                        ui.drawParameterMenu(layouts.TRANSFORM);
                        break;
	                case (Mode.scaleMode):
                        scaleModifier.GetComponent<Modifier>().setVisible(true);
                        serverAdapter.sendLock(currentSelection, true);
                        ui.resetRangeSlider();
                        ConnectRangeSlider(currentSelection.GetComponent<SceneObject>(), "ScaleX", 0.02f);
                        ui.drawParameterMenu(layouts.TRANSFORM);
                        break;
	                case (Mode.objectLinkCamera):
	                    if (currentSelection.GetComponent<SceneObject>().isSpotLight ||
	                        currentSelection.GetComponent<SceneObject>().isPointLight ||
	                        currentSelection.GetComponent<SceneObject>().isDirectionalLight)
	                    {
                            oldParent = currentSelection.parent;
	                        currentSelection.parent = Camera.main.transform;
                        }
                        else
	                    {
 	                        currentSelection.GetComponent<SceneObject>().setKinematic(true);
							oldParent = currentSelection.parent;
	                        currentSelection.parent = Camera.main.transform;
	                    }
                        ui.hideRangeSlider();
                        // ui.drawParameterMenu(layouts.TRANSLATION);
                        serverAdapter.sendLock(currentSelection, true);
                        break;
	                case (Mode.pointToMoveMode):
                        ui.hideRangeSlider();
                        // ui.drawParameterMenu(layouts.TRANSLATION);
                        serverAdapter.sendLock(currentSelection, true);
                        break;
	                case (Mode.objectMenuMode):
	                    if (currentSelection && ui.LayoutUI != layouts.SCOUT )
	                    {
	                        if (ui.LayoutUI == layouts.ANIMATION)
	                        {
	                            ui.drawCenterMenu(layouts.ANIMATION);
                                ui.drawSecondaryMenu(layouts.ANIMATION);
	                        }
                            else if (currentSelection.GetComponent<SceneObject>().isMocapTrigger) // mocap trigger component at object
                            {
                                ui.drawCenterMenu(layouts.MOCAP);
                            }
                            else
	                        {
	                            ui.drawCenterMenu(layouts.OBJECT);
	                        }
                        }
                        break;
	                case (Mode.lightMenuMode):
	                    if (currentSelection && ui.LayoutUI != layouts.SCOUT)
	                    {
                            if (ui.LayoutUI == layouts.ANIMATION)
                            {
                                ui.drawCenterMenu(layouts.ANIMATION);
                                ui.drawSecondaryMenu(layouts.ANIMATION);
                            }
                            else
                            {
                                ui.drawCenterMenu(layouts.LIGHT);
                            }
                            /*
	                        if (currentSelection.GetComponent<SceneObject>().isDirectionalLight)
	                            ui.drawCenterMenu(layouts.LIGHTDIR);
	                        else if (currentSelection.GetComponent<SceneObject>().isPointLight)
	                            ui.drawCenterMenu(layouts.LIGHTPOINT);
	                        else if (currentSelection.GetComponent<SceneObject>().isSpotLight)
	                            ui.drawCenterMenu(layouts.LIGHTSPOT);
                            */
                        }
                        break;
	                case (Mode.lightSettingsMode):
                        if (currentSelection)
                        {
                            currentSelection.GetComponent<SceneObject>().hideLightVisualization(true);
                        }
                        // ConnectRangeSlider(currentSelection.GetComponent<SceneObject>(), "LightIntensity", 1f);
                        ConnectRangeSlider(currentSelection.GetComponent<SceneObject>().setLightIntensity, currentSelection.GetComponent<SceneObject>().getLightIntensity(), 0.01f);
                        if (currentSelection.GetComponent<SceneObject>().isDirectionalLight)
                            ui.drawParameterMenu(layouts.LIGHTDIR);
                        else if (currentSelection.GetComponent<SceneObject>().isPointLight)
                            ui.drawParameterMenu(layouts.LIGHTPOINT);
                        else if (currentSelection.GetComponent<SceneObject>().isSpotLight)
                            ui.drawParameterMenu(layouts.LIGHTSPOT);
                        ui.drawLightSettingsWidget();
                        serverAdapter.sendLock(currentSelection, true);
                        break;
	                case (Mode.addMode):
	                    break;
	                default:
	                    break;
	            }
	            //renew old state
	            oldState = activeMode;
	            oldSelection = currentSelection;
	        }
		}
}
}