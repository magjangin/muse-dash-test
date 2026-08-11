using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class FollowSkeletonUtilityRootRotation : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_FLIP_ANGLE_THRESHOLD;

	private static readonly IntPtr NativeFieldInfoPtr_reference;

	private static readonly IntPtr NativeFieldInfoPtr_prevLocalEulerAngles;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CompensatePositionToYRotation_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CompensatePositionToXRotation_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float FLIP_ANGLE_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FLIP_ANGLE_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FLIP_ANGLE_THRESHOLD, (void*)(&num));
		}
	}

	public unsafe Transform reference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reference);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reference)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Vector3 prevLocalEulerAngles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prevLocalEulerAngles);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_prevLocalEulerAngles)) = vector;
		}
	}

	static FollowSkeletonUtilityRootRotation()
	{
		Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "FollowSkeletonUtilityRootRotation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr);
		NativeFieldInfoPtr_FLIP_ANGLE_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, "FLIP_ANGLE_THRESHOLD");
		NativeFieldInfoPtr_reference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, "reference");
		NativeFieldInfoPtr_prevLocalEulerAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, "prevLocalEulerAngles");
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, 100664266);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, 100664267);
		NativeMethodInfoPtr_CompensatePositionToYRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, 100664268);
		NativeMethodInfoPtr_CompensatePositionToXRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, 100664269);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr, 100664270);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791992, XrefRangeEnd = 791994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791994, XrefRangeEnd = 792032, MetadataInitTokenRva = 46442864L, MetadataInitFlagRva = 59827168L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792032, XrefRangeEnd = 792045, MetadataInitTokenRva = 46442820L, MetadataInitFlagRva = 59827169L)]
	public unsafe void CompensatePositionToYRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompensatePositionToYRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792045, XrefRangeEnd = 792058, MetadataInitTokenRva = 46442792L, MetadataInitFlagRva = 59827170L)]
	public unsafe void CompensatePositionToXRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompensatePositionToXRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(91)]
	[CachedScanResults(RefRangeStart = 37408, RefRangeEnd = 37499, XrefRangeStart = 37408, XrefRangeEnd = 37499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FollowSkeletonUtilityRootRotation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FollowSkeletonUtilityRootRotation>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FollowSkeletonUtilityRootRotation(IntPtr pointer)
		: base(pointer)
	{
	}
}
