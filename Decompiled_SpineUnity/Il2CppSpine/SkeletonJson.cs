using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;

namespace Il2CppSpine;

public class SkeletonJson : Il2CppSystem.Object
{
	public class LinkedMesh : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_parent;

		private static readonly System.IntPtr NativeFieldInfoPtr_skin;

		private static readonly System.IntPtr NativeFieldInfoPtr_slotIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_mesh;

		private static readonly System.IntPtr NativeFieldInfoPtr_inheritDeform;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_MeshAttachment_String_Int32_String_Boolean_0;

		public unsafe string parent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string skin
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skin);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skin)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe int slotIndex
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotIndex);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotIndex)) = num;
			}
		}

		public unsafe MeshAttachment mesh
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshAttachment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshAttachment));
			}
		}

		public unsafe bool inheritDeform
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inheritDeform);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inheritDeform)) = flag;
			}
		}

		static LinkedMesh()
		{
			Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, "LinkedMesh");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr);
			NativeFieldInfoPtr_parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, "parent");
			NativeFieldInfoPtr_skin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, "skin");
			NativeFieldInfoPtr_slotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, "slotIndex");
			NativeFieldInfoPtr_mesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, "mesh");
			NativeFieldInfoPtr_inheritDeform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, "inheritDeform");
			NativeMethodInfoPtr__ctor_Public_Void_MeshAttachment_String_Int32_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr, 100663804);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 786016, XrefRangeEnd = 786020, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LinkedMesh(MeshAttachment mesh, string skin, int slotIndex, string parent, bool inheritDeform)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LinkedMesh>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[5];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(skin);
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &slotIndex;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(parent);
			*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &inheritDeform;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MeshAttachment_String_Int32_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public LinkedMesh(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Scale_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentLoader;

	private static readonly System.IntPtr NativeFieldInfoPtr_linkedMeshes;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Scale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Scale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_TextReader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadAttachment_Private_Attachment_Dictionary_2_String_Object_Skin_Int32_String_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadVertices_Private_Void_Dictionary_2_String_Object_VertexAttachment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadAnimation_Private_Void_Dictionary_2_String_Object_String_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadCurve_Private_Static_Void_Dictionary_2_String_Object_CurveTimeline_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFloatArray_Private_Static_Il2CppStructArray_1_Single_Dictionary_2_String_Object_String_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetIntArray_Private_Static_Il2CppStructArray_1_Int32_Dictionary_2_String_Object_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFloat_Private_Static_Single_Dictionary_2_String_Object_String_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInt_Private_Static_Int32_Dictionary_2_String_Object_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBoolean_Private_Static_Boolean_Dictionary_2_String_Object_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetString_Private_Static_String_Dictionary_2_String_Object_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToColor_Private_Static_Single_String_Int32_Int32_0;

	public unsafe float _Scale_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Scale_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Scale_k__BackingField)) = num;
		}
	}

	public unsafe AttachmentLoader attachmentLoader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentLoader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AttachmentLoader>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentLoader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachmentLoader));
		}
	}

	public unsafe List<LinkedMesh> linkedMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linkedMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<LinkedMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linkedMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float Scale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Scale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 348308, RefRangeEnd = 348309, XrefRangeStart = 348308, XrefRangeEnd = 348309, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Scale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SkeletonJson()
	{
		Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SkeletonJson");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr);
		NativeFieldInfoPtr__Scale_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, "<Scale>k__BackingField");
		NativeFieldInfoPtr_attachmentLoader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, "attachmentLoader");
		NativeFieldInfoPtr_linkedMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, "linkedMeshes");
		NativeMethodInfoPtr_get_Scale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663789);
		NativeMethodInfoPtr_set_Scale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663790);
		NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663791);
		NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_TextReader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663792);
		NativeMethodInfoPtr_ReadAttachment_Private_Attachment_Dictionary_2_String_Object_Skin_Int32_String_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663793);
		NativeMethodInfoPtr_ReadVertices_Private_Void_Dictionary_2_String_Object_VertexAttachment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663794);
		NativeMethodInfoPtr_ReadAnimation_Private_Void_Dictionary_2_String_Object_String_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663795);
		NativeMethodInfoPtr_ReadCurve_Private_Static_Void_Dictionary_2_String_Object_CurveTimeline_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663796);
		NativeMethodInfoPtr_GetFloatArray_Private_Static_Il2CppStructArray_1_Single_Dictionary_2_String_Object_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663797);
		NativeMethodInfoPtr_GetIntArray_Private_Static_Il2CppStructArray_1_Int32_Dictionary_2_String_Object_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663798);
		NativeMethodInfoPtr_GetFloat_Private_Static_Single_Dictionary_2_String_Object_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663799);
		NativeMethodInfoPtr_GetInt_Private_Static_Int32_Dictionary_2_String_Object_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663800);
		NativeMethodInfoPtr_GetBoolean_Private_Static_Boolean_Dictionary_2_String_Object_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663801);
		NativeMethodInfoPtr_GetString_Private_Static_String_Dictionary_2_String_Object_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663802);
		NativeMethodInfoPtr_ToColor_Private_Static_Single_String_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr, 100663803);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 786028, RefRangeEnd = 786030, XrefRangeStart = 786020, XrefRangeEnd = 786028, MetadataInitTokenRva = 46273284L, MetadataInitFlagRva = 59867952L)]
	public unsafe SkeletonJson(AttachmentLoader attachmentLoader)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonJson>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachmentLoader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 786729, RefRangeEnd = 786731, XrefRangeStart = 786030, XrefRangeEnd = 786729, MetadataInitTokenRva = 46273152L, MetadataInitFlagRva = 59867953L)]
	public unsafe SkeletonData ReadSkeletonData(TextReader reader)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_TextReader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 786800, RefRangeEnd = 786801, XrefRangeStart = 786731, XrefRangeEnd = 786800, MetadataInitTokenRva = 46273096L, MetadataInitFlagRva = 59867954L)]
	public unsafe Attachment ReadAttachment(Dictionary<string, Il2CppSystem.Object> map, Skin skin, int slotIndex, string name, SkeletonData skeletonData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &slotIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadAttachment_Private_Attachment_Dictionary_2_String_Object_Skin_Int32_String_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 786843, RefRangeEnd = 786844, XrefRangeStart = 786801, XrefRangeEnd = 786843, MetadataInitTokenRva = 46273200L, MetadataInitFlagRva = 59867955L)]
	public unsafe void ReadVertices(Dictionary<string, Il2CppSystem.Object> map, VertexAttachment attachment, int verticesLength)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &verticesLength;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadVertices_Private_Void_Dictionary_2_String_Object_VertexAttachment_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787534, RefRangeEnd = 787535, XrefRangeStart = 786844, XrefRangeEnd = 787534, MetadataInitTokenRva = 46273048L, MetadataInitFlagRva = 59867956L)]
	public unsafe void ReadAnimation(Dictionary<string, Il2CppSystem.Object> map, string name, SkeletonData skeletonData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadAnimation_Private_Void_Dictionary_2_String_Object_String_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 787544, RefRangeEnd = 787553, XrefRangeStart = 787535, XrefRangeEnd = 787544, MetadataInitTokenRva = 46273112L, MetadataInitFlagRva = 59867957L)]
	public unsafe static void ReadCurve(Dictionary<string, Il2CppSystem.Object> valueMap, CurveTimeline timeline, int frameIndex)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueMap);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &frameIndex;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadCurve_Private_Static_Void_Dictionary_2_String_Object_CurveTimeline_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 787564, RefRangeEnd = 787566, XrefRangeStart = 787553, XrefRangeEnd = 787564, MetadataInitTokenRva = 46272840L, MetadataInitFlagRva = 59867958L)]
	public unsafe static Il2CppStructArray<float> GetFloatArray(Dictionary<string, Il2CppSystem.Object> map, string name, float scale)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFloatArray_Private_Static_Il2CppStructArray_1_Single_Dictionary_2_String_Object_String_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 787566, XrefRangeEnd = 787577, MetadataInitTokenRva = 46272928L, MetadataInitFlagRva = 59867959L)]
	public unsafe static Il2CppStructArray<int> GetIntArray(Dictionary<string, Il2CppSystem.Object> map, string name)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIntArray_Private_Static_Il2CppStructArray_1_Int32_Dictionary_2_String_Object_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
	}

	[CallerCount(67)]
	[CachedScanResults(RefRangeStart = 787584, RefRangeEnd = 787651, XrefRangeStart = 787577, XrefRangeEnd = 787584, MetadataInitTokenRva = 46272900L, MetadataInitFlagRva = 59867960L)]
	public unsafe static float GetFloat(Dictionary<string, Il2CppSystem.Object> map, string name, float defaultValue)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultValue;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFloat_Private_Static_Single_Dictionary_2_String_Object_String_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 787658, RefRangeEnd = 787665, XrefRangeStart = 787651, XrefRangeEnd = 787658, MetadataInitTokenRva = 46272944L, MetadataInitFlagRva = 59867961L)]
	public unsafe static int GetInt(Dictionary<string, Il2CppSystem.Object> map, string name, int defaultValue)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultValue;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInt_Private_Static_Int32_Dictionary_2_String_Object_String_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 787672, RefRangeEnd = 787685, XrefRangeStart = 787665, XrefRangeEnd = 787672, MetadataInitTokenRva = 46272816L, MetadataInitFlagRva = 59867962L)]
	public unsafe static bool GetBoolean(Dictionary<string, Il2CppSystem.Object> map, string name, bool defaultValue)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultValue;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBoolean_Private_Static_Boolean_Dictionary_2_String_Object_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 787691, RefRangeEnd = 787699, XrefRangeStart = 787685, XrefRangeEnd = 787691, MetadataInitTokenRva = 46273004L, MetadataInitFlagRva = 59867963L)]
	public unsafe static string GetString(Dictionary<string, Il2CppSystem.Object> map, string name, string defaultValue)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)map);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultValue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetString_Private_Static_String_Dictionary_2_String_Object_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(22)]
	[CachedScanResults(RefRangeStart = 787704, RefRangeEnd = 787726, XrefRangeStart = 787699, XrefRangeEnd = 787704, MetadataInitTokenRva = 46273244L, MetadataInitFlagRva = 59867964L)]
	public unsafe static float ToColor(string hexString, int colorIndex, int expectedLength = 8)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(hexString);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &colorIndex;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &expectedLength;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToColor_Private_Static_Single_String_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public SkeletonJson(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
