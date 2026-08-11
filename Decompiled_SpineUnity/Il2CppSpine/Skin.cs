using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSpine.Collections;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class Skin : Il2CppSystem.Object
{
	public sealed class SkinEntry : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_slotIndex;

		private static readonly System.IntPtr NativeFieldInfoPtr_name;

		private static readonly System.IntPtr NativeFieldInfoPtr_attachment;

		private static readonly System.IntPtr NativeFieldInfoPtr_hashCode;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_String_Attachment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SlotIndex_Public_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0;

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

		public unsafe Attachment attachment
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment));
			}
		}

		public unsafe int hashCode
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hashCode);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hashCode)) = num;
			}
		}

		public unsafe int SlotIndex
		{
			[CallerCount(196)]
			[CachedScanResults(RefRangeStart = 197020, RefRangeEnd = 197216, XrefRangeStart = 197020, XrefRangeEnd = 197216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SlotIndex_Public_get_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe string Name
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
		}

		public unsafe Attachment Attachment
		{
			[CallerCount(11)]
			[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
			}
		}

		static SkinEntry()
		{
			Il2CppClassPointerStore<SkinEntry>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Skin>.NativeClassPtr, "SkinEntry");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr);
			NativeFieldInfoPtr_slotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, "slotIndex");
			NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, "name");
			NativeFieldInfoPtr_attachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, "attachment");
			NativeFieldInfoPtr_hashCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, "hashCode");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_String_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, 100663812);
			NativeMethodInfoPtr_get_SlotIndex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, 100663813);
			NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, 100663814);
			NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr, 100663815);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 787726, XrefRangeEnd = 787728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkinEntry(int slotIndex, string name, Attachment attachment)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&slotIndex);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_String_Attachment_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkinEntry(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public SkinEntry()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkinEntry>.NativeClassPtr))
		{
		}
	}

	public class SkinEntryComparer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Instance;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__Equals_Private_Virtual_Final_New_Boolean_SkinEntry_SkinEntry_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__GetHashCode_Private_Virtual_Final_New_Int32_SkinEntry_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe static SkinEntryComparer Instance
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Instance, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkinEntryComparer>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skinEntryComparer));
			}
		}

		static SkinEntryComparer()
		{
			Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<Skin>.NativeClassPtr, "SkinEntryComparer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr);
			NativeFieldInfoPtr_Instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr, "Instance");
			NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__Equals_Private_Virtual_Final_New_Boolean_SkinEntry_SkinEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr, 100663816);
			NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__GetHashCode_Private_Virtual_Final_New_Int32_SkinEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr, 100663817);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr, 100663818);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 787728, XrefRangeEnd = 787729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__Equals(SkinEntry e1, SkinEntry e2)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)e1));
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)e2));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__Equals_Private_Virtual_Final_New_Boolean_SkinEntry_SkinEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe virtual int System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__GetHashCode(SkinEntry e)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)e));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_Skin_SkinEntry__GetHashCode_Private_Virtual_Final_New_Int32_SkinEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(2392)]
		[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkinEntryComparer()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkinEntryComparer>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkinEntryComparer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachments;

	private static readonly System.IntPtr NativeFieldInfoPtr_bones;

	private static readonly System.IntPtr NativeFieldInfoPtr_constraints;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Attachments_Public_get_OrderedDictionary_2_SkinEntry_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAttachment_Public_Void_Int32_String_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAttachments_Public_Void_Int32_List_1_SkinEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttachAll_Internal_Void_Skeleton_Skin_0;

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

	public unsafe OrderedDictionary<SkinEntry, Attachment> attachments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachments);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OrderedDictionary<SkinEntry, Attachment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)orderedDictionary));
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

	public unsafe ExposedList<ConstraintData> constraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constraints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<ConstraintData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constraints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe OrderedDictionary<SkinEntry, Attachment> Attachments
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Attachments_Public_get_OrderedDictionary_2_SkinEntry_Attachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OrderedDictionary<SkinEntry, Attachment>>(intPtr) : null;
		}
	}

	static Skin()
	{
		Il2CppClassPointerStore<Skin>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Skin");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Skin>.NativeClassPtr);
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skin>.NativeClassPtr, "name");
		NativeFieldInfoPtr_attachments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skin>.NativeClassPtr, "attachments");
		NativeFieldInfoPtr_bones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skin>.NativeClassPtr, "bones");
		NativeFieldInfoPtr_constraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Skin>.NativeClassPtr, "constraints");
		NativeMethodInfoPtr_get_Attachments_Public_get_OrderedDictionary_2_SkinEntry_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663805);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663806);
		NativeMethodInfoPtr_SetAttachment_Public_Void_Int32_String_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663807);
		NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663808);
		NativeMethodInfoPtr_GetAttachments_Public_Void_Int32_List_1_SkinEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663809);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663810);
		NativeMethodInfoPtr_AttachAll_Internal_Void_Skeleton_Skin_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Skin>.NativeClassPtr, 100663811);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 787757, RefRangeEnd = 787761, XrefRangeStart = 787729, XrefRangeEnd = 787757, MetadataInitTokenRva = 46283048L, MetadataInitFlagRva = 59867965L)]
	public unsafe Skin(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Skin>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 787766, RefRangeEnd = 787770, XrefRangeStart = 787761, XrefRangeEnd = 787766, MetadataInitTokenRva = 46283008L, MetadataInitFlagRva = 59867966L)]
	public unsafe void SetAttachment(int slotIndex, string name, Attachment attachment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&slotIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAttachment_Public_Void_Int32_String_Attachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(21)]
	[CachedScanResults(RefRangeStart = 787775, RefRangeEnd = 787796, XrefRangeStart = 787770, XrefRangeEnd = 787775, MetadataInitTokenRva = 46282940L, MetadataInitFlagRva = 59867967L)]
	public unsafe Attachment GetAttachment(int slotIndex, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&slotIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAttachment_Public_Attachment_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 787812, RefRangeEnd = 787815, XrefRangeStart = 787796, XrefRangeEnd = 787812, MetadataInitTokenRva = 46282996L, MetadataInitFlagRva = 59867968L)]
	public unsafe void GetAttachments(int slotIndex, List<SkinEntry> attachments)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&slotIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachments);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAttachments_Public_Void_Int32_List_1_SkinEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 787832, RefRangeEnd = 787834, XrefRangeStart = 787815, XrefRangeEnd = 787832, MetadataInitTokenRva = 46282880L, MetadataInitFlagRva = 59867969L)]
	public unsafe void AttachAll(Skeleton skeleton, Skin oldSkin)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)oldSkin);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttachAll_Internal_Void_Skeleton_Skin_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Skin(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
