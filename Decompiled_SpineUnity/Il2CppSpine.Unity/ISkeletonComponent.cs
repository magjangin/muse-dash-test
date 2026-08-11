using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine.Unity;

public class ISkeletonComponent : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_Skeleton_Public_Abstract_Virtual_New_get_Skeleton_0;

	public unsafe virtual Skeleton Skeleton
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Skeleton_Public_Abstract_Virtual_New_get_Skeleton_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
	}

	static ISkeletonComponent()
	{
		Il2CppClassPointerStore<ISkeletonComponent>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "ISkeletonComponent");
		NativeMethodInfoPtr_get_Skeleton_Public_Abstract_Virtual_New_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonComponent>.NativeClassPtr, 100664333);
	}

	public ISkeletonComponent(IntPtr pointer)
		: base(pointer)
	{
	}
}
