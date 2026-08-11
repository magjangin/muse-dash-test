using Il2CppInterop.Common.Attributes;

namespace Il2CppSpine.Unity;

[OriginalName("spine-unity.dll", "Spine.Unity", "UpdateMode")]
public enum UpdateMode
{
	Nothing = 0,
	OnlyAnimationStatus = 1,
	OnlyEventTimelines = 4,
	EverythingExceptMesh = 2,
	FullUpdate = 3
}
