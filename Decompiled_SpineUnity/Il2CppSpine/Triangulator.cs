using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class Triangulator : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_convexPolygons;

	private static readonly System.IntPtr NativeFieldInfoPtr_convexPolygonsIndices;

	private static readonly System.IntPtr NativeFieldInfoPtr_indicesArray;

	private static readonly System.IntPtr NativeFieldInfoPtr_isConcaveArray;

	private static readonly System.IntPtr NativeFieldInfoPtr_triangles;

	private static readonly System.IntPtr NativeFieldInfoPtr_polygonPool;

	private static readonly System.IntPtr NativeFieldInfoPtr_polygonIndicesPool;

	private static readonly System.IntPtr NativeMethodInfoPtr_Triangulate_Public_ExposedList_1_Int32_ExposedList_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Decompose_Public_ExposedList_1_ExposedList_1_Single_ExposedList_1_Single_ExposedList_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsConcave_Private_Static_Boolean_Int32_Int32_Il2CppStructArray_1_Single_Il2CppStructArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PositiveArea_Private_Static_Boolean_Single_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Winding_Private_Static_Int32_Single_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ExposedList<ExposedList<float>> convexPolygons
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_convexPolygons);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ExposedList<float>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_convexPolygons)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<ExposedList<int>> convexPolygonsIndices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_convexPolygonsIndices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ExposedList<int>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_convexPolygonsIndices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<int> indicesArray
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_indicesArray);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_indicesArray)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<bool> isConcaveArray
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isConcaveArray);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<bool>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isConcaveArray)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<int> triangles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_triangles);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_triangles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Pool<ExposedList<float>> polygonPool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polygonPool);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Pool<ExposedList<float>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polygonPool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pool));
		}
	}

	public unsafe Pool<ExposedList<int>> polygonIndicesPool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polygonIndicesPool);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Pool<ExposedList<int>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polygonIndicesPool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pool));
		}
	}

	static Triangulator()
	{
		Il2CppClassPointerStore<Triangulator>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Triangulator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Triangulator>.NativeClassPtr);
		NativeFieldInfoPtr_convexPolygons = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "convexPolygons");
		NativeFieldInfoPtr_convexPolygonsIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "convexPolygonsIndices");
		NativeFieldInfoPtr_indicesArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "indicesArray");
		NativeFieldInfoPtr_isConcaveArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "isConcaveArray");
		NativeFieldInfoPtr_triangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "triangles");
		NativeFieldInfoPtr_polygonPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "polygonPool");
		NativeFieldInfoPtr_polygonIndicesPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, "polygonIndicesPool");
		NativeMethodInfoPtr_Triangulate_Public_ExposedList_1_Int32_ExposedList_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663840);
		NativeMethodInfoPtr_Decompose_Public_ExposedList_1_ExposedList_1_Single_ExposedList_1_Single_ExposedList_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663841);
		NativeMethodInfoPtr_IsConcave_Private_Static_Boolean_Int32_Int32_Il2CppStructArray_1_Single_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663842);
		NativeMethodInfoPtr_PositiveArea_Private_Static_Boolean_Single_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663843);
		NativeMethodInfoPtr_Winding_Private_Static_Int32_Single_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663844);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Triangulator>.NativeClassPtr, 100663845);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788087, RefRangeEnd = 788088, XrefRangeStart = 788052, XrefRangeEnd = 788087, MetadataInitTokenRva = 47221212L, MetadataInitFlagRva = 59867988L)]
	public unsafe ExposedList<int> Triangulate(ExposedList<float> verticesArray)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verticesArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Triangulate_Public_ExposedList_1_Int32_ExposedList_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<int>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788170, RefRangeEnd = 788171, XrefRangeStart = 788088, XrefRangeEnd = 788170, MetadataInitTokenRva = 47221184L, MetadataInitFlagRva = 59867989L)]
	public unsafe ExposedList<ExposedList<float>> Decompose(ExposedList<float> verticesArray, ExposedList<int> triangles)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verticesArray);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)triangles);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Decompose_Public_ExposedList_1_ExposedList_1_Single_ExposedList_1_Single_ExposedList_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ExposedList<float>>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 788171, RefRangeEnd = 788174, XrefRangeStart = 788171, XrefRangeEnd = 788171, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsConcave(int index, int vertexCount, Il2CppStructArray<float> vertices, Il2CppStructArray<int> indices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&index);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)indices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsConcave_Private_Static_Boolean_Int32_Int32_Il2CppStructArray_1_Single_Il2CppStructArray_1_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool PositiveArea(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&p1x);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1y;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2x;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2y;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &p3x;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &p3y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PositiveArea_Private_Static_Boolean_Single_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static int Winding(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&p1x);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1y;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2x;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2y;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &p3x;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &p3y;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Winding_Private_Static_Int32_Single_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788174, XrefRangeEnd = 788211, MetadataInitTokenRva = 47221284L, MetadataInitFlagRva = 59867990L)]
	public unsafe Triangulator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Triangulator>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Triangulator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
