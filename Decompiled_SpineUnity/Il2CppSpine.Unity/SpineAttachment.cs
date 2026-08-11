using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppSpine.Unity;

public class SpineAttachment : SpineAttributeBase
{
	private static readonly IntPtr NativeFieldInfoPtr_returnAttachmentPath;

	private static readonly IntPtr NativeFieldInfoPtr_currentSkinOnly;

	private static readonly IntPtr NativeFieldInfoPtr_placeholdersOnly;

	private static readonly IntPtr NativeFieldInfoPtr_skinField;

	private static readonly IntPtr NativeFieldInfoPtr_slotField;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Boolean_String_String_String_Boolean_Boolean_0;

	public unsafe bool returnAttachmentPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_returnAttachmentPath);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_returnAttachmentPath)) = flag;
		}
	}

	public unsafe bool currentSkinOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSkinOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSkinOnly)) = flag;
		}
	}

	public unsafe bool placeholdersOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_placeholdersOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_placeholdersOnly)) = flag;
		}
	}

	public unsafe string skinField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skinField);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skinField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string slotField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotField);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static SpineAttachment()
	{
		Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr);
		NativeFieldInfoPtr_returnAttachmentPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, "returnAttachmentPath");
		NativeFieldInfoPtr_currentSkinOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, "currentSkinOnly");
		NativeFieldInfoPtr_placeholdersOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, "placeholdersOnly");
		NativeFieldInfoPtr_skinField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, "skinField");
		NativeFieldInfoPtr_slotField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, "slotField");
		NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Boolean_String_String_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr, 100664395);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793416, XrefRangeEnd = 793430, MetadataInitTokenRva = 46303156L, MetadataInitFlagRva = 59848234L)]
	public unsafe SpineAttachment(bool currentSkinOnly = true, bool returnAttachmentPath = false, bool placeholdersOnly = false, string slotField = "", string dataField = "", string skinField = "", bool includeNone = true, bool fallbackToTextField = false)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineAttachment>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[8];
		*ptr = (nint)(&currentSkinOnly);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &returnAttachmentPath;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &placeholdersOnly;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(slotField);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(dataField);
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(skinField);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &includeNone;
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = &fallbackToTextField;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Boolean_String_String_String_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineAttachment(IntPtr pointer)
		: base(pointer)
	{
	}
}
