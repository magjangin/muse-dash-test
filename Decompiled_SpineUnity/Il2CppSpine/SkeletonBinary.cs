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

public class SkeletonBinary : Il2CppSystem.Object
{
	public class Vertices : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_bones;

		private static readonly System.IntPtr NativeFieldInfoPtr_vertices;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

		static Vertices()
		{
			Il2CppClassPointerStore<Vertices>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "Vertices");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Vertices>.NativeClassPtr);
			NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertices>.NativeClassPtr, "bones");
			NativeFieldInfoPtr_vertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Vertices>.NativeClassPtr, "vertices");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vertices>.NativeClassPtr, 100663752);
		}

		[CallerCount(2392)]
		[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Vertices()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Vertices>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Vertices(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class SkeletonInput : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_chars;

		private static readonly System.IntPtr NativeFieldInfoPtr_bytesBigEndian;

		private static readonly System.IntPtr NativeFieldInfoPtr_strings;

		private static readonly System.IntPtr NativeFieldInfoPtr_input;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Stream_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadByte_Public_Byte_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadSByte_Public_SByte_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadBoolean_Public_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadFloat_Public_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadInt_Public_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadInt_Public_Int32_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadString_Public_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadStringRef_Public_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReadFully_Public_Void_Il2CppStructArray_1_Byte_Int32_Int32_0;

		public unsafe Il2CppStructArray<byte> chars
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_chars);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_chars)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppStructArray<byte> bytesBigEndian
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bytesBigEndian);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<byte>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bytesBigEndian)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe ExposedList<string> strings
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_strings);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<string>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_strings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
			}
		}

		public unsafe Stream input
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_input);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stream>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_input)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stream));
			}
		}

		static SkeletonInput()
		{
			Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "SkeletonInput");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr);
			NativeFieldInfoPtr_chars = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, "chars");
			NativeFieldInfoPtr_bytesBigEndian = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, "bytesBigEndian");
			NativeFieldInfoPtr_strings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, "strings");
			NativeFieldInfoPtr_input = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, "input");
			NativeMethodInfoPtr__ctor_Public_Void_Stream_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663753);
			NativeMethodInfoPtr_ReadByte_Public_Byte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663754);
			NativeMethodInfoPtr_ReadSByte_Public_SByte_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663755);
			NativeMethodInfoPtr_ReadBoolean_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663756);
			NativeMethodInfoPtr_ReadFloat_Public_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663757);
			NativeMethodInfoPtr_ReadInt_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663758);
			NativeMethodInfoPtr_ReadInt_Public_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663759);
			NativeMethodInfoPtr_ReadString_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663760);
			NativeMethodInfoPtr_ReadStringRef_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663761);
			NativeMethodInfoPtr_ReadFully_Public_Void_Il2CppStructArray_1_Byte_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr, 100663762);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784873, XrefRangeEnd = 784882, MetadataInitTokenRva = 46272760L, MetadataInitFlagRva = 59867930L)]
		public unsafe SkeletonInput(Stream input)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonInput>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Stream_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 784882, XrefRangeEnd = 784883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe byte ReadByte()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadByte_Public_Byte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(byte*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 784884, RefRangeEnd = 784887, XrefRangeStart = 784883, XrefRangeEnd = 784884, MetadataInitTokenRva = 46272664L, MetadataInitFlagRva = 59867931L)]
		public unsafe sbyte ReadSByte()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSByte_Public_SByte_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(sbyte*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe bool ReadBoolean()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadBoolean_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(73)]
		[CachedScanResults(RefRangeStart = 784908, RefRangeEnd = 784981, XrefRangeStart = 784887, XrefRangeEnd = 784908, MetadataInitTokenRva = 46272588L, MetadataInitFlagRva = 59867932L)]
		public unsafe float ReadFloat()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadFloat_Public_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 784981, RefRangeEnd = 784989, XrefRangeStart = 784981, XrefRangeEnd = 784981, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int ReadInt()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadInt_Public_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(82)]
		[CachedScanResults(RefRangeStart = 784989, RefRangeEnd = 785071, XrefRangeStart = 784989, XrefRangeEnd = 784989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int ReadInt(bool optimizePositive)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&optimizePositive);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadInt_Public_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(14)]
		[CachedScanResults(RefRangeStart = 785073, RefRangeEnd = 785087, XrefRangeStart = 785071, XrefRangeEnd = 785073, MetadataInitTokenRva = 46272720L, MetadataInitFlagRva = 59867933L)]
		public unsafe string ReadString()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadString_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 785088, RefRangeEnd = 785093, XrefRangeStart = 785087, XrefRangeEnd = 785088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe string ReadStringRef()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadStringRef_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785093, XrefRangeEnd = 785094, MetadataInitTokenRva = 46272624L, MetadataInitFlagRva = 59867934L)]
		public unsafe void ReadFully(Il2CppStructArray<byte> buffer, int offset, int length)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buffer);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &offset;
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadFully_Public_Void_Il2CppStructArray_1_Byte_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkeletonInput(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Scale_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentLoader;

	private static readonly System.IntPtr NativeFieldInfoPtr_linkedMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_TransformModeValues;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Scale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Scale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_Stream_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadSkin_Private_Skin_SkeletonInput_SkeletonData_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadAttachment_Private_Attachment_SkeletonInput_SkeletonData_Skin_Int32_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadVertices_Private_Vertices_SkeletonInput_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadFloatArray_Private_Il2CppStructArray_1_Single_SkeletonInput_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadShortArray_Private_Il2CppStructArray_1_Int32_SkeletonInput_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadAnimation_Private_Animation_String_SkeletonInput_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadCurve_Private_Void_SkeletonInput_Int32_CurveTimeline_0;

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

	public unsafe List<SkeletonJson.LinkedMesh> linkedMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linkedMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SkeletonJson.LinkedMesh>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_linkedMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe static Il2CppStructArray<TransformMode> TransformModeValues
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TransformModeValues, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<TransformMode>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TransformModeValues, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
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

	static SkeletonBinary()
	{
		Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SkeletonBinary");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr);
		NativeFieldInfoPtr__Scale_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "<Scale>k__BackingField");
		NativeFieldInfoPtr_attachmentLoader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "attachmentLoader");
		NativeFieldInfoPtr_linkedMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "linkedMeshes");
		NativeFieldInfoPtr_TransformModeValues = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, "TransformModeValues");
		NativeMethodInfoPtr_get_Scale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663740);
		NativeMethodInfoPtr_set_Scale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663741);
		NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663742);
		NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_Stream_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663743);
		NativeMethodInfoPtr_ReadSkin_Private_Skin_SkeletonInput_SkeletonData_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663744);
		NativeMethodInfoPtr_ReadAttachment_Private_Attachment_SkeletonInput_SkeletonData_Skin_Int32_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663745);
		NativeMethodInfoPtr_ReadVertices_Private_Vertices_SkeletonInput_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663746);
		NativeMethodInfoPtr_ReadFloatArray_Private_Il2CppStructArray_1_Single_SkeletonInput_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663747);
		NativeMethodInfoPtr_ReadShortArray_Private_Il2CppStructArray_1_Int32_SkeletonInput_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663748);
		NativeMethodInfoPtr_ReadAnimation_Private_Animation_String_SkeletonInput_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663749);
		NativeMethodInfoPtr_ReadCurve_Private_Void_SkeletonInput_Int32_CurveTimeline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr, 100663750);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785102, RefRangeEnd = 785103, XrefRangeStart = 785094, XrefRangeEnd = 785102, MetadataInitTokenRva = 46269716L, MetadataInitFlagRva = 59867921L)]
	public unsafe SkeletonBinary(AttachmentLoader attachmentLoader)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonBinary>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachmentLoader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_AttachmentLoader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785344, RefRangeEnd = 785345, XrefRangeStart = 785103, XrefRangeEnd = 785344, MetadataInitTokenRva = 46269544L, MetadataInitFlagRva = 59867922L)]
	public unsafe SkeletonData ReadSkeletonData(Stream file)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)file);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSkeletonData_Public_SkeletonData_Stream_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 785382, RefRangeEnd = 785384, XrefRangeStart = 785345, XrefRangeEnd = 785382, MetadataInitTokenRva = 46269600L, MetadataInitFlagRva = 59867923L)]
	public unsafe Skin ReadSkin(SkeletonInput input, SkeletonData skeletonData, bool defaultSkin, bool nonessential)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultSkin;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &nonessential;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadSkin_Private_Skin_SkeletonInput_SkeletonData_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785408, RefRangeEnd = 785409, XrefRangeStart = 785384, XrefRangeEnd = 785408, MetadataInitTokenRva = 46269444L, MetadataInitFlagRva = 59867924L)]
	public unsafe Attachment ReadAttachment(SkeletonInput input, SkeletonData skeletonData, Skin skin, int slotIndex, string attachmentName, bool nonessential)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &slotIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &nonessential;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadAttachment_Private_Attachment_SkeletonInput_SkeletonData_Skin_Int32_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785442, RefRangeEnd = 785443, XrefRangeStart = 785409, XrefRangeEnd = 785442, MetadataInitTokenRva = 46269628L, MetadataInitFlagRva = 59867925L)]
	public unsafe Vertices ReadVertices(SkeletonInput input, int vertexCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &vertexCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadVertices_Private_Vertices_SkeletonInput_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Vertices>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785443, XrefRangeEnd = 785447, MetadataInitTokenRva = 46269488L, MetadataInitFlagRva = 59867926L)]
	public unsafe Il2CppStructArray<float> ReadFloatArray(SkeletonInput input, int n, float scale)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &n;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadFloatArray_Private_Il2CppStructArray_1_Single_SkeletonInput_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 785447, XrefRangeEnd = 785451, MetadataInitTokenRva = 46269512L, MetadataInitFlagRva = 59867927L)]
	public unsafe Il2CppStructArray<int> ReadShortArray(SkeletonInput input)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadShortArray_Private_Il2CppStructArray_1_Int32_SkeletonInput_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 785682, RefRangeEnd = 785683, XrefRangeStart = 785451, XrefRangeEnd = 785682, MetadataInitTokenRva = 46269392L, MetadataInitFlagRva = 59867928L)]
	public unsafe Animation ReadAnimation(string name, SkeletonInput input, SkeletonData skeletonData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadAnimation_Private_Animation_String_SkeletonInput_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 785684, RefRangeEnd = 785693, XrefRangeStart = 785683, XrefRangeEnd = 785684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReadCurve(SkeletonInput input, int frameIndex, CurveTimeline timeline)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &frameIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadCurve_Private_Void_SkeletonInput_Int32_CurveTimeline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonBinary(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
