using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppSpine;

[OriginalName("spine-unity.dll", "Spine", "TransformMode")]
[Flags]
public enum TransformMode
{
	Normal = 0,
	OnlyTranslation = 7,
	NoRotationOrReflection = 1,
	NoScale = 2,
	NoScaleOrReflection = 6
}
