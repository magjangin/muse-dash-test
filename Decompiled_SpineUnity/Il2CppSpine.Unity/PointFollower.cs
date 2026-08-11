using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class PointFollower : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly IntPtr NativeFieldInfoPtr_slotName;

	private static readonly IntPtr NativeFieldInfoPtr_pointAttachmentName;

	private static readonly IntPtr NativeFieldInfoPtr_followRotation;

	private static readonly IntPtr NativeFieldInfoPtr_followSkeletonFlip;

	private static readonly IntPtr NativeFieldInfoPtr_followSkeletonZPosition;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransform;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonTransformIsParent;

	private static readonly IntPtr NativeFieldInfoPtr_point;

	private static readonly IntPtr NativeFieldInfoPtr_bone;

	private static readonly IntPtr NativeFieldInfoPtr_valid;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonRenderer_Public_Virtual_Final_New_get_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_SkeletonComponent_Public_Virtual_Final_New_get_ISkeletonComponent_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleRebuildRenderer_Private_Void_SkeletonRenderer_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateReferences_Private_Void_0;

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

	public unsafe string slotName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string pointAttachmentName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointAttachmentName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pointAttachmentName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool followRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followRotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followRotation)) = flag;
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

	public unsafe bool followSkeletonZPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followSkeletonZPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_followSkeletonZPosition)) = flag;
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

	public unsafe PointAttachment point
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_point);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PointAttachment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_point)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pointAttachment));
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

	public unsafe virtual SkeletonRenderer SkeletonRenderer
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonRenderer_Public_Virtual_Final_New_get_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
	}

	public unsafe virtual ISkeletonComponent SkeletonComponent
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonComponent_Public_Virtual_Final_New_get_ISkeletonComponent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonComponent>(intPtr) : null;
		}
	}

	public unsafe bool IsValid
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PointFollower()
	{
		Il2CppClassPointerStore<PointFollower>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "PointFollower");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PointFollower>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_slotName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "slotName");
		NativeFieldInfoPtr_pointAttachmentName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "pointAttachmentName");
		NativeFieldInfoPtr_followRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "followRotation");
		NativeFieldInfoPtr_followSkeletonFlip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "followSkeletonFlip");
		NativeFieldInfoPtr_followSkeletonZPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "followSkeletonZPosition");
		NativeFieldInfoPtr_skeletonTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "skeletonTransform");
		NativeFieldInfoPtr_skeletonTransformIsParent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "skeletonTransformIsParent");
		NativeFieldInfoPtr_point = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "point");
		NativeFieldInfoPtr_bone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "bone");
		NativeFieldInfoPtr_valid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, "valid");
		NativeMethodInfoPtr_get_SkeletonRenderer_Public_Virtual_Final_New_get_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663967);
		NativeMethodInfoPtr_get_SkeletonComponent_Public_Virtual_Final_New_get_ISkeletonComponent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663968);
		NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663969);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663970);
		NativeMethodInfoPtr_HandleRebuildRenderer_Private_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663971);
		NativeMethodInfoPtr_UpdateReferences_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663972);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663973);
		NativeMethodInfoPtr_LateUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663974);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PointFollower>.NativeClassPtr, 100663975);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789428, XrefRangeEnd = 789434, MetadataInitTokenRva = 46294768L, MetadataInitFlagRva = 59827198L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46294768L, MetadataInitFlagRva = 59827198L)]
	public unsafe void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRebuildRenderer_Private_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 789459, RefRangeEnd = 789462, XrefRangeStart = 789434, XrefRangeEnd = 789459, MetadataInitTokenRva = 46294892L, MetadataInitFlagRva = 59827199L)]
	public unsafe void UpdateReferences()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateReferences_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789462, XrefRangeEnd = 789471, MetadataInitTokenRva = 46294864L, MetadataInitFlagRva = 59827200L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789471, XrefRangeEnd = 789509, MetadataInitTokenRva = 46294836L, MetadataInitFlagRva = 59827201L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789509, XrefRangeEnd = 789510, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PointFollower()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PointFollower>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PointFollower(IntPtr pointer)
		: base(pointer)
	{
	}
}
