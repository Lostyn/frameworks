using System;

namespace fr.lostyn.inspector
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SliderAttribute : DrawerAttribute {
        public float MinValue { get; private set;}
        public float MaxValue { get; private set;}

        public SliderAttribute(float min, float max){
            MinValue = min;
            MaxValue = max;
        }

        public SliderAttribute(int min, int max){
            MinValue = min;
            MaxValue = max;
        }
    }
}