using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public struct TemporaryLabelWidth : IDisposable {
        static readonly Stack<float> temporaryWidths = new Stack<float>();

        public TemporaryLabelWidth( float width ) {
            temporaryWidths.Push( EditorGUIUtility.labelWidth );
            EditorGUIUtility.labelWidth = width;
        }

        public void Dispose() => EditorGUIUtility.labelWidth = temporaryWidths.Pop();
    }

    public struct TemporaryColor : IDisposable {
        static readonly Stack<Color> temporaryColors = new Stack<Color>();

        public TemporaryColor( Color color ) {
            temporaryColors.Push( GUI.color );
            GUI.color = color;
        }

        public void Dispose() => GUI.color = temporaryColors.Pop();
    }
}