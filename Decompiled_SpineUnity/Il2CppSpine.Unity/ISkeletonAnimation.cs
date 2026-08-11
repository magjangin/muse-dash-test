using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine.Unity;

public class ISkeletonAnimation : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateLocal_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateLocal_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateWorld_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateWorld_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateComplete_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateComplete_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0;

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

	static ISkeletonAnimation()
	{
		Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "ISkeletonAnimation");
		NativeMethodInfoPtr_add_UpdateLocal_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664326);
		NativeMethodInfoPtr_remove_UpdateLocal_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664327);
		NativeMethodInfoPtr_add_UpdateWorld_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664328);
		NativeMethodInfoPtr_remove_UpdateWorld_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664329);
		NativeMethodInfoPtr_add_UpdateComplete_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664330);
		NativeMethodInfoPtr_remove_UpdateComplete_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664331);
		NativeMethodInfoPtr_get_Skeleton_Public_Abstract_Virtual_New_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISkeletonAnimation>.NativeClassPtr, 100664332);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void add_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_UpdateLocal_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void remove_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_UpdateLocal_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void add_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_UpdateWorld_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void remove_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_UpdateWorld_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void add_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_UpdateComplete_Public_Abstract_Virtual_New_add_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void remove_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_UpdateComplete_Public_Abstract_Virtual_New_rem_Void_UpdateBonesDelegate_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ISkeletonAnimation(IntPtr pointer)
		: base(pointer)
	{
	}
}
