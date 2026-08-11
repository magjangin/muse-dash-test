using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class SkeletonClipping : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_triangulator;

	private static readonly System.IntPtr NativeFieldInfoPtr_clippingPolygon;

	private static readonly System.IntPtr NativeFieldInfoPtr_clipOutput;

	private static readonly System.IntPtr NativeFieldInfoPtr_clippedVertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_clippedTriangles;

	private static readonly System.IntPtr NativeFieldInfoPtr_clippedUVs;

	private static readonly System.IntPtr NativeFieldInfoPtr_scratch;

	private static readonly System.IntPtr NativeFieldInfoPtr_clipAttachment;

	private static readonly System.IntPtr NativeFieldInfoPtr_clippingPolygons;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsClipping_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClipStart_Public_Int32_Slot_ClippingAttachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClipEnd_Public_Void_Slot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClipEnd_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClipTriangles_Public_Void_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clip_Internal_Boolean_Single_Single_Single_Single_Single_Single_ExposedList_1_Single_ExposedList_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeClockwise_Public_Static_Void_ExposedList_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Triangulator triangulator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_triangulator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Triangulator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_triangulator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)triangulator));
		}
	}

	public unsafe ExposedList<float> clippingPolygon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippingPolygon);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippingPolygon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> clipOutput
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipOutput);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipOutput)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> clippedVertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedVertices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedVertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<int> clippedTriangles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedTriangles);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedTriangles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> clippedUVs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedUVs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippedUVs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> scratch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scratch);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scratch)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ClippingAttachment clipAttachment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipAttachment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ClippingAttachment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipAttachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clippingAttachment));
		}
	}

	public unsafe ExposedList<ExposedList<float>> clippingPolygons
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippingPolygons);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ExposedList<float>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clippingPolygons)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe bool IsClipping
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 121248, RefRangeEnd = 121249, XrefRangeStart = 121248, XrefRangeEnd = 121249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsClipping_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonClipping()
	{
		Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SkeletonClipping");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr);
		NativeFieldInfoPtr_triangulator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "triangulator");
		NativeFieldInfoPtr_clippingPolygon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clippingPolygon");
		NativeFieldInfoPtr_clipOutput = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clipOutput");
		NativeFieldInfoPtr_clippedVertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clippedVertices");
		NativeFieldInfoPtr_clippedTriangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clippedTriangles");
		NativeFieldInfoPtr_clippedUVs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clippedUVs");
		NativeFieldInfoPtr_scratch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "scratch");
		NativeFieldInfoPtr_clipAttachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clipAttachment");
		NativeFieldInfoPtr_clippingPolygons = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, "clippingPolygons");
		NativeMethodInfoPtr_get_IsClipping_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663763);
		NativeMethodInfoPtr_ClipStart_Public_Int32_Slot_ClippingAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663764);
		NativeMethodInfoPtr_ClipEnd_Public_Void_Slot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663765);
		NativeMethodInfoPtr_ClipEnd_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663766);
		NativeMethodInfoPtr_ClipTriangles_Public_Void_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663767);
		NativeMethodInfoPtr_Clip_Internal_Boolean_Single_Single_Single_Single_Single_Single_ExposedList_1_Single_ExposedList_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663768);
		NativeMethodInfoPtr_MakeClockwise_Public_Static_Void_ExposedList_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663769);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr, 100663770);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785716, RefRangeEnd = 785718, XrefRangeStart = 785693, XrefRangeEnd = 785716, MetadataInitTokenRva = 46269796L, MetadataInitFlagRva = 59867935L)]
	public unsafe int ClipStart(Slot slot, ClippingAttachment clip)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clip);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClipStart_Public_Int32_Slot_ClippingAttachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785727, RefRangeEnd = 785729, XrefRangeStart = 785718, XrefRangeEnd = 785727, MetadataInitTokenRva = 46269776L, MetadataInitFlagRva = 59867936L)]
	public unsafe void ClipEnd(Slot slot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClipEnd_Public_Void_Slot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785738, RefRangeEnd = 785740, XrefRangeStart = 785729, XrefRangeEnd = 785738, MetadataInitTokenRva = 46269776L, MetadataInitFlagRva = 59867936L)]
	public unsafe void ClipEnd()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClipEnd_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785763, RefRangeEnd = 785764, XrefRangeStart = 785740, XrefRangeEnd = 785763, MetadataInitTokenRva = 46269848L, MetadataInitFlagRva = 59867937L)]
	public unsafe void ClipTriangles(Il2CppStructArray<float> vertices, int verticesLength, Il2CppStructArray<int> triangles, int trianglesLength, Il2CppStructArray<float> uvs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &verticesLength;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)triangles);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &trianglesLength;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uvs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClipTriangles_Public_Void_Il2CppStructArray_1_Single_Int32_Il2CppStructArray_1_Int32_Int32_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785819, RefRangeEnd = 785820, XrefRangeStart = 785764, XrefRangeEnd = 785819, MetadataInitTokenRva = 46269900L, MetadataInitFlagRva = 59867938L)]
	public unsafe bool Clip(float x1, float y1, float x2, float y2, float x3, float y3, ExposedList<float> clippingArea, ExposedList<float> output)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = (nint)(&x1);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y1;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &x2;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &y2;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &x3;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &y3;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clippingArea);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)output);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clip_Internal_Boolean_Single_Single_Single_Single_Single_Single_ExposedList_1_Single_ExposedList_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785820, RefRangeEnd = 785822, XrefRangeStart = 785820, XrefRangeEnd = 785820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeClockwise(ExposedList<float> polygon)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)polygon);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeClockwise_Public_Static_Void_ExposedList_1_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785894, RefRangeEnd = 785895, XrefRangeStart = 785822, XrefRangeEnd = 785894, MetadataInitTokenRva = 46269936L, MetadataInitFlagRva = 59867939L)]
	public unsafe SkeletonClipping()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonClipping>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonClipping(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
