using System;

namespace Hyperfiction.Core
{
    public class ScriptExecutionOrderAttribute : Attribute {
        public int value;
        public ScriptExecutionOrderAttribute(int id) {
            this.value = id;
        }
    }
}