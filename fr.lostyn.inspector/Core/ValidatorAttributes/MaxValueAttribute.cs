using System;

namespace fr.lostyn.inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute{
        public float MaxValue { get; private set; }

        public MaxValueAttribute(float maxValue)
        {
            this.MaxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            this.MaxValue = maxValue;
        }
    }
}