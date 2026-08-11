using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonMecanimRootMotion : SkeletonRootMotionBase
{
	private static readonly IntPtr NativeFieldInfoPtr_DefaultMecanimLayerFlags;

	private static readonly IntPtr NativeFieldInfoPtr_mecanimLayerFlags;

	private static readonly IntPtr NativeFieldInfoPtr_movementDelta;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonMecanim;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonMecanim_Public_get_SkeletonMecanim_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnClipApplied_Private_Void_Animation_Int32_Single_Single_Single_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Virtual_Vector2_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static int DefaultMecanimLayerFlags
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultMecanimLayerFlags, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultMecanimLayerFlags, (void*)(&num));
		}
	}

	public unsafe int mecanimLayerFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mecanimLayerFlags);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mecanimLayerFlags)) = num;
		}
	}

	public unsafe Vector2 movementDelta
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_movementDelta);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_movementDelta)) = vector;
		}
	}

	public unsafe SkeletonMecanim skeletonMecanim
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonMecanim);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonMecanim>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonMecanim)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonMecanim));
		}
	}

	public unsafe SkeletonMecanim SkeletonMecanim
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789510, XrefRangeEnd = 789514, MetadataInitTokenRva = 46273500L, MetadataInitFlagRva = 59848128L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonMecanim_Public_get_SkeletonMecanim_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonMecanim>(intPtr) : null;
		}
	}

	static SkeletonMecanimRootMotion()
	{
		Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonMecanimRootMotion");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultMecanimLayerFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, "DefaultMecanimLayerFlags");
		NativeFieldInfoPtr_mecanimLayerFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, "mecanimLayerFlags");
		NativeFieldInfoPtr_movementDelta = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, "movementDelta");
		NativeFieldInfoPtr_skeletonMecanim = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, "skeletonMecanim");
		NativeMethodInfoPtr_get_SkeletonMecanim_Public_get_SkeletonMecanim_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663976);
		NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663977);
		NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663978);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663979);
		NativeMethodInfoPtr_OnClipApplied_Private_Void_Animation_Int32_Single_Single_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663980);
		NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Virtual_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663981);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr, 100663982);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789514, XrefRangeEnd = 789517, MetadataInitTokenRva = 46273364L, MetadataInitFlagRva = 59848129L)]
	public unsafe override Vector2 GetRemainingRootMotion(int layerIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&layerIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetRemainingRootMotion_Public_Virtual_Vector2_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789517, XrefRangeEnd = 789518, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789518, XrefRangeEnd = 789542, MetadataInitTokenRva = 46273440L, MetadataInitFlagRva = 59848130L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789542, XrefRangeEnd = 789554, MetadataInitTokenRva = 46273416L, MetadataInitFlagRva = 59848131L)]
	public unsafe void OnClipApplied(Animation animation, int layerIndex, float weight, float time, float lastTime, bool playsBackward)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &layerIndex;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &weight;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &lastTime;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &playsBackward;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnClipApplied_Private_Void_Animation_Int32_Single_Single_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789554, XrefRangeEnd = 789558, MetadataInitTokenRva = 46273328L, MetadataInitFlagRva = 59848132L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789558, XrefRangeEnd = 789559, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkeletonMecanimRootMotion()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonMecanimRootMotion>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonMecanimRootMotion(IntPtr pointer)
		: base(pointer)
	{
	}
}
