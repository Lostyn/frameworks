using fr.lostyn.inspector;

namespace fr.lostyneditor.inspector {
    [PropertyDrawCondition(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawCondition : ShowIfPropertyDrawCondition { }
}