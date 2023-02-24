using System;

namespace fr.lostyneditor.inspector {
    public interface IAttribute {
        Type TargetAttributeType { get; }
    }
}