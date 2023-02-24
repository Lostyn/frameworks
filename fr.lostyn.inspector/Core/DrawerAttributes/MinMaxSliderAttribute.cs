using System;

namespace fr.lostyn.inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinMaxSliderAttribute : DrawerAttribute {
        public float MinValue { get; private set;}
        public float MaxValue { get; private set;}

        public MinMaxSliderAttribute(float min, float max){
            MinValue = min;
            MaxValue = max;
        }

        public MinMaxSliderAttribute(int min, int max){
            MinValue = min;
            MaxValue = max;
        }
    }
}