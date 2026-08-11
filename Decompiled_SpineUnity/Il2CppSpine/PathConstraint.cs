using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class PathConstraint : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_NONE;

	private static readonly System.IntPtr NativeFieldInfoPtr_BEFORE;

	private static readonly System.IntPtr NativeFieldInfoPtr_AFTER;

	private static readonly System.IntPtr NativeFieldInfoPtr_Epsilon;

	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_target;

	private static readonly System.IntPtr NativeFieldInfoPtr_position;

	private static readonly System.IntPtr NativeFieldInfoPtr_spacing;

	private static readonly System.IntPtr NativeFieldInfoPtr_rotateMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_translateMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_active;

	private static readonly System.IntPtr NativeFieldInfoPtr_spaces;

	private static readonly System.IntPtr NativeFieldInfoPtr_positions;

	private static readonly System.IntPtr NativeFieldInfoPtr_world;

	private static readonly System.IntPtr NativeFieldInfoPtr_curves;

	private static readonly System.IntPtr NativeFieldInfoPtr_lengths;

	private static readonly System.IntPtr NativeFieldInfoPtr_segments;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_PathConstraintData_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeWorldPositions_Private_Il2CppStructArray_1_Single_PathAttachment_Int32_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddBeforePosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddAfterPosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddCurvePosition_Private_Static_Void_Single_Single_Single_Single_Single_Single_Single_Single_Single_Il2CppStructArray_1_Single_Int32_Boolean_0;

	public unsafe static int NONE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NONE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NONE, (void*)(&num));
		}
	}

	public unsafe static int BEFORE
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BEFORE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BEFORE, (void*)(&num));
		}
	}

	public unsafe static int AFTER
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AFTER, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AFTER, (void*)(&num));
		}
	}

	public unsafe static float Epsilon
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Epsilon, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Epsilon, (void*)(&num));
		}
	}

	public unsafe PathConstraintData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PathConstraintData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathConstraintData));
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

	public unsafe Slot target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Slot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot));
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

	public unsafe bool active
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active)) = flag;
		}
	}

	public unsafe ExposedList<float> spaces
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spaces);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spaces)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> positions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> world
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_world);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_world)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> curves
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_curves);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_curves)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> lengths
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengths);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengths)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Il2CppStructArray<float> segments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static PathConstraint()
	{
		Il2CppClassPointerStore<PathConstraint>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "PathConstraint");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr);
		NativeFieldInfoPtr_NONE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "NONE");
		NativeFieldInfoPtr_BEFORE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "BEFORE");
		NativeFieldInfoPtr_AFTER = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "AFTER");
		NativeFieldInfoPtr_Epsilon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "Epsilon");
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "data");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "target");
		NativeFieldInfoPtr_position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "position");
		NativeFieldInfoPtr_spacing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "spacing");
		NativeFieldInfoPtr_rotateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "rotateMix");
		NativeFieldInfoPtr_translateMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "translateMix");
		NativeFieldInfoPtr_active = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "active");
		NativeFieldInfoPtr_spaces = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "spaces");
		NativeFieldInfoPtr_positions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "positions");
		NativeFieldInfoPtr_world = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "world");
		NativeFieldInfoPtr_curves = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "curves");
		NativeFieldInfoPtr_lengths = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "lengths");
		NativeFieldInfoPtr_segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, "segments");
		NativeMethodInfoPtr__ctor_Public_Void_PathConstraintData_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663696);
		NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663697);
		NativeMethodInfoPtr_ComputeWorldPositions_Private_Il2CppStructArray_1_Single_PathAttachment_Int32_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663698);
		NativeMethodInfoPtr_AddBeforePosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663699);
		NativeMethodInfoPtr_AddAfterPosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663700);
		NativeMethodInfoPtr_AddCurvePosition_Private_Static_Void_Single_Single_Single_Single_Single_Single_Single_Single_Single_Il2CppStructArray_1_Single_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr, 100663701);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784332, RefRangeEnd = 784333, XrefRangeStart = 784282, XrefRangeEnd = 784332, MetadataInitTokenRva = 47162364L, MetadataInitFlagRva = 59849696L)]
	public unsafe PathConstraint(PathConstraintData data, Skeleton skeleton)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathConstraint>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_PathConstraintData_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784333, XrefRangeEnd = 784380, MetadataInitTokenRva = 47162320L, MetadataInitFlagRva = 59849697L)]
	public unsafe virtual void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784446, RefRangeEnd = 784447, XrefRangeStart = 784380, XrefRangeEnd = 784446, MetadataInitTokenRva = 47162276L, MetadataInitFlagRva = 59849698L)]
	public unsafe Il2CppStructArray<float> ComputeWorldPositions(PathAttachment path, int spacesCount, bool tangents, bool percentPosition, bool percentSpacing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &spacesCount;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tangents;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &percentPosition;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &percentSpacing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWorldPositions_Private_Il2CppStructArray_1_Single_PathAttachment_Int32_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784459, RefRangeEnd = 784461, XrefRangeStart = 784447, XrefRangeEnd = 784459, MetadataInitTokenRva = 47162224L, MetadataInitFlagRva = 59849699L)]
	public unsafe static void AddBeforePosition(float p, Il2CppStructArray<float> temp, int i, Il2CppStructArray<float> output, int o)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&p);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)temp);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &i;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)output);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &o;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBeforePosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784473, RefRangeEnd = 784475, XrefRangeStart = 784461, XrefRangeEnd = 784473, MetadataInitTokenRva = 47162176L, MetadataInitFlagRva = 59849700L)]
	public unsafe static void AddAfterPosition(float p, Il2CppStructArray<float> temp, int i, Il2CppStructArray<float> output, int o)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&p);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)temp);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &i;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)output);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &o;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddAfterPosition_Private_Static_Void_Single_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784480, RefRangeEnd = 784482, XrefRangeStart = 784475, XrefRangeEnd = 784480, MetadataInitTokenRva = 47162256L, MetadataInitFlagRva = 59849701L)]
	public unsafe static void AddCurvePosition(float p, float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x2, float y2, Il2CppStructArray<float> output, int o, bool tangents)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[12];
		*ptr = (nint)(&p);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x1;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &y1;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &cx1;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &cy1;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &cx2;
		*(float**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &cy2;
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &x2;
		*(float**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &y2;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)output);
		*(int**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &o;
		*(bool**)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = &tangents;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddCurvePosition_Private_Static_Void_Single_Single_Single_Single_Single_Single_Single_Single_Single_Il2CppStructArray_1_Single_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PathConstraint(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
