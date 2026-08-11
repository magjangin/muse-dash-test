using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonUtilityBone : MonoBehaviour
{
	[OriginalName("spine-unity.dll", "", "Mode")]
	public enum Mode
	{
		Follow,
		Override
	}

	[OriginalName("spine-unity.dll", "", "UpdatePhase")]
	public enum UpdatePhase
	{
		Local,
		World,
		Complete
	}

	private static readonly IntPtr NativeFieldInfoPtr_boneName;

	private static readonly IntPtr NativeFieldInfoPtr_parentReference;

	private static readonly IntPtr NativeFieldInfoPtr_mode;

	private static readonly IntPtr NativeFieldInfoPtr_position;

	private static readonly IntPtr NativeFieldInfoPtr_rotation;

	private static readonly IntPtr NativeFieldInfoPtr_scale;

	private static readonly IntPtr NativeFieldInfoPtr_zPosition;

	private static readonly IntPtr NativeFieldInfoPtr_overrideAlpha;

	private static readonly IntPtr NativeFieldInfoPtr_hierarchy;

	private static readonly IntPtr NativeFieldInfoPtr_bone;

	private static readonly IntPtr NativeFieldInfoPtr_transformLerpComplete;

	private static readonly IntPtr NativeFieldInfoPtr_valid;

	private static readonly IntPtr NativeFieldInfoPtr_cachedTransform;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransform;

	private static readonly IntPtr NativeFieldInfoPtr_incompatibleTransformMode;

	private static readonly IntPtr NativeMethodInfoPtr_get_IncompatibleTransformMode_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleOnReset_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoUpdate_Public_Void_UpdatePhase_0;

	private static readonly IntPtr NativeMethodInfoPtr_BoneTransformModeIncompatible_Public_Static_Boolean_Bone_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddBoundingBox_Public_Void_String_String_String_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string boneName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Transform parentReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parentReference);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parentReference)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Mode mode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mode);
			return *(Mode*)num;
		}
		set
		{
			*(Mode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mode)) = mode;
		}
	}

	public unsafe bool position
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_position);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_position)) = flag;
		}
	}

	public unsafe bool rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation)) = flag;
		}
	}

	public unsafe bool scale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scale);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scale)) = flag;
		}
	}

	public unsafe bool zPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_zPosition)) = flag;
		}
	}

	public unsafe float overrideAlpha
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideAlpha);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideAlpha)) = num;
		}
	}

	public unsafe SkeletonUtility hierarchy
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hierarchy);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonUtility>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hierarchy)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonUtility));
		}
	}

	public unsafe Bone bone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bone);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone));
		}
	}

	public unsafe bool transformLerpComplete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformLerpComplete);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformLerpComplete)) = flag;
		}
	}

	public unsafe bool valid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_valid);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_valid)) = flag;
		}
	}

	public unsafe Transform cachedTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform skeletonTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonTransform);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonTransform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool incompatibleTransformMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_incompatibleTransformMode);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_incompatibleTransformMode)) = flag;
		}
	}

	public unsafe bool IncompatibleTransformMode
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IncompatibleTransformMode_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonUtilityBone()
	{
		Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonUtilityBone");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr);
		NativeFieldInfoPtr_boneName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "boneName");
		NativeFieldInfoPtr_parentReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "parentReference");
		NativeFieldInfoPtr_mode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "mode");
		NativeFieldInfoPtr_position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "position");
		NativeFieldInfoPtr_rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "rotation");
		NativeFieldInfoPtr_scale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "scale");
		NativeFieldInfoPtr_zPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "zPosition");
		NativeFieldInfoPtr_overrideAlpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "overrideAlpha");
		NativeFieldInfoPtr_hierarchy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "hierarchy");
		NativeFieldInfoPtr_bone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "bone");
		NativeFieldInfoPtr_transformLerpComplete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "transformLerpComplete");
		NativeFieldInfoPtr_valid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "valid");
		NativeFieldInfoPtr_cachedTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "cachedTransform");
		NativeFieldInfoPtr_skeletonTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "skeletonTransform");
		NativeFieldInfoPtr_incompatibleTransformMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, "incompatibleTransformMode");
		NativeMethodInfoPtr_get_IncompatibleTransformMode_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664309);
		NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664310);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664311);
		NativeMethodInfoPtr_HandleOnReset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664312);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664313);
		NativeMethodInfoPtr_DoUpdate_Public_Void_UpdatePhase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664314);
		NativeMethodInfoPtr_BoneTransformModeIncompatible_Public_Static_Boolean_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664315);
		NativeMethodInfoPtr_AddBoundingBox_Public_Void_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664316);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr, 100664317);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792505, RefRangeEnd = 792507, XrefRangeStart = 792474, XrefRangeEnd = 792505, MetadataInitTokenRva = 46276728L, MetadataInitFlagRva = 59848224L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792507, XrefRangeEnd = 792528, MetadataInitTokenRva = 46276684L, MetadataInitFlagRva = 59848225L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792528, XrefRangeEnd = 792529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleOnReset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleOnReset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792529, XrefRangeEnd = 792541, MetadataInitTokenRva = 46276640L, MetadataInitFlagRva = 59848226L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792565, RefRangeEnd = 792567, XrefRangeStart = 792541, XrefRangeEnd = 792565, MetadataInitTokenRva = 46276600L, MetadataInitFlagRva = 59848227L)]
	public unsafe void DoUpdate(UpdatePhase phase)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&phase);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoUpdate_Public_Void_UpdatePhase_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792567, XrefRangeEnd = 792568, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool BoneTransformModeIncompatible(Bone bone)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BoneTransformModeIncompatible_Public_Static_Boolean_Bone_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792568, XrefRangeEnd = 792582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddBoundingBox(string skinName, string slotName, string attachmentName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(skinName);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(slotName);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBoundingBox_Public_Void_String_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792582, XrefRangeEnd = 792583, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkeletonUtilityBone()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonUtilityBone>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonUtilityBone(IntPtr pointer)
		: base(pointer)
	{
	}
}
