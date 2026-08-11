using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class Attachment : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Name_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Name_Private_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Copy_Public_Abstract_Virtual_New_Attachment_0;

	public unsafe string _Name_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Name_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Name_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Name
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 51089, RefRangeEnd = 51105, XrefRangeStart = 51089, XrefRangeEnd = 51105, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Name_Private_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Attachment()
	{
		Il2CppClassPointerStore<Attachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Attachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Attachment>.NativeClassPtr);
		NativeFieldInfoPtr__Name_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Attachment>.NativeClassPtr, "<Name>k__BackingField");
		NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Attachment>.NativeClassPtr, 100663518);
		NativeMethodInfoPtr_set_Name_Private_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Attachment>.NativeClassPtr, 100663519);
		NativeMethodInfoPtr__ctor_Protected_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Attachment>.NativeClassPtr, 100663520);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Attachment>.NativeClassPtr, 100663521);
		NativeMethodInfoPtr_Copy_Public_Abstract_Virtual_New_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Attachment>.NativeClassPtr, 100663522);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783396, RefRangeEnd = 783398, XrefRangeStart = 783386, XrefRangeEnd = 783396, MetadataInitTokenRva = 47239392L, MetadataInitFlagRva = 59849646L)]
	public unsafe Attachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Attachment>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

	[CallerCount(0)]
	public unsafe virtual Attachment Copy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Copy_Public_Abstract_Virtual_New_Attachment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public Attachment(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
