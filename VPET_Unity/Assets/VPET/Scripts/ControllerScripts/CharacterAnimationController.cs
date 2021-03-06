﻿/*
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace vpet
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator animator;
        public Quaternion[] animationState;
        public Quaternion[] animationInitState;
        public Vector3 bodyPosition;
        private int numHBones;
        private MainController mainController;

        // Start is called before the first frame update
        void Start()
        {
            mainController = GameObject.Find("MainController").GetComponent<MainController>();
            animator = this.GetComponent<Animator>();
            numHBones = Enum.GetNames(typeof(HumanBodyBones)).Length - 1;
            animationState = new Quaternion[numHBones];
            animationInitState = new Quaternion[numHBones];
            HumanDescription humanDescription = this.GetComponent<Animator>().avatar.humanDescription;
            for (int i = 1; i < Math.Min(numHBones, humanDescription.skeleton.Length); i++)
            {
                animationInitState[i] = humanDescription.skeleton[i].rotation;
            }
        }

        // Update is called once per frame
        void OnAnimatorIK(int layerIndex)
        {
            if (!mainController.isRotating)
                animator.SetBoneLocalRotation(HumanBodyBones.Hips, animationState[0]);

            for (int i = 1; i < numHBones; i++)
            {
                if(!(animationState[i] == Quaternion.identity))
                    animator.SetBoneLocalRotation((HumanBodyBones)i, animationState[i]);
            }

            animator.bodyPosition = bodyPosition;
            Vector3 scaleVec = new Vector3(1.0f / transform.lossyScale.x, 1.0f / transform.lossyScale.y, 1.0f / transform.lossyScale.z);
            GetComponent<BoxCollider>().center = Vector3.Scale((bodyPosition - animator.rootPosition), scaleVec);
        }
    }
}
