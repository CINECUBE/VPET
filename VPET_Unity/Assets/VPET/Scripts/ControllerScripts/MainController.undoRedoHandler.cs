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
//! MainController part connecting UI to undo/redo controller
//!
namespace vpet
{
	public partial class MainController : MonoBehaviour {
	
	    //!
	    //! undo last action
	    //!
	    public void undoLastAction()
	    {
	        undoRedoController.undoAction();
	    }
	
	    //!
	    //! redo last action
	    //!
	    public void redoLastAction()
	    {
	        undoRedoController.redoAction();
	    }
	
	    //!
	    //! make the undo button unselectable
	    //!
	    public void deactivateUndoButton()
	    {
	        //ui.setUndoActive(false);
	    }
	
	    //!
	    //! make the redo button unselectable
	    //!
	    public void deactivateRedoButton()
	    {
	       // ui.setRedoActive(false);
	    }
	
	    //!
	    //! make the undo button selectable
	    //!
	    public void activateUndoButton()
	    {
	        //ui.setUndoActive(true);
	    }
	
	    //!
	    //! make the redo button selectable
	    //!
	    public void activateRedoButton()
	    {
	        //ui.setRedoActive(true);
	    }
}
}