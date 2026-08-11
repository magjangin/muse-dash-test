using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class Skeleton : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_slots;

	private static readonly System.IntPtr NativeFieldInfoPtr_drawOrder;

	private static readonly System.IntPtr NativeFieldInfoPtr_ikConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_transformConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_pathConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateCache;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateCacheReset;

	private static readonly System.IntPtr NativeFieldInfoPtr_skin;

	private static readonly System.IntPtr NativeFieldInfoPtr_r;

	private static readonly System.IntPtr NativeFieldInfoPtr_g;

	private static readonly System.IntPtr NativeFieldInfoPtr_b;

	private static readonly System.IntPtr NativeFieldInfoPtr_a;

	private static readonly System.IntPtr NativeFieldInfoPtr_time;

	private static readonly System.IntPtr NativeFieldInfoPtr_scaleX;

	private static readonly System.IntPtr NativeFieldInfoPtr_scaleY;

	private static readonly System.IntPtr NativeFieldInfoPtr_x;

	private static readonly System.IntPtr NativeFieldInfoPtr_y;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Data_Public_get_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_Slot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IkConstraints_Public_get_ExposedList_1_IkConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TransformConstraints_Public_get_ExposedList_1_TransformConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_R_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_G_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_B_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_A_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_A_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RootBone_Public_get_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateCache_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortIkConstraint_Private_Void_IkConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortPathConstraint_Private_Void_PathConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortTransformConstraint_Private_Void_TransformConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Skin_Int32_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Attachment_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortBone_Private_Void_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SortReset_Private_Static_Void_ExposedList_1_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateWorldTransform_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetToSetupPose_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBonesToSetupPose_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSlotsToSetupPose_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBone_Public_Bone_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBoneIndex_Public_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindSlot_Public_Slot_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindSlotIndex_Public_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSkin_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSkin_Public_Void_Skin_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Void_Single_0;

	public unsafe SkeletonData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData));
		}
	}

	public unsafe ExposedList<Bone> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Bone>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Slot> slots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Slot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Slot> drawOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drawOrder);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Slot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drawOrder)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<IkConstraint> ikConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<IkConstraint>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<TransformConstraint> transformConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TransformConstraint>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<PathConstraint> pathConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<PathConstraint>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<IUpdatable> updateCache
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateCache);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<IUpdatable>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateCache)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Bone> updateCacheReset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateCacheReset);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Bone>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateCacheReset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Skin skin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skin);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skin)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin));
		}
	}

	public unsafe float r
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r)) = num;
		}
	}

	public unsafe float g
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g)) = num;
		}
	}

	public unsafe float b
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b)) = num;
		}
	}

	public unsafe float a
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a)) = num;
		}
	}

	public unsafe float time
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_time);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_time)) = num;
		}
	}

	public unsafe float scaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleX)) = num;
		}
	}

	public unsafe float scaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleY)) = num;
		}
	}

	public unsafe float x
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x)) = num;
		}
	}

	public unsafe float y
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y)) = num;
		}
	}

	public unsafe SkeletonData Data
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Data_Public_get_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
	}

	public unsafe ExposedList<Bone> Bones
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Bone>>(intPtr) : null;
		}
	}

	public unsafe ExposedList<Slot> Slots
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 37499, RefRangeEnd = 37502, XrefRangeStart = 37499, XrefRangeEnd = 37502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_Slot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Slot>>(intPtr) : null;
		}
	}

	public unsafe ExposedList<IkConstraint> IkConstraints
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 37367, RefRangeEnd = 37377, XrefRangeStart = 37367, XrefRangeEnd = 37377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IkConstraints_Public_get_ExposedList_1_IkConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<IkConstraint>>(intPtr) : null;
		}
	}

	public unsafe ExposedList<TransformConstraint> TransformConstraints
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 51248, RefRangeEnd = 51256, XrefRangeStart = 51248, XrefRangeEnd = 51256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TransformConstraints_Public_get_ExposedList_1_TransformConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TransformConstraint>>(intPtr) : null;
		}
	}

	public unsafe float R
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_R_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float G
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_G_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float B
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_B_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float A
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_A_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_A_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float ScaleX
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float ScaleY
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 784502, RefRangeEnd = 784506, XrefRangeStart = 784500, XrefRangeEnd = 784502, MetadataInitTokenRva = 46278544L, MetadataInitFlagRva = 59849712L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Bone RootBone
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 784506, RefRangeEnd = 784510, XrefRangeStart = 784506, XrefRangeEnd = 784506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RootBone_Public_get_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
		}
	}

	static Skeleton()
	{
		Il2CppClassPointerStore<Skeleton>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Skeleton");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Skeleton>.NativeClassPtr);
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "data");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_slots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "slots");
		NativeFieldInfoPtr_drawOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "drawOrder");
		NativeFieldInfoPtr_ikConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "ikConstraints");
		NativeFieldInfoPtr_transformConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "transformConstraints");
		NativeFieldInfoPtr_pathConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "pathConstraints");
		NativeFieldInfoPtr_updateCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "updateCache");
		NativeFieldInfoPtr_updateCacheReset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "updateCacheReset");
		NativeFieldInfoPtr_skin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "skin");
		NativeFieldInfoPtr_r = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "r");
		NativeFieldInfoPtr_g = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "g");
		NativeFieldInfoPtr_b = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "b");
		NativeFieldInfoPtr_a = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "a");
		NativeFieldInfoPtr_time = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "time");
		NativeFieldInfoPtr_scaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "scaleX");
		NativeFieldInfoPtr_scaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "scaleY");
		NativeFieldInfoPtr_x = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "x");
		NativeFieldInfoPtr_y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, "y");
		NativeMethodInfoPtr_get_Data_Public_get_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663704);
		NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663705);
		NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_Slot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663706);
		NativeMethodInfoPtr_get_IkConstraints_Public_get_ExposedList_1_IkConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663707);
		NativeMethodInfoPtr_get_TransformConstraints_Public_get_ExposedList_1_TransformConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663708);
		NativeMethodInfoPtr_set_R_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663709);
		NativeMethodInfoPtr_set_G_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663710);
		NativeMethodInfoPtr_set_B_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663711);
		NativeMethodInfoPtr_get_A_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663712);
		NativeMethodInfoPtr_set_A_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663713);
		NativeMethodInfoPtr_get_ScaleX_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663714);
		NativeMethodInfoPtr_set_ScaleX_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663715);
		NativeMethodInfoPtr_get_ScaleY_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663716);
		NativeMethodInfoPtr_set_ScaleY_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663717);
		NativeMethodInfoPtr_get_RootBone_Public_get_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663718);
		NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663719);
		NativeMethodInfoPtr_UpdateCache_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663720);
		NativeMethodInfoPtr_SortIkConstraint_Private_Void_IkConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663721);
		NativeMethodInfoPtr_SortPathConstraint_Private_Void_PathConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663722);
		NativeMethodInfoPtr_SortTransformConstraint_Private_Void_TransformConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663723);
		NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Skin_Int32_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663724);
		NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Attachment_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663725);
		NativeMethodInfoPtr_SortBone_Private_Void_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663726);
		NativeMethodInfoPtr_SortReset_Private_Static_Void_ExposedList_1_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663727);
		NativeMethodInfoPtr_UpdateWorldTransform_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663728);
		NativeMethodInfoPtr_SetToSetupPose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663729);
		NativeMethodInfoPtr_SetBonesToSetupPose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663730);
		NativeMethodInfoPtr_SetSlotsToSetupPose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663731);
		NativeMethodInfoPtr_FindBone_Public_Bone_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663732);
		NativeMethodInfoPtr_FindBoneIndex_Public_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663733);
		NativeMethodInfoPtr_FindSlot_Public_Slot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663734);
		NativeMethodInfoPtr_FindSlotIndex_Public_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663735);
		NativeMethodInfoPtr_SetSkin_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663736);
		NativeMethodInfoPtr_SetSkin_Public_Void_Skin_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663737);
		NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663738);
		NativeMethodInfoPtr_Update_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skeleton>.NativeClassPtr, 100663739);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784628, RefRangeEnd = 784630, XrefRangeStart = 784510, XrefRangeEnd = 784628, MetadataInitTokenRva = 46278488L, MetadataInitFlagRva = 59849713L)]
	public unsafe Skeleton(SkeletonData data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Skeleton>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 784649, RefRangeEnd = 784652, XrefRangeStart = 784630, XrefRangeEnd = 784649, MetadataInitTokenRva = 46278436L, MetadataInitFlagRva = 59849714L)]
	public unsafe void UpdateCache()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCache_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784674, RefRangeEnd = 784675, XrefRangeStart = 784652, XrefRangeEnd = 784674, MetadataInitTokenRva = 46278216L, MetadataInitFlagRva = 59849715L)]
	public unsafe void SortIkConstraint(IkConstraint constraint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)constraint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortIkConstraint_Private_Void_IkConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784693, RefRangeEnd = 784694, XrefRangeStart = 784675, XrefRangeEnd = 784693, MetadataInitTokenRva = 46278364L, MetadataInitFlagRva = 59849716L)]
	public unsafe void SortPathConstraint(PathConstraint constraint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)constraint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortPathConstraint_Private_Void_PathConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784711, RefRangeEnd = 784712, XrefRangeStart = 784694, XrefRangeEnd = 784711, MetadataInitTokenRva = 46278388L, MetadataInitFlagRva = 59849717L)]
	public unsafe void SortTransformConstraint(TransformConstraint constraint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)constraint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortTransformConstraint_Private_Void_TransformConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784728, RefRangeEnd = 784730, XrefRangeStart = 784712, XrefRangeEnd = 784728, MetadataInitTokenRva = 46278256L, MetadataInitFlagRva = 59849718L)]
	public unsafe void SortPathConstraintAttachment(Skin skin, int slotIndex, Bone slotBone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &slotIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotBone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Skin_Int32_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784736, RefRangeEnd = 784738, XrefRangeStart = 784730, XrefRangeEnd = 784736, MetadataInitTokenRva = 46278300L, MetadataInitFlagRva = 59849719L)]
	public unsafe void SortPathConstraintAttachment(Attachment attachment, Bone slotBone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotBone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortPathConstraintAttachment_Private_Void_Attachment_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 784742, RefRangeEnd = 784750, XrefRangeStart = 784738, XrefRangeEnd = 784742, MetadataInitTokenRva = 46278196L, MetadataInitFlagRva = 59849720L)]
	public unsafe void SortBone(Bone bone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortBone_Private_Void_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784751, RefRangeEnd = 784755, XrefRangeStart = 784750, XrefRangeEnd = 784751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SortReset(ExposedList<Bone> bones)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bones);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SortReset_Private_Static_Void_ExposedList_1_Bone_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(17)]
	[CachedScanResults(RefRangeStart = 784758, RefRangeEnd = 784775, XrefRangeStart = 784755, XrefRangeEnd = 784758, MetadataInitTokenRva = 46278484L, MetadataInitFlagRva = 59849721L)]
	public unsafe void UpdateWorldTransform()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateWorldTransform_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784782, RefRangeEnd = 784783, XrefRangeStart = 784775, XrefRangeEnd = 784782, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetToSetupPose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetToSetupPose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 784783, RefRangeEnd = 784786, XrefRangeStart = 784783, XrefRangeEnd = 784783, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBonesToSetupPose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBonesToSetupPose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784786, XrefRangeEnd = 784792, MetadataInitTokenRva = 46278148L, MetadataInitFlagRva = 59849722L)]
	public unsafe void SetSlotsToSetupPose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSlotsToSetupPose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 784794, RefRangeEnd = 784819, XrefRangeStart = 784792, XrefRangeEnd = 784794, MetadataInitTokenRva = 46277928L, MetadataInitFlagRva = 59849723L)]
	public unsafe Bone FindBone(string boneName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(boneName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindBone_Public_Bone_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784821, RefRangeEnd = 784823, XrefRangeStart = 784819, XrefRangeEnd = 784821, MetadataInitTokenRva = 46277900L, MetadataInitFlagRva = 59849724L)]
	public unsafe int FindBoneIndex(string boneName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(boneName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindBoneIndex_Public_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 784825, RefRangeEnd = 784833, XrefRangeStart = 784823, XrefRangeEnd = 784825, MetadataInitTokenRva = 46278008L, MetadataInitFlagRva = 59849725L)]
	public unsafe Slot FindSlot(string slotName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(slotName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindSlot_Public_Slot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Slot>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784835, RefRangeEnd = 784839, XrefRangeStart = 784833, XrefRangeEnd = 784835, MetadataInitTokenRva = 46277984L, MetadataInitFlagRva = 59849726L)]
	public unsafe int FindSlotIndex(string slotName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(slotName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindSlotIndex_Public_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 784847, RefRangeEnd = 784853, XrefRangeStart = 784839, XrefRangeEnd = 784847, MetadataInitTokenRva = 46278104L, MetadataInitFlagRva = 59849727L)]
	public unsafe void SetSkin(string skinName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(skinName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSkin_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 784859, RefRangeEnd = 784862, XrefRangeStart = 784853, XrefRangeEnd = 784859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetSkin(Skin newSkin)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newSkin);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSkin_Public_Void_Skin_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784865, RefRangeEnd = 784869, XrefRangeStart = 784862, XrefRangeEnd = 784865, MetadataInitTokenRva = 46278060L, MetadataInitFlagRva = 59849728L)]
	public unsafe Attachment GetAttachment(int slotIndex, string attachmentName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&slotIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 784869, RefRangeEnd = 784873, XrefRangeStart = 784869, XrefRangeEnd = 784869, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update(float delta)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&delta);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Skeleton(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
