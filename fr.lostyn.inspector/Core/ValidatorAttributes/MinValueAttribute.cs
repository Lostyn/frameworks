using System;

namespace fr.lostyn.inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute{
        public float MinValue { get; private set; }

        public MinValueAttribute(float maxValue)
        {
            this.MinValue = maxValue;
        }

        public MinValueAttribute(int maxValue)
        {
            this.MinValue = maxValue;
        }
    }
}