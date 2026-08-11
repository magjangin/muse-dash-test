using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class VertexAttachment : Attachment
{
	private static readonly System.IntPtr NativeFieldInfoPtr_nextID;

	private static readonly System.IntPtr NativeFieldInfoPtr_nextIdLock;

	private static readonly System.IntPtr NativeFieldInfoPtr_id;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_vertices;

	private static readonly System.IntPtr NativeFieldInfoPtr_worldVerticesLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_deformAttachment;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_WorldVerticesLength_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_WorldVerticesLength_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DeformAttachment_Public_get_VertexAttachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_DeformAttachment_Public_set_Void_VertexAttachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Int32_Int32_Il2CppStructArray_1_Single_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTo_Internal_Void_VertexAttachment_0;

	public unsafe static int nextID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_nextID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_nextID, (void*)(&num));
		}
	}

	public unsafe static Il2CppSystem.Object nextIdLock
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_nextIdLock, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_nextIdLock, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe int id
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id)) = num;
		}
	}

	public unsafe Il2CppStructArray<int> bones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<float> vertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int worldVerticesLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_worldVerticesLength);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_worldVerticesLength)) = num;
		}
	}

	public unsafe VertexAttachment deformAttachment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deformAttachment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VertexAttachment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deformAttachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertexAttachment));
		}
	}

	public unsafe Il2CppStructArray<float> Vertices
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38638, RefRangeEnd = 38645, XrefRangeStart = 38638, XrefRangeEnd = 38645, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
	}

	public unsafe int WorldVerticesLength
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 51227, RefRangeEnd = 51228, XrefRangeStart = 51227, XrefRangeEnd = 51228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_WorldVerticesLength_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 51228, RefRangeEnd = 51229, XrefRangeStart = 51228, XrefRangeEnd = 51229, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_WorldVerticesLength_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe VertexAttachment DeformAttachment
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 51248, RefRangeEnd = 51256, XrefRangeStart = 51248, XrefRangeEnd = 51256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DeformAttachment_Public_get_VertexAttachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VertexAttachment>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DeformAttachment_Public_set_Void_VertexAttachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static VertexAttachment()
	{
		Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "VertexAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr);
		NativeFieldInfoPtr_nextID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "nextID");
		NativeFieldInfoPtr_nextIdLock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "nextIdLock");
		NativeFieldInfoPtr_id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "id");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_vertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "vertices");
		NativeFieldInfoPtr_worldVerticesLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "worldVerticesLength");
		NativeFieldInfoPtr_deformAttachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, "deformAttachment");
		NativeMethodInfoPtr_get_Vertices_Public_get_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663589);
		NativeMethodInfoPtr_get_WorldVerticesLength_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663590);
		NativeMethodInfoPtr_set_WorldVerticesLength_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663591);
		NativeMethodInfoPtr_get_DeformAttachment_Public_get_VertexAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663592);
		NativeMethodInfoPtr_set_DeformAttachment_Public_set_Void_VertexAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663593);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663594);
		NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663595);
		NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Int32_Int32_Il2CppStructArray_1_Single_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663596);
		NativeMethodInfoPtr_CopyTo_Internal_Void_VertexAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr, 100663597);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 783588, RefRangeEnd = 783598, XrefRangeStart = 783576, XrefRangeEnd = 783588, MetadataInitTokenRva = 47192152L, MetadataInitFlagRva = 59848243L)]
	public unsafe VertexAttachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VertexAttachment>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783599, RefRangeEnd = 783601, XrefRangeStart = 783598, XrefRangeEnd = 783599, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeWorldVertices(Slot slot, Il2CppStructArray<float> worldVertices)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)worldVertices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 783601, RefRangeEnd = 783611, XrefRangeStart = 783601, XrefRangeEnd = 783601, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ComputeWorldVertices(Slot slot, int start, int count, Il2CppStructArray<float> worldVertices, int offset, int stride = 2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &start;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &count;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)worldVertices);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &offset;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &stride;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeWorldVertices_Public_Void_Slot_Int32_Int32_Il2CppStructArray_1_Single_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 783633, RefRangeEnd = 783637, XrefRangeStart = 783611, XrefRangeEnd = 783633, MetadataInitTokenRva = 47192068L, MetadataInitFlagRva = 59848244L)]
	public unsafe void CopyTo(VertexAttachment attachment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTo_Internal_Void_VertexAttachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VertexAttachment(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
