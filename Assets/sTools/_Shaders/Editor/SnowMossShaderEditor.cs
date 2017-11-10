using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SnowMossShaderEditor : ShaderGUI
{
    //Properties
    MaterialProperty snowVector = null;
    MaterialProperty snowThreshold = null;
    MaterialProperty snowAmout = null;

    MaterialProperty snowColor = null;
    MaterialProperty snowTexture = null;
    MaterialProperty snowMetRange = null;
    MaterialProperty snowGlosRange = null;
    MaterialProperty snowMetallic = null;
    MaterialProperty snowBumpMap = null;

    MaterialProperty otherColor = null;
    MaterialProperty otherTexture = null;
    MaterialProperty otherMetRange = null;
    MaterialProperty otherGlosRange = null;
    MaterialProperty otherMetallic = null;
    MaterialProperty otherBumpMap = null;

    MaterialProperty glitterMap = null;
    MaterialProperty noiseMap = null;

    MaterialEditor m_MaterialEditor = null;

    bool showSnowProperties = false;
    bool showBaseProperties = false;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        m_MaterialEditor = materialEditor;

        //Base Properties
        m_MaterialEditor.ShaderProperty(snowVector, "Snow/Moss Vector");
        m_MaterialEditor.ShaderProperty(snowThreshold, "Snow/Moss Threshold");
        m_MaterialEditor.ShaderProperty(snowAmout, "Snow/Moss Amount");

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        showSnowProperties = EditorGUILayout.Foldout(showSnowProperties, "Show Snow/Grass Texture Properties");
        //Snow Properties
        if (showSnowProperties)
        {
            m_MaterialEditor.ShaderProperty(snowColor, "Snow/Moss Color");
            m_MaterialEditor.ShaderProperty(snowTexture, "Snow/Moss Texture");
            m_MaterialEditor.ShaderProperty(snowMetallic, "Snow/Moss Metallic Map");
            m_MaterialEditor.ShaderProperty(snowMetRange, "Snow/Moss Metallic Range");
            m_MaterialEditor.ShaderProperty(snowGlosRange, "Snow/Moss Glossiness Range");
            m_MaterialEditor.ShaderProperty(snowBumpMap, "Snow/Moss Normal Map");
        }


        showBaseProperties = EditorGUILayout.Foldout(showBaseProperties, "Show Base Texture Properties");
        //Snow Properties
        if (showBaseProperties)
        {
            m_MaterialEditor.ShaderProperty(otherColor, "Base Color");
            m_MaterialEditor.ShaderProperty(otherTexture, "Base Texture");
            m_MaterialEditor.ShaderProperty(otherMetallic, "Base Metallic Map");
            m_MaterialEditor.ShaderProperty(otherMetRange, "Base Metallic Range");
            m_MaterialEditor.ShaderProperty(otherGlosRange, "Base Glossiness Range");
            m_MaterialEditor.ShaderProperty(otherBumpMap, "Base Normal Map");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        m_MaterialEditor.ShaderProperty(glitterMap, "Glitter Map");
        m_MaterialEditor.ShaderProperty(noiseMap, "Noise Map");
    }

    public void FindProperties(MaterialProperty[] properties)
    {
        snowVector = FindProperty("_SnowVector", properties);
        snowThreshold = FindProperty("_SnowThreshold", properties);
        snowAmout = FindProperty("_SnowAmount", properties);

        snowColor = FindProperty("_snowColor", properties);
        snowTexture = FindProperty("_snowTex", properties);
        snowGlosRange = FindProperty("_snowGlossiness", properties);
        snowMetRange = FindProperty("_snowMetallic", properties);
        snowMetallic = FindProperty("_snowMet", properties);
        snowBumpMap = FindProperty("_snowBumpMap", properties);

        otherColor = FindProperty("_otherColor", properties);
        otherTexture = FindProperty("_otherTex", properties);
        otherGlosRange = FindProperty("_otherGlossiness", properties);
        otherMetRange = FindProperty("_otherMetallic", properties);
        otherMetallic = FindProperty("_otherMet", properties);
        otherBumpMap = FindProperty("_otherBumpMap", properties);

        glitterMap = FindProperty("_glitterMap", properties);
        noiseMap = FindProperty("_noiseTex", properties);
    }
}
