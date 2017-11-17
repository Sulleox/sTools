using System;
using UnityEngine;
using UnityEditor;


//Attribute Field lock the utilisation of the attribut to a variables.
//Allow Multiple allow multiple usage on a same variables.
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public class DividerAttribute : PropertyAttribute
{
    public float dividerHeight;

    public DividerAttribute(float dividerHeight)
    {
        this.dividerHeight = dividerHeight;
    }
}
