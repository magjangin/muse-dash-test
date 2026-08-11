using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class BoneFollowerGraphic : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_skeletonGraphic;

	private static readonly IntPtr NativeFieldInfoPtr_initializeOnAwake;

	private static readonly IntPtr NativeFieldInfoPtr_boneName;

	private static readonly IntPtr NativeFieldInfoPtr_followBoneRotation;

	private static readonly IntPtr NativeFieldInfoPtr_followSkeletonFlip;

	private static readonly IntPtr NativeFieldInfoPtr_followLocalScale;

	private static readonly IntPtr NativeFieldInfoPtr_followXYPosition;

	private static readonly IntPtr NativeFieldInfoPtr_followZPosition;

	private static readonly IntPtr NativeFieldInfoPtr_bone;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransform;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransformIsParent;

	private static readonly IntPtr NativeFieldInfoPtr_valid;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonGraphic_Public_get_SkeletonGraphic_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SkeletonGraphic_Public_set_Void_SkeletonGraphic_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetBone_Public_Boolean_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonGraphic skeletonGraphic
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic));
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

	public unsafe SkeletonGraphic SkeletonGraphic
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonGraphic_Public_get_SkeletonGraphic_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788901, XrefRangeEnd = 788903, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SkeletonGraphic_Public_set_Void_SkeletonGraphic_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static BoneFollowerGraphic()
	{
		Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "BoneFollowerGraphic");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonGraphic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "skeletonGraphic");
		NativeFieldInfoPtr_initializeOnAwake = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "initializeOnAwake");
		NativeFieldInfoPtr_boneName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "boneName");
		NativeFieldInfoPtr_followBoneRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "followBoneRotation");
		NativeFieldInfoPtr_followSkeletonFlip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "followSkeletonFlip");
		NativeFieldInfoPtr_followLocalScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "followLocalScale");
		NativeFieldInfoPtr_followXYPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "followXYPosition");
		NativeFieldInfoPtr_followZPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "followZPosition");
		NativeFieldInfoPtr_bone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "bone");
		NativeFieldInfoPtr_skeletonTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "skeletonTransform");
		NativeFieldInfoPtr_skeletonTransformIsParent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "skeletonTransformIsParent");
		NativeFieldInfoPtr_valid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, "valid");
		NativeMethodInfoPtr_get_SkeletonGraphic_Public_get_SkeletonGraphic_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663926);
		NativeMethodInfoPtr_set_SkeletonGraphic_Public_set_Void_SkeletonGraphic_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663927);
		NativeMethodInfoPtr_SetBone_Public_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663928);
		NativeMethodInfoPtr_Awake_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663929);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663930);
		NativeMethodInfoPtr_LateUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663931);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr, 100663932);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788903, XrefRangeEnd = 788907, MetadataInitTokenRva = 46329288L, MetadataInitFlagRva = 59827135L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788907, XrefRangeEnd = 788908, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 788920, RefRangeEnd = 788922, XrefRangeStart = 788908, XrefRangeEnd = 788920, MetadataInitTokenRva = 46329216L, MetadataInitFlagRva = 59827136L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788922, XrefRangeEnd = 788951, MetadataInitTokenRva = 46329260L, MetadataInitFlagRva = 59827137L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788951, XrefRangeEnd = 788952, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BoneFollowerGraphic()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoneFollowerGraphic>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BoneFollowerGraphic(IntPtr pointer)
		: base(pointer)
	{
	}
}
