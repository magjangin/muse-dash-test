using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class SkeletonData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_slots;

	private static readonly System.IntPtr NativeFieldInfoPtr_skins;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultSkin;

	private static readonly System.IntPtr NativeFieldInfoPtr_events;

	private static readonly System.IntPtr NativeFieldInfoPtr_animations;

	private static readonly System.IntPtr NativeFieldInfoPtr_ikConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_transformConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_pathConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_x;

	private static readonly System.IntPtr NativeFieldInfoPtr_y;

	private static readonly System.IntPtr NativeFieldInfoPtr_width;

	private static readonly System.IntPtr NativeFieldInfoPtr_height;

	private static readonly System.IntPtr NativeFieldInfoPtr_version;

	private static readonly System.IntPtr NativeFieldInfoPtr_hash;

	private static readonly System.IntPtr NativeFieldInfoPtr_fps;

	private static readonly System.IntPtr NativeFieldInfoPtr_imagesPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_audioPath;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_SlotData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Skins_Public_get_ExposedList_1_Skin_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DefaultSkin_Public_get_Skin_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Animations_Public_get_ExposedList_1_Animation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBone_Public_BoneData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBoneIndex_Public_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindSlot_Public_SlotData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindSlotIndex_Public_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindSkin_Public_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindEvent_Public_EventData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindAnimation_Public_Animation_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindIkConstraint_Public_IkConstraintData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindTransformConstraint_Public_TransformConstraintData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindPathConstraint_Public_PathConstraintData_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindPathConstraintIndex_Public_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ExposedList<BoneData> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<BoneData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<SlotData> slots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<SlotData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Skin> skins
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skins);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Skin>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skins)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Skin defaultSkin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSkin);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSkin)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin));
		}
	}

	public unsafe ExposedList<EventData> events
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_events);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<EventData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_events)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Animation> animations
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animations);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Animation>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<IkConstraintData> ikConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<IkConstraintData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ikConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<TransformConstraintData> transformConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TransformConstraintData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<PathConstraintData> pathConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<PathConstraintData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
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

	public unsafe float width
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width)) = num;
		}
	}

	public unsafe float height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height)) = num;
		}
	}

	public unsafe string version
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_version);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_version)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string hash
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hash);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hash)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float fps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fps);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fps)) = num;
		}
	}

	public unsafe string imagesPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_imagesPath);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_imagesPath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string audioPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_audioPath);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_audioPath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe ExposedList<BoneData> Bones
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<BoneData>>(intPtr) : null;
		}
	}

	public unsafe ExposedList<SlotData> Slots
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 37499, RefRangeEnd = 37502, XrefRangeStart = 37499, XrefRangeEnd = 37502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_SlotData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<SlotData>>(intPtr) : null;
		}
	}

	public unsafe ExposedList<Skin> Skins
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38638, RefRangeEnd = 38645, XrefRangeStart = 38638, XrefRangeEnd = 38645, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Skins_Public_get_ExposedList_1_Skin_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Skin>>(intPtr) : null;
		}
	}

	public unsafe Skin DefaultSkin
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 37367, RefRangeEnd = 37377, XrefRangeStart = 37367, XrefRangeEnd = 37377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DefaultSkin_Public_get_Skin_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
		}
	}

	public unsafe ExposedList<Animation> Animations
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 37591, RefRangeEnd = 37595, XrefRangeStart = 37591, XrefRangeEnd = 37595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Animations_Public_get_ExposedList_1_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Animation>>(intPtr) : null;
		}
	}

	static SkeletonData()
	{
		Il2CppClassPointerStore<SkeletonData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SkeletonData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr);
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "name");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_slots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "slots");
		NativeFieldInfoPtr_skins = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "skins");
		NativeFieldInfoPtr_defaultSkin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "defaultSkin");
		NativeFieldInfoPtr_events = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "events");
		NativeFieldInfoPtr_animations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "animations");
		NativeFieldInfoPtr_ikConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "ikConstraints");
		NativeFieldInfoPtr_transformConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "transformConstraints");
		NativeFieldInfoPtr_pathConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "pathConstraints");
		NativeFieldInfoPtr_x = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "x");
		NativeFieldInfoPtr_y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "y");
		NativeFieldInfoPtr_width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "width");
		NativeFieldInfoPtr_height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "height");
		NativeFieldInfoPtr_version = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "version");
		NativeFieldInfoPtr_hash = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "hash");
		NativeFieldInfoPtr_fps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "fps");
		NativeFieldInfoPtr_imagesPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "imagesPath");
		NativeFieldInfoPtr_audioPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, "audioPath");
		NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663771);
		NativeMethodInfoPtr_get_Slots_Public_get_ExposedList_1_SlotData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663772);
		NativeMethodInfoPtr_get_Skins_Public_get_ExposedList_1_Skin_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663773);
		NativeMethodInfoPtr_get_DefaultSkin_Public_get_Skin_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663774);
		NativeMethodInfoPtr_get_Animations_Public_get_ExposedList_1_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663775);
		NativeMethodInfoPtr_FindBone_Public_BoneData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663776);
		NativeMethodInfoPtr_FindBoneIndex_Public_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663777);
		NativeMethodInfoPtr_FindSlot_Public_SlotData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663778);
		NativeMethodInfoPtr_FindSlotIndex_Public_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663779);
		NativeMethodInfoPtr_FindSkin_Public_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663780);
		NativeMethodInfoPtr_FindEvent_Public_EventData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663781);
		NativeMethodInfoPtr_FindAnimation_Public_Animation_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663782);
		NativeMethodInfoPtr_FindIkConstraint_Public_IkConstraintData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663783);
		NativeMethodInfoPtr_FindTransformConstraint_Public_TransformConstraintData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663784);
		NativeMethodInfoPtr_FindPathConstraint_Public_PathConstraintData_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663785);
		NativeMethodInfoPtr_FindPathConstraintIndex_Public_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663786);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663787);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr, 100663788);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 785897, RefRangeEnd = 785905, XrefRangeStart = 785895, XrefRangeEnd = 785897, MetadataInitTokenRva = 46270372L, MetadataInitFlagRva = 59867940L)]
	public unsafe BoneData FindBone(string boneName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(boneName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindBone_Public_BoneData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BoneData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785905, XrefRangeEnd = 785907, MetadataInitTokenRva = 46270324L, MetadataInitFlagRva = 59867941L)]
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

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785909, RefRangeEnd = 785910, XrefRangeStart = 785907, XrefRangeEnd = 785909, MetadataInitTokenRva = 46270640L, MetadataInitFlagRva = 59867942L)]
	public unsafe SlotData FindSlot(string slotName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(slotName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindSlot_Public_SlotData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SlotData>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 785912, RefRangeEnd = 785916, XrefRangeStart = 785910, XrefRangeEnd = 785912, MetadataInitTokenRva = 46270596L, MetadataInitFlagRva = 59867943L)]
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

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 785926, RefRangeEnd = 785936, XrefRangeStart = 785916, XrefRangeEnd = 785926, MetadataInitTokenRva = 46270572L, MetadataInitFlagRva = 59867944L)]
	public unsafe Skin FindSkin(string skinName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(skinName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindSkin_Public_Skin_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785946, RefRangeEnd = 785948, XrefRangeStart = 785936, XrefRangeEnd = 785946, MetadataInitTokenRva = 46270388L, MetadataInitFlagRva = 59867945L)]
	public unsafe EventData FindEvent(string eventDataName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(eventDataName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindEvent_Public_EventData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 785950, RefRangeEnd = 785958, XrefRangeStart = 785948, XrefRangeEnd = 785950, MetadataInitTokenRva = 46270272L, MetadataInitFlagRva = 59867946L)]
	public unsafe Animation FindAnimation(string animationName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(animationName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindAnimation_Public_Animation_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785960, RefRangeEnd = 785962, XrefRangeStart = 785958, XrefRangeEnd = 785960, MetadataInitTokenRva = 46270460L, MetadataInitFlagRva = 59867947L)]
	public unsafe IkConstraintData FindIkConstraint(string constraintName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(constraintName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindIkConstraint_Public_IkConstraintData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IkConstraintData>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785964, RefRangeEnd = 785966, XrefRangeStart = 785962, XrefRangeEnd = 785964, MetadataInitTokenRva = 46270700L, MetadataInitFlagRva = 59867948L)]
	public unsafe TransformConstraintData FindTransformConstraint(string constraintName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(constraintName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindTransformConstraint_Public_TransformConstraintData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TransformConstraintData>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785968, RefRangeEnd = 785969, XrefRangeStart = 785966, XrefRangeEnd = 785968, MetadataInitTokenRva = 46270504L, MetadataInitFlagRva = 59867949L)]
	public unsafe PathConstraintData FindPathConstraint(string constraintName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(constraintName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindPathConstraint_Public_PathConstraintData_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PathConstraintData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785969, XrefRangeEnd = 785971, MetadataInitTokenRva = 46270488L, MetadataInitFlagRva = 59867950L)]
	public unsafe int FindPathConstraintIndex(string pathConstraintName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(pathConstraintName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindPathConstraintIndex_Public_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785971, XrefRangeEnd = 785972, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 786014, RefRangeEnd = 786016, XrefRangeStart = 785972, XrefRangeEnd = 786014, MetadataInitTokenRva = 46270720L, MetadataInitFlagRva = 59867951L)]
	public unsafe SkeletonData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
