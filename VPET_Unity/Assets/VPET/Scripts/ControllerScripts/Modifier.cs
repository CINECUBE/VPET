/*
-----------------------------------------------------------------------------
This source file is part of VPET - Virtual Production Editing Tool
http://vpet.research.animationsinstitut.de/
http://github.com/FilmakademieRnd/VPET

Copyright (c) 2018 Filmakademie Baden-Wuerttemberg, Animationsinstitut R&D Lab

This project has been initiated in the scope of the EU funded project 
Dreamspace under grant agreement no 610005 in the years 2014, 2015 and 2016.
http://dreamspaceproject.eu/
Post Dreamspace the project has been further developed on behalf of the 
research and development activities of Animationsinstitut.

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
//! This class provides functionality to the translation, scale and rotation modifieres (objects you can grab to move/rotate/scale selected objects)
//! This class is beeing applied on each modifier gameObject container containing the actual modifier parts as child objects.
//! It automatically adjusts the orientation of the modifiers for best visibility (real coordinate system is beeing ignored).
//!
namespace vpet
{
	public class Modifier : MonoBehaviour {
	
	    //!
	    //! if pause is set to true all repositioning / rotating of the modifers is avoided
	    //! pause is activated when the modifier is not visible
	    //!
	    private bool pause = true;
	
	    //!
	    //! stores the last position of the modifiers to be able to detect weather it moved.
	    //!
	    Vector3 lastPosition;
	
	    //!
	    //! Use this for initialization
	    //!
		void Start () 
	    {
	        this.setVisible(false);
	        lastPosition = this.transform.position;
		}
	
		/*
	    //!
	    //! Update is called once per frame
	    //!
		void Update () 
	    {
	        if (!pause && lastPosition != this.transform.position)
	        {
	            this.adjustOrientation();
	            lastPosition = this.transform.position;
	        }
		}
		*/

	    //!
	    //! hides or showes all parts of this modifier (beeing present as childs of this container gameObject)
	    //! @param    visible     shall the modifier be visible or not    
	    //!
	    public void setVisible(bool visible)
	    {
	        pause = !visible;
	        foreach (Transform child in transform)
	        {
	            child.GetComponent<ModifierComponent>().setVisible(visible);
	        }
	    }
	
	    //!
	    //! resets the color of all parts (beeing present as childs of this container gameObject) of this modifier to their default "unselected" color
	    //!
	    public void resetColors()
	    {
	        pause = false;
	        foreach (Transform child in transform)
	            child.gameObject.GetComponent<ModifierComponent>().resetColor();
	    }
	
	    //!
	    //! sets the alpha of the color of all parts (beeing present as childs of this container gameObject) of this modifier to a value < 1 (beeing defined in the ModifierComponent script).
	    //!
	    public void makeTransparent()
	    {
	        foreach (Transform child in transform)
	            child.gameObject.GetComponent<ModifierComponent>().makeTransparent();
	    }
	
	    //!
	    //! enables an external script to set pause to true
	    //!
	    public void isUsed()
	    {
	        pause = true;
	    }
	
	    //!
	    //! This function adjusts the orientation of the modifiers. This will break the real coordiante system for visibility reasons.
	    //! So if you are e.g. looking along the z-axis the z-axis modifier will not point into z direction (since it would then be behind the x and y modifiers)
	    //! but instead points towards the negative z-axis, or in other words more towards the camera.
	    //!
	    private void adjustOrientation()
	    {
	        if (this.name == "TranslateModifier")
	        {
	            if (Camera.main.transform.position.x > this.transform.position.x)
	            {
	                this.transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
	                this.transform.GetChild(3).localPosition = new Vector3(0.5f, this.transform.GetChild(3).localPosition.y, this.transform.GetChild(3).localPosition.z);
	                this.transform.GetChild(4).localPosition = new Vector3(0.5f, this.transform.GetChild(4).localPosition.y, this.transform.GetChild(4).localPosition.z);
	                this.transform.GetChild(7).localPosition = new Vector3(0.5f, this.transform.GetChild(7).localPosition.y, this.transform.GetChild(7).localPosition.z);
	                this.transform.GetChild(8).localPosition = new Vector3(0.5f, this.transform.GetChild(8).localPosition.y, this.transform.GetChild(8).localPosition.z);
	            }
	            else
	            {
	                this.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
	                this.transform.GetChild(3).localPosition = new Vector3(-0.5f, this.transform.GetChild(3).localPosition.y, this.transform.GetChild(3).localPosition.z);
	                this.transform.GetChild(4).localPosition = new Vector3(-0.5f, this.transform.GetChild(4).localPosition.y, this.transform.GetChild(4).localPosition.z);
	                this.transform.GetChild(7).localPosition = new Vector3(-0.5f, this.transform.GetChild(7).localPosition.y, this.transform.GetChild(7).localPosition.z);
	                this.transform.GetChild(8).localPosition = new Vector3(-0.5f, this.transform.GetChild(8).localPosition.y, this.transform.GetChild(8).localPosition.z);
	            }
	
	            if (Camera.main.transform.position.y > this.transform.position.y)
	            {
	                this.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
	                this.transform.GetChild(3).localPosition = new Vector3(this.transform.GetChild(3).localPosition.x, 0.5f, this.transform.GetChild(3).localPosition.z);
	                this.transform.GetChild(4).localPosition = new Vector3(this.transform.GetChild(4).localPosition.x, 0.5f, this.transform.GetChild(4).localPosition.z);
	                this.transform.GetChild(5).localPosition = new Vector3(this.transform.GetChild(5).localPosition.x, 0.5f, this.transform.GetChild(5).localPosition.z);
	                this.transform.GetChild(6).localPosition = new Vector3(this.transform.GetChild(6).localPosition.x, 0.5f, this.transform.GetChild(6).localPosition.z);
	            }
	            else
	            {
	                this.transform.GetChild(1).rotation = Quaternion.Euler(180, 0, 0);
	                this.transform.GetChild(3).localPosition = new Vector3(this.transform.GetChild(3).localPosition.x, -0.5f, this.transform.GetChild(3).localPosition.z);
	                this.transform.GetChild(4).localPosition = new Vector3(this.transform.GetChild(4).localPosition.x, -0.5f, this.transform.GetChild(4).localPosition.z);
	                this.transform.GetChild(5).localPosition = new Vector3(this.transform.GetChild(5).localPosition.x, -0.5f, this.transform.GetChild(5).localPosition.z);
	                this.transform.GetChild(6).localPosition = new Vector3(this.transform.GetChild(6).localPosition.x, -0.5f, this.transform.GetChild(6).localPosition.z);
	            }
	
	            if (Camera.main.transform.position.z > this.transform.position.z)
	            {
	                this.transform.GetChild(2).rotation = Quaternion.Euler(0, 0, 0);
	                this.transform.GetChild(5).localPosition = new Vector3(this.transform.GetChild(5).localPosition.x, this.transform.GetChild(5).localPosition.y, 0.5f);
	                this.transform.GetChild(6).localPosition = new Vector3(this.transform.GetChild(6).localPosition.x, this.transform.GetChild(6).localPosition.y, 0.5f);
	                this.transform.GetChild(7).localPosition = new Vector3(this.transform.GetChild(7).localPosition.x, this.transform.GetChild(7).localPosition.y, 0.5f);
	                this.transform.GetChild(8).localPosition = new Vector3(this.transform.GetChild(8).localPosition.x, this.transform.GetChild(8).localPosition.y, 0.5f);
	            }
	            else
	            {
	                this.transform.GetChild(2).rotation = Quaternion.Euler(0, 180, 0);
	                this.transform.GetChild(5).localPosition = new Vector3(this.transform.GetChild(5).localPosition.x, this.transform.GetChild(5).localPosition.y, -0.5f);
	                this.transform.GetChild(6).localPosition = new Vector3(this.transform.GetChild(6).localPosition.x, this.transform.GetChild(6).localPosition.y, -0.5f);
	                this.transform.GetChild(7).localPosition = new Vector3(this.transform.GetChild(7).localPosition.x, this.transform.GetChild(7).localPosition.y, -0.5f);
	                this.transform.GetChild(8).localPosition = new Vector3(this.transform.GetChild(8).localPosition.x, this.transform.GetChild(8).localPosition.y, -0.5f);
	            }
	        }
	    }
}
}