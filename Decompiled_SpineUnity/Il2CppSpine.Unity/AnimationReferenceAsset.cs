using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class AnimationReferenceAsset : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_QuietSkeletonData;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonDataAsset;

	private static readonly IntPtr NativeFieldInfoPtr_animationName;

	private static readonly IntPtr NativeFieldInfoPtr_animation;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Animation_Public_get_Animation_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_op_Implicit_Public_Static_Animation_AnimationReferenceAsset_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static bool QuietSkeletonData
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_QuietSkeletonData, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_QuietSkeletonData, (void*)(&flag));
		}
	}

	public unsafe SkeletonDataAsset skeletonDataAsset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset));
		}
	}

	public unsafe string animationName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Animation animation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
		}
	}

	public unsafe virtual SkeletonDataAsset SkeletonDataAsset
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
	}

	public unsafe Animation Animation
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788218, XrefRangeEnd = 788219, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Animation_Public_get_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
	}

	static AnimationReferenceAsset()
	{
		Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "AnimationReferenceAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr);
		NativeFieldInfoPtr_QuietSkeletonData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, "QuietSkeletonData");
		NativeFieldInfoPtr_skeletonDataAsset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, "skeletonDataAsset");
		NativeFieldInfoPtr_animationName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, "animationName");
		NativeFieldInfoPtr_animation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, "animation");
		NativeMethodInfoPtr_get_SkeletonDataAsset_Public_Virtual_Final_New_get_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, 100663850);
		NativeMethodInfoPtr_get_Animation_Public_get_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, 100663851);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, 100663852);
		NativeMethodInfoPtr_op_Implicit_Public_Static_Animation_AnimationReferenceAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, 100663853);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr, 100663854);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 788237, RefRangeEnd = 788239, XrefRangeStart = 788219, XrefRangeEnd = 788237, MetadataInitTokenRva = 47221424L, MetadataInitFlagRva = 59827090L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788239, XrefRangeEnd = 788240, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static implicit operator Animation(AnimationReferenceAsset asset)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)asset);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Implicit_Public_Static_Animation_AnimationReferenceAsset_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 18721, RefRangeEnd = 18752, XrefRangeStart = 18721, XrefRangeEnd = 18752, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AnimationReferenceAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationReferenceAsset>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AnimationReferenceAsset(IntPtr pointer)
		: base(pointer)
	{
	}
}
