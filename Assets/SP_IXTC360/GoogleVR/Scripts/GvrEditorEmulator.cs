// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using System;
using System.Collections.Generic;

using Gvr.Internal;
using UI;

/// Provides mouse-controlled head tracking emulation in the Unity editor.
public class GvrEditorEmulator : MonoBehaviour {
  // GvrEditorEmulator should only be compiled in the Editor.
  //
  // Otherwise, it will override the camera pose every frame on device which causes the
  // following behaviour:
  //
  // The rendered camera pose will still be correct because the VR.InputTracking pose
  // gets applied after LateUpdate has occured. However, any functionality that
  // queries the camera pose during Update or LateUpdate after GvrEditorEmulator has been
  // updated will get the wrong value applied by GvrEditorEmulator intsead.
//#if UNITY_EDITOR
  public static GvrEditorEmulator Instance { get; private set; }
  // Allocate an initial capacity; this will be resized if needed.
  private static Camera[] AllCameras = new Camera[32];
  private const string AXIS_MOUSE_X = "Mouse X";
  private const string AXIS_MOUSE_Y = "Mouse Y";

  // Simulated neck model.  Vector from the neck pivot point to the point between the eyes.
  private static readonly Vector3 NECK_OFFSET = new Vector3(0, 0.075f, 0.08f);

  // Use mouse to emulate head in the editor.
  // These variables must be static so that head pose is maintained between scene changes,
  // as it is on device.
  private float mouseX = 0;
  private float mouseY = 0;
  private float mouseZ = 0;

  public Vector3 HeadPosition { get; private set; }
  public Quaternion HeadRotation { get; private set; }

  public void Recenter() {
    mouseX = mouseZ = 0;  // Do not reset pitch, which is how it works on the phone.
    UpdateHeadPositionAndRotation();
    ApplyHeadOrientationToVRCameras();
  }

  public void UpdateEditorEmulation() {
    if (GvrControllerInput.Recentered) {
      Recenter();
    }
    bool rolled = false;
    if (CanChangeYawPitch()) {
      GvrCursorHelper.HeadEmulationActive = true;
      mouseX += Input.GetAxis(AXIS_MOUSE_X) * 5;
      if (mouseX <= -180) {
        mouseX += 360;
      } else if (mouseX > 180) {
        mouseX -= 360;
      }
      mouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 2.4f;
      mouseY = Mathf.Clamp(mouseY, -85, 85);
    } else if (CanChangeRoll()) {
      GvrCursorHelper.HeadEmulationActive = true;
      rolled = true;
      mouseZ += Input.GetAxis(AXIS_MOUSE_X) * 5;
      mouseZ = Mathf.Clamp(mouseZ, -85, 85);
    } else {
      GvrCursorHelper.HeadEmulationActive = false;
    }

    if (!rolled) {
      // People don't usually leave their heads tilted to one side for long.
      mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
    }

    UpdateHeadPositionAndRotation();
    ApplyHeadOrientationToVRCameras();
  }

  void Awake() {
    if (Instance != null) {
      Debug.LogError("More than one GvrEditorEmulator instance was found in your scene. "
        + "Ensure that there is only one GvrEditorEmulator.");
      this.enabled = false;
      return;
    }
    Instance = this;
  }

  void Update() {
    // GvrControllerInput automatically updates GvrEditorEmulator.
    // This guarantees that GvrEditorEmulator is updated before anything else responds to
    // controller input, which ensures that re-centering works correctly in the editor.
    // If GvrControllerInput is not available, then fallback to using Update().
    if (GvrControllerInput.ApiStatus != GvrControllerApiStatus.Error) {
      return;
    }
        if (PauseMenuController.S_PAUSED) return;

    UpdateEditorEmulation();
  }

  private bool CanChangeYawPitch() {
    // If the MouseControllerProvider is currently active, then don't move the camera.
    if (MouseControllerProvider.IsActivateButtonPressed) {
      return false;
    }

    return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
  }

  private bool CanChangeRoll() {

    //return false; //HACK! to disallow tilting. YT
    // If the MouseControllerProvider is currently active, then don't move the camera.
    if (MouseControllerProvider.IsActivateButtonPressed) {
      return false;
    }

    return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
  }

  private void UpdateHeadPositionAndRotation() {
    HeadRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);
    HeadPosition = HeadRotation * NECK_OFFSET - NECK_OFFSET.y * Vector3.up;
  }

  private void ApplyHeadOrientationToVRCameras() {
    // Get all Cameras in the scene using persistent data structures.
    if (Camera.allCamerasCount > AllCameras.Length) {
      int newAllCamerasSize = Camera.allCamerasCount;
      while (Camera.allCamerasCount > newAllCamerasSize) {
        newAllCamerasSize *= 2;
      }
      AllCameras = new Camera[newAllCamerasSize];
    }
    // The GetAllCameras method doesn't allocate memory (Camera.allCameras does).
    Camera.GetAllCameras(AllCameras);
    // Update all VR cameras using Head position and rotation information.
    for (int i=0; i < Camera.allCamerasCount; ++i) {
      Camera cam = AllCameras[i];
      // Check if the Camera is a valid VR Camera, and if so update it to track head motion.
      if (cam && cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {
        cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
        cam.transform.localRotation = HeadRotation;
      }
    }
  }
//#endif  // UNITY_EDITOR
}
