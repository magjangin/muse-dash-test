using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppSpine.Unity;

public class IHasSkeletonDataAsset : Il2CppObjectBase
{
	static IHasSkeletonDataAsset()
	{
		Il2CppClassPointerStore<IHasSkeletonDataAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "IHasSkeletonDataAsset");
	}

	public IHasSkeletonDataAsset(IntPtr pointer)
		: base(pointer)
	{
	}
}
