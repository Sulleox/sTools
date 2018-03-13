using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DividerAttribute))]
public class DividerDrawer : DecoratorDrawer
{
    private readonly float Height = 6;

    public override float GetHeight()
    {
        return ((DividerAttribute)attribute).dividerHeight + Height;
    }

    public override void OnGUI(Rect position)
    {
        Rect newRect = position;
        newRect.height -= Height;

        EditorGUI.DrawRect(newRect, Color.green);
    }
}
