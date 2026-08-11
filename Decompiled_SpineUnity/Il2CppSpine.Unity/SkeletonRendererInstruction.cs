using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity;

public class SkeletonRendererInstruction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_submeshInstructions;

	private static readonly System.IntPtr NativeFieldInfoPtr_immutableTriangles;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasActiveClipping;

	private static readonly System.IntPtr NativeFieldInfoPtr_rawVertexCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachments;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetWithSubset_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Set_Public_Void_SkeletonRendererInstruction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GeometryNotEqual_Public_Static_Boolean_SkeletonRendererInstruction_SkeletonRendererInstruction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ExposedList<SubmeshInstruction> submeshInstructions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshInstructions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<SubmeshInstruction>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_submeshInstructions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe bool immutableTriangles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_immutableTriangles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_immutableTriangles)) = flag;
		}
	}

	public unsafe bool hasActiveClipping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasActiveClipping);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasActiveClipping)) = flag;
		}
	}

	public unsafe int rawVertexCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawVertexCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawVertexCount)) = num;
		}
	}

	public unsafe ExposedList<Attachment> attachments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachments);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Attachment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	static SkeletonRendererInstruction()
	{
		Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRendererInstruction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr);
		NativeFieldInfoPtr_submeshInstructions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, "submeshInstructions");
		NativeFieldInfoPtr_immutableTriangles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, "immutableTriangles");
		NativeFieldInfoPtr_hasActiveClipping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, "hasActiveClipping");
		NativeFieldInfoPtr_rawVertexCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, "rawVertexCount");
		NativeFieldInfoPtr_attachments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, "attachments");
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, 100664375);
		NativeMethodInfoPtr_SetWithSubset_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, 100664376);
		NativeMethodInfoPtr_Set_Public_Void_SkeletonRendererInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, 100664377);
		NativeMethodInfoPtr_GeometryNotEqual_Public_Static_Boolean_SkeletonRendererInstruction_SkeletonRendererInstruction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, 100664378);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr, 100664379);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 793195, RefRangeEnd = 793205, XrefRangeStart = 793189, XrefRangeEnd = 793195, MetadataInitTokenRva = 46274880L, MetadataInitFlagRva = 59848177L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793217, RefRangeEnd = 793218, XrefRangeStart = 793205, XrefRangeEnd = 793217, MetadataInitTokenRva = 46274928L, MetadataInitFlagRva = 59848178L)]
	public unsafe void SetWithSubset(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instructions);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startSubmesh;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &endSubmesh;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetWithSubset_Public_Void_ExposedList_1_SubmeshInstruction_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 793236, RefRangeEnd = 793239, XrefRangeStart = 793218, XrefRangeEnd = 793236, MetadataInitTokenRva = 46274948L, MetadataInitFlagRva = 59848179L)]
	public unsafe void Set(SkeletonRendererInstruction other)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Set_Public_Void_SkeletonRendererInstruction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 793239, RefRangeEnd = 793243, XrefRangeStart = 793239, XrefRangeEnd = 793239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GeometryNotEqual(SkeletonRendererInstruction a, SkeletonRendererInstruction b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GeometryNotEqual_Public_Static_Boolean_SkeletonRendererInstruction_SkeletonRendererInstruction_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 793255, RefRangeEnd = 793260, XrefRangeStart = 793243, XrefRangeEnd = 793255, MetadataInitTokenRva = 46274984L, MetadataInitFlagRva = 59848180L)]
	public unsafe SkeletonRendererInstruction()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRendererInstruction>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRendererInstruction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
