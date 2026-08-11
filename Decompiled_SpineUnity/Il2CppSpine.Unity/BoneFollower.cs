using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class BoneFollower : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_boneName;

	private static readonly IntPtr NativeFieldInfoPtr_followXYPosition;

	private static readonly IntPtr NativeFieldInfoPtr_followZPosition;

	private static readonly IntPtr NativeFieldInfoPtr_followBoneRotation;

	private static readonly IntPtr NativeFieldInfoPtr_followSkeletonFlip;

	private static readonly IntPtr NativeFieldInfoPtr_followLocalScale;

	private static readonly IntPtr NativeFieldInfoPtr_initializeOnAwake;

	private static readonly IntPtr NativeFieldInfoPtr_valid;

	private static readonly IntPtr NativeFieldInfoPtr_bone;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransform;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransformIsParent;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetBone_Public_Boolean_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleRebuildRenderer_Public_Void_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonRenderer skeletonRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer));
		}
	}

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

	public unsafe bool followXYPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followXYPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followXYPosition)) = flag;
		}
	}

	public unsafe bool followZPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followZPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followZPosition)) = flag;
		}
	}

	public unsafe bool followBoneRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followBoneRotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followBoneRotation)) = flag;
		}
	}

	public unsafe bool followSkeletonFlip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followSkeletonFlip);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followSkeletonFlip)) = flag;
		}
	}

	public unsafe bool followLocalScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followLocalScale);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followLocalScale)) = flag;
		}
	}

	public unsafe bool initializeOnAwake
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initializeOnAwake);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_initializeOnAwake)) = flag;
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

	public unsafe bool skeletonTransformIsParent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonTransformIsParent);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonTransformIsParent)) = flag;
		}
	}

	public unsafe SkeletonRenderer SkeletonRenderer
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 788810, RefRangeEnd = 788812, XrefRangeStart = 788808, XrefRangeEnd = 788810, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static BoneFollower()
	{
		Il2CppClassPointerStore<BoneFollower>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "BoneFollower");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_boneName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "boneName");
		NativeFieldInfoPtr_followXYPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "followXYPosition");
		NativeFieldInfoPtr_followZPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "followZPosition");
		NativeFieldInfoPtr_followBoneRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "followBoneRotation");
		NativeFieldInfoPtr_followSkeletonFlip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "followSkeletonFlip");
		NativeFieldInfoPtr_followLocalScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "followLocalScale");
		NativeFieldInfoPtr_initializeOnAwake = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "initializeOnAwake");
		NativeFieldInfoPtr_valid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "valid");
		NativeFieldInfoPtr_bone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "bone");
		NativeFieldInfoPtr_skeletonTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "skeletonTransform");
		NativeFieldInfoPtr_skeletonTransformIsParent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, "skeletonTransformIsParent");
		NativeMethodInfoPtr_get_SkeletonRenderer_Public_get_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663917);
		NativeMethodInfoPtr_set_SkeletonRenderer_Public_set_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663918);
		NativeMethodInfoPtr_SetBone_Public_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663919);
		NativeMethodInfoPtr_Awake_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663920);
		NativeMethodInfoPtr_HandleRebuildRenderer_Public_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663921);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663922);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663923);
		NativeMethodInfoPtr_LateUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663924);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr, 100663925);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 788816, RefRangeEnd = 788823, XrefRangeStart = 788812, XrefRangeEnd = 788816, MetadataInitTokenRva = 46329476L, MetadataInitFlagRva = 59827131L)]
	public unsafe bool SetBone(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBone_Public_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788823, XrefRangeEnd = 788824, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788824, XrefRangeEnd = 788825, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRebuildRenderer_Public_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 788847, RefRangeEnd = 788857, XrefRangeStart = 788825, XrefRangeEnd = 788847, MetadataInitTokenRva = 46329348L, MetadataInitFlagRva = 59827132L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788857, XrefRangeEnd = 788866, MetadataInitTokenRva = 46329420L, MetadataInitFlagRva = 59827133L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 788893, RefRangeEnd = 788900, XrefRangeStart = 788866, XrefRangeEnd = 788893, MetadataInitTokenRva = 46329368L, MetadataInitFlagRva = 59827134L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788900, XrefRangeEnd = 788901, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BoneFollower()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoneFollower>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BoneFollower(IntPtr pointer)
		: base(pointer)
	{
	}
}
