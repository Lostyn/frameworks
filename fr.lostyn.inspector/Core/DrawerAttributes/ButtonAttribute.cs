using System;

namespace fr.lostyn.inspector {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute {
        public string Text { get; private set; }

        public ButtonAttribute(string text = null ) {
            Text = text;
        }
    }
}