using System;
using UnityEditor;

namespace Hyperfiction.Core
{
    [InitializeOnLoad]
    class GameScriptExecutionOrder
    {
        static GameScriptExecutionOrder()
        {
            foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
            {
                if (monoScript.GetClass() != null){
                    Attribute[] attrs = Attribute.GetCustomAttributes(monoScript.GetClass(), typeof(ScriptExecutionOrderAttribute));
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        var currentScriptOrder = MonoImporter.GetExecutionOrder(monoScript);
                        var definedScriptOrder = ((ScriptExecutionOrderAttribute)attrs[i]).value;
                        if (currentScriptOrder != definedScriptOrder) {
                            MonoImporter.SetExecutionOrder(monoScript, definedScriptOrder);
                        }
                    }
                }
            }
        }
    }
}