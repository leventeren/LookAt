﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using OneDayGame;

[CustomEditor(typeof(LookAt))]
public class LookAtEditor : Editor {

	private SerializedProperty _labelStyle;
	private SerializedProperty _target;

	public void OnEnable() {
		_labelStyle = serializedObject.FindProperty("_labelStyle");
		_target = serializedObject.FindProperty("_target");
	}

	public override void OnInspectorGUI() {
		LookAt script = (LookAt)target;
		serializedObject.Update();

		EditorGUILayout.PropertyField(_target);
		EditorGUILayout.PropertyField(
				_labelStyle,
				new GUIContent(
					"Label Style",
					"Style of the 'on scene' info label"),
				true);
		script.Option = (LookAt.Options)EditorGUILayout.EnumPopup(
				"Option",
				script.Option);
		script.OverrideRoot = (Transform)EditorGUILayout.ObjectField(
				"Transform",
				script.OverrideRoot,
				typeof(Transform),
				true);
		script.ClickInstantRot = EditorGUILayout.Toggle(
				"Instant Rotation",
				script.ClickInstantRot);

		switch (script.Option) {
			case LookAt.Options.Standard:
				break;
			case LookAt.Options.YAxisOnly:
				break;
			case LookAt.Options.RotWithSlerp:
				script.Speed = EditorGUILayout.FloatField(
						"Speed",
						script.Speed);
				break;
			case LookAt.Options.RotThreshold:
				script.MaxRotSpeed = EditorGUILayout.FloatField(
						"Max Rot. Speed",
						script.MaxRotSpeed);
				script.MinTimeToReach = EditorGUILayout.FloatField(
						"Min Time to Reach",
						script.MinTimeToReach);
				script.ThresholdAngle = EditorGUILayout.FloatField(
						"Threshold angle",
						script.ThresholdAngle);
				break;
			case LookAt.Options.RotWithSDA:
				script.MaxRotSpeed = EditorGUILayout.FloatField(
						"Max Rot. Speed",
						script.MaxRotSpeed);
				script.MinTimeToReach = EditorGUILayout.FloatField(
						"Min Time to Reach",
						script.MinTimeToReach);
				break;
		}
		
		// Save changes
		if (GUI.changed) {
			EditorUtility.SetDirty(script);
		}
		serializedObject.ApplyModifiedProperties();
	}

	public void OnSceneGUI() {
		LookAt script = (LookAt)target;

		switch (script.Option) {
			case LookAt.Options.RotThreshold:
				Handles.color = Color.blue;
				Handles.Label(
						// TODO Add param for label position.
						script.transform.position + Vector3.up * 1.5f,
						"ThresholdAngle: " + script.ThresholdAngle.ToString(),
						script.LabelStyle);
				Handles.DrawWireArc(
						script.transform.position, 
						script.transform.up, 
						// Make the arc simetrical on the left and right
						// side of the player.
						Quaternion.AngleAxis(
							-script.ThresholdAngle / 2, 
							Vector3.up) * script.transform.forward,
						script.ThresholdAngle, 
						2);
				script.ThresholdAngle = Handles.ScaleValueHandle(
						script.ThresholdAngle,
						script.transform.position + script.transform.forward * 2,
						script.transform.rotation,
						1,
						Handles.ConeCap,
						1);
				break;
		}
	}
}