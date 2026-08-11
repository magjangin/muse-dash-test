using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class PathConstraintData : ConstraintData
{
	private static readonly IntPtr NativeFieldInfoPtr_bones;

	private static readonly IntPtr NativeFieldInfoPtr_target;

	private static readonly IntPtr NativeFieldInfoPtr_positionMode;

	private static readonly IntPtr NativeFieldInfoPtr_spacingMode;

	private static readonly IntPtr NativeFieldInfoPtr_rotateMode;

	private static readonly IntPtr NativeFieldInfoPtr_offsetRotation;

	private static readonly IntPtr NativeFieldInfoPtr_position;

	private static readonly IntPtr NativeFieldInfoPtr_spacing;

	private static readonly IntPtr NativeFieldInfoPtr_rotateMix;

	private static readonly IntPtr NativeFieldInfoPtr_translateMix;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0;

	public unsafe ExposedList<BoneData> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<BoneData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe SlotData target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SlotData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotData));
		}
	}

	public unsafe PositionMode positionMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positionMode);
			return *(PositionMode*)num;
		}
		set
		{
			*(PositionMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positionMode)) = positionMode;
		}
	}

	public unsafe SpacingMode spacingMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spacingMode);
			return *(SpacingMode*)num;
		}
		set
		{
			*(SpacingMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spacingMode)) = spacingMode;
		}
	}

	public unsafe RotateMode rotateMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMode);
			return *(RotateMode*)num;
		}
		set
		{
			*(RotateMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMode)) = rotateMode;
		}
	}

	public unsafe float offsetRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetRotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_offsetRotation)) = num;
		}
	}

	public unsafe float position
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_position);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_position)) = num;
		}
	}

	public unsafe float spacing
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spacing);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spacing)) = num;
		}
	}

	public unsafe float rotateMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotateMix)) = num;
		}
	}

	public unsafe float translateMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translateMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translateMix)) = num;
		}
	}

	public unsafe ExposedList<BoneData> Bones
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 37499, RefRangeEnd = 37502, XrefRangeStart = 37499, XrefRangeEnd = 37502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<BoneData>>(intPtr) : null;
		}
	}

	static PathConstraintData()
	{
		Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "PathConstraintData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr);
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "target");
		NativeFieldInfoPtr_positionMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "positionMode");
		NativeFieldInfoPtr_spacingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "spacingMode");
		NativeFieldInfoPtr_rotateMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "rotateMode");
		NativeFieldInfoPtr_offsetRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "offsetRotation");
		NativeFieldInfoPtr_position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "position");
		NativeFieldInfoPtr_spacing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "spacing");
		NativeFieldInfoPtr_rotateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "rotateMix");
		NativeFieldInfoPtr_translateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, "translateMix");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, 100663702);
		NativeMethodInfoPtr_get_Bones_Public_get_ExposedList_1_BoneData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr, 100663703);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784498, RefRangeEnd = 784500, XrefRangeStart = 784482, XrefRangeEnd = 784498, MetadataInitTokenRva = 47162052L, MetadataInitFlagRva = 59849702L)]
	public unsafe PathConstraintData(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathConstraintData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PathConstraintData(IntPtr pointer)
		: base(pointer)
	{
	}
}
