using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine.Unity;

public class IAnimationStateComponent : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_AnimationState_Public_Abstract_Virtual_New_get_AnimationState_0;

	public unsafe virtual AnimationState AnimationState
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_AnimationState_Public_Abstract_Virtual_New_get_AnimationState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
	}

	static IAnimationStateComponent()
	{
		Il2CppClassPointerStore<IAnimationStateComponent>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "IAnimationStateComponent");
		NativeMethodInfoPtr_get_AnimationState_Public_Abstract_Virtual_New_get_AnimationState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IAnimationStateComponent>.NativeClassPtr, 100664334);
	}

	public IAnimationStateComponent(IntPtr pointer)
		: base(pointer)
	{
	}
}
