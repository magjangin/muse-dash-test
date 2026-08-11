using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonRootMotion : SkeletonRootMotionBase
{
	private static readonly IntPtr NativeFieldInfoPtr_DefaultAnimationTrackFlags;

	private static readonly IntPtr NativeFieldInfoPtr_animationTrackFlags;

	private static readonly IntPtr NativeFieldInfoPtr_animationState;

	private static readonly IntPtr NativeFieldInfoPtr_canvas;

	private static readonly IntPtr NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Virtual_Vector2_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyMixAlphaToDelta_Private_Void_byref_Vector2_TrackEntry_TrackEntry_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static int DefaultAnimationTrackFlags
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultAnimationTrackFlags, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultAnimationTrackFlags, (void*)(&num));
		}
	}

	public unsafe int animationTrackFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationTrackFlags);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationTrackFlags)) = num;
		}
	}

	public unsafe AnimationState animationState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationState);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationState));
		}
	}

	public unsafe Canvas canvas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvas);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Canvas>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas));
		}
	}

	public unsafe override float AdditionalScale
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789563, XrefRangeEnd = 789568, MetadataInitTokenRva = 46276564L, MetadataInitFlagRva = 59848182L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_get_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonRootMotion()
	{
		Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRootMotion");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultAnimationTrackFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, "DefaultAnimationTrackFlags");
		NativeFieldInfoPtr_animationTrackFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, "animationTrackFlags");
		NativeFieldInfoPtr_animationState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, "animationState");
		NativeFieldInfoPtr_canvas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, "canvas");
		NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663983);
		NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663984);
		NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663985);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663986);
		NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Virtual_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663987);
		NativeMethodInfoPtr_ApplyMixAlphaToDelta_Private_Void_byref_Vector2_TrackEntry_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663988);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr, 100663989);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789559, XrefRangeEnd = 789563, MetadataInitTokenRva = 46276496L, MetadataInitFlagRva = 59848181L)]
	public unsafe override Vector2 GetRemainingRootMotion(int trackIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&trackIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789568, XrefRangeEnd = 789583, MetadataInitTokenRva = 46276532L, MetadataInitFlagRva = 59848183L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789583, XrefRangeEnd = 789602, MetadataInitTokenRva = 46276440L, MetadataInitFlagRva = 59848184L)]
	public unsafe override Vector2 CalculateAnimationsMovementDelta()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Virtual_Vector2_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789602, XrefRangeEnd = 789609, MetadataInitTokenRva = 46276380L, MetadataInitFlagRva = 59848185L)]
	public unsafe void ApplyMixAlphaToDelta(ref Vector2 currentDelta, TrackEntry next, TrackEntry track)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)Unsafe.AsPointer(ref currentDelta);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)next);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)track);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyMixAlphaToDelta_Private_Void_byref_Vector2_TrackEntry_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkeletonRootMotion()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRootMotion>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRootMotion(IntPtr pointer)
		: base(pointer)
	{
	}
}
